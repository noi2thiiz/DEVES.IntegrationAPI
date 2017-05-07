USE [CRMQA_MSCRM]
GO
/****** Object:  StoredProcedure [dbo].[p_DeleteCustomActivityEntries]    Script Date: 7/5/2560 19:34:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


--TODO: Tune this for performance.

create procedure [dbo].[p_DeleteCustomActivityEntries](@activityTypeCode integer) as
begin
set nocount on

delete ActivityPartyBase from ActivityPartyBase 
join ActivityPointerBase act on 
(ActivityPartyBase.ActivityId = act.ActivityId and act.ActivityTypeCode = @activityTypeCode)

delete ActivityPointerBase from ActivityPointerBase act where 
act.ActivityTypeCode = @activityTypeCode

end	
GO
