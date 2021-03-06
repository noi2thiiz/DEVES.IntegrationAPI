USE [CRMQA_MSCRM]
GO
/****** Object:  StoredProcedure [dbo].[sp_Query_Policy]    Script Date: 7/5/2560 19:34:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Tanachai>
-- Create date: <5/5/2017,,>
-- Description:	<Query Policy,,>
-- =============================================
--CREATE PROCEDURE [dbo].[sp_Query_Policy]
CREATE PROCEDURE [dbo].[sp_Query_Policy]
	-- Add the parameters for the stored procedure here
	@policyno NVARCHAR(10),
	@chassisno NVARCHAR(20),
	@plateno NVARCHAR(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select ph.pfc_cus_fullname as [insuredFullName]--[ชื่อผู้เอาประกัน]
	,(pfc_reg_num+''+pfc_reg_num_prov) as [policyCarRegisterNo]--[ทะเบียนรถ]
	,ph.pfc_policy_number as [policyNo]--[เลขที่กรมธรรม์]
	, convert(nvarchar(10),dbo.f_ConvertcrmDateTime('get',ph.pfc_effective_date) ,103) as [policyEffectiveDate]--[เรื่ม]
	, convert(nvarchar(10),dbo.f_ConvertcrmDateTime('get',ph.pfc_expiry_date) ,103) as [policyExpiryDate]--[สิ้นสุด]
	,	case when pd.pfc_policy_mc_nmc = 100000000
			then 
				(case when pd.pfc_cover_code = 'TP' then pd.pfc_cover_name
				else pd.pfc_cover_name +' ['+ dbo.f_GetPicklistName('pfc_policy_additional','pfc_garage_flag',pfc_garage_flag)+']' end)
			when pd.pfc_policy_mc_nmc = 100000001
			then 
				(select tmp.pfc_product_type_name from dbo.pfc_product_typeBase tmp (nolock) where pfc_product_typeId = ph.pfc_product_typeId)
			end	
				 as [policyContractType]--[ประเภทกรมธรรม์]
	, case when pd.pfc_policy_mc_nmc = 100000000 then ph.pfc_polisy_campaign_desc2
		when pd.pfc_policy_mc_nmc = 100000001 then ph.pfc_polisy_campaign_desc1
		end  as [Campaign]
	, '(' + ph.pfc_handle_dept_no +')'+ ph.pfc_handle_dept_name as [handleDept]--[ฝ่ายที่ดูแล]	
	, ph.pfc_handle_fullname as [handleFullname]--[เจ้าหน้าที่ที่ดูแล]
	, ph.pfc_source_name + ' ('+ ph.pfc_source_no+')' as [Source]
	, ph.pfc_agent_fullname + '/'+ ph.pfc_agent_code as [Agent]
	, convert(nvarchar(10),dbo.f_ConvertcrmDateTime('get',AddOnPh.pfc_effective_date) ,103)
		+ ' - ' + convert(nvarchar(10),dbo.f_ConvertcrmDateTime('get',AddOnPh.pfc_expiry_date) ,103) as [motorAddOn]--[Motor Add On]
	, dbo.f_GetPicklistName('pfc_policy','pfc_client_status__code',ph.pfc_client_status__code) as [Status]
	,'|' as '|'
	, null as [เทเวศ One Plus ..API]
	, ph.pfc_policy_remark as [หมายเหตุ ..API]
	, null as [หมายเหตุ General Page API]
	, dbo.f_GetPicklistName('pfc_policy','pfc_receive_status',ph.pfc_receive_status) as [การชำระเบี้ย ..API]
	,'|' as '|**[System Zone]**|'
	, pd.pfc_policy_status
	, ph.pfc_chdr_num
	, ph.pfc_zren_num
	, pd.pfc_tran_num
	, pd.pfc_rsk_num
	, pd.pfc_car_check_code
	, pd.pfc_insurance_card
from dbo.pfc_policy_additionalBase pd (nolock)
inner join dbo.pfc_policyBase ph (nolock) on ph.pfc_policyId = pd.pfc_policyId
left join dbo.pfc_policyBase AddOnPh (nolock) on AddOnPh.pfc_policyId = ph.pfc_policy_add_onId
where pd.statecode= 0

order by ph.pfc_chdr_num
END

GO
