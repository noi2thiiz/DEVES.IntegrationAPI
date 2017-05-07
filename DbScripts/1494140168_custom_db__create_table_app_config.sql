USE [CRM_CUSTOM_APP]
GO

/****** Object:  Table [dbo].[AppConfig]    Script Date: 7/5/2560 13:30:38 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AppConfig](
	[Key] [nvarchar](50) NOT NULL,
	[Enveronment] [nvarchar](50) NOT NULL,
	[Type] [nvarchar](50) NULL,
	[Value] [nvarchar](max) NULL,
	[LOCAL] [nvarchar](max) NULL,
	[DEV] [nvarchar](max) NULL,
	[QA] [nvarchar](max) NULL,
	[PRODUCTION] [nvarchar](max) NULL,
 CONSTRAINT [PK_AppConfig] PRIMARY KEY CLUSTERED 
(
	[Key] ASC,
	[Enveronment] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO


