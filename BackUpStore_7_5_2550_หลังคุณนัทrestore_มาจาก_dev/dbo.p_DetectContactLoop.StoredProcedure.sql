USE [CRMQA_MSCRM]
GO
/****** Object:  StoredProcedure [dbo].[p_DetectContactLoop]    Script Date: 7/5/2560 19:34:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE procedure [dbo].[p_DetectContactLoop](
 @ParentId uniqueidentifier, 
 @ChildId uniqueidentifier) 
as
/*
	This proc is to be used to determine if parenting one contact to another would create a loop inside the contact hierarchy.
It also detects if a loop already exists somewhere at or above the parent contact.

Return Values : 
ErrorCode ErrorMessage
  '4'  'The ParentId passed in is not valid' as ErrorMessage.'
  '3'  'The ChildId passed in is not valid' as ErrorMessage.'
  '2'  'This association would create a loop in the database.'
  '1'  'There is already a loop existing in the database.'
*/
set nocount on
Declare @TempParentId uniqueidentifier
Declare @TempParentIdType int
SET @TempParentIdType = NULL	

   -- make sure that the @ParentId is a valid contact id.
   IF not exists(select ContactId from ContactBase where ContactId = @ParentId)
     BEGIN 
      select N'4' as ErrorCode,  N'The ParentId passed in is not valid.' as ErrorMessage
      return 
     END

   -- make sure that the @ChildId is a valid account id.
   IF not exists(select ContactId from ContactBase where ContactId = @ChildId)
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

   select @TempParentId = ParentCustomerId, @TempParentIdType = ParentCustomerIdType from ContactBase  where ContactId = @TempParentId

   IF (@TempParentId is null OR @TempParentIdType is null or @TempParentIdType != 2 ) -- 2 means contact
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
