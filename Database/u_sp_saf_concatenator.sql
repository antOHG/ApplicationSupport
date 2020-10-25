CREATE PROCEDURE [dbo].[u_sp_saf_concatenator]  
  
AS   
  
DECLARE @SQLQuery AS NVARCHAR(2000)  
DECLARE @PackagePath AS NVARCHAR(200)  
SET @PackagePath = 'E:\Process SAFs Live\Process SAFs Live.dtsx'  
  
SET @SQLQuery = 'DTExec /FILE "'+@PackagePath+'" '  
--SET @SQLQuery = @SQLQuery + ' /SET \Package.Variables[EmpCode].Value;'  
--SET @SQLQuery = @SQLQuery + ' /SET \Package.Variables[EmpName].Value;'  
  
EXEC master..xp_cmdshell @SQLQuery   
   
If @@ERROR <> 0 GoTo ErrorHandler   
SET NoCount OFF   
Return(0)   
   
ErrorHandler:   
Return(@@ERROR)  