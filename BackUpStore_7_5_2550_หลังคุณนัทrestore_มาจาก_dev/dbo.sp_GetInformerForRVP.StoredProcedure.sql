USE [CRMQA_MSCRM]
GO
/****** Object:  StoredProcedure [dbo].[sp_GetInformerForRVP]    Script Date: 7/5/2560 19:34:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		 Patiwat
-- Create date:  7/4/2556
-- Description:	ใช้สำหรับหา  pfc_informer_name RVP 
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetInformerForRVP]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT TOP 1 *FROM [CRMDEV_MSCRM].[dbo].[AccountBase] where pfc_polisy_client_id = '10077508'
END

GO
