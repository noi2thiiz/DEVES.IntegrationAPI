USE [CRMQA_MSCRM]
GO

/****** Object:  StoredProcedure [dbo].[sp_Query_Occupation]    Script Date: 27/4/2560 19:37:10 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Patiwat
-- Create date: 9/4/2560 13:53:00
-- สำหรับ ใช้หาชื่ออำเภอ
-- =============================================
CREATE PROCEDURE [dbo].[sp_Query_Occupation]
	-- Add the parameters for the stored procedure here
	@Id NVARCHAR(150) = NULL,
	@Code NVARCHAR(5) = NULL,
	@PolisyCode NVARCHAR(3) = NULL,
	@Name NVARCHAR(3) = NULL
AS
BEGIN
    

			SELECT  [pfc_master_occupationId] as Id
			       ,[pfc_master_occupation_name] as [Name]
				   ,[pfc_master_occupation_code] as Code
			       ,[pfc_ref_polisy_descpf_t3644] as Polisycode
		    FROM    [dbo].[pfc_master_occupationBase]
		where statecode =0
		    AND ( [pfc_master_occupationId] = Cast(@Id AS uniqueidentifier)   OR  @Id IS  NULL )
		    AND ( [pfc_master_occupation_code]= @Code  OR @Code IS  NULL )
			AND ( [pfc_ref_polisy_descpf_t3644] = @PolisyCode  OR  @PolisyCode IS  NULL )
			AND ( [pfc_master_occupation_name] = @Name  OR  @Name IS  NULL )
			
END


GO


