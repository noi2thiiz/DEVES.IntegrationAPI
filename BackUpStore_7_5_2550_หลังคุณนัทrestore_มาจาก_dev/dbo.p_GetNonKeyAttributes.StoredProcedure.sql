USE [CRMQA_MSCRM]
GO
/****** Object:  StoredProcedure [dbo].[p_GetNonKeyAttributes]    Script Date: 7/5/2560 19:34:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


create procedure [dbo].[p_GetNonKeyAttributes](@entityid uniqueidentifier) as
set nocount on

select a.*,
	'character-count' = case 
		when cast(at.Description as nvarchar) in ('nvarchar', 'nchar') then cast(a.Length / 2 as nvarchar) 
		when cast(at.Description as nvarchar) in ('varchar', 'char') then cast(a.Length as nvarchar) 
		else '' 
		end,
    'type' = at.Description,
    'isquoted' = at.Quoted
from AttributeView a join AttributeTypes at on (a.AttributeTypeId = at.AttributeTypeId)
where EntityId = @entityid
  and a.IsPKAttribute = 0
order by ColumnNumber

GO
