USE [CRMQA_MSCRM]
GO
/****** Object:  StoredProcedure [dbo].[p_BusinessUnitMapAddBusinessUnit]    Script Date: 7/5/2560 19:34:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


create procedure [dbo].[p_BusinessUnitMapAddBusinessUnit](@BusinessId uniqueidentifier)
as
begin
	-- ASSUMPTION: provided @BusinessId is for leaf node.
	set nocount on

	-- add new BU to the map
	insert BusinessUnitMap(BusinessId, SubBusinessId) values (@BusinessId, @BusinessId)

	-- select parent
	declare @parentBuId uniqueidentifier = @BusinessId
	select @parentBuId = bu.ParentBusinessUnitId from BusinessUnitBase bu where bu.BusinessUnitId = @parentBuId

	-- for all ancestors insert a row in BusinessUnitMap
	while (@parentBuId is not null)
	begin
		insert BusinessUnitMap (BusinessId, SubBusinessId) values (@parentBuId, @BusinessId)
		select @parentBuId = bu.ParentBusinessUnitId from BusinessUnitBase bu where bu.BusinessUnitId = @parentBuId
	end
end
GO
