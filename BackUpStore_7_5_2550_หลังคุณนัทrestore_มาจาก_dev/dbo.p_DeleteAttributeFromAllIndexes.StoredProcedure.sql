USE [CRMQA_MSCRM]
GO
/****** Object:  StoredProcedure [dbo].[p_DeleteAttributeFromAllIndexes]    Script Date: 7/5/2560 19:34:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


create procedure [dbo].[p_DeleteAttributeFromAllIndexes] (@AttributeId uniqueidentifier) as
set nocount on

declare @indexes table (IndexId uniqueidentifier not null)
insert into @indexes (IndexId) select IndexId from IndexAttributes where AttributeId = @AttributeId UNION select IndexId from IndexFilters where AttributeId = @AttributeId 

declare @indexid uniqueidentifier

declare indexAttributeCursor cursor local FORWARD_ONLY READ_ONLY for
select IndexId from @indexes

open indexAttributeCursor
fetch next from indexAttributeCursor into @indexid
while (@@fetch_status = 0)
begin

	exec dbo.p_DeleteAttributeFromIndexById @AttributeId, @indexid

	fetch next from indexAttributeCursor into @indexid
end
close indexAttributeCursor
deallocate indexAttributeCursor 

select IndexId from @indexes




GO
