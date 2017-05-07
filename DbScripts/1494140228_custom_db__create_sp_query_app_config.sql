USE [CRM_CUSTOM_APP]
GO

/****** Object:  StoredProcedure [dbo].[sp_Query_AppConfig]    Script Date: 7/5/2560 13:36:28 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Query_AppConfig]
	-- Add the parameters for the stored procedure here
     @Environment nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT [Key],
	    CASE WHEN @Environment ='LOCAL'  THEN ISNULL([LOCAL],[Value] )
		     WHEN @Environment ='QA'  THEN ISNULL([QA] ,[Value] )
		     WHEN @Environment ='DEV'  THEN ISNULL([DEV] ,[Value] )
		     WHEN @Environment ='PRODUCTION'  THEN ISNULL([PRODUCTION] ,[Value] )
		    ELSE [Value] END As [Value],
			[Type]
	from AppConfig where Enveronment = @Environment

	SELECT 0 as Offset,0 as Limit ,count(*) as Total	
	 from AppConfig where Enveronment = @Environment
END

GO


