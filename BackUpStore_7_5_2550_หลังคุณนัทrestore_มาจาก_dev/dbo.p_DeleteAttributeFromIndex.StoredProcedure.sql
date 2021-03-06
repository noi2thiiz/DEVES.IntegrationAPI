USE [CRMQA_MSCRM]
GO
/****** Object:  StoredProcedure [dbo].[p_DeleteAttributeFromIndex]    Script Date: 7/5/2560 19:34:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



create procedure [dbo].[p_DeleteAttributeFromIndex] (@ObjectTypeCode int,
	@AttributeId uniqueidentifier, @indexType int) as
set nocount on

declare @indexid uniqueidentifier

select @indexid = ei.IndexId from EntityIndex ei
    join EntityView e on (e.EntityId = ei.EntityId)
where e.ObjectTypeCode = @ObjectTypeCode and ei.IndexType = @indexType

exec dbo.p_DeleteAttributeFromIndexById @AttributeId, @indexid

GO
