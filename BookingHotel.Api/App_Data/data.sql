USE [vnsosoft_vietcoupon]
GO
/****** Object:  User [vnsosoft_vietcoupon]    Script Date: 4/13/2021 1:19:00 AM ******/
CREATE USER [vnsosoft_vietcoupon] FOR LOGIN [vnsosoft_vietcoupon] WITH DEFAULT_SCHEMA=[vnsosoft_vietcoupon]
GO
ALTER ROLE [db_ddladmin] ADD MEMBER [vnsosoft_vietcoupon]
GO
ALTER ROLE [db_backupoperator] ADD MEMBER [vnsosoft_vietcoupon]
GO
ALTER ROLE [db_datareader] ADD MEMBER [vnsosoft_vietcoupon]
GO
ALTER ROLE [db_datawriter] ADD MEMBER [vnsosoft_vietcoupon]
GO
/****** Object:  Schema [vnsosoft_vietcoupon]    Script Date: 4/13/2021 1:19:02 AM ******/
CREATE SCHEMA [vnsosoft_vietcoupon]
GO
/****** Object:  Table [dbo].[About]    Script Date: 4/13/2021 1:19:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[About](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Avatar] [nvarchar](1024) NULL,
	[Thumb] [nvarchar](1024) NULL,
	[Content] [ntext] NULL,
	[ShortContent] [ntext] NULL,
	[Email] [varchar](300) NULL,
	[Phone] [varchar](100) NULL,
	[Tel] [varchar](300) NULL,
	[Hotline] [varchar](255) NULL,
	[Fax] [varchar](255) NULL,
	[Website] [varchar](255) NULL,
	[Facebook] [varchar](255) NULL,
	[Zalo] [varchar](255) NULL,
	[Blog] [varchar](255) NULL,
	[Youtube] [varchar](255) NULL,
	[Instagram] [varchar](255) NULL,
	[Twitter] [nvarchar](1024) NULL,
	[Pinterest] [varchar](255) NULL,
	[Linkedin] [varchar](255) NULL,
	[MetaTitle] [nvarchar](max) NULL,
	[MetaDescription] [nvarchar](max) NULL,
	[MetaKeywords] [nvarchar](max) NULL,
	[CreateBy] [int] NULL,
	[CreateTime] [datetime] NULL,
	[ModifyBy] [int] NULL,
	[ModifyTime] [datetime] NULL,
	[Address] [nvarchar](300) NULL,
	[Title] [nvarchar](1024) NULL,
	[Logo] [varchar](255) NULL,
 CONSTRAINT [PK_About] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Account]    Script Date: 4/13/2021 1:19:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Account](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](100) NULL,
	[Password] [varchar](500) NULL,
	[Status] [bit] NULL,
	[CreateBy] [int] NULL,
	[CreateTime] [datetime] NULL,
	[ModifyBy] [int] NULL,
	[ModifyTime] [datetime] NULL,
	[AccountTypeID] [int] NOT NULL,
 CONSTRAINT [PK_Account] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AccountFunction]    Script Date: 4/13/2021 1:19:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AccountFunction](
	[FunctionID] [int] NOT NULL,
	[AccountID] [int] NOT NULL,
	[CreateBy] [int] NULL,
	[CreateTime] [datetime] NULL,
	[ModifyBy] [int] NULL,
	[ModifyTime] [datetime] NULL,
 CONSTRAINT [PK_AccountFunction] PRIMARY KEY CLUSTERED 
(
	[FunctionID] ASC,
	[AccountID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AccountGroupAccount]    Script Date: 4/13/2021 1:19:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AccountGroupAccount](
	[AccountID] [int] NOT NULL,
	[GroupAccountID] [int] NOT NULL,
	[CreateTime] [datetime] NULL,
	[CreateBy] [int] NULL,
	[ModifyBy] [int] NULL,
	[ModifyTime] [datetime] NULL,
	[IsDefault] [bit] NULL,
 CONSTRAINT [PK_AccountGroupAccount] PRIMARY KEY CLUSTERED 
(
	[AccountID] ASC,
	[GroupAccountID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AccountType]    Script Date: 4/13/2021 1:19:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AccountType](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](1024) NULL,
	[Status] [bit] NULL,
	[Position] [int] NULL,
	[CreateBy] [int] NULL,
	[CreateTime] [datetime] NULL,
	[ModifyBy] [int] NULL,
	[ModifyTime] [datetime] NULL,
	[Key] [varchar](100) NULL,
	[IsDelete] [bit] NULL,
 CONSTRAINT [PK_AccountType] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Article]    Script Date: 4/13/2021 1:19:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Article](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](1024) NULL,
	[Description] [nvarchar](500) NULL,
	[Avatar] [nvarchar](1024) NULL,
	[Thumb] [nvarchar](1024) NULL,
	[Content] [ntext] NULL,
	[SourcePage] [nvarchar](1024) NULL,
	[SourceLink] [nvarchar](1024) NULL,
	[Position] [int] NULL,
	[Status] [bit] NULL,
	[ViewTime] [int] NULL,
	[TotalAssess] [int] NULL,
	[ValueAssess] [decimal](5, 1) NULL,
	[CreateBy] [int] NULL,
	[CreateTime] [datetime] NULL,
	[ModifyBy] [int] NULL,
	[ModifyTime] [datetime] NULL,
	[Schemas] [ntext] NULL,
	[MetaTitle] [nvarchar](max) NULL,
	[MetaDescription] [nvarchar](max) NULL,
	[MetaKeywords] [nvarchar](max) NULL,
	[ArticleCategoryID] [int] NULL,
 CONSTRAINT [PK_Article] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ArticleCategory]    Script Date: 4/13/2021 1:19:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ArticleCategory](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](1024) NULL,
	[Description] [nvarchar](500) NULL,
	[Status] [bit] NOT NULL,
	[Position] [int] NULL,
	[MetaTitle] [nvarchar](max) NULL,
	[MetaDescription] [nvarchar](max) NULL,
	[MetaKeywords] [nvarchar](max) NULL,
	[Schemas] [ntext] NULL,
	[CreateTime] [datetime] NOT NULL,
	[CreateBy] [int] NULL,
	[ModifyTime] [datetime] NULL,
	[ModifyBy] [int] NULL,
	[ParentID] [int] NULL,
 CONSTRAINT [PK_ArticleCategory] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Assess]    Script Date: 4/13/2021 1:19:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Assess](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FullName] [nvarchar](300) NULL,
	[Content] [nvarchar](500) NULL,
	[Note] [nvarchar](500) NULL,
	[NumberStar] [int] NULL,
	[KeyID] [int] NULL,
	[KeyName] [varchar](500) NULL,
	[CreateTime] [datetime] NULL,
	[CreateBy] [int] NULL,
	[ModifyBy] [int] NULL,
	[ModifyTime] [datetime] NULL,
	[AccountID] [int] NULL,
 CONSTRAINT [PK_Assess] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Banner]    Script Date: 4/13/2021 1:19:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Banner](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](1024) NULL,
	[Avatar] [nvarchar](1024) NULL,
	[Thumb] [nvarchar](1024) NULL,
	[Url] [varchar](1024) NULL,
	[Status] [bit] NULL,
	[CreateBy] [int] NULL,
	[CreateTime] [datetime] NULL,
	[ModifyBy] [int] NULL,
	[ModifyTime] [datetime] NULL,
 CONSTRAINT [PK_Banner] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ConfigSystem]    Script Date: 4/13/2021 1:19:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ConfigSystem](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Key] [varchar](100) NULL,
	[Values] [varchar](1000) NULL,
	[CreateBy] [int] NULL,
	[CreateTime] [datetime] NULL,
	[ModifyBy] [int] NULL,
	[ModifyTime] [datetime] NULL,
	[Description] [nvarchar](500) NULL,
 CONSTRAINT [PK_ConfigSystem] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Contact]    Script Date: 4/13/2021 1:19:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Contact](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ApproveBy] [int] NULL,
	[FullName] [nvarchar](300) NULL,
	[Email] [varchar](300) NULL,
	[Address] [nvarchar](300) NULL,
	[Mobi] [nvarchar](13) NULL,
	[Subject] [nvarchar](250) NULL,
	[Content] [nvarchar](4000) NULL,
	[Status] [bit] NULL,
	[CreateTime] [datetime] NULL,
	[ModifyBy] [int] NULL,
	[ModifyTime] [datetime] NULL,
 CONSTRAINT [PK_Contact] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Customer]    Script Date: 4/13/2021 1:19:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customer](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Email] [varchar](300) NULL,
	[FullName] [nvarchar](300) NULL,
	[Gender] [bit] NULL,
	[Birthday] [varchar](100) NULL,
	[Avatar] [nvarchar](1024) NULL,
	[Phone] [varchar](100) NULL,
	[Tel] [varchar](300) NULL,
	[Address] [nvarchar](300) NULL,
	[CreateBy] [int] NULL,
	[CreateTime] [datetime] NULL,
	[ModifyBy] [int] NULL,
	[ModifyTime] [datetime] NULL,
	[CustomerTypeID] [int] NULL,
	[NumberTakeCare] [int] NULL,
	[CompanyName] [nvarchar](1000) NULL,
	[AccoutID] [int] NULL,
	[Point] [int] NULL,
	[ReferralCode] [varchar](max) NULL,
	[Code] [varchar](max) NULL,
 CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CustomerPartner]    Script Date: 4/13/2021 1:19:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CustomerPartner](
	[CustomerID] [int] NOT NULL,
	[PartnerID] [int] NOT NULL,
	[CreateBy] [int] NULL,
	[CreateTime] [datetime] NULL,
	[ModifyBy] [int] NULL,
	[ModifyTime] [datetime] NULL,
 CONSTRAINT [PK_CustomerPartner] PRIMARY KEY CLUSTERED 
(
	[CustomerID] ASC,
	[PartnerID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CustomerType]    Script Date: 4/13/2021 1:19:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CustomerType](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](1024) NULL,
	[Description] [nvarchar](500) NULL,
	[Status] [bit] NULL,
	[CreateBy] [int] NULL,
	[CreateTime] [datetime] NULL,
	[ModifyBy] [int] NULL,
	[ModifyTime] [datetime] NULL,
	[Position] [int] NULL,
 CONSTRAINT [PK_CustomerType] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[District]    Script Date: 4/13/2021 1:19:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[District](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](1024) NULL,
	[ProvinceID] [int] NULL,
	[CreateBy] [int] NULL,
	[CreateTime] [datetime] NULL,
	[ModifyBy] [int] NULL,
	[ModifyTime] [datetime] NULL,
	[Status] [bit] NULL,
	[IsDelete] [bit] NULL,
 CONSTRAINT [PK_District] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Faq]    Script Date: 4/13/2021 1:19:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Faq](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](1024) NULL,
	[Description] [nvarchar](500) NULL,
	[Position] [int] NULL,
	[Status] [bit] NULL,
	[CreateBy] [int] NULL,
	[CreateTime] [datetime] NULL,
	[ModifyBy] [int] NULL,
	[ModifyTime] [datetime] NULL,
	[Alias] [varchar](1000) NULL,
	[IsDelete] [bit] NULL,
 CONSTRAINT [PK_Faq] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Feedback]    Script Date: 4/13/2021 1:19:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Feedback](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FullName] [nvarchar](300) NULL,
	[Avatar] [nvarchar](1024) NULL,
	[Thumb] [nvarchar](1024) NULL,
	[Content] [nvarchar](1024) NULL,
	[Regency] [nvarchar](1024) NULL,
	[Status] [bit] NULL,
	[CreateBy] [int] NULL,
	[CreateTime] [datetime] NULL,
	[ModifyBy] [int] NULL,
	[ModifyTime] [datetime] NULL,
	[IsDelete] [bit] NULL,
 CONSTRAINT [PK_Feedback] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Function]    Script Date: 4/13/2021 1:19:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Function](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](1024) NULL,
	[Description] [nvarchar](500) NULL,
	[Controller] [varchar](500) NULL,
	[Action] [varchar](500) NULL,
	[Url] [varchar](255) NULL,
	[Note] [ntext] NULL,
	[Position] [int] NULL,
	[Status] [bit] NOT NULL,
	[IsShow] [bit] NULL,
	[CreateBy] [int] NULL,
	[CreateTime] [datetime] NULL,
	[ModifyBy] [int] NULL,
	[ModifyTime] [datetime] NULL,
	[ModuleID] [int] NULL,
	[ParentID] [int] NULL,
 CONSTRAINT [PK_Function] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FunctionGroupFunction]    Script Date: 4/13/2021 1:19:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FunctionGroupFunction](
	[GroupFunctionID] [int] NOT NULL,
	[FunctionID] [int] NOT NULL,
	[CreateBy] [int] NULL,
	[CreateTime] [datetime] NULL,
	[ModifyBy] [int] NULL,
	[ModifyTime] [datetime] NULL,
 CONSTRAINT [PK_FunctionGroupFunction] PRIMARY KEY CLUSTERED 
(
	[GroupFunctionID] ASC,
	[FunctionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GroupAccount]    Script Date: 4/13/2021 1:19:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GroupAccount](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Key] [varchar](100) NULL,
	[Title] [nvarchar](1024) NULL,
	[Status] [bit] NULL,
	[CreateBy] [int] NULL,
	[CreateTime] [datetime] NULL,
	[ModifyBy] [int] NULL,
	[ModifyTime] [datetime] NULL,
	[IsDelete] [bit] NULL,
 CONSTRAINT [PK_GroupAccount] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GroupFunction]    Script Date: 4/13/2021 1:19:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GroupFunction](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](1024) NULL,
	[Description] [nvarchar](500) NULL,
	[Status] [bit] NULL,
	[CreateBy] [int] NULL,
	[CreateTime] [datetime] NULL,
	[ModifyBy] [int] NULL,
	[ModifyTime] [datetime] NULL,
	[IsDelete] [bit] NULL,
 CONSTRAINT [PK_GroupFunction] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Module]    Script Date: 4/13/2021 1:19:20 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Module](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](1024) NULL,
	[Description] [nvarchar](500) NULL,
	[Position] [int] NULL,
	[Status] [bit] NULL,
	[CreateBy] [int] NULL,
	[CreateTime] [datetime] NULL,
	[ModifyBy] [int] NULL,
	[ModifyTime] [datetime] NULL,
 CONSTRAINT [PK_Module] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Notification]    Script Date: 4/13/2021 1:19:20 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Notification](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](1024) NULL,
	[Description] [nvarchar](500) NULL,
	[Content] [ntext] NULL,
	[NotificationTypeID] [int] NULL,
	[Receiver] [ntext] NULL,
	[CreateBy] [int] NULL,
	[CreateTime] [datetime] NULL,
	[ModifyBy] [int] NULL,
	[ModifyTime] [datetime] NULL,
	[Status] [bit] NULL,
 CONSTRAINT [PK_Notification] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NotificationType]    Script Date: 4/13/2021 1:19:21 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NotificationType](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](1024) NULL,
	[Status] [bit] NULL,
	[Position] [int] NULL,
	[CreateBy] [int] NULL,
	[CreateTime] [datetime] NULL,
	[ModifyBy] [int] NULL,
	[ModifyTime] [datetime] NULL,
	[Code] [varchar](500) NULL,
 CONSTRAINT [PK_NotificationType] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Order]    Script Date: 4/13/2021 1:19:22 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Order](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Code] [varchar](500) NULL,
	[TotalPrice] [decimal](18, 2) NULL,
	[FullName] [nvarchar](300) NULL,
	[Email] [varchar](300) NULL,
	[Mobi] [nvarchar](13) NULL,
	[Address] [nvarchar](300) NULL,
	[Status] [int] NULL,
	[IsDelete] [bit] NULL,
	[Remark] [nvarchar](1024) NULL,
	[PayTypeID] [int] NULL,
	[CustomerID] [int] NULL,
	[NoteSale] [nvarchar](200) NULL,
	[SaleOff] [decimal](18, 2) NULL,
	[NoteFeeShip] [nvarchar](200) NULL,
	[OrderStatusID] [int] NULL,
	[CreateBy] [int] NULL,
	[CreateTime] [datetime] NULL,
	[ModifyBy] [int] NULL,
	[ModifyTime] [datetime] NULL,
	[IsPoint] [bit] NULL,
	[Point] [int] NULL,
 CONSTRAINT [PK_Order] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderDetail]    Script Date: 4/13/2021 1:19:22 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderDetail](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[OrderID] [int] NOT NULL,
	[ProductID] [int] NOT NULL,
	[Quantity] [int] NULL,
	[Price] [decimal](18, 2) NULL,
	[OriginalPrice] [decimal](18, 2) NULL,
	[Option] [ntext] NULL,
 CONSTRAINT [PK_OrderDetail] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderStatus]    Script Date: 4/13/2021 1:19:23 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderStatus](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](1024) NULL,
	[Description] [nvarchar](500) NULL,
	[Position] [int] NULL,
	[Status] [bit] NULL,
	[IsDelete] [bit] NULL,
	[CreateBy] [int] NULL,
	[CreateTime] [datetime] NULL,
	[ModifyBy] [int] NULL,
	[ModifyTime] [datetime] NULL,
 CONSTRAINT [PK_OrderStatus] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Page]    Script Date: 4/13/2021 1:19:24 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Page](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](1024) NULL,
	[Description] [nvarchar](500) NULL,
	[Content] [ntext] NULL,
	[Status] [bit] NULL,
	[Position] [int] NULL,
	[CreateBy] [int] NULL,
	[CreateTime] [datetime] NULL,
	[ModifyBy] [int] NULL,
	[ModifyTime] [datetime] NULL,
	[Schemas] [ntext] NULL,
	[MetaTitle] [nvarchar](max) NULL,
	[MetaDescription] [nvarchar](max) NULL,
	[MetaKeywords] [nvarchar](max) NULL,
	[PageTypeID] [int] NULL,
	[Url] [varchar](1024) NULL,
	[IsUrl] [bit] NULL,
	[Alias] [varchar](1000) NULL,
 CONSTRAINT [PK_Page] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PageType]    Script Date: 4/13/2021 1:19:24 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PageType](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](1024) NULL,
	[Description] [nvarchar](500) NULL,
	[Status] [bit] NULL,
	[Position] [int] NULL,
	[CreateBy] [int] NULL,
	[CreateTime] [datetime] NULL,
	[ModifyBy] [int] NULL,
	[ModifyTime] [datetime] NULL,
 CONSTRAINT [PK_PageType] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Partner]    Script Date: 4/13/2021 1:19:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Partner](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Url] [varchar](1024) NULL,
	[Title] [nvarchar](1024) NULL,
	[Avatar] [nvarchar](1024) NULL,
	[CreateTime] [datetime] NULL,
	[CreateBy] [int] NULL,
	[Status] [bit] NULL,
	[Position] [int] NULL,
	[Description] [nvarchar](500) NULL,
	[ModifyTime] [datetime] NULL,
	[ModifyBy] [int] NULL,
	[PartnerTypeID] [int] NULL,
	[AccountID] [int] NULL,
	[Thumb] [nvarchar](1024) NULL,
	[Content] [ntext] NULL,
	[Banner] [nvarchar](1024) NULL,
	[ViewTime] [int] NULL,
	[TotalAssess] [int] NULL,
	[ValueAssess] [decimal](5, 1) NULL,
 CONSTRAINT [PK_Partner] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PartnerLocal]    Script Date: 4/13/2021 1:19:26 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PartnerLocal](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](1024) NULL,
	[Description] [nvarchar](500) NULL,
	[Status] [bit] NULL,
	[CreateBy] [int] NULL,
	[CreateTime] [datetime] NULL,
	[ModifyBy] [int] NULL,
	[ModifyTime] [datetime] NULL,
	[IsDelete] [bit] NULL,
	[PartnerID] [int] NULL,
	[Local] [nvarchar](max) NULL,
 CONSTRAINT [PK_PartnerLocal] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PartnerProduct]    Script Date: 4/13/2021 1:19:26 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PartnerProduct](
	[PartnerID] [int] NOT NULL,
	[ProductID] [int] NOT NULL,
	[CreateBy] [int] NULL,
	[CreateTime] [datetime] NULL,
	[ModifyBy] [int] NULL,
	[ModifyTime] [datetime] NULL,
 CONSTRAINT [PK_PartnerProduct] PRIMARY KEY CLUSTERED 
(
	[PartnerID] ASC,
	[ProductID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PartnerType]    Script Date: 4/13/2021 1:19:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PartnerType](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](1024) NULL,
	[Position] [int] NULL,
	[Status] [bit] NULL,
	[CreateBy] [int] NULL,
	[CreateTime] [datetime] NULL,
	[ModifyBy] [int] NULL,
	[ModifyTime] [datetime] NULL,
 CONSTRAINT [PK_PartnerType] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PayType]    Script Date: 4/13/2021 1:19:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PayType](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](1024) NULL,
	[Position] [int] NULL,
	[Status] [bit] NULL,
	[CreateBy] [int] NULL,
	[CreateTime] [datetime] NULL,
	[ModifyBy] [int] NULL,
	[ModifyTime] [datetime] NULL,
 CONSTRAINT [PK_PayType] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Product]    Script Date: 4/13/2021 1:19:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Product](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Code] [varchar](500) NULL,
	[Title] [nvarchar](1024) NULL,
	[Description] [nvarchar](1024) NULL,
	[Content] [ntext] NULL,
	[Avatar] [nvarchar](1024) NULL,
	[Thumb] [nvarchar](1024) NULL,
	[ImageListProduct] [nvarchar](4000) NULL,
	[Position] [int] NULL,
	[MetaTitle] [nvarchar](max) NULL,
	[MetaDescription] [nvarchar](max) NULL,
	[MetaKeywords] [nvarchar](max) NULL,
	[Alias] [varchar](1000) NULL,
	[Price] [decimal](18, 2) NULL,
	[Sale] [int] NULL,
	[SaleDeadLine] [datetime] NULL,
	[ViewTime] [int] NULL,
	[StockStatus] [bit] NULL,
	[WishlistView] [int] NULL,
	[SoldView] [int] NULL,
	[Schemas] [ntext] NULL,
	[TotalAssess] [int] NOT NULL,
	[ValueAssess] [decimal](5, 1) NULL,
	[Status] [bit] NULL,
	[IsDelete] [bit] NULL,
	[CreateTime] [datetime] NULL,
	[ModifyTime] [datetime] NULL,
	[CreateBy] [int] NULL,
	[ModifyBy] [int] NULL,
	[ProductCategoryID] [int] NULL,
	[SaleID] [int] NULL,
	[UnitID] [int] NULL,
	[EffectiveDate] [datetime] NULL,
	[ExpiryDate] [datetime] NULL,
	[Point] [int] NULL,
 CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductCategory]    Script Date: 4/13/2021 1:19:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductCategory](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](1024) NULL,
	[Description] [nvarchar](500) NULL,
	[MetaTitle] [nvarchar](max) NULL,
	[MetaDescription] [nvarchar](max) NULL,
	[MetaKeywords] [nvarchar](max) NULL,
	[Avatar] [nvarchar](1024) NULL,
	[Thumb] [nvarchar](1024) NULL,
	[Alias] [varchar](1000) NULL,
	[Schemas] [ntext] NULL,
	[Position] [int] NULL,
	[Status] [bit] NULL,
	[IsDelete] [bit] NULL,
	[CreateTime] [datetime] NULL,
	[CreateBy] [int] NULL,
	[ModifyBy] [int] NULL,
	[ModifyTime] [datetime] NULL,
	[ParentID] [int] NULL,
 CONSTRAINT [PK_ProductCategory] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductCustomer]    Script Date: 4/13/2021 1:19:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductCustomer](
	[CustomerID] [int] NOT NULL,
	[ProductID] [int] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[CreateBy] [int] NULL,
	[ModifyTime] [datetime] NULL,
	[ModifyBy] [int] NULL,
	[IsUse] [bit] NULL,
	[UseTime] [datetime] NULL,
	[OriginKey] [varchar](500) NULL,
	[OriginValue] [int] NULL,
 CONSTRAINT [PK_ProductCustomer] PRIMARY KEY CLUSTERED 
(
	[CustomerID] ASC,
	[ProductID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductTab]    Script Date: 4/13/2021 1:19:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductTab](
	[TabID] [int] NOT NULL,
	[ProductID] [int] NOT NULL,
	[Value] [nvarchar](max) NULL,
	[CreateTime] [datetime] NOT NULL,
	[CreateBy] [int] NULL,
	[ModifyTime] [datetime] NULL,
	[ModifyBy] [int] NULL,
 CONSTRAINT [PK_ProductTab] PRIMARY KEY CLUSTERED 
(
	[TabID] ASC,
	[ProductID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductWishlist]    Script Date: 4/13/2021 1:19:31 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductWishlist](
	[ProductID] [int] NOT NULL,
	[AccountID] [int] NOT NULL,
	[CreateBy] [int] NULL,
	[CreateTime] [datetime] NULL,
 CONSTRAINT [PK_ProductWishlist] PRIMARY KEY CLUSTERED 
(
	[ProductID] ASC,
	[AccountID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Province]    Script Date: 4/13/2021 1:19:31 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Province](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](1024) NULL,
	[FeeShip] [decimal](18, 2) NULL,
	[CreateBy] [int] NULL,
	[CreateTime] [datetime] NULL,
	[ModifyBy] [int] NULL,
	[ModifyTime] [datetime] NOT NULL,
	[Status] [bit] NULL,
	[IsDelete] [bit] NULL,
 CONSTRAINT [PK_Province] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Recruitment]    Script Date: 4/13/2021 1:19:32 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Recruitment](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](1024) NULL,
	[Description] [nvarchar](500) NULL,
	[Avatar] [nvarchar](1024) NULL,
	[Thumb] [nvarchar](1024) NULL,
	[Content] [nvarchar](max) NOT NULL,
	[ViewTime] [int] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[CreateBy] [int] NULL,
	[ModifyTime] [datetime] NULL,
	[ModifyBy] [int] NULL,
	[Status] [bit] NOT NULL,
	[Position] [int] NULL,
	[MetaTitle] [nvarchar](max) NULL,
	[MetaDescription] [nvarchar](max) NULL,
	[MetaKeywords] [nvarchar](max) NULL,
	[Schemas] [ntext] NULL,
	[IsDelete] [bit] NULL,
 CONSTRAINT [PK_Recruitment] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sale]    Script Date: 4/13/2021 1:19:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sale](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ShortTitle] [nvarchar](300) NULL,
	[Title] [nvarchar](1024) NULL,
	[Description] [nvarchar](500) NULL,
	[Content] [ntext] NULL,
	[Avatar] [nvarchar](1024) NULL,
	[Thumb] [nvarchar](1024) NULL,
	[IsSalePrice] [bit] NULL,
	[PriceSale] [decimal](18, 2) NULL,
	[KeySale] [varchar](500) NULL,
	[TimeStart] [datetime] NULL,
	[TimeEnd] [datetime] NULL,
	[PercentSale] [int] NULL,
	[MetaTitle] [nvarchar](max) NULL,
	[MetaDescription] [nvarchar](max) NULL,
	[MetaKeywords] [nvarchar](max) NULL,
	[Alias] [varchar](1000) NULL,
	[Schemas] [ntext] NULL,
	[Position] [int] NULL,
	[Status] [bit] NULL,
	[IsDelete] [bit] NULL,
	[CreateBy] [int] NULL,
	[CreateTime] [datetime] NULL,
	[ModifyBy] [int] NULL,
	[ModifyTime] [datetime] NULL,
 CONSTRAINT [PK_Sale] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Service]    Script Date: 4/13/2021 1:19:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Service](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](1024) NULL,
	[Description] [nvarchar](500) NULL,
	[Avatar] [nvarchar](1024) NULL,
	[Thumb] [nvarchar](1024) NULL,
	[Content] [ntext] NULL,
	[Position] [int] NULL,
	[Status] [bit] NULL,
	[ViewTime] [int] NULL,
	[TotalAssess] [int] NULL,
	[ValueAssess] [decimal](5, 1) NULL,
	[CreateBy] [int] NULL,
	[CreateTime] [datetime] NULL,
	[ModifyBy] [int] NULL,
	[ModifyTime] [datetime] NULL,
	[Schemas] [ntext] NULL,
	[MetaTitle] [nvarchar](max) NULL,
	[MetaDescription] [nvarchar](max) NULL,
	[MetaKeywords] [nvarchar](max) NULL,
	[ServiceCategoryID] [int] NULL,
	[Alias] [varchar](1000) NULL,
	[IsDelete] [bit] NULL,
 CONSTRAINT [PK_Service] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ServiceCategory]    Script Date: 4/13/2021 1:19:34 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ServiceCategory](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](1024) NULL,
	[Description] [nvarchar](500) NULL,
	[Status] [bit] NOT NULL,
	[Position] [int] NULL,
	[MetaTitle] [nvarchar](max) NULL,
	[MetaDescription] [nvarchar](max) NULL,
	[MetaKeywords] [nvarchar](max) NULL,
	[Schemas] [ntext] NULL,
	[CreateTime] [datetime] NOT NULL,
	[CreateBy] [int] NULL,
	[ModifyTime] [datetime] NULL,
	[ModifyBy] [int] NULL,
	[ParentID] [int] NULL,
	[Alias] [varchar](1000) NULL,
	[IsDelete] [bit] NULL,
 CONSTRAINT [PK_ServiceCategory] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tab]    Script Date: 4/13/2021 1:19:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tab](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Code] [varchar](200) NULL,
	[Title] [nvarchar](1024) NULL,
	[Content] [ntext] NULL,
	[Position] [int] NULL,
	[IsDelete] [bit] NULL,
	[Status] [bit] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[CreateBy] [int] NULL,
	[ModifyTime] [datetime] NULL,
	[ModifyBy] [int] NULL,
 CONSTRAINT [PK_Tab] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Unit]    Script Date: 4/13/2021 1:19:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Unit](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Code] [varchar](500) NULL,
	[Title] [nvarchar](1024) NULL,
	[Status] [bit] NOT NULL,
	[IsDelete] [bit] NULL,
	[CreateTime] [datetime] NOT NULL,
	[CreateBy] [int] NULL,
	[ModifyTime] [datetime] NULL,
	[ModifyBy] [int] NULL,
 CONSTRAINT [PK_Unit] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [vnsosoft_vietcoupon].[__EFMigrationsHistory]    Script Date: 4/13/2021 1:19:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [vnsosoft_vietcoupon].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Account] ON 
GO
INSERT [dbo].[Account] ([ID], [UserName], [Password], [Status], [CreateBy], [CreateTime], [ModifyBy], [ModifyTime], [AccountTypeID]) VALUES (2, N'admin', N'123', 1, NULL, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Account] ([ID], [UserName], [Password], [Status], [CreateBy], [CreateTime], [ModifyBy], [ModifyTime], [AccountTypeID]) VALUES (3, N'minhthuan', N'123', 1, NULL, NULL, NULL, NULL, 2)
GO
INSERT [dbo].[Account] ([ID], [UserName], [Password], [Status], [CreateBy], [CreateTime], [ModifyBy], [ModifyTime], [AccountTypeID]) VALUES (4, N'123', N'123', 1, NULL, CAST(N'2021-04-06T07:43:25.767' AS DateTime), NULL, CAST(N'2021-04-06T07:43:25.767' AS DateTime), 2)
GO
INSERT [dbo].[Account] ([ID], [UserName], [Password], [Status], [CreateBy], [CreateTime], [ModifyBy], [ModifyTime], [AccountTypeID]) VALUES (5, N'circlek', N'123456', 1, 2, CAST(N'2021-04-09T00:00:00.000' AS DateTime), 2, CAST(N'2021-04-09T00:00:00.000' AS DateTime), 3)
GO
INSERT [dbo].[Account] ([ID], [UserName], [Password], [Status], [CreateBy], [CreateTime], [ModifyBy], [ModifyTime], [AccountTypeID]) VALUES (6, N'ministop', N'123456', 1, 2, CAST(N'2021-04-09T00:00:00.000' AS DateTime), 2, CAST(N'2021-04-09T00:00:00.000' AS DateTime), 3)
GO
INSERT [dbo].[Account] ([ID], [UserName], [Password], [Status], [CreateBy], [CreateTime], [ModifyBy], [ModifyTime], [AccountTypeID]) VALUES (7, N'coopmart', N'123456', 1, 2, CAST(N'2021-04-09T00:00:00.000' AS DateTime), 2, CAST(N'2021-04-09T00:00:00.000' AS DateTime), 3)
GO
INSERT [dbo].[Account] ([ID], [UserName], [Password], [Status], [CreateBy], [CreateTime], [ModifyBy], [ModifyTime], [AccountTypeID]) VALUES (8, N'thecoffeehouse', N'123456', 1, 2, CAST(N'2021-04-09T00:00:00.000' AS DateTime), 2, CAST(N'2021-04-09T00:00:00.000' AS DateTime), 3)
GO
INSERT [dbo].[Account] ([ID], [UserName], [Password], [Status], [CreateBy], [CreateTime], [ModifyBy], [ModifyTime], [AccountTypeID]) VALUES (9, N'ganuongooo', N'123456', 1, 2, CAST(N'2021-04-09T00:00:00.000' AS DateTime), 2, CAST(N'2021-04-09T00:00:00.000' AS DateTime), 3)
GO
SET IDENTITY_INSERT [dbo].[Account] OFF
GO
SET IDENTITY_INSERT [dbo].[AccountType] ON 
GO
INSERT [dbo].[AccountType] ([ID], [Title], [Status], [Position], [CreateBy], [CreateTime], [ModifyBy], [ModifyTime], [Key], [IsDelete]) VALUES (1, N'Admin', 1, 2, 2, CAST(N'2021-04-09T00:00:00.000' AS DateTime), 2, CAST(N'2021-04-09T00:00:00.000' AS DateTime), N'Admin', 0)
GO
INSERT [dbo].[AccountType] ([ID], [Title], [Status], [Position], [CreateBy], [CreateTime], [ModifyBy], [ModifyTime], [Key], [IsDelete]) VALUES (2, N'Customer', 1, 2, 2, CAST(N'2021-04-09T00:00:00.000' AS DateTime), 2, CAST(N'2021-04-09T00:00:00.000' AS DateTime), N'Customer', 0)
GO
INSERT [dbo].[AccountType] ([ID], [Title], [Status], [Position], [CreateBy], [CreateTime], [ModifyBy], [ModifyTime], [Key], [IsDelete]) VALUES (3, N'Partner', 1, 2, 2, CAST(N'2021-04-09T00:00:00.000' AS DateTime), 2, CAST(N'2021-04-09T00:00:00.000' AS DateTime), N'Partner', 0)
GO
SET IDENTITY_INSERT [dbo].[AccountType] OFF
GO
SET IDENTITY_INSERT [dbo].[Article] ON 
GO
INSERT [dbo].[Article] ([ID], [Title], [Description], [Avatar], [Thumb], [Content], [SourcePage], [SourceLink], [Position], [Status], [ViewTime], [TotalAssess], [ValueAssess], [CreateBy], [CreateTime], [ModifyBy], [ModifyTime], [Schemas], [MetaTitle], [MetaDescription], [MetaKeywords], [ArticleCategoryID]) VALUES (3, N'4.4 ngày đôi sale gấp bội', N'4.4 ngày đôi sale gấp bội', N'#', N'#', N'4.4 ngày đôi sale gấp bội', N'#', N'#', 1, 1, 0, 0, CAST(0.0 AS Decimal(5, 1)), NULL, CAST(N'2021-04-04T00:00:00.000' AS DateTime), NULL, NULL, N'#', N'4.4 ngày đôi sale gấp bội', N'4.4 ngày đôi sale gấp bội', N'4.4 ngày đôi sale gấp bội', 1)
GO
INSERT [dbo].[Article] ([ID], [Title], [Description], [Avatar], [Thumb], [Content], [SourcePage], [SourceLink], [Position], [Status], [ViewTime], [TotalAssess], [ValueAssess], [CreateBy], [CreateTime], [ModifyBy], [ModifyTime], [Schemas], [MetaTitle], [MetaDescription], [MetaKeywords], [ArticleCategoryID]) VALUES (4, N'Siêu deal 0đ ngàn voucher giảm sâu', N'Siêu deal 0đ ngàn voucher giảm sâu', N'#', N'#', N'Siêu deal 0đ ngàn voucher giảm sâu', N'#', N'#', 2, 1, 0, 0, CAST(0.0 AS Decimal(5, 1)), NULL, CAST(N'2021-04-04T00:00:00.000' AS DateTime), NULL, NULL, N'#', N'Siêu deal 0đ ngàn voucher giảm sâu', N'Siêu deal 0đ ngàn voucher giảm sâu', N'Siêu deal 0đ ngàn voucher giảm sâu', 1)
GO
INSERT [dbo].[Article] ([ID], [Title], [Description], [Avatar], [Thumb], [Content], [SourcePage], [SourceLink], [Position], [Status], [ViewTime], [TotalAssess], [ValueAssess], [CreateBy], [CreateTime], [ModifyBy], [ModifyTime], [Schemas], [MetaTitle], [MetaDescription], [MetaKeywords], [ArticleCategoryID]) VALUES (5, N'Tổng hợp chương trình Lazada Siêu Sale Sinh Nhật 2021', N'Tổng hợp chương trình Lazada Siêu Sale Sinh Nhật 2021', N'#', N'#', N'Tổng hợp chương trình Lazada Siêu Sale Sinh Nhật 2021', N'#', N'#', 3, 1, 0, 0, CAST(0.0 AS Decimal(5, 1)), NULL, CAST(N'2021-04-04T00:00:00.000' AS DateTime), NULL, NULL, N'#', N'Tổng hợp chương trình Lazada Siêu Sale Sinh Nhật 2021', N'Tổng hợp chương trình Lazada Siêu Sale Sinh Nhật 2021', N'Tổng hợp chương trình Lazada Siêu Sale Sinh Nhật 2021', 1)
GO
INSERT [dbo].[Article] ([ID], [Title], [Description], [Avatar], [Thumb], [Content], [SourcePage], [SourceLink], [Position], [Status], [ViewTime], [TotalAssess], [ValueAssess], [CreateBy], [CreateTime], [ModifyBy], [ModifyTime], [Schemas], [MetaTitle], [MetaDescription], [MetaKeywords], [ArticleCategoryID]) VALUES (6, N'Shopee Tết Sale 2021: Freeship 0Đ + Voucher 8K, 88K, 888K', N'Shopee Tết Sale 2021: Freeship 0Đ + Voucher 8K, 88K, 888K', N'#', N'#', N'Shopee Tết Sale 2021: Freeship 0Đ + Voucher 8K, 88K, 888K', N'#', N'#', 4, 1, 0, 0, CAST(0.0 AS Decimal(5, 1)), NULL, CAST(N'2021-04-04T00:00:00.000' AS DateTime), NULL, NULL, N'#', N'Shopee Tết Sale 2021: Freeship 0Đ + Voucher 8K, 88K, 888K', N'Shopee Tết Sale 2021: Freeship 0Đ + Voucher 8K, 88K, 888K', N'Shopee Tết Sale 2021: Freeship 0Đ + Voucher 8K, 88K, 888K', 1)
GO
SET IDENTITY_INSERT [dbo].[Article] OFF
GO
SET IDENTITY_INSERT [dbo].[ArticleCategory] ON 
GO
INSERT [dbo].[ArticleCategory] ([ID], [Title], [Description], [Status], [Position], [MetaTitle], [MetaDescription], [MetaKeywords], [Schemas], [CreateTime], [CreateBy], [ModifyTime], [ModifyBy], [ParentID]) VALUES (1, N'Tin khuyến mãi', N'Các chương trình khuyến mãi', 1, 1, N'Tin khuyến mãi', N'Các chương trình khuyến mãi', N'khuyến mãi, giảm giá, săn sale', N'#', CAST(N'2021-04-07T07:38:03.083' AS DateTime), NULL, CAST(N'2021-04-07T07:38:03.083' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[ArticleCategory] ([ID], [Title], [Description], [Status], [Position], [MetaTitle], [MetaDescription], [MetaKeywords], [Schemas], [CreateTime], [CreateBy], [ModifyTime], [ModifyBy], [ParentID]) VALUES (2, N'Tiêu dùng', N'Tin tức tiêu dùng', 1, 1, N'Tiêu dùng', N'Tin tức tiêu dùng', N'tiêu dùng, giảm giá, săn sale', N'#', CAST(N'2021-04-07T07:38:03.083' AS DateTime), NULL, CAST(N'2021-04-07T07:38:03.083' AS DateTime), NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[ArticleCategory] OFF
GO
SET IDENTITY_INSERT [dbo].[Assess] ON 
GO
INSERT [dbo].[Assess] ([ID], [FullName], [Content], [Note], [NumberStar], [KeyID], [KeyName], [CreateTime], [CreateBy], [ModifyBy], [ModifyTime], [AccountID]) VALUES (1, N'Nguyễn Văn A', N'Sản phẩm chất lượng', N'Cần ra nhiều mã hơn', 5, 2, N'Product', CAST(N'2021-04-06T04:17:25.713' AS DateTime), NULL, NULL, CAST(N'2021-04-06T04:17:25.713' AS DateTime), 3)
GO
INSERT [dbo].[Assess] ([ID], [FullName], [Content], [Note], [NumberStar], [KeyID], [KeyName], [CreateTime], [CreateBy], [ModifyBy], [ModifyTime], [AccountID]) VALUES (2, N'Huỳnh Văn B', N'Tạm chấp nhận', N'#', 4, 2, N'Product', CAST(N'2021-04-06T04:17:25.713' AS DateTime), NULL, NULL, CAST(N'2021-04-06T04:17:25.713' AS DateTime), 3)
GO
INSERT [dbo].[Assess] ([ID], [FullName], [Content], [Note], [NumberStar], [KeyID], [KeyName], [CreateTime], [CreateBy], [ModifyBy], [ModifyTime], [AccountID]) VALUES (3, N'Lê Thị C', N'Tạm chấp nhận', N'#', 4, 2, N'Product', CAST(N'2021-04-06T04:17:25.713' AS DateTime), NULL, NULL, CAST(N'2021-04-06T04:17:25.713' AS DateTime), 3)
GO
INSERT [dbo].[Assess] ([ID], [FullName], [Content], [Note], [NumberStar], [KeyID], [KeyName], [CreateTime], [CreateBy], [ModifyBy], [ModifyTime], [AccountID]) VALUES (4, N'Trương văn D', N'Good!', N'Cần ra nhiều mã giảm giá hơn', 5, 2, N'Product', CAST(N'2021-04-06T10:28:02.057' AS DateTime), NULL, NULL, CAST(N'2021-04-06T10:28:02.057' AS DateTime), 4)
GO
INSERT [dbo].[Assess] ([ID], [FullName], [Content], [Note], [NumberStar], [KeyID], [KeyName], [CreateTime], [CreateBy], [ModifyBy], [ModifyTime], [AccountID]) VALUES (5, N'Trương văn E', N'Good!', N'Cần ra nhiều mã giảm giá hơn', 5, 2, N'Product', CAST(N'2021-04-06T10:28:02.057' AS DateTime), NULL, NULL, CAST(N'2021-04-06T10:28:02.057' AS DateTime), 4)
GO
INSERT [dbo].[Assess] ([ID], [FullName], [Content], [Note], [NumberStar], [KeyID], [KeyName], [CreateTime], [CreateBy], [ModifyBy], [ModifyTime], [AccountID]) VALUES (6, N'#', N'#', N'#', 5, 2, N'Product', CAST(N'2021-04-06T10:42:11.057' AS DateTime), NULL, NULL, CAST(N'2021-04-06T10:42:11.057' AS DateTime), 3)
GO
INSERT [dbo].[Assess] ([ID], [FullName], [Content], [Note], [NumberStar], [KeyID], [KeyName], [CreateTime], [CreateBy], [ModifyBy], [ModifyTime], [AccountID]) VALUES (7, N'#1', N'tạm được', N'cố gắng cải thiện', 3, 2, N'Product', CAST(N'2021-04-06T10:49:19.130' AS DateTime), NULL, NULL, CAST(N'2021-04-06T10:49:19.130' AS DateTime), 3)
GO
SET IDENTITY_INSERT [dbo].[Assess] OFF
GO
SET IDENTITY_INSERT [dbo].[Banner] ON 
GO
INSERT [dbo].[Banner] ([ID], [Title], [Avatar], [Thumb], [Url], [Status], [CreateBy], [CreateTime], [ModifyBy], [ModifyTime]) VALUES (1, N'Săn DEAL Bia chill tại nhà', N'https://www.vietcoupon.vnsosoft.com/FileUploads/Images/banner-1.jpg', N'https://www.vietcoupon.vnsosoft.com/FileUploads/Images/banner-1.jpg', N'#', 1, NULL, CAST(N'2021-04-07T07:24:11.757' AS DateTime), NULL, CAST(N'2021-04-07T07:24:11.757' AS DateTime))
GO
INSERT [dbo].[Banner] ([ID], [Title], [Avatar], [Thumb], [Url], [Status], [CreateBy], [CreateTime], [ModifyBy], [ModifyTime]) VALUES (2, N'Hội sách online tháng 3, nhập mã giảm 10%, săn sách đồng giá', N'https://www.vietcoupon.vnsosoft.com/FileUploads/Images/banner-2.jpg', N'https://www.vietcoupon.vnsosoft.com/FileUploads/Images/banner-1.jpg', N'#', 1, NULL, CAST(N'2021-04-07T07:24:11.757' AS DateTime), NULL, CAST(N'2021-04-07T07:24:11.757' AS DateTime))
GO
INSERT [dbo].[Banner] ([ID], [Title], [Avatar], [Thumb], [Url], [Status], [CreateBy], [CreateTime], [ModifyBy], [ModifyTime]) VALUES (3, N'Đặt vé máy bay coupon giảm ngày 5%', N'https://www.vietcoupon.vnsosoft.com/FileUploads/Images/banner-3.jpg', N'https://www.vietcoupon.vnsosoft.com/FileUploads/Images/banner-3.jpg', N'#', 1, NULL, CAST(N'2021-04-07T07:24:11.757' AS DateTime), NULL, CAST(N'2021-04-07T07:24:11.757' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[Banner] OFF
GO
SET IDENTITY_INSERT [dbo].[Contact] ON 
GO
INSERT [dbo].[Contact] ([ID], [ApproveBy], [FullName], [Email], [Address], [Mobi], [Subject], [Content], [Status], [CreateTime], [ModifyBy], [ModifyTime]) VALUES (1, NULL, NULL, N'abc@gmail.com', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Contact] ([ID], [ApproveBy], [FullName], [Email], [Address], [Mobi], [Subject], [Content], [Status], [CreateTime], [ModifyBy], [ModifyTime]) VALUES (2, NULL, NULL, N'abc@gmail.com', NULL, NULL, NULL, N'Contact me', NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Contact] ([ID], [ApproveBy], [FullName], [Email], [Address], [Mobi], [Subject], [Content], [Status], [CreateTime], [ModifyBy], [ModifyTime]) VALUES (3, NULL, NULL, N'abc@gmail.com', NULL, N'0354018013', NULL, N'Contact me', NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Contact] ([ID], [ApproveBy], [FullName], [Email], [Address], [Mobi], [Subject], [Content], [Status], [CreateTime], [ModifyBy], [ModifyTime]) VALUES (4, NULL, NULL, N'abc@gmail.com', NULL, N'0354018013', N'something goes wrong', N'Contact me', NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Contact] ([ID], [ApproveBy], [FullName], [Email], [Address], [Mobi], [Subject], [Content], [Status], [CreateTime], [ModifyBy], [ModifyTime]) VALUES (5, NULL, N'test', N'abc@gmail.com', NULL, N'0354018013', N'something goes wrong', N'Contact me', NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Contact] ([ID], [ApproveBy], [FullName], [Email], [Address], [Mobi], [Subject], [Content], [Status], [CreateTime], [ModifyBy], [ModifyTime]) VALUES (6, NULL, NULL, N'test@gmail.com', NULL, N'0354019013', NULL, N'Amazing!', NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Contact] ([ID], [ApproveBy], [FullName], [Email], [Address], [Mobi], [Subject], [Content], [Status], [CreateTime], [ModifyBy], [ModifyTime]) VALUES (7, NULL, NULL, N'test@gmail.com', NULL, N'0354019013', NULL, N'Amazing!', NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Contact] ([ID], [ApproveBy], [FullName], [Email], [Address], [Mobi], [Subject], [Content], [Status], [CreateTime], [ModifyBy], [ModifyTime]) VALUES (8, NULL, NULL, N'test@gmail.com', NULL, N'0354019013', NULL, N'Amazing!', NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Contact] ([ID], [ApproveBy], [FullName], [Email], [Address], [Mobi], [Subject], [Content], [Status], [CreateTime], [ModifyBy], [ModifyTime]) VALUES (9, NULL, NULL, N'test@gmail.com', NULL, N'0354019013', NULL, N'Amazing!', NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Contact] ([ID], [ApproveBy], [FullName], [Email], [Address], [Mobi], [Subject], [Content], [Status], [CreateTime], [ModifyBy], [ModifyTime]) VALUES (10, NULL, NULL, N'test@gmail.com', NULL, N'0354019013', NULL, N'Amazing!', NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Contact] ([ID], [ApproveBy], [FullName], [Email], [Address], [Mobi], [Subject], [Content], [Status], [CreateTime], [ModifyBy], [ModifyTime]) VALUES (11, NULL, NULL, N'test@gmail.com', NULL, N'0354019013', NULL, N'Amazing!', NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Contact] ([ID], [ApproveBy], [FullName], [Email], [Address], [Mobi], [Subject], [Content], [Status], [CreateTime], [ModifyBy], [ModifyTime]) VALUES (12, NULL, NULL, N'test@gmail.com', NULL, N'0354019013', NULL, N'Amazing!', NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Contact] ([ID], [ApproveBy], [FullName], [Email], [Address], [Mobi], [Subject], [Content], [Status], [CreateTime], [ModifyBy], [ModifyTime]) VALUES (13, NULL, NULL, N'test@gmail.com', NULL, N'0354019013', NULL, N'Amazing!', NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Contact] ([ID], [ApproveBy], [FullName], [Email], [Address], [Mobi], [Subject], [Content], [Status], [CreateTime], [ModifyBy], [ModifyTime]) VALUES (14, NULL, NULL, N'test@gmail.com', NULL, N'0354019013', NULL, N'Amazing!', NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Contact] ([ID], [ApproveBy], [FullName], [Email], [Address], [Mobi], [Subject], [Content], [Status], [CreateTime], [ModifyBy], [ModifyTime]) VALUES (15, NULL, NULL, N'test@gmail.com', NULL, N'0354019013', NULL, N'Amazing!', NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Contact] ([ID], [ApproveBy], [FullName], [Email], [Address], [Mobi], [Subject], [Content], [Status], [CreateTime], [ModifyBy], [ModifyTime]) VALUES (16, NULL, NULL, N'test@gmail.com', NULL, N'0354019013', NULL, N'Amazing!', NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Contact] ([ID], [ApproveBy], [FullName], [Email], [Address], [Mobi], [Subject], [Content], [Status], [CreateTime], [ModifyBy], [ModifyTime]) VALUES (17, NULL, NULL, N'test@gmail.com', NULL, N'0354019013', NULL, N'Amazing!', NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Contact] ([ID], [ApproveBy], [FullName], [Email], [Address], [Mobi], [Subject], [Content], [Status], [CreateTime], [ModifyBy], [ModifyTime]) VALUES (18, NULL, NULL, N'test@gmail.com', NULL, N'0354019013', NULL, N'Amazing!', NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Contact] ([ID], [ApproveBy], [FullName], [Email], [Address], [Mobi], [Subject], [Content], [Status], [CreateTime], [ModifyBy], [ModifyTime]) VALUES (19, NULL, NULL, N'test@gmail.com', NULL, N'0354019013', NULL, N'Amazing!', NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Contact] ([ID], [ApproveBy], [FullName], [Email], [Address], [Mobi], [Subject], [Content], [Status], [CreateTime], [ModifyBy], [ModifyTime]) VALUES (20, NULL, NULL, N'test@gmail.com', NULL, N'0354019013', NULL, N'Amazing!', NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Contact] ([ID], [ApproveBy], [FullName], [Email], [Address], [Mobi], [Subject], [Content], [Status], [CreateTime], [ModifyBy], [ModifyTime]) VALUES (21, NULL, NULL, N'test@gmail.com', NULL, N'0354019013', NULL, N'Amazing!', NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Contact] ([ID], [ApproveBy], [FullName], [Email], [Address], [Mobi], [Subject], [Content], [Status], [CreateTime], [ModifyBy], [ModifyTime]) VALUES (22, NULL, NULL, N'test@gmail.com', NULL, N'0354019013', NULL, N'Amazing!', NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Contact] ([ID], [ApproveBy], [FullName], [Email], [Address], [Mobi], [Subject], [Content], [Status], [CreateTime], [ModifyBy], [ModifyTime]) VALUES (23, NULL, NULL, N'test@gmail.com', NULL, N'0354019013', NULL, N'Amazing!', NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Contact] ([ID], [ApproveBy], [FullName], [Email], [Address], [Mobi], [Subject], [Content], [Status], [CreateTime], [ModifyBy], [ModifyTime]) VALUES (24, NULL, NULL, N'test@gmail.com', NULL, N'0354019013', NULL, N'Amazing!', NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Contact] ([ID], [ApproveBy], [FullName], [Email], [Address], [Mobi], [Subject], [Content], [Status], [CreateTime], [ModifyBy], [ModifyTime]) VALUES (25, NULL, NULL, NULL, NULL, NULL, NULL, N'Ko rnh', NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Contact] ([ID], [ApproveBy], [FullName], [Email], [Address], [Mobi], [Subject], [Content], [Status], [CreateTime], [ModifyBy], [ModifyTime]) VALUES (26, NULL, NULL, NULL, NULL, NULL, NULL, N'satisfied ', NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Contact] ([ID], [ApproveBy], [FullName], [Email], [Address], [Mobi], [Subject], [Content], [Status], [CreateTime], [ModifyBy], [ModifyTime]) VALUES (27, NULL, NULL, NULL, NULL, NULL, NULL, N'satisfied ', NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Contact] ([ID], [ApproveBy], [FullName], [Email], [Address], [Mobi], [Subject], [Content], [Status], [CreateTime], [ModifyBy], [ModifyTime]) VALUES (28, NULL, NULL, NULL, NULL, NULL, NULL, N'a', NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Contact] ([ID], [ApproveBy], [FullName], [Email], [Address], [Mobi], [Subject], [Content], [Status], [CreateTime], [ModifyBy], [ModifyTime]) VALUES (29, NULL, NULL, NULL, NULL, NULL, NULL, N'a', NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Contact] ([ID], [ApproveBy], [FullName], [Email], [Address], [Mobi], [Subject], [Content], [Status], [CreateTime], [ModifyBy], [ModifyTime]) VALUES (30, NULL, NULL, NULL, NULL, NULL, NULL, N'abc', NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Contact] ([ID], [ApproveBy], [FullName], [Email], [Address], [Mobi], [Subject], [Content], [Status], [CreateTime], [ModifyBy], [ModifyTime]) VALUES (31, NULL, NULL, NULL, NULL, NULL, NULL, N'abc', NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Contact] ([ID], [ApproveBy], [FullName], [Email], [Address], [Mobi], [Subject], [Content], [Status], [CreateTime], [ModifyBy], [ModifyTime]) VALUES (32, NULL, NULL, NULL, NULL, NULL, NULL, N'a', NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Contact] ([ID], [ApproveBy], [FullName], [Email], [Address], [Mobi], [Subject], [Content], [Status], [CreateTime], [ModifyBy], [ModifyTime]) VALUES (33, NULL, NULL, NULL, NULL, NULL, NULL, N'a', NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Contact] ([ID], [ApproveBy], [FullName], [Email], [Address], [Mobi], [Subject], [Content], [Status], [CreateTime], [ModifyBy], [ModifyTime]) VALUES (34, NULL, NULL, NULL, NULL, NULL, NULL, N'a', NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Contact] ([ID], [ApproveBy], [FullName], [Email], [Address], [Mobi], [Subject], [Content], [Status], [CreateTime], [ModifyBy], [ModifyTime]) VALUES (35, NULL, NULL, NULL, NULL, NULL, NULL, N'a', NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Contact] ([ID], [ApproveBy], [FullName], [Email], [Address], [Mobi], [Subject], [Content], [Status], [CreateTime], [ModifyBy], [ModifyTime]) VALUES (36, NULL, NULL, NULL, NULL, NULL, NULL, N'a', NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Contact] ([ID], [ApproveBy], [FullName], [Email], [Address], [Mobi], [Subject], [Content], [Status], [CreateTime], [ModifyBy], [ModifyTime]) VALUES (37, NULL, NULL, NULL, NULL, NULL, NULL, N'ok', NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Contact] ([ID], [ApproveBy], [FullName], [Email], [Address], [Mobi], [Subject], [Content], [Status], [CreateTime], [ModifyBy], [ModifyTime]) VALUES (42, 2, N'string', N'string', N'string', N'string', N'string', N'string', 1, CAST(N'2021-04-05T17:43:25.837' AS DateTime), 2, CAST(N'2021-04-05T17:43:25.837' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[Contact] OFF
GO
SET IDENTITY_INSERT [dbo].[Customer] ON 
GO
INSERT [dbo].[Customer] ([ID], [Email], [FullName], [Gender], [Birthday], [Avatar], [Phone], [Tel], [Address], [CreateBy], [CreateTime], [ModifyBy], [ModifyTime], [CustomerTypeID], [NumberTakeCare], [CompanyName], [AccoutID], [Point], [ReferralCode], [Code]) VALUES (1, N'nguyenvan@gmail.com', N'Nguyễn Văn A', 1, NULL, NULL, N'0369988998', NULL, N'tp.hcm', NULL, CAST(N'2021-04-06T12:40:47.247' AS DateTime), NULL, NULL, NULL, NULL, NULL, 3, NULL, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[Customer] OFF
GO
SET IDENTITY_INSERT [dbo].[CustomerType] ON 
GO
INSERT [dbo].[CustomerType] ([ID], [Title], [Description], [Status], [CreateBy], [CreateTime], [ModifyBy], [ModifyTime], [Position]) VALUES (1, N'Thường', N'Thành viên thường', 1, 0, CAST(N'2021-04-06T16:32:13.160' AS DateTime), 0, CAST(N'2021-04-06T16:32:13.160' AS DateTime), 0)
GO
INSERT [dbo].[CustomerType] ([ID], [Title], [Description], [Status], [CreateBy], [CreateTime], [ModifyBy], [ModifyTime], [Position]) VALUES (2, N'Bạc', N'Thành viên bạc', 1, 0, CAST(N'2021-04-06T16:32:13.160' AS DateTime), 0, CAST(N'2021-04-06T16:32:13.160' AS DateTime), 0)
GO
INSERT [dbo].[CustomerType] ([ID], [Title], [Description], [Status], [CreateBy], [CreateTime], [ModifyBy], [ModifyTime], [Position]) VALUES (3, N'Vàng', N'Thành viên vàng', 1, 0, CAST(N'2021-04-06T16:32:13.160' AS DateTime), 0, CAST(N'2021-04-06T16:32:13.160' AS DateTime), 0)
GO
SET IDENTITY_INSERT [dbo].[CustomerType] OFF
GO
SET IDENTITY_INSERT [dbo].[Feedback] ON 
GO
INSERT [dbo].[Feedback] ([ID], [FullName], [Avatar], [Thumb], [Content], [Regency], [Status], [CreateBy], [CreateTime], [ModifyBy], [ModifyTime], [IsDelete]) VALUES (3, N'string', N'string', N'string', N'string', N'string', 1, 2, CAST(N'2021-04-05T17:34:57.407' AS DateTime), 0, CAST(N'2021-04-05T17:34:57.407' AS DateTime), 1)
GO
SET IDENTITY_INSERT [dbo].[Feedback] OFF
GO
SET IDENTITY_INSERT [dbo].[GroupFunction] ON 
GO
INSERT [dbo].[GroupFunction] ([ID], [Title], [Description], [Status], [CreateBy], [CreateTime], [ModifyBy], [ModifyTime], [IsDelete]) VALUES (1, N'Thêm danh sách', N'string', 0, 2, CAST(N'2021-04-06T15:16:08.147' AS DateTime), 2, CAST(N'2021-04-06T15:16:08.147' AS DateTime), 1)
GO
INSERT [dbo].[GroupFunction] ([ID], [Title], [Description], [Status], [CreateBy], [CreateTime], [ModifyBy], [ModifyTime], [IsDelete]) VALUES (2, N'Xoá danh sách', N'string', 1, 2, CAST(N'2021-04-06T15:13:01.323' AS DateTime), 0, CAST(N'2021-04-06T15:13:01.323' AS DateTime), 0)
GO
INSERT [dbo].[GroupFunction] ([ID], [Title], [Description], [Status], [CreateBy], [CreateTime], [ModifyBy], [ModifyTime], [IsDelete]) VALUES (3, N'Sửa danh sách', N'string', 1, 2, CAST(N'2021-04-06T15:13:01.323' AS DateTime), 0, CAST(N'2021-04-06T15:13:01.323' AS DateTime), 0)
GO
SET IDENTITY_INSERT [dbo].[GroupFunction] OFF
GO
SET IDENTITY_INSERT [dbo].[Module] ON 
GO
INSERT [dbo].[Module] ([ID], [Title], [Description], [Position], [Status], [CreateBy], [CreateTime], [ModifyBy], [ModifyTime]) VALUES (2, N'Xoá tin tức', N'string', 0, 1, 2, CAST(N'2021-04-06T15:28:39.193' AS DateTime), 0, CAST(N'2021-04-06T15:28:39.193' AS DateTime))
GO
INSERT [dbo].[Module] ([ID], [Title], [Description], [Position], [Status], [CreateBy], [CreateTime], [ModifyBy], [ModifyTime]) VALUES (3, N'Sửa tin tức', N'string', 0, 1, 2, CAST(N'2021-04-06T15:28:39.193' AS DateTime), 0, CAST(N'2021-04-06T15:28:39.193' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[Module] OFF
GO
SET IDENTITY_INSERT [dbo].[Order] ON 
GO
INSERT [dbo].[Order] ([ID], [Code], [TotalPrice], [FullName], [Email], [Mobi], [Address], [Status], [IsDelete], [Remark], [PayTypeID], [CustomerID], [NoteSale], [SaleOff], [NoteFeeShip], [OrderStatusID], [CreateBy], [CreateTime], [ModifyBy], [ModifyTime], [IsPoint], [Point]) VALUES (5, N'#1', CAST(1000000.00 AS Decimal(18, 2)), N'Nguyễn Văn A', N'nguyenvan@gmail.com', N'0369988998', N'TP.HCM', 1, 0, N'#', 1, 1, N'#', CAST(0.00 AS Decimal(18, 2)), N'#', 1, NULL, CAST(N'2021-04-06T07:05:24.933' AS DateTime), NULL, CAST(N'2021-04-06T07:05:24.933' AS DateTime), 1, 0)
GO
SET IDENTITY_INSERT [dbo].[Order] OFF
GO
SET IDENTITY_INSERT [dbo].[OrderDetail] ON 
GO
INSERT [dbo].[OrderDetail] ([ID], [OrderID], [ProductID], [Quantity], [Price], [OriginalPrice], [Option]) VALUES (13, 5, 2, 1, CAST(100000.00 AS Decimal(18, 2)), CAST(100000.00 AS Decimal(18, 2)), N'#')
GO
INSERT [dbo].[OrderDetail] ([ID], [OrderID], [ProductID], [Quantity], [Price], [OriginalPrice], [Option]) VALUES (14, 5, 3, 2, CAST(50000.00 AS Decimal(18, 2)), CAST(50000.00 AS Decimal(18, 2)), N'#')
GO
INSERT [dbo].[OrderDetail] ([ID], [OrderID], [ProductID], [Quantity], [Price], [OriginalPrice], [Option]) VALUES (15, 5, 4, 1, CAST(50000.00 AS Decimal(18, 2)), CAST(50000.00 AS Decimal(18, 2)), N'#')
GO
SET IDENTITY_INSERT [dbo].[OrderDetail] OFF
GO
SET IDENTITY_INSERT [dbo].[OrderStatus] ON 
GO
INSERT [dbo].[OrderStatus] ([ID], [Title], [Description], [Position], [Status], [IsDelete], [CreateBy], [CreateTime], [ModifyBy], [ModifyTime]) VALUES (1, N'Chờ thanh toán', N'chờ thanh toán đơn hàng', 1, 1, 0, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[OrderStatus] ([ID], [Title], [Description], [Position], [Status], [IsDelete], [CreateBy], [CreateTime], [ModifyBy], [ModifyTime]) VALUES (2, N'Đã thanh toán', N'đơn hàng đã thanh toán', 1, 1, 0, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[OrderStatus] ([ID], [Title], [Description], [Position], [Status], [IsDelete], [CreateBy], [CreateTime], [ModifyBy], [ModifyTime]) VALUES (3, N'Đã hủy', N'đơn hàng đã bị hủy', 2, 1, 0, NULL, NULL, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[OrderStatus] OFF
GO
SET IDENTITY_INSERT [dbo].[PageType] ON 
GO
INSERT [dbo].[PageType] ([ID], [Title], [Description], [Status], [Position], [CreateBy], [CreateTime], [ModifyBy], [ModifyTime]) VALUES (1, N'Cửa hàng tiện lợi', N'Cửa hàng tiện lợi', 1, 0, 0, CAST(N'2021-04-09T15:39:32.270' AS DateTime), 0, CAST(N'2021-04-09T15:39:32.270' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[PageType] OFF
GO
SET IDENTITY_INSERT [dbo].[Partner] ON 
GO
INSERT [dbo].[Partner] ([ID], [Url], [Title], [Avatar], [CreateTime], [CreateBy], [Status], [Position], [Description], [ModifyTime], [ModifyBy], [PartnerTypeID], [AccountID], [Thumb], [Content], [Banner], [ViewTime], [TotalAssess], [ValueAssess]) VALUES (4, N'#', N'Circle K', N'https://www.vietcoupon.vnsosoft.com/FileUploads/Images/circle-k.jpg', CAST(N'2021-04-09T00:00:00.000' AS DateTime), 2, 1, 1, N'Chuỗi cửa hàng tiện lợi - Mở cửa 24/7', CAST(N'2021-04-09T00:00:00.000' AS DateTime), 2, 1, 5, N'https://www.vietcoupon.vnsosoft.com/FileUploads/Images/circle-k.jpg', N'Mua sắm Circle K tiện lợi cùng Ví MoMo
Bắt đầu từ năm 1951 tại bang Texas, Mỹ, tới nay, Circle K đã trở thành một trong những thương hiệu cửa hàng tiện lợi uy tín rộng khắp, nổi tiếng vì chất lượng sản phẩm và dịch vụ chăm sóc khách hàng tuyệt vời với hơn 16.000 cửa hàng trên toàn thế giới.

Tại Việt Nam, Circle K chú trọng vào việc phát triển nhanh chóng trong lĩnh vực kinh doanh chuỗi cửa hàng tiện lợi theo giấy phép nhượng quyền thương hiệu của Circle K Mỹ. Circle K tự hào là chuỗi cửa hàng tiện lợi quốc tế đầu tiên tại Việt Nam.

Circle K Việt Nam hiện đã có hơn 300 cửa hàng tại các thành phố lớn như Hà Nội, Hạ Long, Hồ Chí Minh, Vũng Tàu, và sẽ còn tiếp tục phát triển để đáp ứng nhu cầu của khách hàng ở khắp mọi nơi.

Cam Kết Dịch Vụ của Circle K đối với khách hàng được gói gọn trong 4 chữ F (4Fs) (Tươi, Thân Thiện, Nhanh, và Đầy Đủ).

Circle K là một trong những đối tác quan trọng của Ví MoMo. Hiện tại, khách hàng đã có thể thanh toán bằng Ví MoMo tại các cửa hàng của Circle K, góp phần làm cho việc thanh toán trở nên nhanh chóng và tiện lợi hơn bao giờ hết.', N'https://www.vietcoupon.vnsosoft.com/FileUploads/Images/banner-circle-k.jpg', 0, 0, CAST(0.0 AS Decimal(5, 1)))
GO
INSERT [dbo].[Partner] ([ID], [Url], [Title], [Avatar], [CreateTime], [CreateBy], [Status], [Position], [Description], [ModifyTime], [ModifyBy], [PartnerTypeID], [AccountID], [Thumb], [Content], [Banner], [ViewTime], [TotalAssess], [ValueAssess]) VALUES (5, N'#', N'Ministop', N'https://www.vietcoupon.vnsosoft.com/FileUploads/Images/ministop.jpg', CAST(N'2021-04-09T00:00:00.000' AS DateTime), 2, 1, 1, N'Chuỗi cửa hàng tiện lợi của tập đoàn AEON', CAST(N'2021-04-09T00:00:00.000' AS DateTime), 2, 1, 6, N'https://www.vietcoupon.vnsosoft.com/FileUploads/Images/ministop.jpg', N'Khám phá cửa hàng tiện lợi Nhật Bản Ministop cùng Ví MoMo
Với sứ mệnh xây dựng xã hội tràn ngập nụ cười bằng sự “tươi ngon”, “thân thiện”, “tiện lợi”, MINISTOP chăm chút việc cung cấp từng sản phẩm đến Quý khách hàng, đặt mục tiêu trở thành cửa hàng không thể thiếu cho cuộc sống hằng ngày của Quý khách.

MINISTOP không ngừng phấn đấu hoàn thiện cơ cấu hàng hóa với các sản phẩm cần thiết cho đời sống thường nhật để trở thành cửa hàng tiện lợi được Quý khách hàng lựa chọn sử dụng hàng ngày.

Ngoài ra, MINISTOP còn trang bị khu vực ăn uống trong cửa hàng COMBO, mô hình kết hợp giữa cửa hàng tiện lợi và cửa hàng thức ăn nhanh chế biến tại chỗ để trở thành cửa hàng phù hợp với lối sống của Quý khách và để MINISTOP trở thành nơi Quý khách hàng có thể trò chuyện thư giãn và thưởng thức các món ăn ngon.', N'https://www.vietcoupon.vnsosoft.com/FileUploads/Images/banner-ministop.jpg', 0, 0, CAST(0.0 AS Decimal(5, 1)))
GO
INSERT [dbo].[Partner] ([ID], [Url], [Title], [Avatar], [CreateTime], [CreateBy], [Status], [Position], [Description], [ModifyTime], [ModifyBy], [PartnerTypeID], [AccountID], [Thumb], [Content], [Banner], [ViewTime], [TotalAssess], [ValueAssess]) VALUES (6, N'#', N'Co.opmart', N'https://www.vietcoupon.vnsosoft.com/FileUploads/Images/coopmart.jpg', CAST(N'2021-04-09T00:00:00.000' AS DateTime), 2, 1, 1, N'Chuỗi siêu thị bán lẻ lớn nhất Việt Nam', CAST(N'2021-04-09T00:00:00.000' AS DateTime), 2, 2, 7, N'https://www.vietcoupon.vnsosoft.com/FileUploads/Images/coopmart.jpg', N'Mua sắm cực tiện lợi tại Co.op Mart - Vì đã có Ví MoMo
Siêu thị Co.opmart được coi như lựa chọn hàng đầu của các bà nội trợ tại Việt Nam khi có nhu cầu mua sắm hàng tiêu dùng, không những thế khi đến Co.opmart người tiêu dùng đã có thể thanh toán một chạm bằng Ví MoMo - Siêu ứng dụng thanh toán số 1 Việt Nam. 

Co.opmart (còn được gọi là Co.op Mart, Co-opmart, Coopmart) là một hệ thống siêu thị bán lẻ của Việt Nam trực thuộc Liên hiệp các Hợp tác xã Thương mại Thành phố Hồ Chí Minh (Saigon Co.op). Tính đến 04/2016, hệ thống Co.opmart có 82 siêu thị bao gồm 32 Co.opmart ở TPHCM và 50 Co.opmart tại các tỉnh/thành cả nước. ', N'https://www.vietcoupon.vnsosoft.com/FileUploads/Images/banner-coopmart.jpg', 0, 0, CAST(0.0 AS Decimal(5, 1)))
GO
INSERT [dbo].[Partner] ([ID], [Url], [Title], [Avatar], [CreateTime], [CreateBy], [Status], [Position], [Description], [ModifyTime], [ModifyBy], [PartnerTypeID], [AccountID], [Thumb], [Content], [Banner], [ViewTime], [TotalAssess], [ValueAssess]) VALUES (7, N'#', N'The Coffee House', N'https://www.vietcoupon.vnsosoft.com/FileUploads/Images/the-coffee-house.jpg', CAST(N'2021-04-09T00:00:00.000' AS DateTime), 2, 1, 1, N'Chuỗi cửa hàng cà phê, trà & bánh', CAST(N'2021-04-09T00:00:00.000' AS DateTime), 2, 4, 8, N'https://www.vietcoupon.vnsosoft.com/FileUploads/Images/the-coffee-house.jpg', N'Dùng nước tại The Coffee House - Thanh toán thả ga với Ví MoMo
The Coffee House là chuỗi cửa hàng cà phê được thành lập từ năm 2014. Sau 5 năm hoạt động, The Coffee House đã vươn lên trở thành một trong những chuỗi cửa hàng cà phê lớn nhất Việt Nam, với hơn 100 cửa hàng trên khắp các tỉnh thành, phục vụ hơn 40.000 khách hàng mỗi ngày và tiêu tốn khoảng 300 tấn cà phê trong năm 2018.

"Cà phê nhé" - Một lời hẹn rất riêng của người Việt. Một lời ngỏ mộc mạc để mình ngồi lại bên nhau và sẻ chia câu chuyện của riêng mình.

Tại The Coffee House, chuỗi cửa hàng cà phê này luôn trân trọng những câu chuyện và đề cao giá trị Kết nối con người. Với mong muốn The Coffee House sẽ trở thành “Nhà Cà Phê", nơi mọi người xích lại gần nhau và tìm thấy niềm vui, sự sẻ chia thân tình bên những tách cà phê đượm hương, chất lượng.', N'https://www.vietcoupon.vnsosoft.com/FileUploads/Images/banner-the-coffee-house.jpg', 0, 0, CAST(0.0 AS Decimal(5, 1)))
GO
INSERT [dbo].[Partner] ([ID], [Url], [Title], [Avatar], [CreateTime], [CreateBy], [Status], [Position], [Description], [ModifyTime], [ModifyBy], [PartnerTypeID], [AccountID], [Thumb], [Content], [Banner], [ViewTime], [TotalAssess], [ValueAssess]) VALUES (8, N'#', N'Gà Nướng Ò Ó O', N'https://www.vietcoupon.vnsosoft.com/FileUploads/Images/o-o-o.jpg', CAST(N'2021-04-09T00:00:00.000' AS DateTime), 2, 1, 1, N'Nhà Hàng Lẩu & Gà Nướng Đủ Vị', CAST(N'2021-04-09T00:00:00.000' AS DateTime), 2, 4, 9, N'https://www.vietcoupon.vnsosoft.com/FileUploads/Images/o-o-o.jpg', N'Ăn Gà Nướng Ò Ó O cực phủ phê không lo quên ví - Vì đã có Ví MoMo
Món nướng – đặc biệt là gà nướng – là phần không thể thiếu trong bữa ăn hay tụ họp bạn bè của người Việt Nam. Tuần này, hãy đến nhà hàng Gà Nướng Ò Ó O để thưởng thức món ngon và thanh toán bằng Ví MoMo nhanh chóng, tiện lợi nhé! Nếu lần đầu tiên ăn uống tại nhà hàng Gà Nướng Ò Ó O, bạn nhất định không thể bỏ qua món gà nướng với mười hương vị đặc trưng ở đây.

Từ gà nướng vị Địa Trung Hải, gà nướng vị mắm tỏi phi đến gà nướng vị chanh dây mật ong, mỗi món ăn đều mang đến cung bậc cảm xúc khác nhau, làm hài lòng ngay cả thực khách sành ăn khó tính nhất. Bạn có thể dùng gà nướng với đa dạng các món ăn kèm khác, đơn cử như salad trộn dầu chanh, rong biển tươi sốt tương mè, kim chi, súp nấm rong biển, đậu nành non....

Nếu muốn làm mới vị giác, hãy gọi một phần lẩu gà lá giang nhé. Vị gà tươi hòa cùng nước lẩu chua cay sẽ là mở đầu tuyệt hảo cho buổi chuyện trò với người thân và bạn bè.', N'https://www.vietcoupon.vnsosoft.com/FileUploads/Images/banner-o-o-o.jpg', 0, 0, CAST(0.0 AS Decimal(5, 1)))
GO
SET IDENTITY_INSERT [dbo].[Partner] OFF
GO
SET IDENTITY_INSERT [dbo].[PartnerType] ON 
GO
INSERT [dbo].[PartnerType] ([ID], [Title], [Position], [Status], [CreateBy], [CreateTime], [ModifyBy], [ModifyTime]) VALUES (1, N'Cửa hàng tiện lợi', 1, 1, 2, CAST(N'2021-04-09T00:00:00.000' AS DateTime), 2, CAST(N'2021-04-09T00:00:00.000' AS DateTime))
GO
INSERT [dbo].[PartnerType] ([ID], [Title], [Position], [Status], [CreateBy], [CreateTime], [ModifyBy], [ModifyTime]) VALUES (2, N'Siêu thị', 1, 1, 2, CAST(N'2021-04-09T00:00:00.000' AS DateTime), 2, CAST(N'2021-04-09T00:00:00.000' AS DateTime))
GO
INSERT [dbo].[PartnerType] ([ID], [Title], [Position], [Status], [CreateBy], [CreateTime], [ModifyBy], [ModifyTime]) VALUES (3, N'Nhà hàng', 1, 1, 2, CAST(N'2021-04-09T00:00:00.000' AS DateTime), 2, CAST(N'2021-04-09T00:00:00.000' AS DateTime))
GO
INSERT [dbo].[PartnerType] ([ID], [Title], [Position], [Status], [CreateBy], [CreateTime], [ModifyBy], [ModifyTime]) VALUES (4, N'Cà phê - Trà sữa', 1, 1, 2, CAST(N'2021-04-09T00:00:00.000' AS DateTime), 2, CAST(N'2021-04-09T00:00:00.000' AS DateTime))
GO
INSERT [dbo].[PartnerType] ([ID], [Title], [Position], [Status], [CreateBy], [CreateTime], [ModifyBy], [ModifyTime]) VALUES (5, N'Mua sắm', 1, 1, 2, CAST(N'2021-04-09T00:00:00.000' AS DateTime), 2, CAST(N'2021-04-09T00:00:00.000' AS DateTime))
GO
INSERT [dbo].[PartnerType] ([ID], [Title], [Position], [Status], [CreateBy], [CreateTime], [ModifyBy], [ModifyTime]) VALUES (6, N'Mua sắm Online', 1, 1, 2, CAST(N'2021-04-09T00:00:00.000' AS DateTime), 2, CAST(N'2021-04-09T00:00:00.000' AS DateTime))
GO
INSERT [dbo].[PartnerType] ([ID], [Title], [Position], [Status], [CreateBy], [CreateTime], [ModifyBy], [ModifyTime]) VALUES (7, N'Giải trí', 1, 1, 2, CAST(N'2021-04-09T00:00:00.000' AS DateTime), 2, CAST(N'2021-04-09T00:00:00.000' AS DateTime))
GO
INSERT [dbo].[PartnerType] ([ID], [Title], [Position], [Status], [CreateBy], [CreateTime], [ModifyBy], [ModifyTime]) VALUES (8, N'Sức khỏe', 1, 1, 2, CAST(N'2021-04-09T00:00:00.000' AS DateTime), 2, CAST(N'2021-04-09T00:00:00.000' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[PartnerType] OFF
GO
SET IDENTITY_INSERT [dbo].[PayType] ON 
GO
INSERT [dbo].[PayType] ([ID], [Title], [Position], [Status], [CreateBy], [CreateTime], [ModifyBy], [ModifyTime]) VALUES (1, N'Chuyển khoản', 1, 1, NULL, NULL, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[PayType] OFF
GO
SET IDENTITY_INSERT [dbo].[Product] ON 
GO
INSERT [dbo].[Product] ([ID], [Code], [Title], [Description], [Content], [Avatar], [Thumb], [ImageListProduct], [Position], [MetaTitle], [MetaDescription], [MetaKeywords], [Alias], [Price], [Sale], [SaleDeadLine], [ViewTime], [StockStatus], [WishlistView], [SoldView], [Schemas], [TotalAssess], [ValueAssess], [Status], [IsDelete], [CreateTime], [ModifyTime], [CreateBy], [ModifyBy], [ProductCategoryID], [SaleID], [UnitID], [EffectiveDate], [ExpiryDate], [Point]) VALUES (2, N'M1', N'[Việt Nha] Tẩy trắng SUPPER WHITEMAX', N'E-voucher giảm giá áp dụng cho tất cả các sản phẩm dịch vụ trong thời gian khuyến mãi của Voucher trên BookingHotel', N'E-voucher giảm giá áp dụng cho tất cả các sản phẩm dịch vụ trong thời gian khuyến mãi của Voucher trên BookingHotel', N'~/FileUploads/Images/p-1.jpg', N'~/FileUploads/Images/pt-1.jpg', N'#', 1, N'E-voucher giảm giá áp dụng cho tất cả các sản phẩm dịch vụ trong thời gian khuyến mãi của Voucher trên BookingHotel', N'E-voucher giảm giá áp dụng cho tất cả các sản phẩm dịch vụ trong thời gian khuyến mãi của Voucher trên BookingHotel', N'E-voucher giảm giá áp dụng cho tất cả các sản phẩm dịch vụ trong thời gian khuyến mãi của Voucher trên BookingHotel', N'#', CAST(100000.00 AS Decimal(18, 2)), 0, NULL, NULL, NULL, NULL, NULL, NULL, 30, CAST(4.4 AS Decimal(5, 1)), 1, 0, CAST(N'2021-04-06T00:00:00.000' AS DateTime), NULL, NULL, NULL, 1, NULL, NULL, CAST(N'2021-04-01T00:00:00.000' AS DateTime), CAST(N'2021-04-30T00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[Product] ([ID], [Code], [Title], [Description], [Content], [Avatar], [Thumb], [ImageListProduct], [Position], [MetaTitle], [MetaDescription], [MetaKeywords], [Alias], [Price], [Sale], [SaleDeadLine], [ViewTime], [StockStatus], [WishlistView], [SoldView], [Schemas], [TotalAssess], [ValueAssess], [Status], [IsDelete], [CreateTime], [ModifyTime], [CreateBy], [ModifyBy], [ProductCategoryID], [SaleID], [UnitID], [EffectiveDate], [ExpiryDate], [Point]) VALUES (3, N'M2', N'[TOUS les JOURS] Giảm 200k cho toàn Menu', N'E-voucher giảm giá áp dụng cho tất cả các sản phẩm dịch vụ trong thời gian khuyến mãi của Voucher trên BookingHotel', N'E-voucher giảm giá áp dụng cho tất cả các sản phẩm dịch vụ trong thời gian khuyến mãi của Voucher trên BookingHotel', N'~/FileUploads/Images/p-2.jpg', N'~/FileUploads/Images/pt-2.jpg', N'#', 1, N'E-voucher giảm giá áp dụng cho tất cả các sản phẩm dịch vụ trong thời gian khuyến mãi của Voucher trên BookingHotel', N'E-voucher giảm giá áp dụng cho tất cả các sản phẩm dịch vụ trong thời gian khuyến mãi của Voucher trên BookingHotel', N'E-voucher giảm giá áp dụng cho tất cả các sản phẩm dịch vụ trong thời gian khuyến mãi của Voucher trên BookingHotel', N'#', CAST(500000.00 AS Decimal(18, 2)), 0, NULL, NULL, NULL, NULL, NULL, NULL, 44, CAST(4.6 AS Decimal(5, 1)), 1, 0, CAST(N'2021-04-06T00:00:00.000' AS DateTime), NULL, NULL, NULL, 4, NULL, NULL, CAST(N'2021-04-01T00:00:00.000' AS DateTime), CAST(N'2021-04-30T00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[Product] ([ID], [Code], [Title], [Description], [Content], [Avatar], [Thumb], [ImageListProduct], [Position], [MetaTitle], [MetaDescription], [MetaKeywords], [Alias], [Price], [Sale], [SaleDeadLine], [ViewTime], [StockStatus], [WishlistView], [SoldView], [Schemas], [TotalAssess], [ValueAssess], [Status], [IsDelete], [CreateTime], [ModifyTime], [CreateBy], [ModifyBy], [ProductCategoryID], [SaleID], [UnitID], [EffectiveDate], [ExpiryDate], [Point]) VALUES (4, N'M3', N'[SPALA CLINIC] Giảm 900k cho hóa đơn dịch vụ về Da từ 1 triệu', N'E-voucher giảm giá áp dụng cho tất cả các sản phẩm dịch vụ trong thời gian khuyến mãi của Voucher trên BookingHotel', N'E-voucher giảm giá áp dụng cho tất cả các sản phẩm dịch vụ trong thời gian khuyến mãi của Voucher trên BookingHotel', N'~/FileUploads/Images/p-3.jpg', N'~/FileUploads/Images/pt-3.jpg', N'#', 1, N'E-voucher giảm giá áp dụng cho tất cả các sản phẩm dịch vụ trong thời gian khuyến mãi của Voucher trên BookingHotel', N'E-voucher giảm giá áp dụng cho tất cả các sản phẩm dịch vụ trong thời gian khuyến mãi của Voucher trên BookingHotel', N'E-voucher giảm giá áp dụng cho tất cả các sản phẩm dịch vụ trong thời gian khuyến mãi của Voucher trên BookingHotel', N'#', CAST(500000.00 AS Decimal(18, 2)), 0, NULL, NULL, NULL, NULL, NULL, NULL, 101, CAST(4.9 AS Decimal(5, 1)), 1, 0, CAST(N'2021-04-06T00:00:00.000' AS DateTime), NULL, NULL, NULL, 4, NULL, NULL, CAST(N'2021-04-01T00:00:00.000' AS DateTime), CAST(N'2021-04-30T00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[Product] ([ID], [Code], [Title], [Description], [Content], [Avatar], [Thumb], [ImageListProduct], [Position], [MetaTitle], [MetaDescription], [MetaKeywords], [Alias], [Price], [Sale], [SaleDeadLine], [ViewTime], [StockStatus], [WishlistView], [SoldView], [Schemas], [TotalAssess], [ValueAssess], [Status], [IsDelete], [CreateTime], [ModifyTime], [CreateBy], [ModifyBy], [ProductCategoryID], [SaleID], [UnitID], [EffectiveDate], [ExpiryDate], [Point]) VALUES (5, N'M4', N'[Mishio Kachi] Nồi chiên không dầu Mishio Mk41', N'E-voucher giảm giá áp dụng cho tất cả các sản phẩm dịch vụ trong thời gian khuyến mãi của Voucher trên BookingHotel', N'E-voucher giảm giá áp dụng cho tất cả các sản phẩm dịch vụ trong thời gian khuyến mãi của Voucher trên BookingHotel', N'~/FileUploads/Images/p-4.jpg', N'~/FileUploads/Images/pt-4.jpg', N'#', 1, N'E-voucher giảm giá áp dụng cho tất cả các sản phẩm dịch vụ trong thời gian khuyến mãi của Voucher trên BookingHotel', N'E-voucher giảm giá áp dụng cho tất cả các sản phẩm dịch vụ trong thời gian khuyến mãi của Voucher trên BookingHotel', N'E-voucher giảm giá áp dụng cho tất cả các sản phẩm dịch vụ trong thời gian khuyến mãi của Voucher trên BookingHotel', N'#', CAST(779000.00 AS Decimal(18, 2)), 0, NULL, NULL, NULL, NULL, NULL, NULL, 68, CAST(4.0 AS Decimal(5, 1)), 1, 0, CAST(N'2021-04-06T00:00:00.000' AS DateTime), NULL, NULL, NULL, 4, NULL, NULL, CAST(N'2021-04-01T00:00:00.000' AS DateTime), CAST(N'2021-04-30T00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[Product] ([ID], [Code], [Title], [Description], [Content], [Avatar], [Thumb], [ImageListProduct], [Position], [MetaTitle], [MetaDescription], [MetaKeywords], [Alias], [Price], [Sale], [SaleDeadLine], [ViewTime], [StockStatus], [WishlistView], [SoldView], [Schemas], [TotalAssess], [ValueAssess], [Status], [IsDelete], [CreateTime], [ModifyTime], [CreateBy], [ModifyBy], [ProductCategoryID], [SaleID], [UnitID], [EffectiveDate], [ExpiryDate], [Point]) VALUES (6, N'M5', N'[Gas24h] Giảm 150k giao GAS tận nhà tại TP.HCM', N'E-voucher giảm giá áp dụng cho tất cả các sản phẩm dịch vụ trong thời gian khuyến mãi của Voucher trên BookingHotel', N'E-voucher giảm giá áp dụng cho tất cả các sản phẩm dịch vụ trong thời gian khuyến mãi của Voucher trên BookingHotel', N'~/FileUploads/Images/p-5.jpg', N'~/FileUploads/Images/pt-5.jpg', N'~/FileUploads/Images/p-5.jpg', 1, N'E-voucher giảm giá áp dụng cho tất cả các sản phẩm dịch vụ trong thời gian khuyến mãi của Voucher trên BookingHotel', N'E-voucher giảm giá áp dụng cho tất cả các sản phẩm dịch vụ trong thời gian khuyến mãi của Voucher trên BookingHotel', N'E-voucher giảm giá áp dụng cho tất cả các sản phẩm dịch vụ trong thời gian khuyến mãi của Voucher trên BookingHotel', N'#', CAST(50000.00 AS Decimal(18, 2)), 0, NULL, NULL, NULL, NULL, NULL, NULL, 86, CAST(4.8 AS Decimal(5, 1)), 1, 0, CAST(N'2021-04-06T00:00:00.000' AS DateTime), NULL, NULL, NULL, 1, NULL, NULL, CAST(N'2021-04-01T00:00:00.000' AS DateTime), CAST(N'2021-04-30T00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[Product] ([ID], [Code], [Title], [Description], [Content], [Avatar], [Thumb], [ImageListProduct], [Position], [MetaTitle], [MetaDescription], [MetaKeywords], [Alias], [Price], [Sale], [SaleDeadLine], [ViewTime], [StockStatus], [WishlistView], [SoldView], [Schemas], [TotalAssess], [ValueAssess], [Status], [IsDelete], [CreateTime], [ModifyTime], [CreateBy], [ModifyBy], [ProductCategoryID], [SaleID], [UnitID], [EffectiveDate], [ExpiryDate], [Point]) VALUES (7, N'M6', N'[Shark Market] Giảm 30% cho sữa Vegemil (thùng)', N'E-voucher giảm giá áp dụng cho tất cả các sản phẩm dịch vụ trong thời gian khuyến mãi của Voucher trên BookingHotel', N'E-voucher giảm giá áp dụng cho tất cả các sản phẩm dịch vụ trong thời gian khuyến mãi của Voucher trên BookingHotel', N'~/FileUploads/Images/p-6.jpg', N'~/FileUploads/Images/pt-6.jpg', N'#', 1, N'E-voucher giảm giá áp dụng cho tất cả các sản phẩm dịch vụ trong thời gian khuyến mãi của Voucher trên BookingHotel', N'E-voucher giảm giá áp dụng cho tất cả các sản phẩm dịch vụ trong thời gian khuyến mãi của Voucher trên BookingHotel', N'E-voucher giảm giá áp dụng cho tất cả các sản phẩm dịch vụ trong thời gian khuyến mãi của Voucher trên BookingHotel', N'#', CAST(50000.00 AS Decimal(18, 2)), 0, NULL, NULL, NULL, NULL, NULL, NULL, 28, CAST(4.5 AS Decimal(5, 1)), 1, 0, CAST(N'2021-04-06T00:00:00.000' AS DateTime), NULL, NULL, NULL, 1, NULL, NULL, CAST(N'2021-04-01T00:00:00.000' AS DateTime), CAST(N'2021-04-30T00:00:00.000' AS DateTime), NULL)
GO
SET IDENTITY_INSERT [dbo].[Product] OFF
GO
SET IDENTITY_INSERT [dbo].[ProductCategory] ON 
GO
INSERT [dbo].[ProductCategory] ([ID], [Title], [Description], [MetaTitle], [MetaDescription], [MetaKeywords], [Avatar], [Thumb], [Alias], [Schemas], [Position], [Status], [IsDelete], [CreateTime], [CreateBy], [ModifyBy], [ModifyTime], [ParentID]) VALUES (1, N'Cà phê', N'Cà phê', N'Cà phê', N'Cà phê', N'Cà phê', N'https://www.vietcoupon.vnsosoft.com/FileUploads/Images/coffee.png', N'https://www.vietcoupon.vnsosoft.com/FileUploads/Images/coffee.png', N'#', N'#', 1, 1, 0, CAST(N'2021-04-07T07:46:30.027' AS DateTime), NULL, NULL, CAST(N'2021-04-07T07:46:30.027' AS DateTime), NULL)
GO
INSERT [dbo].[ProductCategory] ([ID], [Title], [Description], [MetaTitle], [MetaDescription], [MetaKeywords], [Avatar], [Thumb], [Alias], [Schemas], [Position], [Status], [IsDelete], [CreateTime], [CreateBy], [ModifyBy], [ModifyTime], [ParentID]) VALUES (2, N'Nhà hàng', N'Nhà hàng', N'Nhà hàng', N'Nhà hàng', N'Nhà hàng', N'https://www.vietcoupon.vnsosoft.com/FileUploads/Images/dish.png', N'https://www.vietcoupon.vnsosoft.com/FileUploads/Images/dish.png', N'#', N'#', 1, 1, 0, CAST(N'2021-04-07T07:46:30.027' AS DateTime), NULL, NULL, CAST(N'2021-04-07T07:46:30.027' AS DateTime), NULL)
GO
INSERT [dbo].[ProductCategory] ([ID], [Title], [Description], [MetaTitle], [MetaDescription], [MetaKeywords], [Avatar], [Thumb], [Alias], [Schemas], [Position], [Status], [IsDelete], [CreateTime], [CreateBy], [ModifyBy], [ModifyTime], [ParentID]) VALUES (3, N'Du lịch', N'Du lịch', N'Du lịch', N'Du lịch', N'giảm giá vé, tour du lịch, khách sạn, resort', N'https://www.vietcoupon.vnsosoft.com/FileUploads/Images/travel.png', N'https://www.vietcoupon.vnsosoft.com/FileUploads/Images/travel.png', N'#', N'#', 1, 1, 0, CAST(N'2021-04-07T07:46:30.027' AS DateTime), NULL, NULL, CAST(N'2021-04-07T07:46:30.027' AS DateTime), NULL)
GO
INSERT [dbo].[ProductCategory] ([ID], [Title], [Description], [MetaTitle], [MetaDescription], [MetaKeywords], [Avatar], [Thumb], [Alias], [Schemas], [Position], [Status], [IsDelete], [CreateTime], [CreateBy], [ModifyBy], [ModifyTime], [ParentID]) VALUES (4, N'Mua sắm', N'Mua sắm', N'Mua sắm', N'Mua sắm', N'tivi, máy giặt, máy lạnh giảm giá', N'https://www.vietcoupon.vnsosoft.com/FileUploads/Images/online-shopping.png', N'https://www.vietcoupon.vnsosoft.com/FileUploads/Images/online-shopping.png', N'#', N'#', 1, 1, 0, CAST(N'2021-04-07T07:46:30.027' AS DateTime), NULL, NULL, CAST(N'2021-04-07T07:46:30.027' AS DateTime), NULL)
GO
INSERT [dbo].[ProductCategory] ([ID], [Title], [Description], [MetaTitle], [MetaDescription], [MetaKeywords], [Avatar], [Thumb], [Alias], [Schemas], [Position], [Status], [IsDelete], [CreateTime], [CreateBy], [ModifyBy], [ModifyTime], [ParentID]) VALUES (5, N'Giải trí', N'Giải trí', N'Giải trí', N'Giải trí', N'máy chơi game, giải trí', N'https://www.vietcoupon.vnsosoft.com/FileUploads/Images/game-console.png', N'https://www.vietcoupon.vnsosoft.com/FileUploads/Images/game-console.png', N'#', N'#', 1, 1, 0, CAST(N'2021-04-07T07:46:30.027' AS DateTime), NULL, NULL, CAST(N'2021-04-07T07:46:30.027' AS DateTime), NULL)
GO
INSERT [dbo].[ProductCategory] ([ID], [Title], [Description], [MetaTitle], [MetaDescription], [MetaKeywords], [Avatar], [Thumb], [Alias], [Schemas], [Position], [Status], [IsDelete], [CreateTime], [CreateBy], [ModifyBy], [ModifyTime], [ParentID]) VALUES (6, N'CH tiện lợi', N'CH tiện lợi', N'CH tiện lợi', N'CH tiện lợi', N'CH tiện lợi', N'https://www.vietcoupon.vnsosoft.com/FileUploads/Images/store.png', N'https://www.vietcoupon.vnsosoft.com/FileUploads/Images/store.png', N'#', N'#', 1, 1, 0, CAST(N'2021-04-07T07:46:30.027' AS DateTime), NULL, NULL, CAST(N'2021-04-07T07:46:30.027' AS DateTime), NULL)
GO
INSERT [dbo].[ProductCategory] ([ID], [Title], [Description], [MetaTitle], [MetaDescription], [MetaKeywords], [Avatar], [Thumb], [Alias], [Schemas], [Position], [Status], [IsDelete], [CreateTime], [CreateBy], [ModifyBy], [ModifyTime], [ParentID]) VALUES (7, N'Thức ăn nhanh', N'Thức ăn nhanh', N'Thức ăn nhanh', N'Thức ăn nhanh', N'Thức ăn nhanh', N'https://www.vietcoupon.vnsosoft.com/FileUploads/Images/fast-food.png', N'https://www.vietcoupon.vnsosoft.com/FileUploads/Images/fast-food.png', N'#', N'#', 1, 1, 0, CAST(N'2021-04-07T00:00:00.000' AS DateTime), NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[ProductCategory] ([ID], [Title], [Description], [MetaTitle], [MetaDescription], [MetaKeywords], [Avatar], [Thumb], [Alias], [Schemas], [Position], [Status], [IsDelete], [CreateTime], [CreateBy], [ModifyBy], [ModifyTime], [ParentID]) VALUES (8, N'Xem phim', N'Xem phim', N'Xem phim', N'Xem phim', N'Xem phim', N'https://www.vietcoupon.vnsosoft.com/FileUploads/Images/movie.png', N'https://www.vietcoupon.vnsosoft.com/FileUploads/Images/movie.png', N'#', N'#', 1, 1, 0, CAST(N'2021-04-07T00:00:00.000' AS DateTime), NULL, NULL, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[ProductCategory] OFF
GO
SET IDENTITY_INSERT [dbo].[Recruitment] ON 
GO
INSERT [dbo].[Recruitment] ([ID], [Title], [Description], [Avatar], [Thumb], [Content], [ViewTime], [CreateTime], [CreateBy], [ModifyTime], [ModifyBy], [Status], [Position], [MetaTitle], [MetaDescription], [MetaKeywords], [Schemas], [IsDelete]) VALUES (3, N'string', N'string', N'string', N'string', N'string', 0, CAST(N'2021-04-05T17:40:54.523' AS DateTime), 2, CAST(N'2021-04-05T17:40:54.523' AS DateTime), 0, 1, 0, N'string', N'string', N'string', N'string', 1)
GO
SET IDENTITY_INSERT [dbo].[Recruitment] OFF
GO
SET IDENTITY_INSERT [dbo].[Service] ON 
GO
INSERT [dbo].[Service] ([ID], [Title], [Description], [Avatar], [Thumb], [Content], [Position], [Status], [ViewTime], [TotalAssess], [ValueAssess], [CreateBy], [CreateTime], [ModifyBy], [ModifyTime], [Schemas], [MetaTitle], [MetaDescription], [MetaKeywords], [ServiceCategoryID], [Alias], [IsDelete]) VALUES (1, N'Dịch vụ 1', N'Dịch vụ 1', N'#', N'#', N'Dịch vụ 1', 1, 1, 0, 0, CAST(0.0 AS Decimal(5, 1)), NULL, CAST(N'2021-04-04T00:00:00.000' AS DateTime), NULL, NULL, N'#', NULL, NULL, NULL, 1, NULL, 0)
GO
INSERT [dbo].[Service] ([ID], [Title], [Description], [Avatar], [Thumb], [Content], [Position], [Status], [ViewTime], [TotalAssess], [ValueAssess], [CreateBy], [CreateTime], [ModifyBy], [ModifyTime], [Schemas], [MetaTitle], [MetaDescription], [MetaKeywords], [ServiceCategoryID], [Alias], [IsDelete]) VALUES (2, N'Dịch vụ 2', N'Dịch vụ 2', N'#', N'#', N'Dịch vụ 2', 2, 1, 0, 0, CAST(0.0 AS Decimal(5, 1)), NULL, CAST(N'2021-04-04T00:00:00.000' AS DateTime), NULL, NULL, N'#', NULL, NULL, NULL, 1, NULL, 0)
GO
INSERT [dbo].[Service] ([ID], [Title], [Description], [Avatar], [Thumb], [Content], [Position], [Status], [ViewTime], [TotalAssess], [ValueAssess], [CreateBy], [CreateTime], [ModifyBy], [ModifyTime], [Schemas], [MetaTitle], [MetaDescription], [MetaKeywords], [ServiceCategoryID], [Alias], [IsDelete]) VALUES (3, N'Dịch vụ 3', N'Dịch vụ 3', N'#', N'#', N'Dịch vụ 3', 3, 1, 0, 0, CAST(0.0 AS Decimal(5, 1)), NULL, CAST(N'2021-04-04T00:00:00.000' AS DateTime), NULL, NULL, N'#', NULL, NULL, NULL, 4, NULL, 0)
GO
INSERT [dbo].[Service] ([ID], [Title], [Description], [Avatar], [Thumb], [Content], [Position], [Status], [ViewTime], [TotalAssess], [ValueAssess], [CreateBy], [CreateTime], [ModifyBy], [ModifyTime], [Schemas], [MetaTitle], [MetaDescription], [MetaKeywords], [ServiceCategoryID], [Alias], [IsDelete]) VALUES (4, N'Dịch vụ 4', N'Dịch vụ 4', N'#', N'#', N'Dịch vụ 4', 4, 1, 0, 0, CAST(0.0 AS Decimal(5, 1)), NULL, CAST(N'2021-04-04T00:00:00.000' AS DateTime), NULL, NULL, N'#', NULL, NULL, NULL, 4, NULL, 0)
GO
SET IDENTITY_INSERT [dbo].[Service] OFF
GO
SET IDENTITY_INSERT [dbo].[ServiceCategory] ON 
GO
INSERT [dbo].[ServiceCategory] ([ID], [Title], [Description], [Status], [Position], [MetaTitle], [MetaDescription], [MetaKeywords], [Schemas], [CreateTime], [CreateBy], [ModifyTime], [ModifyBy], [ParentID], [Alias], [IsDelete]) VALUES (1, N'Lazada', N'Lazada', 1, 1, N'Lazada', N'Lazada', N'Lazada', N'Lazada', CAST(N'2021-04-07T07:55:04.317' AS DateTime), NULL, CAST(N'2021-04-07T07:55:04.317' AS DateTime), NULL, NULL, N'#', 0)
GO
INSERT [dbo].[ServiceCategory] ([ID], [Title], [Description], [Status], [Position], [MetaTitle], [MetaDescription], [MetaKeywords], [Schemas], [CreateTime], [CreateBy], [ModifyTime], [ModifyBy], [ParentID], [Alias], [IsDelete]) VALUES (4, N'Adayroi', N'Adayroi', 1, 1, N'Adayroi', N'Adayroi', N'Adayroi', N'Adayroi', CAST(N'2021-04-07T00:00:00.000' AS DateTime), NULL, NULL, NULL, NULL, N'#', 0)
GO
INSERT [dbo].[ServiceCategory] ([ID], [Title], [Description], [Status], [Position], [MetaTitle], [MetaDescription], [MetaKeywords], [Schemas], [CreateTime], [CreateBy], [ModifyTime], [ModifyBy], [ParentID], [Alias], [IsDelete]) VALUES (5, N'Shoppe', N'Shoppe', 1, 1, N'Shoppe', N'Shoppe', N'Shoppe', N'Shoppe', CAST(N'2021-04-07T00:00:00.000' AS DateTime), NULL, NULL, NULL, NULL, N'#', 0)
GO
INSERT [dbo].[ServiceCategory] ([ID], [Title], [Description], [Status], [Position], [MetaTitle], [MetaDescription], [MetaKeywords], [Schemas], [CreateTime], [CreateBy], [ModifyTime], [ModifyBy], [ParentID], [Alias], [IsDelete]) VALUES (7, N'Tiki', N'Tiki', 1, 1, N'Tiki', N'Tiki', N'Tiki', N'Tiki', CAST(N'2021-04-04T00:00:00.000' AS DateTime), NULL, NULL, NULL, NULL, N'#', 0)
GO
SET IDENTITY_INSERT [dbo].[ServiceCategory] OFF
GO
INSERT [vnsosoft_vietcoupon].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20210404191027_init', N'5.0.4')
GO
INSERT [vnsosoft_vietcoupon].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20210404193607_init_05042021', N'5.0.4')
GO
ALTER TABLE [dbo].[Customer] ADD  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[About]  WITH CHECK ADD  CONSTRAINT [FK_About_Account] FOREIGN KEY([CreateBy])
REFERENCES [dbo].[Account] ([ID])
GO
ALTER TABLE [dbo].[About] CHECK CONSTRAINT [FK_About_Account]
GO
ALTER TABLE [dbo].[Account]  WITH CHECK ADD  CONSTRAINT [FK_Account_AccountType] FOREIGN KEY([AccountTypeID])
REFERENCES [dbo].[AccountType] ([ID])
GO
ALTER TABLE [dbo].[Account] CHECK CONSTRAINT [FK_Account_AccountType]
GO
ALTER TABLE [dbo].[AccountFunction]  WITH CHECK ADD  CONSTRAINT [FK_AccountFunction_Account] FOREIGN KEY([AccountID])
REFERENCES [dbo].[Account] ([ID])
GO
ALTER TABLE [dbo].[AccountFunction] CHECK CONSTRAINT [FK_AccountFunction_Account]
GO
ALTER TABLE [dbo].[AccountFunction]  WITH CHECK ADD  CONSTRAINT [FK_AccountFunction_Function] FOREIGN KEY([FunctionID])
REFERENCES [dbo].[Function] ([ID])
GO
ALTER TABLE [dbo].[AccountFunction] CHECK CONSTRAINT [FK_AccountFunction_Function]
GO
ALTER TABLE [dbo].[AccountGroupAccount]  WITH CHECK ADD  CONSTRAINT [FK_AccountGroupAccount_Account] FOREIGN KEY([AccountID])
REFERENCES [dbo].[Account] ([ID])
GO
ALTER TABLE [dbo].[AccountGroupAccount] CHECK CONSTRAINT [FK_AccountGroupAccount_Account]
GO
ALTER TABLE [dbo].[AccountGroupAccount]  WITH CHECK ADD  CONSTRAINT [FK_AccountGroupAccount_GroupAccount] FOREIGN KEY([GroupAccountID])
REFERENCES [dbo].[GroupAccount] ([ID])
GO
ALTER TABLE [dbo].[AccountGroupAccount] CHECK CONSTRAINT [FK_AccountGroupAccount_GroupAccount]
GO
ALTER TABLE [dbo].[Article]  WITH CHECK ADD  CONSTRAINT [FK_Article_ArticleCategory] FOREIGN KEY([ArticleCategoryID])
REFERENCES [dbo].[ArticleCategory] ([ID])
GO
ALTER TABLE [dbo].[Article] CHECK CONSTRAINT [FK_Article_ArticleCategory]
GO
ALTER TABLE [dbo].[ArticleCategory]  WITH CHECK ADD  CONSTRAINT [FK_ArticleCategory_Account] FOREIGN KEY([CreateBy])
REFERENCES [dbo].[Account] ([ID])
GO
ALTER TABLE [dbo].[ArticleCategory] CHECK CONSTRAINT [FK_ArticleCategory_Account]
GO
ALTER TABLE [dbo].[Assess]  WITH CHECK ADD  CONSTRAINT [FK_Assess_Account] FOREIGN KEY([AccountID])
REFERENCES [dbo].[Account] ([ID])
GO
ALTER TABLE [dbo].[Assess] CHECK CONSTRAINT [FK_Assess_Account]
GO
ALTER TABLE [dbo].[Banner]  WITH CHECK ADD  CONSTRAINT [FK_Banner_Account] FOREIGN KEY([CreateBy])
REFERENCES [dbo].[Account] ([ID])
GO
ALTER TABLE [dbo].[Banner] CHECK CONSTRAINT [FK_Banner_Account]
GO
ALTER TABLE [dbo].[ConfigSystem]  WITH CHECK ADD  CONSTRAINT [FK_ConfigSystem_Account] FOREIGN KEY([CreateBy])
REFERENCES [dbo].[Account] ([ID])
GO
ALTER TABLE [dbo].[ConfigSystem] CHECK CONSTRAINT [FK_ConfigSystem_Account]
GO
ALTER TABLE [dbo].[Contact]  WITH CHECK ADD  CONSTRAINT [FK_Contact_Account] FOREIGN KEY([ApproveBy])
REFERENCES [dbo].[Account] ([ID])
GO
ALTER TABLE [dbo].[Contact] CHECK CONSTRAINT [FK_Contact_Account]
GO
ALTER TABLE [dbo].[Customer]  WITH CHECK ADD  CONSTRAINT [FK_Customer_Account] FOREIGN KEY([CreateBy])
REFERENCES [dbo].[Account] ([ID])
GO
ALTER TABLE [dbo].[Customer] CHECK CONSTRAINT [FK_Customer_Account]
GO
ALTER TABLE [dbo].[Customer]  WITH CHECK ADD  CONSTRAINT [FK_Customer_CustomerType] FOREIGN KEY([CustomerTypeID])
REFERENCES [dbo].[CustomerType] ([ID])
GO
ALTER TABLE [dbo].[Customer] CHECK CONSTRAINT [FK_Customer_CustomerType]
GO
ALTER TABLE [dbo].[CustomerPartner]  WITH CHECK ADD  CONSTRAINT [FK_CustomerPartner_Customer] FOREIGN KEY([CustomerID])
REFERENCES [dbo].[Customer] ([ID])
GO
ALTER TABLE [dbo].[CustomerPartner] CHECK CONSTRAINT [FK_CustomerPartner_Customer]
GO
ALTER TABLE [dbo].[CustomerPartner]  WITH CHECK ADD  CONSTRAINT [FK_CustomerPartner_Partner] FOREIGN KEY([PartnerID])
REFERENCES [dbo].[Partner] ([ID])
GO
ALTER TABLE [dbo].[CustomerPartner] CHECK CONSTRAINT [FK_CustomerPartner_Partner]
GO
ALTER TABLE [dbo].[District]  WITH CHECK ADD  CONSTRAINT [FK_District_Province] FOREIGN KEY([ProvinceID])
REFERENCES [dbo].[Province] ([ID])
GO
ALTER TABLE [dbo].[District] CHECK CONSTRAINT [FK_District_Province]
GO
ALTER TABLE [dbo].[Faq]  WITH CHECK ADD  CONSTRAINT [FK_Faq_Account] FOREIGN KEY([CreateBy])
REFERENCES [dbo].[Account] ([ID])
GO
ALTER TABLE [dbo].[Faq] CHECK CONSTRAINT [FK_Faq_Account]
GO
ALTER TABLE [dbo].[Feedback]  WITH CHECK ADD  CONSTRAINT [FK_Feedback_Account] FOREIGN KEY([CreateBy])
REFERENCES [dbo].[Account] ([ID])
GO
ALTER TABLE [dbo].[Feedback] CHECK CONSTRAINT [FK_Feedback_Account]
GO
ALTER TABLE [dbo].[Function]  WITH CHECK ADD  CONSTRAINT [FK_Function_Module] FOREIGN KEY([ModuleID])
REFERENCES [dbo].[Module] ([ID])
GO
ALTER TABLE [dbo].[Function] CHECK CONSTRAINT [FK_Function_Module]
GO
ALTER TABLE [dbo].[FunctionGroupFunction]  WITH CHECK ADD  CONSTRAINT [FK_FunctionGroupFunction_Function] FOREIGN KEY([FunctionID])
REFERENCES [dbo].[Function] ([ID])
GO
ALTER TABLE [dbo].[FunctionGroupFunction] CHECK CONSTRAINT [FK_FunctionGroupFunction_Function]
GO
ALTER TABLE [dbo].[FunctionGroupFunction]  WITH CHECK ADD  CONSTRAINT [FK_FunctionGroupFunction_GroupFunction] FOREIGN KEY([GroupFunctionID])
REFERENCES [dbo].[GroupFunction] ([ID])
GO
ALTER TABLE [dbo].[FunctionGroupFunction] CHECK CONSTRAINT [FK_FunctionGroupFunction_GroupFunction]
GO
ALTER TABLE [dbo].[Notification]  WITH CHECK ADD  CONSTRAINT [FK_Notification_Account] FOREIGN KEY([CreateBy])
REFERENCES [dbo].[Account] ([ID])
GO
ALTER TABLE [dbo].[Notification] CHECK CONSTRAINT [FK_Notification_Account]
GO
ALTER TABLE [dbo].[Notification]  WITH CHECK ADD  CONSTRAINT [FK_Notification_NotificationType] FOREIGN KEY([NotificationTypeID])
REFERENCES [dbo].[NotificationType] ([ID])
GO
ALTER TABLE [dbo].[Notification] CHECK CONSTRAINT [FK_Notification_NotificationType]
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [FK_Order_Customer] FOREIGN KEY([CustomerID])
REFERENCES [dbo].[Customer] ([ID])
GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [FK_Order_Customer]
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [FK_Order_OrderStatus] FOREIGN KEY([OrderStatusID])
REFERENCES [dbo].[OrderStatus] ([ID])
GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [FK_Order_OrderStatus]
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [FK_Order_PayType] FOREIGN KEY([PayTypeID])
REFERENCES [dbo].[PayType] ([ID])
GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [FK_Order_PayType]
GO
ALTER TABLE [dbo].[OrderDetail]  WITH CHECK ADD  CONSTRAINT [FK_OrderDetail_Order] FOREIGN KEY([OrderID])
REFERENCES [dbo].[Order] ([ID])
GO
ALTER TABLE [dbo].[OrderDetail] CHECK CONSTRAINT [FK_OrderDetail_Order]
GO
ALTER TABLE [dbo].[OrderDetail]  WITH CHECK ADD  CONSTRAINT [FK_OrderDetail_Product] FOREIGN KEY([ProductID])
REFERENCES [dbo].[Product] ([ID])
GO
ALTER TABLE [dbo].[OrderDetail] CHECK CONSTRAINT [FK_OrderDetail_Product]
GO
ALTER TABLE [dbo].[Page]  WITH CHECK ADD  CONSTRAINT [FK_Page_Account] FOREIGN KEY([CreateBy])
REFERENCES [dbo].[Account] ([ID])
GO
ALTER TABLE [dbo].[Page] CHECK CONSTRAINT [FK_Page_Account]
GO
ALTER TABLE [dbo].[Page]  WITH CHECK ADD  CONSTRAINT [FK_Page_PageType] FOREIGN KEY([PageTypeID])
REFERENCES [dbo].[PageType] ([ID])
GO
ALTER TABLE [dbo].[Page] CHECK CONSTRAINT [FK_Page_PageType]
GO
ALTER TABLE [dbo].[Partner]  WITH CHECK ADD  CONSTRAINT [FK_Partner_Account] FOREIGN KEY([CreateBy])
REFERENCES [dbo].[Account] ([ID])
GO
ALTER TABLE [dbo].[Partner] CHECK CONSTRAINT [FK_Partner_Account]
GO
ALTER TABLE [dbo].[Partner]  WITH CHECK ADD  CONSTRAINT [FK_Partner_PartnerType] FOREIGN KEY([PartnerTypeID])
REFERENCES [dbo].[PartnerType] ([ID])
GO
ALTER TABLE [dbo].[Partner] CHECK CONSTRAINT [FK_Partner_PartnerType]
GO
ALTER TABLE [dbo].[PartnerLocal]  WITH CHECK ADD  CONSTRAINT [FK_PartnerLocal_Partner] FOREIGN KEY([PartnerID])
REFERENCES [dbo].[Partner] ([ID])
GO
ALTER TABLE [dbo].[PartnerLocal] CHECK CONSTRAINT [FK_PartnerLocal_Partner]
GO
ALTER TABLE [dbo].[PartnerProduct]  WITH CHECK ADD  CONSTRAINT [FK_PartnerProduct_Partner] FOREIGN KEY([PartnerID])
REFERENCES [dbo].[Partner] ([ID])
GO
ALTER TABLE [dbo].[PartnerProduct] CHECK CONSTRAINT [FK_PartnerProduct_Partner]
GO
ALTER TABLE [dbo].[PartnerProduct]  WITH CHECK ADD  CONSTRAINT [FK_PartnerProduct_Product] FOREIGN KEY([ProductID])
REFERENCES [dbo].[Product] ([ID])
GO
ALTER TABLE [dbo].[PartnerProduct] CHECK CONSTRAINT [FK_PartnerProduct_Product]
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [FK_Product_ProductCategory] FOREIGN KEY([ProductCategoryID])
REFERENCES [dbo].[ProductCategory] ([ID])
GO
ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [FK_Product_ProductCategory]
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [FK_Product_Sale] FOREIGN KEY([SaleID])
REFERENCES [dbo].[Sale] ([ID])
GO
ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [FK_Product_Sale]
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [FK_Product_Unit] FOREIGN KEY([UnitID])
REFERENCES [dbo].[Unit] ([ID])
GO
ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [FK_Product_Unit]
GO
ALTER TABLE [dbo].[ProductCustomer]  WITH CHECK ADD  CONSTRAINT [FK_ProductCustomer_Customer] FOREIGN KEY([CustomerID])
REFERENCES [dbo].[Customer] ([ID])
GO
ALTER TABLE [dbo].[ProductCustomer] CHECK CONSTRAINT [FK_ProductCustomer_Customer]
GO
ALTER TABLE [dbo].[ProductCustomer]  WITH CHECK ADD  CONSTRAINT [FK_ProductCustomer_Product] FOREIGN KEY([ProductID])
REFERENCES [dbo].[Product] ([ID])
GO
ALTER TABLE [dbo].[ProductCustomer] CHECK CONSTRAINT [FK_ProductCustomer_Product]
GO
ALTER TABLE [dbo].[ProductTab]  WITH CHECK ADD  CONSTRAINT [FK_ProductTab_Product] FOREIGN KEY([ProductID])
REFERENCES [dbo].[Product] ([ID])
GO
ALTER TABLE [dbo].[ProductTab] CHECK CONSTRAINT [FK_ProductTab_Product]
GO
ALTER TABLE [dbo].[ProductTab]  WITH CHECK ADD  CONSTRAINT [FK_ProductTab_Tab] FOREIGN KEY([TabID])
REFERENCES [dbo].[Tab] ([ID])
GO
ALTER TABLE [dbo].[ProductTab] CHECK CONSTRAINT [FK_ProductTab_Tab]
GO
ALTER TABLE [dbo].[ProductWishlist]  WITH CHECK ADD  CONSTRAINT [FK_ProductWishlist_Account] FOREIGN KEY([AccountID])
REFERENCES [dbo].[Account] ([ID])
GO
ALTER TABLE [dbo].[ProductWishlist] CHECK CONSTRAINT [FK_ProductWishlist_Account]
GO
ALTER TABLE [dbo].[ProductWishlist]  WITH CHECK ADD  CONSTRAINT [FK_ProductWishlist_Product] FOREIGN KEY([ProductID])
REFERENCES [dbo].[Product] ([ID])
GO
ALTER TABLE [dbo].[ProductWishlist] CHECK CONSTRAINT [FK_ProductWishlist_Product]
GO
ALTER TABLE [dbo].[Province]  WITH CHECK ADD  CONSTRAINT [FK_Province_Account] FOREIGN KEY([CreateBy])
REFERENCES [dbo].[Account] ([ID])
GO
ALTER TABLE [dbo].[Province] CHECK CONSTRAINT [FK_Province_Account]
GO
ALTER TABLE [dbo].[Recruitment]  WITH CHECK ADD  CONSTRAINT [FK_Recruitment_Account] FOREIGN KEY([CreateBy])
REFERENCES [dbo].[Account] ([ID])
GO
ALTER TABLE [dbo].[Recruitment] CHECK CONSTRAINT [FK_Recruitment_Account]
GO
ALTER TABLE [dbo].[Service]  WITH CHECK ADD  CONSTRAINT [FK_Service_ServiceCategory] FOREIGN KEY([ServiceCategoryID])
REFERENCES [dbo].[ServiceCategory] ([ID])
GO
ALTER TABLE [dbo].[Service] CHECK CONSTRAINT [FK_Service_ServiceCategory]
GO
ALTER TABLE [dbo].[ServiceCategory]  WITH CHECK ADD  CONSTRAINT [FK_ServiceCategory_Account] FOREIGN KEY([CreateBy])
REFERENCES [dbo].[Account] ([ID])
GO
ALTER TABLE [dbo].[ServiceCategory] CHECK CONSTRAINT [FK_ServiceCategory_Account]
GO
