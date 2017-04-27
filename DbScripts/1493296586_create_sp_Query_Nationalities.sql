USE [CRMQA_MSCRM]
GO

/****** Object:  StoredProcedure [dbo].[sp_Query_Nationalities]    Script Date: 27/4/2560 19:36:00 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Patiwat
-- Create date: 9/4/2560 13:53:00
-- สำหรับ ใช้หาชื่ออำเภอ
-- =============================================
CREATE PROCEDURE [dbo].[sp_Query_Nationalities]
	-- Add the parameters for the stored procedure here
	@Id NVARCHAR(150) = NULL,
	@Code NVARCHAR(4) = NULL,
	@PolisyCode NVARCHAR(4) = NULL,
	@Name NVARCHAR(100) = NULL
AS
BEGIN
    

			SELECT [pfc_master_nationalityId] as Id
		  ,[pfc_master_nationality_name] as [Name]
		  ,[pfc_master_nationality_code] as [Code]
		  ,[pfc_ref_polisy_descpf_t3645] as [PolisyCode]
	  FROM [dbo].[pfc_master_nationalityBase]
		where statecode =0
		    AND ( [pfc_master_nationalityId] = Cast(@Id AS uniqueidentifier)   OR  @Id IS  NULL )
		    AND ( [pfc_master_nationality_code]= @Code  OR @Code IS  NULL )
			AND ( [pfc_ref_polisy_descpf_t3645] = @PolisyCode  OR  @PolisyCode IS  NULL )
			AND ( [pfc_master_nationality_name] = @Name  OR  @Name IS  NULL )
			
END


GO


