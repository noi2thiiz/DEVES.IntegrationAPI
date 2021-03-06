USE [CRMQA_MSCRM]
GO
/****** Object:  StoredProcedure [dbo].[p_ReinitSystemUserManagerMap]    Script Date: 7/5/2560 19:34:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--@MaxDepthForHierarchicalSecurityModel is the maxDepth to be used while expanding the SystemUser Hierarchy
--This depth will be considered relative to each user in the hierarchy.
--Only child users at hierarchy level within the maxDepth limit will be added to the SystemUserManagerMap table
--for the particular ParentSystemUser.
CREATE     procedure [dbo].[p_ReinitSystemUserManagerMap] @MaxDepthForHierarchicalSecurityModel int
AS
BEGIN

declare @hsmEnabled bit = 0;
declare @positionEnabled bit = 0;
declare @OtcSystemUserManagerMap int = 51 -- ObjectTypeCode for SystemUserManagerMap

SELECT @hsmEnabled = IsHierarchicalSecurityModelEnabled, @positionEnabled = UsePositionHierarchy
FROM OrganizationBase 

IF @hsmEnabled = 0
RETURN;

IF @MaxDepthForHierarchicalSecurityModel = -1
	SET @MaxDepthForHierarchicalSecurityModel = 3; -- Default value for maxDepth of Hierarchy that is set for an Org

	-- Create a temp table to store the actual Hierarchy level(relative to the root)
	-- If @positionEnabled is true it will store for all the Positions in PositionBase table else 
	-- for all the SystemUsers in SystemUser table.
	CREATE TABLE #HierarchyLevels(ChildId uniqueidentifier primary key, ParentId uniqueidentifier, HierarchyLevel int not null)
	
	IF @positionEnabled = 1
	--insert all the existing rows in PositionBase table into #HierarchyLevels with default value 0 for HierarchyLevel
	INSERT INTO #HierarchyLevels(ChildId, ParentId, HierarchyLevel)
	SELECT p.PositionId,p.ParentPositionId,0 FROM PositionBase p 
	ELSE
	--insert all the existing rows in SystemUser table into #HierarchyLevels with default value 0 for HierarchyLevel
	INSERT INTO #HierarchyLevels(ChildId, ParentId, HierarchyLevel)
	SELECT s.SystemUserId,s.ParentSystemUserId,0 FROM SystemUserBase s 

	-- Set Level = 1 For Rows With No ParentIds
	UPDATE #HierarchyLevels SET HierarchyLevel = 1
	WHERE ParentId is Null -- root -- level 1

	--Update level for remaining rows in #HierarchyLevels Table
	WHILE (SELECT COUNT(*) FROM #HierarchyLevels WHERE HierarchyLevel = 0) > 0
		BEGIN
			UPDATE sChild SET HierarchyLevel = sParent.HierarchyLevel + 1
			FROM #HierarchyLevels sChild
			INNER JOIN #HierarchyLevels sParent
			ON sChild.ParentId = sParent.ChildId
			WHERE
				sChild.HierarchyLevel = 0 AND
				sParent.HierarchyLevel != 0
		END


  -- temp table for new map
	CREATE TABLE #NewSystemUserManagerMap(
		HierarchyLevel int NOT NULL DEFAULT (0),
		ParentSystemUserId uniqueidentifier NOT NULL,
		SystemUserId uniqueidentifier NOT NULL)

	IF @positionEnabled = 1
	BEGIN
		-- expand position hierarchy
		 WITH ParentPositions
		 AS
		 ( 
			SELECT s.ParentPositionId, s.PositionId 
			FROM PositionBase s
			WHERE ParentPositionId is not null AND StateCode = 0
			UNION ALL
			SELECT p.ParentPositionId, sb.PositionId 
			FROM ParentPositions p,
				 PositionBase sb
			WHERE sb.ParentPositionId = p.PositionId   AND sb.StateCode = 0
		)
		-- insert all users in each position for each position. Select child users having
		-- hierarchy level <= maxdepth relative to the Hierarchy level of
		-- current parentSystemUserId
		insert into #NewSystemUserManagerMap(ParentSystemUserId, SystemUserId, HierarchyLevel)
		select b.SystemUserId as ParentSystemUserId, c.SystemUserId, suh.HierarchyLevel - sInner.HierarchyLevel from ParentPositions a
		 join SystemUserBase b on a.ParentPositionId = b.PositionId AND b.IsDisabled = 0
		 join SystemUserBase c on a.PositionId = c.PositionId	AND c.IsDisabled = 0
		 inner join #HierarchyLevels suh 
		 on c.PositionId = suh.ChildId
		 inner join #HierarchyLevels sInner
		 on sInner.ChildId = b.PositionId
				 where (suh.HierarchyLevel  <= 
							 --get hierarchy level of current parent and add maxDepth value to it.
							 ( sInner.HierarchyLevel + @MaxDepthForHierarchicalSecurityModel)
						) 
	END
	ELSE
	BEGIN
		-- expand user hierarchy
		 WITH ParentUsers
		 AS
		 ( 
			SELECT s.ParentSystemUserId, s.SystemUserId 
			FROM SystemUserBase s 
			WHERE ParentSystemUserId is not null  AND s.IsDisabled = 0
			UNION ALL
			SELECT p.ParentSystemUserId, sb.SystemUserId 
			FROM ParentUsers p,
				 SystemUserBase sb 
			WHERE sb.ParentSystemUserId = p.SystemUserId AND sb.IsDisabled = 0
		)
		-- insert user hierarchy. Select child users having hierarchy level <= maxdepth relative
		-- to the Hierarchy level of current parentSystemUserId
		insert into #NewSystemUserManagerMap(ParentSystemUserId, SystemUserId, HierarchyLevel)
		select p.ParentSystemUserId, p.SystemUserId, suh.HierarchyLevel -sInner.HierarchyLevel
		FROM ParentUsers p inner join #HierarchyLevels suh 
		on p.SystemUserId = suh.ChildId
		inner join #HierarchyLevels sInner
		on sInner.ChildId = p.ParentSystemUserId
				  where (suh.HierarchyLevel  <= 
							 --get hierarchy level of current parent and add maxDepth value to it.
							 (select sInner.HierarchyLevel + @MaxDepthForHierarchicalSecurityModel)
						 )
	END

	DROP TABLE #HierarchyLevels

	-- insert self records for each user
	insert into #NewSystemUserManagerMap(ParentSystemUserId, SystemUserId)
	select SystemUserId, SystemUserId
	FROM SystemUserBase 

--   New map is built, do diffing with existing old map

--  Delete from old map rows, which do not exist in new map with tracking deleted
delete SystemUserManagerMap 
	OUTPUT DELETED.SystemUserManagerMapId, @OtcSystemUserManagerMap 
		into SubscriptionTrackingDeletedObject(ObjectId, ObjectTypeCode)
	from SystemUserManagerMap as old
	join
	(
		select SystemUserId, ParentSystemUserId, HierarchyLevel from SystemUserManagerMap
		Except
		select SystemUserId, ParentSystemUserId, HierarchyLevel from #NewSystemUserManagerMap
	) deletedMap
	on old.SystemUserId = deletedMap.SystemUserId and old.ParentSystemUserId = deletedMap.ParentSystemUserId and old.HierarchyLevel = deletedMap.HierarchyLevel 

-- Insert into existing map rows from new map, which do not exist in old map
; with NewRows as
(
  select HierarchyLevel, ParentSystemUserId, SystemUserId from #NewSystemUserManagerMap
  except
  select HierarchyLevel, ParentSystemUserId, SystemUserId from SystemUserManagerMap
) 
insert into SystemUserManagerMap(HierarchyLevel, ParentSystemUserId, SystemUserId, SystemUserManagerMapId)
  select HierarchyLevel, ParentSystemUserId, SystemUserId, newid() from NewRows

DROP TABLE #NewSystemUserManagerMap

END

GO
