USE [master]
GO

/****** Object:  LinkedServer [DEVES_CLAIM_SERVER]    Script Date: 22/6/2560 16:09:07 ******/
EXEC master.dbo.sp_addlinkedserver @server = N'DEVES_CLAIM_SERVER', @srvproduct=N'192.168.8.143', @provider=N'SQLNCLI', @datasrc=N'192.168.8.143'
 /* For security reasons the linked server remote logins password is changed with ######## */
EXEC master.dbo.sp_addlinkedsrvlogin @rmtsrvname=N'DEVES_CLAIM_SERVER',@useself=N'False',@locallogin=NULL,@rmtuser=N'mscrm_rd',@rmtpassword='########'
EXEC master.dbo.sp_addlinkedsrvlogin @rmtsrvname=N'DEVES_CLAIM_SERVER',@useself=N'False',@locallogin=N'CRMDevelop',@rmtuser=N'mscrm_rd',@rmtpassword='########'

GO

EXEC master.dbo.sp_serveroption @server=N'DEVES_CLAIM_SERVER', @optname=N'collation compatible', @optvalue=N'false'
GO

EXEC master.dbo.sp_serveroption @server=N'DEVES_CLAIM_SERVER', @optname=N'data access', @optvalue=N'true'
GO

EXEC master.dbo.sp_serveroption @server=N'DEVES_CLAIM_SERVER', @optname=N'dist', @optvalue=N'false'
GO

EXEC master.dbo.sp_serveroption @server=N'DEVES_CLAIM_SERVER', @optname=N'pub', @optvalue=N'false'
GO

EXEC master.dbo.sp_serveroption @server=N'DEVES_CLAIM_SERVER', @optname=N'rpc', @optvalue=N'false'
GO

EXEC master.dbo.sp_serveroption @server=N'DEVES_CLAIM_SERVER', @optname=N'rpc out', @optvalue=N'false'
GO

EXEC master.dbo.sp_serveroption @server=N'DEVES_CLAIM_SERVER', @optname=N'sub', @optvalue=N'false'
GO

EXEC master.dbo.sp_serveroption @server=N'DEVES_CLAIM_SERVER', @optname=N'connect timeout', @optvalue=N'0'
GO

EXEC master.dbo.sp_serveroption @server=N'DEVES_CLAIM_SERVER', @optname=N'collation name', @optvalue=null
GO

EXEC master.dbo.sp_serveroption @server=N'DEVES_CLAIM_SERVER', @optname=N'lazy schema validation', @optvalue=N'false'
GO

EXEC master.dbo.sp_serveroption @server=N'DEVES_CLAIM_SERVER', @optname=N'query timeout', @optvalue=N'0'
GO

EXEC master.dbo.sp_serveroption @server=N'DEVES_CLAIM_SERVER', @optname=N'use remote collation', @optvalue=N'true'
GO

EXEC master.dbo.sp_serveroption @server=N'DEVES_CLAIM_SERVER', @optname=N'remote proc transaction promotion', @optvalue=N'true'
GO


