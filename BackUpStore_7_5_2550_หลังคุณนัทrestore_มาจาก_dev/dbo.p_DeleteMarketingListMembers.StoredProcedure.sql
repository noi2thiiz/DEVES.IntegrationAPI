USE [CRMQA_MSCRM]
GO
/****** Object:  StoredProcedure [dbo].[p_DeleteMarketingListMembers]    Script Date: 7/5/2560 19:34:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE   PROCEDURE [dbo].[p_DeleteMarketingListMembers](@doc ntext, @ListId uniqueidentifier) as

BEGIN
DECLARE @idoc int
DECLARE @incr int

declare @OtcListMember int
select @OtcListMember = 4301

create table #tempMemberBase(EntityId uniqueidentifier
)

EXEC dbo.sp_xml_preparedocument @idoc OUTPUT, @doc

insert into #tempMemberBase(EntityId)
select entityid FROM 
OPENXML(@idoc, '/VALUES/VALUE',1)
WITH (entityid uniqueidentifier)

Create index idx_EntityId on #tempMemberBase(EntityId)

delete ListMemberBase
	OUTPUT  DELETED.ListMemberId, @OtcListMember 
	into SubscriptionTrackingDeletedObject(ObjectId, ObjectTypeCode)
from #tempMemberBase, ListMemberBase
Where ListMemberBase.EntityId = #tempMemberBase.EntityId
and ListId = @ListId

SET @incr = @@ROWCOUNT

drop table #tempMemberBase

exec dbo.sp_xml_removedocument @idoc

return @incr

END



GO
