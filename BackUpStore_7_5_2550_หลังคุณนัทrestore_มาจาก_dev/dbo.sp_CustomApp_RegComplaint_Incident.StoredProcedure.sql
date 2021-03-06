USE [CRMQA_MSCRM]
GO
/****** Object:  StoredProcedure [dbo].[sp_CustomApp_RegComplaint_Incident]    Script Date: 7/5/2560 19:34:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_CustomApp_RegComplaint_Incident]
	@IncidentId UNIQUEIDENTIFIER ,
	@CurrentUserId UNIQUEIDENTIFIER 
AS 
    BEGIN		
        BEGIN TRANSACTION 
		
			--DECLARE @TicketNumber AS NVARCHAR(20) = 'CAS201702-00003';
			--DECLARE @CurrentUserCode AS NVARCHAR(20) = 'G001';
			select [Case].IncidentId
				, [Case].ticketnumber as [caseNo]
				, null as [compDate]
				, dbo.f_GetSystemUserByFieldInfo('EmployeeId',@CurrentUserId) as [empNo]
				, null as [compType]
				, null as [cntType]
				, null as [compCustname]
				, null as [compCustcompany]
				, null as [compCusttype]
				, null as [contrChanel]
				, null as [kpvDate]
				, null as [chanInform]
				, null as [compAddr]
				, null as [compEmail]
				, null as [compMobile]
				, null as [compPhone]
				, null as [compFax]
				, null as [compIdcard]
				, null as [compClaim]
				, null as [compPolicy]
				, null as [compRegno]
				, null as [compDetail]
				, null as [compResolve]
			from dbo.IncidentBase [Case] with (nolock)
			where [Case].IncidentId = @IncidentId

		COMMIT TRANSACTION -- Commit transaction if the transaction is successful
    END

/*
DECLARE @TicketNumber AS NVARCHAR(20) = 'CAS201702-00003';
DECLARE @CurrentUserCode AS NVARCHAR(20) = 'G001';
EXEC [dbo].[sp_CustomApp_RegComplaint_Incident] @TicketNumber, @CurrentUserCode



DECLARE @IncidentId UNIQUEIDENTIFIER = 'D246086E-C1EE-E611-80D4-0050568D1874';
DECLARE @CurrentUserId UNIQUEIDENTIFIER = '50529F88-2BE9-E611-80D4-0050568D1874';
EXEC [dbo].[sp_CustomApp_RegComplaint_Incident] @IncidentId, @CurrentUserId
*/
GO
