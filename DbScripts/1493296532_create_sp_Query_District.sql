USE [CRMQA_MSCRM]
GO

/****** Object:  StoredProcedure [dbo].[sp_Query_District]    Script Date: 27/4/2560 19:35:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Patiwat
-- Create date: 9/4/2560 13:53:00
-- สำหรับ ใช้หาชื่ออำเภอ
-- =============================================
CREATE PROCEDURE [dbo].[sp_Query_District]
	-- Add the parameters for the stored procedure here
	@Id NVARCHAR(100) = NULL,
	@DistrictCode NVARCHAR(4) = NULL,
	@DistrictName NVARCHAR(100) = NULL,
	@DistrictNameEng NVARCHAR(100) = NULL
AS
BEGIN
    

			select  
		pfc_master_districtId as Id,
		pfc_master_district_code as DistrictCode, 
		pfc_master_district_name as DistrictName, 
		pfc_master_district_name_eng as DistrictNameEng
		from pfc_master_districtBase
		where statecode =0
		    AND ( pfc_master_district_code = @DistrictCode  OR @DistrictCode IS  NULL )
			AND ( pfc_master_districtId = Cast(@Id AS uniqueidentifier)   OR  @Id IS  NULL )
			AND ( pfc_master_district_name = @DistrictName  OR @DistrictName IS  NULL )
			AND ( pfc_master_district_name_eng = @DistrictNameEng  OR @DistrictNameEng IS  NULL )

	 

	
END


GO


