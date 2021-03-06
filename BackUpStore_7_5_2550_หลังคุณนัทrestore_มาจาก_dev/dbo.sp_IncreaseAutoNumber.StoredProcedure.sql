USE [CRMQA_MSCRM]
GO
/****** Object:  StoredProcedure [dbo].[sp_IncreaseAutoNumber]    Script Date: 7/5/2560 19:34:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_IncreaseAutoNumber]
	@Domain nvarchar(50) ,
    @entityCode VARCHAR(15) ,
    @resultString VARCHAR(50) OUTPUT
AS 
    BEGIN
        DECLARE @currentDate AS VARCHAR(8) ;
        DECLARE @currentNumber AS VARCHAR(8) ;
        DECLARE @number AS INT ;	
        BEGIN TRANSACTION 
        
        -- Get current date --
        SET @currentDate = ( 
				case
					when UPPER(@Domain) = UPPER('customer') then CONVERT(INT, CONVERT(VARCHAR(6), GETDATE(), 112))	
					when UPPER(@Domain) = UPPER('ClaimNotiNo_MC') then CONVERT(VARCHAR(8), CONVERT(INT, CONVERT(VARCHAR(6), GETDATE(), 112)- 200000))
					when UPPER(@Domain) = UPPER('CaseNo') then CONVERT(INT, CONVERT(VARCHAR(6), GETDATE(), 112))				
				end );
        
        -- Get current number from database and convert it to string --
        IF UPPER(@Domain) = UPPER('customer')
			BEGIN
				UPDATE  A SET     A.RunningNumber = ISNULL(RunningNumber, 1) + 1 , --SELECT
						@number = ISNULL(RunningNumber, 1)
				FROM    _Tb_Utils_RunningNo_Customer A WITH ( UPDLOCK )
				WHERE   UPPER(A.EntityCode) = UPPER(@entityCode) AND A.DateString = @currentDate AND UPPER(A.Domain) = UPPER(@Domain)    
			END
        ELSE IF UPPER(@Domain) = UPPER('ClaimNotiNo_MC')
			BEGIN
				UPDATE  A SET     A.RunningNumber = ISNULL(RunningNumber, 1) + 1 , --SELECT
						@number = ISNULL(RunningNumber, 1)
				FROM    _Tb_Utils_RunningNo_ClaimNotiNo A WITH ( UPDLOCK )
				WHERE   UPPER(A.EntityCode) = UPPER(@entityCode) AND A.DateString = @currentDate AND UPPER(A.Domain) = UPPER(@Domain)    
			END
        ELSE IF UPPER(@Domain) = UPPER('CaseNo')
			BEGIN
				UPDATE  A SET     A.RunningNumber = ISNULL(RunningNumber, 1) + 1 , --SELECT
						@number = ISNULL(RunningNumber, 1)
				FROM    _Tb_Utils_RunningNo_CaseNo A WITH ( UPDLOCK )
				WHERE   UPPER(A.EntityCode) = UPPER(@entityCode) AND A.DateString = @currentDate AND UPPER(A.Domain) = UPPER(@Domain)    
			END
			
        -- Convert the current number --	
        SET @currentNumber = ( case
					when UPPER(@Domain) = UPPER('customer')  then dbo.f_ConvertNumberToRunningNo(7, @number)
					when UPPER(@Domain) = UPPER('ClaimNotiNo_MC')  then dbo.f_ConvertNumberToRunningNo(5, @number)
					when UPPER(@Domain) = UPPER('CaseNo')  then dbo.f_ConvertNumberToRunningNo(5, @number)
				end );
	
		-- Assemble the numbering code --
        SET @resultString = ( case 
					when UPPER(@Domain) = UPPER('customer') then @currentDate + '-' + @currentNumber 
					when UPPER(@Domain) = UPPER('ClaimNotiNo_MC') then @currentDate + '-' + @currentNumber 
					when UPPER(@Domain) = UPPER('CaseNo') then @EntityCode + @currentDate + '-' + @currentNumber 
				end) ;    
            		
        COMMIT TRANSACTION -- Commit transaction if the transaction is successful
    END
/*
*****[How To Use]*****
use DEVCRM_MSCRM
/**[Customer]**/
--DECLARE @Domain AS VARCHAR(20) = UPPER('customer') ;
--DECLARE @entityCode AS VARCHAR(20) = UPPER('all') ;
--DECLARE @autoNumber AS VARCHAR(20) ;

/**[Claim Noti No - MC]**/
--DECLARE @Domain AS VARCHAR(20) = UPPER('ClaimNotiNo_MC') ;
--DECLARE @entityCode AS VARCHAR(20) = UPPER('MC') ;
--DECLARE @autoNumber AS VARCHAR(20) ;

/**[CaseNo]**/
DECLARE @Domain AS VARCHAR(20) = UPPER('CaseNo') ;
DECLARE @entityCode AS VARCHAR(20) = UPPER('CAS') ;
DECLARE @autoNumber AS VARCHAR(20) ;

EXEC sp_IncreaseAutoNumber @Domain, @entityCode, @autoNumber OUTPUT
select @autoNumber
*/    
GO
