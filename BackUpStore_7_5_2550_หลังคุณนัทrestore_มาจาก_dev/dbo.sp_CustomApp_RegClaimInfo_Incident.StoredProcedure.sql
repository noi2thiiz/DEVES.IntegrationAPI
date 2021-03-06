USE [CRMQA_MSCRM]
GO
/****** Object:  StoredProcedure [dbo].[sp_CustomApp_RegClaimInfo_Incident]    Script Date: 7/5/2560 19:34:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_CustomApp_RegClaimInfo_Incident]
	@IncidentId UNIQUEIDENTIFIER ,
	@CurrentUserId UNIQUEIDENTIFIER 
AS 
    BEGIN		
        BEGIN TRANSACTION 
		
			--DECLARE @TicketNumber AS NVARCHAR(20) = 'CAS201702-00003';
			--DECLARE @IncidentId UNIQUEIDENTIFIER = 'D246086E-C1EE-E611-80D4-0050568D1874'
			DECLARE @dateFormat AS NVARCHAR(20) = 'yyyy-MM-dd HH:mm:ss';
			select [Case].IncidentId
				/***[claimHeader]***/
				, [Policy].pfc_prod_type as [premiumClass]
				, [Case].ticketnumber as [ticketNumber] 
				, [Case].pfc_claim_noti_number as [claimNotiNo]
				, [ParentCase].pfc_claim_noti_number as [claimNotiRefer]
				, [Case].pfc_policy_number as [policyNo]
				, [PolicyAddi].pfc_rsk_num as [fleetCarNo]
				, [PolicyAddi].pfc_tran_num as [policySeqNo]
				, [PolicyAddi].pfc_zren_num as [renewalNo]
				, [PolicyAddi].pfc_bar_code as [barcode]
				, [PolicyAddi].pfc_insurance_card as [insureCardNo]
				, dbo.f_ConvertcrmDateTime('get', [PolicyAddi].pfc_issue_date) as [policyIssueDate]
				, dbo.f_ConvertcrmDateTime('get', [PolicyAddi].pfc_effective_date) as [policyEffectiveDate]
				, dbo.f_ConvertcrmDateTime('get', [PolicyAddi].pfc_expiry_date) as [policyExpiryDate]
				, [PolicyAddi].pfc_cover_code as [policyProductTypeCode]
				, [PolicyAddi].pfc_cover_name as [policyProductTypeName]
				, case when [PolicyAddi].pfc_garage_flag = 1 then 'Y' else 'N' end as [policyGarageFlag]
				, case when [PolicyAddi].pfc_receive_status = 1 then 'Y' else 'N' end as [policyPaymentStatus]
				, [PolicyAddi].pfc_reg_num as [policyCarRegisterNo]
				, [PolicyAddi].pfc_reg_num_prov as [policyCarRegisterProv]
				, [PolicyAddi].pfc_chassis_num as [carChassisNo]
				, [PolicyAddi].pfc_vehicle_type as [carVehicleType]
				, [PolicyAddi].pfc_vehicle_brand_model as [carVehicleModel]
				, [PolicyAddi].pfc_vehicle_year as [carVehicleYear]
				, [PolicyAddi].pfc_vehicle_body as [carVehicleBody]
				, [PolicyAddi].pfc_vehicle_size as [carVehicleSize]
				, CAST(ROUND([PolicyAddi].pfc_deduct ,2) as decimal(23,2)) as [policyDeduct]
				, [Policy].pfc_agent_code as [agentCode]
				, [Policy].pfc_agent_fullname as [agentName]
				, [Policy].pfc_agent_branch as [agentBranch ]
				, case when [Case].pfc_customer_vip = 1 then 'Y' else 'N' end as [vipCaseFlag]
				, LEFT([Case].pfc_customer_privilege,1) as [privilegeLevel]
				, case when [Case].pfc_high_loss_case_flag = 1 then 'Y' else 'N' end as [highLossCaseFlag]
				, case when [Case].pfc_legal_case_flag = 1 then 'Y' else 'N' end as [legalCaseFlag] 
				, [Case].pfc_case_of_claim_remark as [claimNotiRemark]
				, case when RIGHT([Case].pfc_claim_type,1) = 0 then 'I' when RIGHT([Case].pfc_claim_type,1) = 1 then 'O' else null end as [claimType]
				, dbo.f_GetSystemUserByFieldInfo('DomainName',[Case].pfc_send_request_survey_by) as [informByCrmId]
				, dbo.f_GetSystemUserByFieldInfo('FullName',[Case].pfc_send_request_survey_by) as [informByCrmName]
				, dbo.f_GetSystemUserByFieldInfo('DomainName',@CurrentUserId) as [submitByCrmId]
				, dbo.f_GetSystemUserByFieldInfo('FullName',@CurrentUserId) as [submitByCrmName]
				, dbo.f_GetSystemUserByFieldInfo('Branch',@CurrentUserId) as [serviceBranch]
				--/***[claimInform]***/
				, [Case].pfc_informer_client_number as [informerClientId]
				, [Case].pfc_informer_nameName  as [informerFullName]--Account/Contact.Name/FullName as [informerFullName]
				, [Case].pfc_informer_mobile as [informerMobile]
				, null as [informerPhoneNo]
				, [Case].pfc_driver_client_number as [driverClientId]
				, [Case].pfc_driver_nameName as [driverFullName]
				, [Case].pfc_driver_mobile as [driverMobile]
				, null as [driverPhoneNo]
				, [Policy].pfc_cus_client_number as [insuredClientId]
				, [Policy].pfc_customerIdName as [insuredFullName]
				, null as [insuredMobile]
				, null as [insuredPhoneNo]
				, RIGHT([Case].pfc_relation_cutomer_accident_party,2) as [relationshipWithInsurer]
				, [Case].pfc_current_reg_num as [currentCarRegisterNo]
				, [Case].pfc_current_reg_num_prov as [currentCarRegisterProv]
				, dbo.f_ConvertcrmDateTime('get', [Case].pfc_notification_date) as [informerOn] 
				, dbo.f_ConvertcrmDateTime('get', [Case].pfc_accident_on) as [accidentOn]
				, [Case].pfc_accident_desc_code as [accidentDescCode]
				, [Case].pfc_accident_desc as [accidentDesc]
				, CAST(RIGHT([Case].pfc_num_of_expect_injuries,2) as INT) as [numOfExpectInjury]
				, [Case].pfc_accident_place as [accidentPlace]
				, [Case].pfc_accident_latitude as [accidentLatitude]
				, [Case].pfc_accident_longitude as [accidentLongitude]
				, null as [accidentProvn]--String	2
				, null as [accidentDist]--String	4
				, RIGHT([Case].pfc_send_out_surveyor,2) as [sendOutSurveyorCode]
				--/***[claimAssignSurv]***/
				, [Case].pfc_surveyor_code as [surveyorCode]
				, [Case].pfc_surveyor_client_number as [surveyorClientNumber]
				, [Case].pfc_surveyor_name as [surveyorName]
				, [Case].pfc_surveyor_company_name as [surveyorCompanyName]
				, [Case].pfc_surveyor_company_mobile as [surveyorCompanyMobile]
				, [Case].pfc_surveyor_mobile as [surveyorMobile]
				, case when RIGHT([Case].pfc_surveyor_type,1) = 1 then 'I' when RIGHT([Case].pfc_surveyor_type,1) = 2 then 'O' else null end as [surveyorType]
				, [Case].pfc_surveyor_team as [surveyTeam]
				/***[claimSurvInform]***/
				, [Case].pfc_deductable_fee as [deductibleFee]
				, [Case].pfc_excess_fee as [excessFee]
				, dbo.f_ConvertcrmDateTime('get', [Case].pfc_accident_prilim_surveyor_report_date) as [reportAccidentResultDate]
				, RIGHT([Case].pfc_accident_legal_result,1) as [accidentLegalResult]
				, [Case].pfc_police_station as [policeStation]
				, [Case].pfc_police_record_id as [policeRecordId]
				, dbo.f_ConvertcrmDateTime('get', [Case].pfc_police_record_date) as [policeRecordDate]
				, case when [Case].pfc_police_bail_flag = 1 then 'Y' else 'N' end as [policeBailFlag]
				, [Case].pfc_demage_of_policy_owner_car as [damageOfPolicyOwnerCar]
				, CAST(RIGHT([Case].pfc_num_of_tow_truck,2) as INT) as [numOfTowTruck]
				, [Case].pfc_name_of_tow_company as [nameOfTowCompany]
				, [Case].pfc_detail_of_tow_event as [detailOfTowEvent]
				, CAST(RIGHT([Case].pfc_num_of_accident_injuries,2) as INT) as [numOfAccidentInjury]
				, [Case].pfc_detail_of_accident_injury as [detailOfAccidentInjury]
				, CAST(RIGHT([Case].pfc_num_of_death,2) as INT) as [numOfDeath]
				, [Case].pfc_detail_of_death as [detailOfDeath]
				, dbo.f_GetSystemUserByFieldInfo('EmployeeId',[Case].pfc_accident_prilim_surveyor_report_by) as [caseOwnerCode]
				, dbo.f_GetSystemUserByFieldInfo('FullName',[Case].pfc_accident_prilim_surveyor_report_by) as [caseOwnerFullName]
			from dbo.IncidentBase [Case] with (nolock)
			left outer join dbo.IncidentBase [ParentCase] with (nolock)
				on [ParentCase].IncidentId = [Case].pfc_parent_caseId
			left outer join dbo.pfc_policy_additionalBase [PolicyAddi] with (nolock) 
				on [PolicyAddi].pfc_policy_additionalId = [Case].pfc_policy_additionalId
			left outer join dbo.pfc_policybase [Policy] with (nolock) 
				on [Policy].pfc_policyId = [PolicyAddi].pfc_policyId
			where [Case].IncidentId = @IncidentId

			----left outer join dbo.AccountBase [informer_Coporate] with (nolock) 
			----	on [Case].pfc_informer_nameIdType = 1 and [Case].pfc_informer_name = [informer_Coporate].AccountId
			----left outer join dbo.ContactBase [informer_Personal] with (nolock) 
			----	on [Case].pfc_informer_nameIdType = 2 and [Case].pfc_informer_name = [informer_Personal].ContactId
			----left outer join dbo.AccountBase [driver_Coporate] with (nolock) 
			----	on [Case].pfc_driver_nameIdType = 1 and [Case].pfc_driver_name = [driver_Coporate].AccountId
			----left outer join dbo.ContactBase [driver_Personal] with (nolock) 
			----	on [Case].pfc_driver_nameIdType = 2 and [Case].pfc_driver_name = [driver_Personal].ContactId

		COMMIT TRANSACTION -- Commit transaction if the transaction is successful
    END

/*
DECLARE @ticketNo AS NVARCHAR(20) = 'CAS201702-00003';
DECLARE




DECLARE @uniqueID AS UNIQUEIDENTIFIER = 'D246086E-C1EE-E611-80D4-0050568D1874';
EXEC [dbo].[sp_CustomApp_RegClaimInfo_Incident] @uniqueID, @ticketNo


DECLARE @IncidentId AS UNIQUEIDENTIFIER = 'D246086E-C1EE-E611-80D4-0050568D1874';
DECLARE @CurrentUserId UNIQUEIDENTIFIER = '50529F88-2BE9-E611-80D4-0050568D1874';
EXEC [dbo].[sp_CustomApp_RegClaimInfo_Incident] @IncidentId, @CurrentUserId

-------------------------------------------------------------------------------------------------
DECLARE @ticketNo AS NVARCHAR(20) = 'CAS201702-00003';
DECLARE @currentDomainUser AS NVARCHAR(1024) = 'crmtest1';

DECLARE @IncidentId AS UNIQUEIDENTIFIER = (SELECT IncidentId from dbo.IncidentBase (nolock) where TicketNumber = @ticketNo);
DECLARE @CurrentUserId UNIQUEIDENTIFIER = (SELECT SystemUserId from dbo.SystemUser (nolock) where SUBSTRING(DomainName,CHARINDEX('\',DomainName)+1,1024) = @currentDomainUser);
select @IncidentId as [IncidentId], @CurrentUserId as [CurrentUserId]
EXEC [dbo].[sp_CustomApp_RegClaimInfo_Incident] @IncidentId, @CurrentUserId
*/

--select IncidentId
--from IncidentBase
--where pfc_claim_noti_number = '1704-00239'
GO
