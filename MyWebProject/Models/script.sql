USE [master]
GO
/****** Object:  Database [MyWebSite]    Script Date: 01/09/2017 15:39:48 ******/
CREATE DATABASE [MyWebSite] ON  PRIMARY 
( NAME = N'MyWebSite', FILENAME = N'd:\Program Files\Microsoft SQL Server\MSSQL10_50.SQLEXPRESS\MSSQL\DATA\MyWebSite.mdf' , SIZE = 3072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'MyWebSite_log', FILENAME = N'd:\Program Files\Microsoft SQL Server\MSSQL10_50.SQLEXPRESS\MSSQL\DATA\MyWebSite_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [MyWebSite] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [MyWebSite].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [MyWebSite] SET ANSI_NULL_DEFAULT OFF
GO
ALTER DATABASE [MyWebSite] SET ANSI_NULLS OFF
GO
ALTER DATABASE [MyWebSite] SET ANSI_PADDING OFF
GO
ALTER DATABASE [MyWebSite] SET ANSI_WARNINGS OFF
GO
ALTER DATABASE [MyWebSite] SET ARITHABORT OFF
GO
ALTER DATABASE [MyWebSite] SET AUTO_CLOSE OFF
GO
ALTER DATABASE [MyWebSite] SET AUTO_CREATE_STATISTICS ON
GO
ALTER DATABASE [MyWebSite] SET AUTO_SHRINK OFF
GO
ALTER DATABASE [MyWebSite] SET AUTO_UPDATE_STATISTICS ON
GO
ALTER DATABASE [MyWebSite] SET CURSOR_CLOSE_ON_COMMIT OFF
GO
ALTER DATABASE [MyWebSite] SET CURSOR_DEFAULT  GLOBAL
GO
ALTER DATABASE [MyWebSite] SET CONCAT_NULL_YIELDS_NULL OFF
GO
ALTER DATABASE [MyWebSite] SET NUMERIC_ROUNDABORT OFF
GO
ALTER DATABASE [MyWebSite] SET QUOTED_IDENTIFIER OFF
GO
ALTER DATABASE [MyWebSite] SET RECURSIVE_TRIGGERS OFF
GO
ALTER DATABASE [MyWebSite] SET  DISABLE_BROKER
GO
ALTER DATABASE [MyWebSite] SET AUTO_UPDATE_STATISTICS_ASYNC OFF
GO
ALTER DATABASE [MyWebSite] SET DATE_CORRELATION_OPTIMIZATION OFF
GO
ALTER DATABASE [MyWebSite] SET TRUSTWORTHY OFF
GO
ALTER DATABASE [MyWebSite] SET ALLOW_SNAPSHOT_ISOLATION OFF
GO
ALTER DATABASE [MyWebSite] SET PARAMETERIZATION SIMPLE
GO
ALTER DATABASE [MyWebSite] SET READ_COMMITTED_SNAPSHOT OFF
GO
ALTER DATABASE [MyWebSite] SET HONOR_BROKER_PRIORITY OFF
GO
ALTER DATABASE [MyWebSite] SET  READ_WRITE
GO
ALTER DATABASE [MyWebSite] SET RECOVERY SIMPLE
GO
ALTER DATABASE [MyWebSite] SET  MULTI_USER
GO
ALTER DATABASE [MyWebSite] SET PAGE_VERIFY CHECKSUM
GO
ALTER DATABASE [MyWebSite] SET DB_CHAINING OFF
GO
USE [MyWebSite]
GO
/****** Object:  User [BlogManager]    Script Date: 01/09/2017 15:39:48 ******/
CREATE USER [BlogManager] WITHOUT LOGIN WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  Table [dbo].[TAG_INFO]    Script Date: 01/09/2017 15:39:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TAG_INFO](
	[TAG_ID] [int] NOT NULL,
	[TAG_NAME] [nvarchar](50) NOT NULL,
	[COUNT] [int] NULL,
 CONSTRAINT [PK_TAG_INFO] PRIMARY KEY CLUSTERED 
(
	[TAG_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[POST_INFO]    Script Date: 01/09/2017 15:39:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[POST_INFO](
	[POST_ID] [int] IDENTITY(1,1) NOT NULL,
	[DATE] [date] NOT NULL,
	[MAIN_TITLE] [nvarchar](50) NOT NULL,
	[VIEW_COUNT] [int] NULL,
	[REPRODUCED_COUNT] [int] NULL,
	[PRAISE_COUNT] [int] NULL,
	[TAG_ID] [varchar](50) NULL,
	[SECOND_TITLE] [nchar](100) NULL,
	[IS_TOP] [bit] NULL,
 CONSTRAINT [PK_POST_INFO] PRIMARY KEY CLUSTERED 
(
	[POST_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'文章编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'POST_INFO', @level2type=N'COLUMN',@level2name=N'POST_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'发布日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'POST_INFO', @level2type=N'COLUMN',@level2name=N'DATE'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'标题' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'POST_INFO', @level2type=N'COLUMN',@level2name=N'MAIN_TITLE'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'浏览次数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'POST_INFO', @level2type=N'COLUMN',@level2name=N'VIEW_COUNT'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'转载次数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'POST_INFO', @level2type=N'COLUMN',@level2name=N'REPRODUCED_COUNT'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'点赞次数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'POST_INFO', @level2type=N'COLUMN',@level2name=N'PRAISE_COUNT'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否首页置顶' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'POST_INFO', @level2type=N'COLUMN',@level2name=N'IS_TOP'
GO
/****** Object:  Table [dbo].[POST_CONTENT]    Script Date: 01/09/2017 15:39:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[POST_CONTENT](
	[POST_ID] [int] IDENTITY(1,1) NOT NULL,
	[POST_CONTENT] [text] NOT NULL,
 CONSTRAINT [PK_TEXT_CONTENT] PRIMARY KEY CLUSTERED 
(
	[POST_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'文章段落ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'POST_CONTENT', @level2type=N'COLUMN',@level2name=N'POST_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'文章段落正文用HTML格式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'POST_CONTENT', @level2type=N'COLUMN',@level2name=N'POST_CONTENT'
GO
/****** Object:  Table [dbo].[CONFIG]    Script Date: 01/09/2017 15:39:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CONFIG](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[KEY_NAME] [nchar](30) NOT NULL,
	[VALUE] [nchar](30) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[COMMENTS]    Script Date: 01/09/2017 15:39:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[COMMENTS](
	[COMMENTS_ID] [int] IDENTITY(1,1) NOT NULL,
	[DATE] [datetime] NOT NULL,
	[NICK_NAME] [nvarchar](30) NULL,
	[TEXT] [nvarchar](500) NOT NULL,
	[EMAIL] [varchar](100) NULL,
	[POST_ID] [int] NOT NULL,
	[BEFOR_COMMENTS_ID] [int] NULL,
	[AVATAR_URL] [text] NULL,
 CONSTRAINT [PK_COMMENTS] PRIMARY KEY CLUSTERED 
(
	[COMMENTS_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'评论ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'COMMENTS', @level2type=N'COLUMN',@level2name=N'COMMENTS_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'评论时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'COMMENTS', @level2type=N'COLUMN',@level2name=N'DATE'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'昵称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'COMMENTS', @level2type=N'COLUMN',@level2name=N'NICK_NAME'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'评论正文' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'COMMENTS', @level2type=N'COLUMN',@level2name=N'TEXT'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'对应文章ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'COMMENTS', @level2type=N'COLUMN',@level2name=N'POST_ID'
GO
