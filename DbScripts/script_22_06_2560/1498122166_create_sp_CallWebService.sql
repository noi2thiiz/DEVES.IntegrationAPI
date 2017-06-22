USE [CRM_CUSTOM_APP]
GO

/****** Object:  StoredProcedure [dbo].[CallWebService]    Script Date: 22/6/2560 16:02:00 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[CallWebService]
    @ServiceEnpoint nvarchar(200) =  'http://192.168.8.121/XrmAPI/api/CreateAssessmentFromLocus'
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	/*
	SET NOCOUNT ON;

	sp_configure 'show advanced options', 1;  
	GO  
	RECONFIGURE;  
	GO  
	sp_configure 'Ole Automation Procedures', 1;  
	GO  
	RECONFIGURE;  
	GO 
	*/

	Declare @Object as Int;
    Declare @ResponseText as Varchar(8000);

    Exec sp_OACreate 'MSXML2.XMLHTTP', @Object OUT;
	
    Exec sp_OAMethod @Object, 'open', NULL, 'get', @ServiceEnpoint,'false'
    
	Exec sp_OAMethod @Object, 'send'
    Exec sp_OAMethod @Object, 'responseText', @ResponseText OUTPUT
    
	Select @ResponseText
    Exec sp_OADestroy @Object
	

END
GO


