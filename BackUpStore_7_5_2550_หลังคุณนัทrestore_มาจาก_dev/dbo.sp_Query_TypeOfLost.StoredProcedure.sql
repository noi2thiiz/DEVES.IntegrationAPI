USE [CRMQA_MSCRM]
GO
/****** Object:  StoredProcedure [dbo].[sp_Query_TypeOfLost]    Script Date: 7/5/2560 19:34:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Patiwat
-- Create date: 24/4/2560 13:53:00
-- สำหรับ RVP
-- =============================================
CREATE PROCEDURE [dbo].[sp_Query_TypeOfLost]
	-- Add the parameters for the stored procedure here
	@Id NVARCHAR(150) = NULL,
	@Code NVARCHAR(3) = NULL,
	@AccidentCode NVARCHAR(3) = NULL
AS
BEGIN
    

			SELECT  [pfc_motor_type_of_lossId] as Id
			       ,[pfc_motor_type_of_loss_name] as [Name]
				   ,[pfc_motor_type_of_lost_code] as Code
			       ,[pfc_motor_ref_polisy_accident_code] as AccidentCode
		    FROM    [dbo].[pfc_motor_type_of_lossBase]
		where statecode =0
		    AND ( [pfc_motor_type_of_lossId] = Cast(@Id AS uniqueidentifier)   OR  @Id IS  NULL )
		    AND ( [pfc_motor_type_of_lost_code]= @Code  OR @Code IS  NULL )
			AND ( [pfc_motor_ref_polisy_accident_code] = @AccidentCode  OR  @AccidentCode IS  NULL )
			
END




GO
