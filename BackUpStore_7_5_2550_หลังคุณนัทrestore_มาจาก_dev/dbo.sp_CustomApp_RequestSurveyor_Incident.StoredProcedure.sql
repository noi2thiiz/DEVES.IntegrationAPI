USE [CRMQA_MSCRM]
GO
/****** Object:  StoredProcedure [dbo].[sp_CustomApp_RequestSurveyor_Incident]    Script Date: 7/5/2560 19:34:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_CustomApp_RequestSurveyor_Incident]
	@IncidentId UNIQUEIDENTIFIER ,
	@CurrentUserId UNIQUEIDENTIFIER 
AS 
    BEGIN		
        BEGIN TRANSACTION 
		
			--DECLARE @TicketNumber AS NVARCHAR(20) = 'CAS201702-00003';
			--DECLARE @CurrentUserCode AS NVARCHAR(20) = 'G001';
			select [Case].IncidentId
				, [Case].ticketnumber as [ticketNunber]
				, [Case].pfc_claim_noti_number as [EventCode]
				, [ParentCase].pfc_claim_noti_number as [claimnotirefer]
				, [Case].pfc_policy_number as [InsureID]
				, [PolicyAddi].pfc_rsk_num as [RSKNo]
				, [PolicyAddi].pfc_tran_num as [TranNo]
				, [Case].pfc_informer_nameName as [NotifyName]
				, [Case].pfc_informer_mobile as [Mobile]
				, [Case].pfc_driver_nameName as [Driver]
				, [Case].pfc_driver_mobile as [DriverTel]
				, [Case].pfc_current_reg_num as [current_VehicleLicence]
				, [Case].pfc_current_reg_num_prov as [current_Province]
				, dbo.f_ConvertcrmDateTime('get', [Case].pfc_notification_date) as [EventDate]		
				, dbo.f_ConvertcrmDateTime('get', [Case].pfc_accident_on) as [ActivityDate]
				, [Case].pfc_accident_desc as [EventDetail]
				, CAST(RIGHT([Case].pfc_num_of_expect_injuries,2) as INT) as [isCasualty]
				, [Case].pfc_accident_place as [EventLocation]
				, [Case].pfc_accident_place  as [accidentLocation]
				, [Case].pfc_accident_latitude as [accidentLat]
				, [Case].pfc_accident_longitude as [accidentLng]
				, case when [Case].pfc_customer_vip = 1 then 'Y' else 'N' end as [IsVIP] 
				, [Case].pfc_case_of_claim_remark as [Remark]
				, case when RIGHT([Case].pfc_claim_type,1) = 0 then 1 when RIGHT([Case].pfc_claim_type,1) = 1 then 0 else null end as [ClameTypeID]
				, CAST(RIGHT([Case].pfc_send_out_surveyor,1) as INT) as [SubClameTypeID]
				, dbo.f_GetSystemUserByFieldInfo('EmployeeId',@CurrentUserId) as [informBy] 
				, [Case].pfc_survey_meeting_latitude as [appointLat]
				, [Case].pfc_survey_meeting_longitude as [appointLong]
				, [Case].pfc_survey_meeting_place as [appointLocation]
				, dbo.f_ConvertcrmDateTime('get', [Case].pfc_survey_meeting_date) as [appointDate]
				, [Case].pfc_appointment_person_name as [appointName]
				, [Case].pfc_appointment_person_phone_no as [appointPhone]
				, null as [contractName]
				, null as [contractPhone]
			from dbo.IncidentBase [Case] with (nolock)
			left outer join dbo.IncidentBase [ParentCase] with (nolock)
				on [ParentCase].IncidentId = [Case].pfc_parent_caseId
			left outer join dbo.pfc_policy_additionalBase [PolicyAddi] with (nolock) 
				on [PolicyAddi].pfc_policy_additionalId = [Case].pfc_policy_additionalId
			where [Case].IncidentId = @IncidentId

		COMMIT TRANSACTION -- Commit transaction if the transaction is successful
    END

/*
DECLARE @TicketNumber AS NVARCHAR(20) = 'CAS201702-00003';
DECLARE @CurrentUserCode AS NVARCHAR(20) = 'G001';
DECLARE @IncidentId UNIQUEIDENTIFIER = 'D246086E-C1EE-E611-80D4-0050568D1874';
DECLARE @CurrentUserId UNIQUEIDENTIFIER = '50529F88-2BE9-E611-80D4-0050568D1874';



--EXEC [dbo].[sp_CustomApp_RequestSurveyor_Incident] @TicketNumber, @CurrentUserCode

EXEC [dbo].[sp_CustomApp_RequestSurveyor_Incident] @IncidentId, @CurrentUserId
*/
GO
