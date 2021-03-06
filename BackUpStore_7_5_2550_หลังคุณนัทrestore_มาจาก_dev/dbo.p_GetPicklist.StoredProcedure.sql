USE [CRMQA_MSCRM]
GO
/****** Object:  StoredProcedure [dbo].[p_GetPicklist]    Script Date: 7/5/2560 19:34:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE procedure [dbo].[p_GetPicklist](
	@ObjectTypeCode int,
	@AttributeName nvarchar(100),
	@LangId int,
	@OrganizationId uniqueidentifier
) 
as
begin

set nocount on

select
	AttributeValue as Code,
	Value
from
	StringMap as Entry
where
	ObjectTypeCode = @ObjectTypeCode and
	AttributeName = @AttributeName and
	LangId = @LangId and
	OrganizationId = @OrganizationId
for
	xml auto, elements

end
GO
