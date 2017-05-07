USE [CRMQA_MSCRM]
GO
/****** Object:  StoredProcedure [dbo].[p_ma_DeleteListMembers]    Script Date: 7/5/2560 19:34:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


create procedure 
[dbo].[p_ma_DeleteListMembers]
(
     @guid_list as uniqueidentifier
)
as
begin 
    declare @message nvarchar(2000)
    set nocount on

    delete from ListMemberBase where ListId = @guid_list
    
end 
GO
