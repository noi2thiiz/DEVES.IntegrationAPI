USE [CRMQA_MSCRM]
GO
/****** Object:  StoredProcedure [dbo].[p_IsInBusinessDeep]    Script Date: 7/5/2560 19:34:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


/*
This proc is to be used to test if ?BusinessId is in ParentId's business hierarchy.
*/
CREATE procedure [dbo].[p_IsInBusinessDeep](
 @BusinessId uniqueidentifier,
 @ParentId uniqueidentifier) 
as
begin
set nocount on

declare @TempParentId uniqueidentifier
select @TempParentId = @BusinessId

declare @CurBizId uniqueidentifier
while (@TempParentId is not null)
begin
	if (@TempParentId = @ParentId)
	begin
		select IsInBusinessDeep = 1
		return
	end

	select @CurBizId = @TempParentId

	select @TempParentId = ParentBusinessUnitId
	from BusinessUnitBase
	where BusinessUnitId = @CurBizId
end

select IsInBusinessDeep = 0

end

GO
