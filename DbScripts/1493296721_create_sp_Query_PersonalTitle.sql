USE [CRMQA_MSCRM]
GO

/****** Object:  StoredProcedure [dbo].[sp_Query_PersonalTitle]    Script Date: 27/4/2560 19:38:07 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Patiwat
-- Create date: 9/4/2560 13:53:00
-- สำหรับ ใช้หาชื่ออำเภอ
-- =============================================
CREATE PROCEDURE [dbo].[sp_Query_PersonalTitle]
	-- Add the parameters for the stored procedure here
	@Id NVARCHAR(150) = NULL,
	@Code NVARCHAR(4) = NULL,
	@PolisyCode NVARCHAR(4) = NULL,
	@Name NVARCHAR(50) = NULL
AS
BEGIN
    

			SELECT [pfc_master_title_personalId] as Id
				  ,[pfc_master_title_personal_name] as [Name]
				  ,[pfc_master_title_personal_code] as Code
				  ,[pfc_ref_polisy_descpf_t3583] as PolisyCode
			  FROM [dbo].[pfc_master_title_personalBase]
		where statecode =0
		    AND ( [pfc_master_title_personalId] = Cast(@Id AS uniqueidentifier)   OR  @Id IS  NULL )
		    AND ( [pfc_master_title_personal_code]= @Code  OR @Code IS  NULL )
			AND ( [pfc_ref_polisy_descpf_t3583] = @PolisyCode  OR  @PolisyCode IS  NULL )
			AND ( [pfc_master_title_personal_name] = @Name  OR  @Name IS  NULL )
			
END


GO


