USE [StockScanner]
GO
/****** Object:  Table [dbo].[BlacklistedKeywords]    Script Date: 2/6/2017 1:13:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BlacklistedKeywords](
	[keyword] [nchar](20) NOT NULL,
	[industry_type] [int] NOT NULL,
 CONSTRAINT [PK_BlacklistedKeywords] PRIMARY KEY CLUSTERED 
(
	[keyword] ASC,
	[industry_type] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Companies]    Script Date: 2/6/2017 1:13:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Companies](
	[symbol] [nchar](5) NOT NULL,
	[industry_type] [int] NOT NULL,
	[name] [nvarchar](50) NOT NULL,
	[price] [float] NOT NULL,
 CONSTRAINT [PK_Companies] PRIMARY KEY CLUSTERED 
(
	[symbol] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[HeadlineKeywords]    Script Date: 2/6/2017 1:13:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HeadlineKeywords](
	[symbol] [nchar](5) NOT NULL,
	[keyword] [nchar](20) NOT NULL,
 CONSTRAINT [PK_HeadlineKeywords] PRIMARY KEY CLUSTERED 
(
	[symbol] ASC,
	[keyword] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Industries]    Script Date: 2/6/2017 1:13:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Industries](
	[industry_type] [int] NOT NULL,
	[name] [nchar](10) NOT NULL,
 CONSTRAINT [PK_Industries] PRIMARY KEY CLUSTERED 
(
	[industry_type] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[KeywordInfluences]    Script Date: 2/6/2017 1:13:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KeywordInfluences](
	[keyword] [nchar](20) NOT NULL,
	[industry_type] [int] NOT NULL,
	[influence] [float] NOT NULL,
	[influence_counter] [int] NOT NULL,
 CONSTRAINT [PK_KeywordInfluences] PRIMARY KEY CLUSTERED 
(
	[keyword] ASC,
	[industry_type] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Predictions]    Script Date: 2/6/2017 1:13:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Predictions](
	[symbol] [nchar](5) NOT NULL,
	[predicted_change] [bit] NOT NULL,
 CONSTRAINT [PK_Predictions] PRIMARY KEY CLUSTERED 
(
	[symbol] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[Companies]  WITH CHECK ADD  CONSTRAINT [FK_Companies_Industries] FOREIGN KEY([industry_type])
REFERENCES [dbo].[Industries] ([industry_type])
GO
ALTER TABLE [dbo].[Companies] CHECK CONSTRAINT [FK_Companies_Industries]
GO
ALTER TABLE [dbo].[HeadlineKeywords]  WITH CHECK ADD  CONSTRAINT [FK_HeadlineKeywords_Companies] FOREIGN KEY([symbol])
REFERENCES [dbo].[Companies] ([symbol])
GO
ALTER TABLE [dbo].[HeadlineKeywords] CHECK CONSTRAINT [FK_HeadlineKeywords_Companies]
GO
ALTER TABLE [dbo].[KeywordInfluences]  WITH CHECK ADD  CONSTRAINT [FK_KeywordInfluence_Industries] FOREIGN KEY([industry_type])
REFERENCES [dbo].[Industries] ([industry_type])
GO
ALTER TABLE [dbo].[KeywordInfluences] CHECK CONSTRAINT [FK_KeywordInfluence_Industries]
GO
ALTER TABLE [dbo].[Predictions]  WITH CHECK ADD  CONSTRAINT [FK_Predictions_Companies] FOREIGN KEY([symbol])
REFERENCES [dbo].[Companies] ([symbol])
GO
ALTER TABLE [dbo].[Predictions] CHECK CONSTRAINT [FK_Predictions_Companies]
GO
