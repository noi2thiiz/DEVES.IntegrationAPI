USE [CRMQA_MSCRM]
GO
/****** Object:  StoredProcedure [dbo].[sp_Query_Countries]    Script Date: 7/5/2560 19:34:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Patiwat
-- Create date: 9/4/2560 13:53:00
-- สำหรับ ใช้หาชื่ออำเภอ
-- =============================================
CREATE PROCEDURE [dbo].[sp_Query_Countries]
	-- Add the parameters for the stored procedure here
	@Id NVARCHAR(150) = NULL,
	@Code NVARCHAR(5) = NULL,
	@PolisyCode NVARCHAR(3) = NULL,
    @SapCode NVARCHAR(2) = NULL
AS
BEGIN
    

			SELECT  [pfc_master_countriesId] as Id
				   ,[pfc_master_countries_name] as CountryName
				   ,[pfc_master_countries_code] as CountryCode
				   ,[pfc_ref_polisy_descpf_t3645] as PolisyCode
				   ,[pfc_ref_sap]  as SapCode
			  FROM  [dbo].[pfc_master_countriesBase]
		where statecode =0
		    AND ( [pfc_master_countriesId] = Cast(@Id AS uniqueidentifier)   OR  @Id IS  NULL )
		    AND ( [pfc_master_countries_code]= @Code  OR @Code IS  NULL )
			AND ( [pfc_ref_polisy_descpf_t3645] = @PolisyCode  OR  @PolisyCode IS  NULL )
			AND ( [pfc_ref_sap] = @SapCode  OR  @SapCode IS  NULL )
			
END

GO
