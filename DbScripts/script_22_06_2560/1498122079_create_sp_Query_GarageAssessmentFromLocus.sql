USE [CRMQA_MSCRM]
GO

/****** Object:  StoredProcedure [dbo].[sp_Query_GarageAssessmentFromLocus]    Script Date: 22/6/2560 16:00:35 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/****** Object:  StoredProcedure [dbo].[sp_Query_GarageAssessmentFromLocus]    Script Date: 13/6/2560 15:51:01 ******/

-- =============================================
-- Author:		Patiwat WIsedsukol
-- Create date: 12/6/2560
-- Description:	สำหรับ ส่ง assessment 
-- =============================================
CREATE PROCEDURE [dbo].[sp_Query_GarageAssessmentFromLocus]
@BACK_DAY  int= 30

AS

BEGIN
DECLARE @POLICY_PRE_CODE NVARCHAR 
SET @POLICY_PRE_CODE = 'V'; -- เลือกเฉพาะ POLICY V
DECLARE @CLIENT_STATUS_CODE int;

SET  @CLIENT_STATUS_CODE = 100000007; 

DECLARE @QUESTION_GUID NVARCHAR(200);
DECLARE @OWNER_GUID NVARCHAR(200);

DECLARE @QUESTION_TYPE INT;

SET @QUESTION_TYPE = 100000001;

SET  @OWNER_GUID = (SELECT TOP 1 [pfc_configure_value]
  FROM [CRMQA_MSCRM].[dbo].[pfc_configureBase]
  WHERE pfc_configure_type = 100000001 AND statecode = 0 AND [pfc_configure_key] = 'ASSESSMENT_OWNER_GUID')
  
DECLARE @ASSESSMENT_SURVEY_SMS_OPTION int;

SET  @ASSESSMENT_SURVEY_SMS_OPTION = (SELECT TOP 1 [pfc_configure_value]
  FROM [CRMQA_MSCRM].[dbo].[pfc_configureBase]
  WHERE pfc_configure_type = 100000001 AND statecode = 0 AND [pfc_configure_key] = 'ASSESSMENT_SURVEY_SMS_OPTION')
  


SET  @QUESTION_GUID = ( SELECT TOP 1 [pfc_questionnairId] 
  FROM [pfc_questionnairBase]
  where [pfc_questionnaire_type] = @QUESTION_TYPE and statecode = 0 
  and [pfc_questionnaire_active_date] <= GETDATE() 
  order by [pfc_questionnaire_active_date]  desc);


DELETE [CRM_CUSTOM_APP].[dbo].[LOCUS_QUOTATION_REPAIRER];


INSERT INTO [CRM_CUSTOM_APP].[dbo].[LOCUS_QUOTATION_REPAIRER]
           ([เลขที่ใบเสนอราคา]
           ,[เลขรับแจ้ง]
           ,[ช่องทางนำเข้า]
           ,[สถานะใบเสนอราคา]
           ,[ชื่ออู่/ศูนย์]
           ,[รหัสอู่/ศูนย์]
           ,[ผู้ทำรายการล่าสุด]
           ,[ทะเบียนรถ]
           ,[ประเภทรถ ประกัน/คู่กรณี]
           ,[ค่าแรงอู่เสนอ]
           ,[ค่าแรงอนุมัติ]
           ,[ค่าอะไหล่อู่เสนอ]
           ,[ค่าอะไหล่อนุมัติ]
           ,[ส่วนลด]
           ,[ค่า Deduct]
           ,[ประเภทค่า Deduct]
           ,[ค่าใช้จ่ายอื่นๆ]
           ,[วันที่เข้ามาติดต่อ]
           ,[วันที่อู่เสนอราคามาที่ประกัน]
           ,[วันที่ขออนุมัติ]
           ,[วันที่อนุมัติ]
           ,[วันที่นำรถเข้าซ่อม]
           ,[จำนวนวันที่คาดว่าจะซ่อมเสร็จ]
           ,[วันที่คาดว่าจะซ่อมเสร็จ]
           ,[วันที่นำรถกลับ]
           ,[วันที่เสร็จสมบูรณ์],
		    APPRORED_DATETIME,
			APPRORED_DATEDIFF
		   )

     SELECT
	   [เลขที่ใบเสนอราคา]
      ,[เลขรับแจ้ง]
      ,[ช่องทางนำเข้า]
      ,[สถานะใบเสนอราคา]
      ,[ชื่ออู่/ศูนย์]
      ,[รหัสอู่/ศูนย์]
      ,[ผู้ทำรายการล่าสุด]
      ,[ทะเบียนรถ]
      ,[ประเภทรถ ประกัน/คู่กรณี]
      ,[ค่าแรงอู่เสนอ]
      ,[ค่าแรงอนุมัติ]
      ,[ค่าอะไหล่อู่เสนอ]
      ,[ค่าอะไหล่อนุมัติ]
      ,[ส่วนลด]
      ,[ค่า Deduct]
      ,[ประเภทค่า Deduct]
      ,[ค่าใช้จ่ายอื่นๆ]
      ,[วันที่เข้ามาติดต่อ]
      ,[วันที่อู่เสนอราคามาที่ประกัน]
      ,[วันที่ขออนุมัติ]
      ,[วันที่อนุมัติ]
      ,[วันที่นำรถเข้าซ่อม]
      ,[จำนวนวันที่คาดว่าจะซ่อมเสร็จ]
      ,[วันที่คาดว่าจะซ่อมเสร็จ]
      ,[วันที่นำรถกลับ]
      ,[วันที่เสร็จสมบูรณ์]
	  ,DATEADD(year,-543,CAST( [วันที่อนุมัติ] AS DATETIME))
	  ,DATEDIFF(day,DATEADD(year,-543,CAST( [วันที่อนุมัติ] AS DATETIME)) ,GETDATE() )
	 
   
  FROM [DEVES_CLAIM_SERVER_PRO].[DEVES_CLAIM_DB].[dbo].[VW_QUOTATION_REPAIRER]
  WHERE ISDATE([วันที่อนุมัติ]) =1 
        AND [ประเภทรถ ประกัน/คู่กรณี] = 0 
        AND DATEDIFF(day,DATEADD(year,-543,CAST( [วันที่อนุมัติ] AS DATETIME)) ,GETDATE() ) >= @BACK_DAY
		AND [เลขรับแจ้ง] NOT IN (
		                              SELECT pfc_claim_noti_number FROM pfc_assessment 
							          WHERE pfc_assessment_type = 100000001 AND statecode=0
							  );



SELECT 
     CAST(IncidentBase.IncidentId as nvarchar(150) )AS Id,
	IncidentBase.pfc_policy_number AS PolicyNumber,
	 CAST(IncidentBase.[pfc_driver_name] as nvarchar(150) )AS DriverGuid,
	IncidentBase.pfc_driver_fullname AS DriverFullname,
	IncidentBase.[pfc_driver_mobile] AS DriverMobile,
	QUOTATION.[ทะเบียนรถ],
	IncidentBase.pfc_current_reg_num AS CurrentRegNum,
	IncidentBase.pfc_current_reg_num_prov AS CurrentRegNumProv,
	IncidentBase.pfc_customer_client_number AS CustomerClientNumber,
	IncidentBase.CustomerIdName, 
	IncidentBase.pfc_assessment_ref_code AS AssessmentRefCode,
	IncidentBase.ticketnumber as  TicketNumber,
	IncidentBase.pfc_claim_noti_number as  ClaimNotiNumber,
	--CLAIM.CLAIM.APPRORED_DATETIME AS ApproredDateTime,
	 CAST(QUOTATION.[ชื่ออู่/ศูนย์] as nvarchar )as [AssesseeName],
	CAST(QUOTATION.[รหัสอู่/ศูนย์] as nvarchar )as [AssesseeCode],
	
	POLICY_INFO.*,
	DATEDIFF(day,QUOTATION.APPRORED_DATETIME ,GETDATE() ) AS DateDiffFromCurrentDate,
	@QUESTION_GUID AS QuestionGuid,
	@QUESTION_TYPE AS QuestionType,
	@OWNER_GUID As [AssessmentOwnerGuid],
	@ASSESSMENT_SURVEY_SMS_OPTION As [SurveySmsOption]

	
	FROM
	(
	   SELECT 
		   [ชื่ออู่/ศูนย์]
		  ,[รหัสอู่/ศูนย์]
		  ,[เลขที่ใบเสนอราคา]
		  ,[เลขรับแจ้ง]
		  ,[ช่องทางนำเข้า]
		  ,[สถานะใบเสนอราคา]
		  ,[ทะเบียนรถ]
		  ,[ประเภทรถ ประกัน/คู่กรณี]
		  ,[วันที่เข้ามาติดต่อ]
		  ,[วันที่ขออนุมัติ]
		  ,[วันที่อนุมัติ]
		  ,CLAIM.APPRORED_DATETIME
		
		FROM [CRM_CUSTOM_APP].[dbo].[LOCUS_QUOTATION_REPAIRER] AS CLAIM with (nolock)
	    WHERE 
			ISDATE([วันที่อนุมัติ]) = 1 
			AND DATEDIFF(day,CLAIM.APPRORED_DATETIME ,GETDATE() ) >= @BACK_DAY
			AND [ประเภทรถ ประกัน/คู่กรณี] =  0
	) QUOTATION
	JOIN IncidentBase with (nolock) ON (IncidentBase.pfc_claim_noti_number = QUOTATION.[เลขรับแจ้ง]  
  )

  JOIN  (
      select 
			 PolDetail.pfc_chdr_num as [policyNo]
			,PolDetail.pfc_cover_code
			,PolDetail.pfc_cover_name
			,PolDetail.pfc_policy_status
			,PolHeader.pfc_client_status__code

		from dbo.pfc_policy_additionalBase PolDetail with (nolock)
		inner join dbo.pfc_policyBase PolHeader with (nolock) on PolHeader.pfc_policyId = PolDetail.pfc_policyId
  )  POLICY_INFO

  ON (POLICY_INFO.[policyNo]= IncidentBase.pfc_policy_number )
  where   pfc_cover_code IN ('CD','CD','FC','FF','FE','FI','TC','TF','TE','TI')
     and  SUBSTRING(pfc_policy_number,1,1)=@POLICY_PRE_CODE-- เอาเฉพาะ policy V
	 and  pfc_client_status__code =@CLIENT_STATUS_CODE  -- 100000007 = IF

END
-- EXEC [dbo].[sp_Query_GarageAssessmentFromLocus]
/*
Update IncidentBase
SET pfc_assessment_ref_code = 'xdergftred'
where IncidentId = '0C9F045F-4D36-E711-80D9-0050568D615F'

Update IncidentBase
SET pfc_assessment_ref_code = 'xdergftrsw'
where IncidentId = '7860D739-F736-E711-80D9-0050568D615F'
*/
GO


