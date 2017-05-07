USE [CRMQA_MSCRM]
GO
/****** Object:  StoredProcedure [dbo].[p_GetAddedRoleCount]    Script Date: 7/5/2560 19:34:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[p_GetAddedRoleCount] as
BEGIN

	set nocount on
	
	SELECT COUNT(*) FROM Role 
		WHERE RoleTemplateId IS NULL 
			OR RoleTemplateId NOT IN (SELECT RoleTemplateId From RoleTemplate)

END
GO
