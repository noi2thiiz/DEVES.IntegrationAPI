USE [CRMQA_MSCRM]
GO
/****** Object:  StoredProcedure [dbo].[sp_GetUserForRVP]    Script Date: 7/5/2560 19:34:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Patiwat
-- Create date: 7/4/2560
-- Description:	User ชั่วคราวของ RvP ตามที่คุณไกด์ให้มา
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetUserForRVP]

AS
BEGIN
	
	SET NOCOUNT ON;
	SELECT TOP (1)*
    FROM [CRMDEV_MSCRM].[dbo].[SystemUserBase]
	WHERE DomainName ='DVS\sasipa.b' 
	
END

GO
