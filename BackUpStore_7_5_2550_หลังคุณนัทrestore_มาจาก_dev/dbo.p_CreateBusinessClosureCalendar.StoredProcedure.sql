USE [CRMQA_MSCRM]
GO
/****** Object:  StoredProcedure [dbo].[p_CreateBusinessClosureCalendar]    Script Date: 7/5/2560 19:34:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[p_CreateBusinessClosureCalendar](@calendarId uniqueidentifier, @organizationId uniqueidentifier, @rootBusinessUnitId uniqueidentifier, @systemUserId uniqueidentifier,@name nvarchar(160),@description nvarchar(max)) as
BEGIN

	set nocount on

	INSERT INTO CalendarBase(CalendarId, BusinessUnitId, CreatedBy, ModifiedBy, Description, Name, OrganizationId, IsShared)
	VALUES(@calendarId, @rootBusinessUnitId, @systemUserId, @systemUserId, @name,@description, @organizationId, 1)

	SELECT @calendarId

END
GO
