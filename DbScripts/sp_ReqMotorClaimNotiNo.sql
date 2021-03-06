USE [CRMDEV_MSCRM]
GO
/****** Object:  StoredProcedure [dbo].[sp_ReqMotorClaimNotiNo]    Script Date: 6/4/2560 19:47:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[sp_ReqMotorClaimNotiNo]
	@uniqueID UNIQUEIDENTIFIER ,
    @requestByName NVARCHAR(250) ,
    @resultCode NVARCHAR(10) OUTPUT ,
    @resultDesc NVARCHAR(1000) OUTPUT
AS 
    BEGIN
		DECLARE @autoNumber AS NVARCHAR(50) ;
		DECLARE @domain AS NVARCHAR(50) = 'ClaimNotiNo_MC' ;
		DECLARE @entityCode AS NVARCHAR(15) = 'MC';	
		select @resultCode	= '1' -- Success(0), Fail(1)
				, @resultDesc	= 'Fail'; 
        BEGIN TRANSACTION 
			EXEC [sp_IncreaseAutoNumber] @domain, @entityCode, @autoNumber OUTPUT ;
						
			update dbo.IncidentBase
			set pfc_claim_noti_number = @autoNumber
				, pfc_claim_noti_numberOn = GETUTCDATE()
				, pfc_request_claim_noti_number_by = @requestByName
			where IncidentBase.IncidentId = @uniqueID

			select @resultCode = '0',@resultDesc = @autoNumber
		COMMIT TRANSACTION -- Commit transaction if the transaction is successful
    END

/*
DECLARE @ticketNo AS NVARCHAR(20) = 'CAS-00012-L1X2C8';
DECLARE @uniqueID AS UNIQUEIDENTIFIER = 'E7E760A9-F0C1-E611-80CB-0050568D1874';
DECLARE @requestByName AS NVARCHAR(250) = 'ADMIN TEST';
DECLARE @resultCode AS NVARCHAR(10);
DECLARE @resultDesc as NVARCHAR(1000) 
----EXEC [dbo].[sp_ReqMotorClaimNotiNo] @uniqueID, @requestByName, @resultCode OUTPUT, @resultDesc;
select @uniqueID = ISNULL(IncidentId,@uniqueID)
from dbo.IncidentBase a with (nolock)
where a.TicketNumber = @ticketNo
EXEC [dbo].[sp_ReqMotorClaimNotiNo] @uniqueID, @requestByName, @resultCode OUTPUT, @resultDesc OUTPUT ;
select @ticketNo as [@ticketNo],@uniqueID as [@uniqueID],@requestByName as [@requestByName],@resultCode as [@resultCode], @resultDesc as [@resultDesc]
*/