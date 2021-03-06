USE [CRMQA_MSCRM]
GO
/****** Object:  StoredProcedure [dbo].[p_ReinitPrincipalObjectAccessReadSnapshots]    Script Date: 7/5/2560 19:34:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE     procedure [dbo].[p_ReinitPrincipalObjectAccessReadSnapshots](@minimumCount int,@isHierarchicalSecurityModelEnabled bit) as
begin
	SET NOCOUNT ON

	-- Create a temp table to store the new data in - we'll then copy the new data over to the existing table.  We
	-- do it in steps so as to avoid dropping all of the data in this table, which leaves a window where we could
	-- end up getting no data for the cache.
	create table #POASnapshotData(PrincipalId uniqueidentifier, ObjectTypeCode int not null, Count bigint not null)
	CREATE CLUSTERED INDEX ndx_POASnapshotData ON #POASnapshotData(PrincipalId, ObjectTypeCode)

	-- first insert data for non-team principals
	insert into #POASnapshotData(PrincipalId, ObjectTypeCode, Count)
	select PrincipalId, ObjectTypeCode, COUNT(PrincipalObjectAccessId)
	from PrincipalObjectAccess POA with (nolock)
	where ((POA.AccessRightsMask|POA.InheritedAccessRightsMask) & 1) = 1
	and POA.PrincipalTypeCode <> 9	-- skip team
	group by PrincipalId, ObjectTypeCode
	having COUNT(PrincipalObjectAccessId) > @minimumCount

	-- insert *user* data corresponding to their team principals
	-- Use a temporary table to pull out values from POA, which will help perf if POA is particularly large
	declare @POAValues table (TeamId uniqueidentifier, ObjectTypeCode int, CountRows int, Primary Key clustered (TeamId, ObjectTypeCode))
	
	insert into @POAValues (TeamId, ObjectTypeCode, CountRows)
	select PrincipalId, ObjectTypeCode, COUNT(PrincipalObjectAccessId)
	from PrincipalObjectAccess POA with (nolock)
	where ((POA.AccessRightsMask|POA.InheritedAccessRightsMask) & 1) = 1
	and POA.PrincipalTypeCode = 9	-- just team
	group by PrincipalId, ObjectTypeCode
	having COUNT(PrincipalObjectAccessId) > @minimumCount

	MERGE #POASnapshotData AS Target
	USING
	(select s.SystemUserId, p.ObjectTypeCode, SUM(p.CountRows) as TeamShareCount from @POAValues p
	join SystemUserPrincipals s WITH (nolock)
	on p.TeamId = s.PrincipalId
	group by s.SystemUserId, p.ObjectTypeCode
		)  AS Source
	ON Source.SystemUserId = Target.PrincipalId AND Source.ObjectTypeCode = Target.ObjectTypeCode
	WHEN MATCHED THEN
		UPDATE SET Target.Count = Source.TeamShareCount + Target.Count
	WHEN NOT MATCHED BY TARGET THEN
		INSERT (PrincipalId, ObjectTypeCode, Count) VALUES 
		(Source.SystemUserId, Source.ObjectTypeCode, Source.TeamShareCount);

	-- Delete anything that's not in #POASnapshotData
	delete from PrincipalObjectAccessReadSnapshot where PrincipalObjectAccessReadSnapshotId in
	(select PrincipalObjectAccessReadSnapshotId from PrincipalObjectAccessReadSnapshot poars
	left join #POASnapshotData psd on poars.PrincipalId = psd.PrincipalId and poars.ObjectTypeCode = psd.ObjectTypeCode
	where psd.ObjectTypeCode is null)

	-- Update rows that are in both
	update poars set poars.Count = psd.Count
	from PrincipalObjectAccessReadSnapshot poars
	join #POASnapshotData psd on poars.PrincipalId = psd.PrincipalId and poars.ObjectTypeCode = psd.ObjectTypeCode

	-- Insert any new rows
	insert into PrincipalObjectAccessReadSnapshot (PrincipalObjectAccessReadSnapshotId, PrincipalId, Count, ObjectTypeCode)
	select NEWID(), psd.PrincipalId, psd.Count, psd.ObjectTypeCode
	from #POASnapshotData psd
	left join PrincipalObjectAccessReadSnapshot poars on poars.PrincipalId = psd.PrincipalId and poars.ObjectTypeCode = psd.ObjectTypeCode
	where poars.PrincipalId is null
	
	if @isHierarchicalSecurityModelEnabled = 1
	begin
	--Update Count for each Principal in POAReadSnapshot with shared count of user's in the hierarchy and child users count
	update PrincipalObjectAccessReadSnapshot  set Count = ShareCount,ChildUserPrincipalsCount= childUserCount
    from
		(select sum(poars.Count) as ShareCount, poars.PrincipalId 
			as parentPrincipalId,
			 case
				 when count(smp.SystemUserId) = 0 then 0 
				 else count(smp.SystemUserId) - 1
			 end	
				as childUserCount from PrincipalObjectAccessReadSnapshot poars
			left join SystemUserManagerMap smp WITH (nolock) on poars.PrincipalId = smp.ParentSystemUserId
			group by poars.PrincipalId) poarsInner
	where PrincipalId = parentPrincipalId

	--insert rows for managers which do not exist already
	--keep share count zero as no POA records are there , compute ChildUsers . 
	insert into PrincipalObjectAccessReadSnapshot (PrincipalObjectAccessReadSnapshotId, PrincipalId, Count, ObjectTypeCode,ChildUserPrincipalsCount)
	select pOuter.*
	from (select NEWID() POAReadSnapshotId,smp.ParentSystemUserId PrincipalId,0 Count,pInner.Objecttypecode ObjectTypeCode,
	count(smp.SystemUserId) childUsersCount
	from SystemUserManagerMap smp right join 
	(select distinct ObjectTypeCode from PrincipalObjectAccessReadSnapshot) pInner
	on pInner.ObjectTypeCode != 0
	where smp.ParentSystemUserId is not null
	group by smp.ParentSystemUserId,pInner.ObjectTypeCode
	having
	count(smp.SystemUserId) > 1) pOuter -- do not consider the self-referential row.
	left join PrincipalObjectAccessReadSnapshot poars on poars.PrincipalId = pOuter.PrincipalId 
	where poars.PrincipalId is null

	--update TeamPrincipalCount for all users and users in hierarchy
	update PrincipalObjectAccessReadSnapshot  set TeamPrincipalsCount = teamCount
	from
		(select count(pem.PrincipalId) as teamCount, poars.PrincipalId as parentPrincipalId
			from PrincipalEntityMap pem join
			SystemUserPrincipals sup on sup.PrincipalId = pem.PrincipalId join
			SystemUserManagerMap smp on smp.SystemUserId = sup.SystemUserId join
			PrincipalObjectAccessReadSnapshot poars on poars.PrincipalId = smp.ParentSystemUserId 
		 group by poars.PrincipalId) poarsInner
	where PrincipalId = parentPrincipalId

	end
	drop table #POASnapshotData
end
GO
