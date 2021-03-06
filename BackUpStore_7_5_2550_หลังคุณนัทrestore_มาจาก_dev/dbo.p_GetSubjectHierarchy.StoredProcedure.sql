USE [CRMQA_MSCRM]
GO
/****** Object:  StoredProcedure [dbo].[p_GetSubjectHierarchy]    Script Date: 7/5/2560 19:34:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


/*
This proc is to be used to get all subject ids lie under given SubjectId.
*/
CREATE procedure [dbo].[p_GetSubjectHierarchy](
 @SubjectId uniqueidentifier) 
as
begin
set nocount on

create table #SubjectIds (SubjectId uniqueidentifier, nLevel int)
insert into #SubjectIds (SubjectId, nLevel) values (@SubjectId, 0)

declare @nLevel int
select @nLevel = 0

while (@@ROWCOUNT > 0) 
begin
	select @nLevel = @nLevel + 1

	insert into #SubjectIds (SubjectId, nLevel)
	select subject.SubjectId, @nLevel 
	from SubjectBase subject
		join #SubjectIds res on subject.ParentSubject = res.SubjectId
	where res.nLevel = @nLevel - 1
end

select distinct SubjectId as 'subjectid' from #SubjectIds
end
GO
