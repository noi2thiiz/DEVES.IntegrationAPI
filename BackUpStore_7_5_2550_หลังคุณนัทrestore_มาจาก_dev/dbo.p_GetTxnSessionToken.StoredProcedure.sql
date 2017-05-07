USE [CRMQA_MSCRM]
GO
/****** Object:  StoredProcedure [dbo].[p_GetTxnSessionToken]    Script Date: 7/5/2560 19:34:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[p_GetTxnSessionToken] as
BEGIN
	SET NOCOUNT ON
	DECLARE @bind_token AS varchar(8000)
	EXECUTE dbo.sp_getbindtoken @bind_token OUTPUT
	SELECT @bind_token as token
END
GO
