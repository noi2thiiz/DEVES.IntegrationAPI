USE [CRMQA_MSCRM]
GO
/****** Object:  StoredProcedure [dbo].[sp_CustomApp_GetCrmClientid_byCleansingId]    Script Date: 7/5/2560 19:34:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE	proc [dbo].[sp_CustomApp_GetCrmClientid_byCleansingId]
@clientType nvarchar(1),
@cleansingId nvarchar(20)
AS
BEGIN
	IF @clientType = 'P'
		select	pfc_crm_person_id as CrmClientId, FullName
		from	ContactBase (nolock)
		where	pfc_cleansing_cusormer_profile_code = @cleansingId
			and StateCode = 0
	Else IF @clientType = 'C'
		select  AccountNumber as CrmClientId, Name
		from	accountbase (nolock)
		where	pfc_cleansing_cusormer_profile_code = @cleansingId
			and StateCode = 0
END
GO
