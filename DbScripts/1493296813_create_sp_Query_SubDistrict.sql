USE [CRMQA_MSCRM]
GO

/****** Object:  StoredProcedure [dbo].[sp_Query_SubDistrict]    Script Date: 27/4/2560 19:39:56 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Patiwat
-- Create date: 19/4/2560 13:53:00
-- สำหรับ sub_distric
-- =============================================
CREATE PROCEDURE [dbo].[sp_Query_SubDistrict]
	-- Add the parameters for the stored procedure here
	@Id NVARCHAR(150) = NULL,
	@SubDistrictCode NVARCHAR(6) = NULL,
	@DistrictCode NVARCHAR(4) = NULL,
	@ProvinceCode NVARCHAR(2) = NULL,
	@PostalCode NVARCHAR(5) = NULL
AS
BEGIN
    

			SELECT [pfc_master_sub_districtId] as Id
			  ,[pfc_master_sub_district_name] as [SubDistrictName]
			  ,[pfc_master_sub_district_code] as SubDistrictCode
			  ,[pfc_ref_district_code] as  [DistrictCode]
			  ,[pfc_ref_province_code] as  [ProvinceCode]
			  ,[pfc_postal_code] as [PostalCode]
		  FROM [dbo].[pfc_master_sub_districtBase]
		where statecode =0
		    AND ( [pfc_master_sub_districtId] = Cast(@Id AS uniqueidentifier)   OR  @Id IS  NULL )
		    AND ( [pfc_master_sub_district_code]= @SubDistrictCode  OR @SubDistrictCode IS  NULL )
			AND ( [pfc_ref_district_code] = @DistrictCode  OR  @DistrictCode IS  NULL )
			AND ( [pfc_ref_province_code] = @ProvinceCode  OR  @ProvinceCode IS  NULL )
			AND ( [pfc_postal_code] = @PostalCode  OR  @PostalCode IS  NULL )
			
END


GO


