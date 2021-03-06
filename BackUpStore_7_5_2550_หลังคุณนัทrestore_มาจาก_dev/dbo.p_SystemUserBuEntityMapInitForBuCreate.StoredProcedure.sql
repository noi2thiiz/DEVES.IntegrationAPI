USE [CRMQA_MSCRM]
GO
/****** Object:  StoredProcedure [dbo].[p_SystemUserBuEntityMapInitForBuCreate]    Script Date: 7/5/2560 19:34:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- this stored procedure inits SystemUserBusinessUnitEntityMap for a BU create
CREATE     procedure [dbo].[p_SystemUserBuEntityMapInitForBuCreate](@BusinessUnitId uniqueidentifier) as
begin
SET NOCOUNT ON

declare @DEEP_MASK int
declare @parentBusinessId uniqueidentifier

select @DEEP_MASK = 4 -- PRIVILEGE_DEPTH_MASK.DEEP_MASK
select @parentBusinessId=ParentBusinessUnitId from BusinessUnit
where BusinessUnitId=@BusinessUnitId

insert into SystemUserBusinessUnitEntityMap
(SystemUserId, BusinessUnitId, ObjectTypeCode, ReadPrivilegeDepth ) 
select SystemUserId, @BusinessUnitId, ObjectTypeCode, ReadPrivilegeDepth 
		from SystemUserBusinessUnitEntityMap
		where BusinessUnitId = @parentBusinessId AND 
		ReadPrivilegeDepth = @DEEP_MASK

end
GO
