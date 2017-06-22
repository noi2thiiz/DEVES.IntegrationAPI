USE [CRM_CUSTOM_APP]
GO

/****** Object:  Table [dbo].[LOCUS_QUOTATION_REPAIRER]    Script Date: 22/6/2560 15:58:21 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LOCUS_QUOTATION_REPAIRER](
	[�Ţ�����ʹ��Ҥ�] [nvarchar](100) NOT NULL,
	[�Ţ�Ѻ��] [nvarchar](50) NULL,
	[��ͧ�ҧ�����] [nvarchar](50) NULL,
	[ʶҹ���ʹ��Ҥ�] [nvarchar](50) NULL,
	[�������/�ٹ��] [nvarchar](100) NULL,
	[�������/�ٹ��] [int] NULL,
	[������¡������ش] [nvarchar](50) NULL,
	[����¹ö] [nvarchar](50) NULL,
	[������ö ��Сѹ/���ó�] [int] NULL,
	[����ç����ʹ�] [numeric](18, 2) NULL,
	[����ç͹��ѵ�] [numeric](18, 2) NULL,
	[�������������ʹ�] [numeric](18, 2) NULL,
	[���������͹��ѵ�] [numeric](18, 2) NULL,
	[��ǹŴ] [numeric](18, 2) NULL,
	[��� Deduct] [numeric](18, 2) NULL,
	[��������� Deduct] [nvarchar](50) NULL,
	[������������] [numeric](18, 2) NULL,
	[�ѹ�������ҵԴ���] [nvarchar](50) NULL,
	[�ѹ�������ʹ��Ҥ��ҷ���Сѹ] [nvarchar](50) NULL,
	[�ѹ����͹��ѵ�] [nvarchar](50) NULL,
	[�ѹ���͹��ѵ�] [nvarchar](50) NULL,
	[�ѹ����ö��ҫ���] [nvarchar](50) NULL,
	[�ӹǹ�ѹ���Ҵ��ҨЫ�������] [numeric](18, 2) NULL,
	[�ѹ���Ҵ��ҨЫ�������] [nvarchar](50) NULL,
	[�ѹ����ö��Ѻ] [nvarchar](50) NULL,
	[�ѹ�����������ó�] [nvarchar](50) NULL,
	[APPRORED_DATETIME] [datetime] NULL,
	[APPRORED_DATEDIFF] [int] NULL,
	[POLICY_TYPE] [nvarchar](50) NULL,
 CONSTRAINT [PK_LOCUS_QUOTATION_REPAIRER] PRIMARY KEY CLUSTERED 
(
	[�Ţ�����ʹ��Ҥ�] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


