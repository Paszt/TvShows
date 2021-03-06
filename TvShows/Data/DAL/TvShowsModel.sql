USE [C:\USERS\STEPHEN\DROPBOX\PROJECTS\VISUAL STUDIO PROJECTS\TVSHOWS\TVSHOWS\DATA\TVSHOWS.MDF]
GO
/****** Object:  Table [dbo].[Episodes]    Script Date: 10/25/2015 4:25:23 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Episodes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AirDate] [nvarchar](max) NULL,
	[SeasonNumber] [int] NOT NULL,
	[EpisodeNumber] [int] NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Overview] [nvarchar](max) NULL,
	[StillPath] [nvarchar](max) NULL,
	[TvShow_Id] [int] NULL,
 CONSTRAINT [PK_dbo.Episodes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TvShows]    Script Date: 10/25/2015 4:25:23 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TvShows](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[FirstAirDate] [nvarchar](max) NULL,
	[HomePage] [nvarchar](1000) NULL,
	[OriginCountry] [nchar](2) NULL,
	[Overview] [nvarchar](max) NULL,
	[PosterPath] [nvarchar](max) NULL,
	[FolderName] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.TvShows] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
ALTER TABLE [dbo].[Episodes]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Episodes_dbo.TvShows_TvShow_Id] FOREIGN KEY([TvShow_Id])
REFERENCES [dbo].[TvShows] ([Id])
GO
ALTER TABLE [dbo].[Episodes] CHECK CONSTRAINT [FK_dbo.Episodes_dbo.TvShows_TvShow_Id]
GO
