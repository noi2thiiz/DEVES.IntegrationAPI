USE [CRMQA_MSCRM]
GO
/****** Object:  StoredProcedure [dbo].[sp_API_InquiryPolicyMotorList]    Script Date: 17/4/2560 9:28:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[sp_API_InquiryPolicyMotorList]
	@policyNo NVARCHAR(100), @chassisNo NVARCHAR(100), @carRegisNo NVARCHAR(100), @carRegisProve NVARCHAR(100)
AS
BEGIN
	BEGIN TRANSACTION 
		--DECLARE @policyNo NVARCHAR(100) = '2010815184834'
		--DECLARE @chassisNo NVARCHAR(100) = ''
		--DECLARE @carRegisNo NVARCHAR(100) = N'ย0532'
		--DECLARE @carRegisProve NVARCHAR(100) = ''	
		select 
			CONVERT(NVARCHAR(40),PolDetail.pfc_policy_additionalId) as [crmPolicyDetailId]
			
			, PolDetail.pfc_policy_additional_primary_key as [crmPolicyDetailCode]
			, PolDetail.pfc_chdr_num as [policyNo]
			, PolDetail.pfc_zren_num as [renewalNo] 
			, PolDetail.pfc_rsk_num as [fleetCarNo]
			, PolDetail.pfc_bar_code as [barcode]
			, PolDetail.pfc_insurance_card as [insureCardNo]
			, PolDetail.pfc_policy_vip as [policyVip]
			, PolDetail.pfc_policy_additional_name as [policyAdditionalName]
			, null as [MCM_SEQ]
			, PolHeader.pfc_policyId as [policyId]
			, PolHeader.pfc_policy_mc_nmc as [policyMcNmc]
			, PolHeader.pfc_tran_num as [policySeqNo]
			, null as [endorseNo]
			, PolHeader.pfc_cnt_branch_code as [branchCode]
			, PolHeader.pfc_prod_type as [contractType]
			, PolDetail.pfc_cover_code as [policyProductTypeCode]
			, PolDetail.pfc_cover_name as [policyProductTypeName]
			, case when PolDetail.pfc_issue_date is null then null else DATEADD(HOUR,7,PolDetail.pfc_issue_date) end as [policyIssueDate]
			, case when PolDetail.pfc_effective_date is null then null else DATEADD(HOUR,7,PolDetail.pfc_effective_date) end as [policyEffectiveDate]
			, case when PolDetail.pfc_expiry_date is null then null else DATEADD(HOUR,7,PolDetail.pfc_expiry_date) end as [policyExpiryDate]
			, case when PolDetail.pfc_garage_flag = 1 then 'Y' else 'N' end as [policyGarageFlag]
			, case when PolDetail.pfc_receive_status = 1 then 'Y' else 'N' end as [policyPaymentStatus]
			, REPLACE(PolDetail.pfc_reg_num,'@','') as [policyCarRegisterNo]
			, PolDetail.pfc_reg_num_prov as [policyCarRegisterProv]
			, PolDetail.pfc_chassis_num as [carChassisNo]
			, PolDetail.pfc_vehicle_type as [carVehicleType]
			, PolDetail.pfc_vehicle_brand_model as [carVehicleModel]
			, PolDetail.pfc_vehicle_year as [carVehicleYear]
			, PolDetail.pfc_vehicle_body as [carVehicleBody]
			, PolDetail.pfc_vehicle_size as [carVehicleSize]
			, PolDetail.pfc_customerId as [customerId]
			, CAST(ROUND(PolDetail.pfc_deduct ,2) as decimal(23,2)) as [policyDeduct]
			, PolHeader.pfc_agent_code as [agentCode]
			, PolHeader.pfc_agent_fullname as [agentName]
			, PolHeader.pfc_agent_branch as [agentBranch]--PolHeader
			, PolHeader.pfc_cus_cleansing_id as [insuredCleansingId] --PolHeader
			, PolHeader.pfc_cus_client_number as [insuredClientId]
			
			
			, dbo.f_GetPicklistName('pfc_policy','pfc_cus_clt_type',PolHeader.pfc_cus_clt_type) as [insuredClientType]
			, PolHeader.pfc_cus_fullname as [insuredFullName]
			, dbo.f_GetPicklistName('pfc_policy','pfc_client_status__code',PolHeader.pfc_client_status__code) as [policyStatus]
			--,'|-----|' as '|',PolHeader.*,'|' as '|',PolDetail.*
		from dbo.pfc_policy_additionalBase PolDetail with (nolock)
		inner join dbo.pfc_policyBase PolHeader with (nolock) on PolHeader.pfc_policyId = PolDetail.pfc_policyId
		
		where 
		PolDetail.statecode = 0 and 
		PolDetail.pfc_source_data = 100000002 and PolHeader.pfc_policy_mc_nmc = 100000000 
		And 
			(
				(
					PolDetail.pfc_bar_code like @policyNo 
					OR PolDetail.pfc_chdr_num like @policyNo
				)
				AND (
					PolDetail.pfc_chassis_num like @chassisNo
					OR REPLACE(PolDetail.pfc_reg_num,'@','') like @carRegisNo
				)
			)
	COMMIT TRANSACTION -- Commit transaction if the transaction is successful

END

/*
DECLARE @policyNo NVARCHAR(100) = '2010034076831'
DECLARE @chassisNo NVARCHAR(100) = ''
DECLARE @carRegisNo NVARCHAR(100) = N'ตม4533'
DECLARE @carRegisProve NVARCHAR(100) = ''
EXEC [dbo].[sp_API_InquiryPolicyMotorList] @policyNo,@chassisNo,@carRegisNo,@carRegisProve

DECLARE @policyNo NVARCHAR(100) = 'C7419930'
DECLARE @chassisNo NVARCHAR(100) = 'MBPUF3JS05008'
DECLARE @carRegisNo NVARCHAR(100) = N'กอ7518'
DECLARE @carRegisProve NVARCHAR(100) = 'ชม'
EXEC [dbo].[sp_API_InquiryPolicyMotorList] @policyNo,@chassisNo,@carRegisNo,@carRegisProve


DECLARE @policyNo NVARCHAR(100) = 'C4989809'
DECLARE @chassisNo NVARCHAR(100) = ''
DECLARE @carRegisNo NVARCHAR(100) = N'ตฬ230'
DECLARE @carRegisProve NVARCHAR(100) = 'กท'
EXEC [dbo].[sp_API_InquiryPolicyMotorList] @policyNo,@chassisNo,@carRegisNo,@carRegisProve



DECLARE @clientType NVARCHAR(1) = 'P'
DECLARE @clientId NVARCHAR(100) = '14492800' 
EXEC [dbo].[sp_API_CustomerClient] @clientType,@clientId


DECLARE @clientType NVARCHAR(1) = 'P'
DECLARE @clientId NVARCHAR(100) = '12206480'
EXEC [dbo].[sp_API_CustomerClient] @clientType,@clientId
*/