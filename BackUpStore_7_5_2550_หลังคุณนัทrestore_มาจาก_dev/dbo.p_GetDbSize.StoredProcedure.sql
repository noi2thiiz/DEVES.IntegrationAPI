USE [CRMQA_MSCRM]
GO
/****** Object:  StoredProcedure [dbo].[p_GetDbSize]    Script Date: 7/5/2560 19:34:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[p_GetDbSize] as
BEGIN

	set nocount on
	
	declare @pageCount bigint
	SELECT @pageCount = SUM(size) FROM sysfiles

	declare @pageSize int
	SELECT @pageSize = convert(nvarchar(11),low) FROM master.dbo.spt_values where type = N'E' and number = 1

	SELECT (@pageCount * @pageSize) / (1024 * 1024) AS DbSize
END
GO
