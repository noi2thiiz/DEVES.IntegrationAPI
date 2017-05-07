USE [CRMQA_MSCRM]
GO
/****** Object:  StoredProcedure [dbo].[p_GetTableRelationships]    Script Date: 7/5/2560 19:34:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


create procedure [dbo].[p_GetTableRelationships](@tablename nvarchar(255) = null, @tableid uniqueidentifier = null) as
set nocount on

if @tablename is null and @tableid is null
begin
   RAISERROR ( 1074008071, 16, 127 ) WITH NOWAIT, SETERROR
   return
end 

if @tableid is null
begin
	select @tableid = EntityId
	from EntityView
	where Name = @tablename
end

select 'SourceEntity' = e1.LogicalName, 
       'LeftKey' = r1.Name, 
       'IntersectEntity' = i1.LogicalName, 
       'RightKey' = r2.Name, 
       'TargetEntity' = e3.LogicalName
from EntityView e1, RelationshipView r1, EntityView i1,
	 EntityView e3, RelationshipView r2, EntityView i2
where e1.EntityId = r1.ReferencedEntityId
  and r1.ReferencingEntityId = i1.EntityId
  and e3.EntityId = r2.ReferencedEntityId
  and r2.ReferencingEntityId = i2.EntityId
  and i1.EntityId = i2.EntityId
  and e3.IsLookupTable = 0
  and e3.IsIntersect = 0
  and e3.IsSecurityIntersect = 0
  and e1.IsLookupTable = 0
  and e1.IsIntersect = 0
  and e1.IsSecurityIntersect = 0
  and e1.Name <> e3.Name
  and e1.EntityId = @tableid
order by 1, 3


GO
