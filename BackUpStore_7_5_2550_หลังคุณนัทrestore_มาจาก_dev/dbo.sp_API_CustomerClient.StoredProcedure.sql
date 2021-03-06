USE [CRMQA_MSCRM]
GO
/****** Object:  StoredProcedure [dbo].[sp_API_CustomerClient]    Script Date: 7/5/2560 19:34:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_API_CustomerClient]
	@clientType NVARCHAR(1),@clientId NVARCHAR(100)
AS
BEGIN
	BEGIN TRANSACTION 
		--DECLARE @clientId NVARCHAR(100) = '2010815184834'
        IF UPPER(@clientType) = 'P'
			BEGIN
				SELECT TOP 1  *
				FROM    ContactBase WITH ( UPDLOCK )
				WHERE   UPPER(pfc_polisy_client_id) = UPPER(@clientId)
			END
        ELSE IF UPPER(@clientType) = 'C'
			BEGIN
				SELECT TOP 1 *
				FROM    AccountBase WITH ( UPDLOCK )
				WHERE   UPPER(AccountNumber) = UPPER(@clientId)
			END
	COMMIT TRANSACTION -- Commit transaction if the transaction is successful

END

/*
DECLARE @clientType NVARCHAR(1) = 'P'
DECLARE @clientId NVARCHAR(100) = 'CRM5555'
EXEC [dbo].[sp_API_CustomerClient] @clientType,@clientId

*/
GO
