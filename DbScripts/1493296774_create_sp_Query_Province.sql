USE [CRMQA_MSCRM]
GO

/****** Object:  StoredProcedure [dbo].[sp_Query_Province]    Script Date: 27/4/2560 19:39:17 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Patiwat
-- Create date: 9/4/2560 13:53:00
-- สำหรับ ใช้หาชื่อจังหวัด 
-- =============================================
CREATE PROCEDURE [dbo].[sp_Query_Province]
	-- Add the parameters for the stored procedure here
	@Id NVARCHAR(100) = NULL,
	@Provincecode NVARCHAR(2) = NULL,
	@ProvinceName NVARCHAR(50) = NULL,
	@ProvinceNameEng NVARCHAR(50) = NULL,
	@SortIndex NVARCHAR(3) = NULL
AS
BEGIN
    

	  select 
	    pfc_master_provinceId as Id, 
		pfc_master_province_code  as ProvinceCode, 
		pfc_master_province_name as ProvinceName, 
		pfc_master_province_name_eng as ProvinceNameEng,
		pfc_sort_by as SortIndex
		from pfc_master_provinceBase
		where  statecode =0 AND
		       ( pfc_master_province_code = @Provincecode  OR @Provincecode IS  NULL )
		        AND ( pfc_master_provinceId = Cast(@Id AS uniqueidentifier)   OR  @Id IS  NULL )
				AND ( pfc_master_province_name = @ProvinceName   OR  @ProvinceName IS  NULL )
				AND ( pfc_master_province_name_eng = @ProvinceNameEng   OR  @ProvinceNameEng IS  NULL )
				AND ( pfc_sort_by = @SortIndex   OR  @SortIndex IS  NULL )

	
END


GO


