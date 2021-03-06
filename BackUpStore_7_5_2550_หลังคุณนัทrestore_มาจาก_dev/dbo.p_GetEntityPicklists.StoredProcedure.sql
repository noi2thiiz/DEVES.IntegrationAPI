USE [CRMQA_MSCRM]
GO
/****** Object:  StoredProcedure [dbo].[p_GetEntityPicklists]    Script Date: 7/5/2560 19:34:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


create procedure [dbo].[p_GetEntityPicklists](@otCode int, @langId int) as

set nocount on 

select
	AttributeName,
	AttributeValue,
	Value 
from 
	StringMap
where
	ObjectTypeCode = @otCode and
	LangId = @langId
order by
	AttributeName,
	AttributeValue

GO
