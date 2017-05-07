USE [CRMQA_MSCRM]
GO
/****** Object:  StoredProcedure [dbo].[p_DetectSubjectLoop]    Script Date: 7/5/2560 19:34:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE procedure [dbo].[p_DetectSubjectLoop](
 @ParentId uniqueidentifier, 
 @ChildId uniqueidentifier) 
as
/*
	This proc is to be used to determine if parenting one subject to another would create a loop inside the subject hierarchy.
It also detects if a loop already exists somewhere at or above the parent subject.

Return Values : 
ErrorCode ErrorMessage
  '4'  'The ParentId passed in is not valid' as ErrorMessage.'
  '3'  'The ChildId passed in is not valid' as ErrorMessage.'
  '2'  'This association would create a loop in the database.'
  '1'  'There is already a loop existing in the database.'
*/
set nocount on
Declare @TempParentId uniqueidentifier

	

   -- make sure that the @ParentId is a valid subject id.
   IF not exists(select SubjectId from SubjectBase where SubjectId = @ParentId)
     BEGIN 
      select N'4' as ErrorCode,  N'The ParentId passed in is not valid.' as ErrorMessage
      return 
     END

   -- make sure that the @ChildId is a valid subject id.
   IF not exists(select SubjectId from SubjectBase where SubjectId = @ChildId)
     BEGIN 
      select N'3' as ErrorCode, N'The ChildId passed in is not valid.' as ErrorMessage
      return 
     END


select @TempParentId = @ParentId 

create table #Ids ( IdVisited uniqueidentifier primary key)
WHILE ( 1 = 1 )

 BEGIN
   if (@TempParentId = @ChildId)
     -- while chasing the parent of the parent (of the parent...) you bumped into the child.  this would create a loop	
     BEGIN
	select N'2' as ErrorCode, N'This association would create a loop in the database.' as ErrorMessage
	BREAK
     END

   select @TempParentId = ParentSubject from SubjectBase  where SubjectId = @TempParentId	

   IF (@TempParentId is null)
     BEGIN 
      -- This implies that there is a dead end to the parent->grand parent-> chain and hence no fear of looping.	
      select N'0' as ErrorCode, N'This association is valid and would create no loop in the database.' as ErrorMessage
      BREAK
     END
   ELSE
     BEGIN 
	if exists (select * from #Ids where @TempParentId = IdVisited)
	  BEGIN
	      -- This should never happen.  if it does, that means some one has been mucking with the DB from the backend.	
	      select N'1' as ErrorCode, N'There is already a loop existing in the database.' as ErrorMessage             
      	      BREAK
	  END
	else
	  BEGIN
		Insert into #Ids(IdVisited) values (@TempParentId)
		CONTINUE
	  END
     END
   BREAK 	
 END

GO
