USE [CRMQA_MSCRM]
GO

/****** Object:  StoredProcedure [dbo].[sp_Query_AddressType]    Script Date: 27/4/2560 19:31:13 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Patiwat
-- Create date: 9/4/2560 13:53:00
-- สำหรับ Address Type
-- =============================================
CREATE PROCEDURE [dbo].[sp_Query_AddressType]
	-- Add the parameters for the stored procedure here
	@Id NVARCHAR(150) = NULL,
	@Code NVARCHAR(3) = NULL,
	@PolisyCode NVARCHAR(3) = NULL,
	@Name NVARCHAR(100) = NULL,
	@NameEng NVARCHAR(100) = NULL
AS
BEGIN
    

			SELECT [pfc_master_address_typeId] as Id
				  ,[pfc_master_address_type_name] as [Name]
				  ,[pfc_master_address_type_code] as Code
				  ,[pfc_master_address_type_name_eng] as NameEng
				  ,[pfc_ref_polisy_descpf_t3690] as PolisyCode
			  FROM [dbo].[pfc_master_address_typeBase]
		where statecode =0
		    AND ( [pfc_master_address_typeId] = Cast(@Id AS uniqueidentifier)   OR  @Id IS  NULL )
		    AND ( [pfc_master_address_type_code]= @Code  OR @Code IS  NULL )
			AND ( [pfc_ref_polisy_descpf_t3690] = @PolisyCode  OR  @PolisyCode IS  NULL )
			AND ( [pfc_master_address_type_name] = @Name  OR  @Name IS  NULL )
			AND ( [pfc_master_address_type_name_eng] = @NameEng  OR  @NameEng IS  NULL )
			
END


GO


