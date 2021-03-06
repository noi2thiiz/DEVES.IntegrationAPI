USE [CRMQA_MSCRM]
GO
/****** Object:  StoredProcedure [dbo].[p_PopulateSystemUserPrincipals]    Script Date: 7/5/2560 19:34:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


create procedure [dbo].[p_PopulateSystemUserPrincipals](@id uniqueidentifier) as
begin
	-- populate teams
	insert SystemUserPrincipals (SystemUserId, PrincipalId) select @id, TeamId from TeamMembership where SystemUserId = @id
		
	-- populate business unit membership	
	insert into SystemUserPrincipals (SystemUserId, PrincipalId) select @id, BusinessUnitId from SystemUserBase where SystemUserId = @id
		
	-- populate organization membership
	insert into SystemUserPrincipals (SystemUserId, PrincipalId) select @id, OrganizationId from BusinessUnitBase where BusinessUnitId = (select BusinessUnitId from SystemUserBase where SystemUserId = @id)
		
	-- insert self	
	insert into SystemUserPrincipals (SystemUserId, PrincipalId) values(@id, @id)
end
GO
