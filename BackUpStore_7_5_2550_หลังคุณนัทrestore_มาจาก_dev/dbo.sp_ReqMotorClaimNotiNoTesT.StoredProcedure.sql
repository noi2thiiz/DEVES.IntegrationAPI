USE [CRMQA_MSCRM]
GO
/****** Object:  StoredProcedure [dbo].[sp_ReqMotorClaimNotiNoTesT]    Script Date: 7/5/2560 19:34:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_ReqMotorClaimNotiNoTesT]
	@uniqueID   NVARCHAR(250),
    @requestByName NVARCHAR(250)
AS  
    BEGIN
		DECLARE @autoNumber AS NVARCHAR(50) ;
		DECLARE @domain AS NVARCHAR(50) = 'ClaimNotiNo_MC' ;
		DECLARE @entityCode AS NVARCHAR(15) = 'MC';	
	
        BEGIN TRANSACTION 
			EXEC [sp_IncreaseAutoNumber] @domain, @entityCode, @autoNumber OUTPUT ;
						
			update dbo.IncidentBase
			set pfc_claim_noti_number = @autoNumber
				, pfc_claim_noti_numberOn = GETUTCDATE()
				, pfc_request_claim_noti_number_by = @requestByName
			where IncidentBase.IncidentId = CAST(@uniqueID AS UNIQUEIDENTIFIER)

			
		COMMIT TRANSACTION -- Commit transaction if the transaction is successful
    END

GO
