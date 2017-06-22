USE [CRM_CUSTOM_APP]
GO

/****** Object:  Table [dbo].[LOCUS_QUOTATION_REPAIRER]    Script Date: 22/6/2560 15:58:21 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LOCUS_QUOTATION_REPAIRER](
	[เลขที่ใบเสนอราคา] [nvarchar](100) NOT NULL,
	[เลขรับแจ้ง] [nvarchar](50) NULL,
	[ช่องทางนำเข้า] [nvarchar](50) NULL,
	[สถานะใบเสนอราคา] [nvarchar](50) NULL,
	[ชื่ออู่/ศูนย์] [nvarchar](100) NULL,
	[รหัสอู่/ศูนย์] [int] NULL,
	[ผู้ทำรายการล่าสุด] [nvarchar](50) NULL,
	[ทะเบียนรถ] [nvarchar](50) NULL,
	[ประเภทรถ ประกัน/คู่กรณี] [int] NULL,
	[ค่าแรงอู่เสนอ] [numeric](18, 2) NULL,
	[ค่าแรงอนุมัติ] [numeric](18, 2) NULL,
	[ค่าอะไหล่อู่เสนอ] [numeric](18, 2) NULL,
	[ค่าอะไหล่อนุมัติ] [numeric](18, 2) NULL,
	[ส่วนลด] [numeric](18, 2) NULL,
	[ค่า Deduct] [numeric](18, 2) NULL,
	[ประเภทค่า Deduct] [nvarchar](50) NULL,
	[ค่าใช้จ่ายอื่นๆ] [numeric](18, 2) NULL,
	[วันที่เข้ามาติดต่อ] [nvarchar](50) NULL,
	[วันที่อู่เสนอราคามาที่ประกัน] [nvarchar](50) NULL,
	[วันที่ขออนุมัติ] [nvarchar](50) NULL,
	[วันที่อนุมัติ] [nvarchar](50) NULL,
	[วันที่นำรถเข้าซ่อม] [nvarchar](50) NULL,
	[จำนวนวันที่คาดว่าจะซ่อมเสร็จ] [numeric](18, 2) NULL,
	[วันที่คาดว่าจะซ่อมเสร็จ] [nvarchar](50) NULL,
	[วันที่นำรถกลับ] [nvarchar](50) NULL,
	[วันที่เสร็จสมบูรณ์] [nvarchar](50) NULL,
	[APPRORED_DATETIME] [datetime] NULL,
	[APPRORED_DATEDIFF] [int] NULL,
	[POLICY_TYPE] [nvarchar](50) NULL,
 CONSTRAINT [PK_LOCUS_QUOTATION_REPAIRER] PRIMARY KEY CLUSTERED 
(
	[เลขที่ใบเสนอราคา] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


