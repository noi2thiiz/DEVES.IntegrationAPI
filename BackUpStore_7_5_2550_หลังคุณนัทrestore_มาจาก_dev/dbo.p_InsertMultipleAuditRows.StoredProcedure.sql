USE [CRMQA_MSCRM]
GO
/****** Object:  StoredProcedure [dbo].[p_InsertMultipleAuditRows]    Script Date: 7/5/2560 19:34:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE   PROCEDURE [dbo].[p_InsertMultipleAuditRows](@doc ntext, @transactionid uniqueidentifier, @EntityType int,
	@auditoperation tinyint, @auditaction tinyint, @audituser uniqueidentifier ,
	@callinguser uniqueidentifier, @attributemask nvarchar(max), @changedata nvarchar(max),
	@audituseradditionalinfo nvarchar(350)) as

BEGIN
DECLARE @idoc int
DECLARE @datetime DATETIME
set @datetime = dbo.fn_GetUtcDateTrunc()

CREATE TABLE #tempMemberBase(ObjectId UNIQUEIDENTIFIER)

EXEC dbo.sp_xml_preparedocument @idoc OUTPUT, @doc

SELECT @doc

--The format of the xml document @doc is like this
--	<Values>
--		<Value ObjectId="GUID1">
--		<Value ObjectId="GUID1">
--		<Value ObjectId="GUID1">
--	</Values>

--Insert all the ObjectIds into the temp table for later use in inserting into AuditBase
INSERT INTO #tempMemberBase SELECT ObjectId FROM 
OPENXML(@idoc, '/Values/Value',1)
WITH (ObjectId UNIQUEIDENTIFIER)

INSERT INTO AuditBase(TransactionId, ObjectTypeCode, Operation, Action,
	UserId, CallingUserId, CreatedOn, ObjectId, AttributeMask, ChangeData,
	UserAdditionalInfo) 
	SELECT @transactionid, @EntityType, @auditoperation, @auditaction,
	@audituser, @callinguser, @datetime, ObjectId, @attributemask, @changedata,
	@audituseradditionalinfo FROM 
	#tempMemberBase
	
DROP TABLE #tempMemberBase

END

GO
