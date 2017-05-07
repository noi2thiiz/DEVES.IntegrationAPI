USE [CRMQA_MSCRM]
GO
/****** Object:  StoredProcedure [dbo].[p_UoMToBase]    Script Date: 7/5/2560 19:34:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



Create Proc [dbo].[p_UoMToBase] 
as
begin

  -- update related fields
  update #UoMBaseTemp
  set #UoMBaseTemp.BaseUoM = b.baseuom, #UoMBaseTemp.Quantity = b.quantity, 
	#UoMBaseTemp.IsScheduleBaseUoM = b.isschedulebaseuom,
	#UoMBaseTemp.BaseName = b.name
  from #UoMBaseTemp inner join FilteredUoM as b on (#UoMBaseTemp.UoMId= b.uomid)


  --- into the loop of finding the base unit
  declare @NotBase int
  select @NotBase = count(*)
  from #UoMBaseTemp as t
  where t.IsScheduleBaseUoM = 0

  while @NotBase <> 0
  begin
    update #UoMBaseTemp 
    set #UoMBaseTemp.BaseUoM = b.baseuom, #UoMBaseTemp.Quantity = #UoMBaseTemp.Quantity * b.quantity, 
	#UoMBaseTemp.IsScheduleBaseUoM = b.isschedulebaseuom,
	#UoMBaseTemp.BaseName = b.name
    from #UoMBaseTemp join FilteredUoM as b on (#UoMBaseTemp.BaseUoM = b.uomid)
    where #UoMBaseTemp.IsScheduleBaseUoM = 0

    select @NotBase = count(*)
    from #UoMBaseTemp as t
    where t.IsScheduleBaseUoM = 0
  end
end

GO
