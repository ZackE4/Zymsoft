USE [Capstone]
GO
/****** Object:  Table [dbo].[FoulLogs]    Script Date: 2019-04-04 11:49:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FoulLogs](
	[FouldLogId] [int] IDENTITY(1,1) NOT NULL,
	[GameTime] [time](7) NOT NULL,
	[Player_PlayerId] [int] NOT NULL,
	[Game_GameId] [int] NOT NULL,
 CONSTRAINT [PK_FoulLogs] PRIMARY KEY CLUSTERED 
(
	[FouldLogId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Games]    Script Date: 2019-04-04 11:49:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Games](
	[GameId] [int] IDENTITY(1,1) NOT NULL,
	[Date] [datetime] NOT NULL,
	[HomeTeamId] [nvarchar](max) NOT NULL,
	[AwayTeamId] [nvarchar](max) NOT NULL,
	[GameComplete] [bit] NOT NULL,
	[Season_SeasonId] [int] NOT NULL,
 CONSTRAINT [PK_Games] PRIMARY KEY CLUSTERED 
(
	[GameId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Leagues]    Script Date: 2019-04-04 11:49:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Leagues](
	[LeagueId] [int] IDENTITY(1,1) NOT NULL,
	[LeagueName] [nvarchar](max) NOT NULL,
	[Logo] [nvarchar](max) NOT NULL,
	[HashPassword] [nvarchar](max) NOT NULL,
	[LeagueKey] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Leagues] PRIMARY KEY CLUSTERED 
(
	[LeagueId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MediaLogs]    Script Date: 2019-04-04 11:49:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MediaLogs](
	[MediaLogId] [int] IDENTITY(1,1) NOT NULL,
	[MediaName] [nvarchar](max) NOT NULL,
	[DurationPlayed] [time](7) NOT NULL,
	[GameTime] [time](7) NOT NULL,
	[Game_GameId] [int] NOT NULL,
 CONSTRAINT [PK_MediaLogs] PRIMARY KEY CLUSTERED 
(
	[MediaLogId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Players]    Script Date: 2019-04-04 11:49:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Players](
	[PlayerId] [int] IDENTITY(1,1) NOT NULL,
	[PlayerNum] [nvarchar](max) NOT NULL,
	[Position] [nvarchar](max) NOT NULL,
	[FirstName] [nvarchar](max) NOT NULL,
	[LastName] [nvarchar](max) NOT NULL,
	[Picture] [nvarchar](max) NOT NULL,
	[Team_TeamId] [int] NOT NULL,
 CONSTRAINT [PK_Players] PRIMARY KEY CLUSTERED 
(
	[PlayerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ScoringLogs]    Script Date: 2019-04-04 11:49:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ScoringLogs](
	[ScoringLogId] [int] IDENTITY(1,1) NOT NULL,
	[GameTime] [time](7) NOT NULL,
	[Points] [int] NOT NULL,
	[Player_PlayerId] [int] NOT NULL,
	[Game_GameId] [int] NOT NULL,
 CONSTRAINT [PK_ScoringLogs] PRIMARY KEY CLUSTERED 
(
	[ScoringLogId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Seasons]    Script Date: 2019-04-04 11:49:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Seasons](
	[SeasonId] [int] IDENTITY(1,1) NOT NULL,
	[SeasonStart] [datetime] NOT NULL,
	[SeasonEnd] [datetime] NOT NULL,
	[League_LeagueId] [int] NOT NULL,
 CONSTRAINT [PK_Seasons] PRIMARY KEY CLUSTERED 
(
	[SeasonId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Teams]    Script Date: 2019-04-04 11:49:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Teams](
	[TeamId] [int] IDENTITY(1,1) NOT NULL,
	[TeamName] [nvarchar](max) NOT NULL,
	[CoachName] [nvarchar](max) NOT NULL,
	[Logo] [nvarchar](max) NOT NULL,
	[League_LeagueId] [int] NOT NULL,
 CONSTRAINT [PK_Teams] PRIMARY KEY CLUSTERED 
(
	[TeamId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[FoulLogs] ON 
GO
INSERT [dbo].[FoulLogs] ([FouldLogId], [GameTime], [Player_PlayerId], [Game_GameId]) VALUES (1, CAST(N'00:02:25' AS Time), 1, 3)
GO
INSERT [dbo].[FoulLogs] ([FouldLogId], [GameTime], [Player_PlayerId], [Game_GameId]) VALUES (2, CAST(N'00:03:15' AS Time), 1, 3)
GO
INSERT [dbo].[FoulLogs] ([FouldLogId], [GameTime], [Player_PlayerId], [Game_GameId]) VALUES (3, CAST(N'00:05:11' AS Time), 2, 3)
GO
SET IDENTITY_INSERT [dbo].[FoulLogs] OFF
GO
SET IDENTITY_INSERT [dbo].[Games] ON 
GO
INSERT [dbo].[Games] ([GameId], [Date], [HomeTeamId], [AwayTeamId], [GameComplete], [Season_SeasonId]) VALUES (3, CAST(N'2019-01-02T00:00:00.000' AS DateTime), N'1', N'2', 1, 6)
GO
SET IDENTITY_INSERT [dbo].[Games] OFF
GO
SET IDENTITY_INSERT [dbo].[Leagues] ON 
GO
INSERT [dbo].[Leagues] ([LeagueId], [LeagueName], [Logo], [HashPassword], [LeagueKey]) VALUES (1, N'TestLeagueA', N'ToBeAdded', N'Password1', N'3iDlb4R1tFRycUbNtVEH')
GO
SET IDENTITY_INSERT [dbo].[Leagues] OFF
GO
SET IDENTITY_INSERT [dbo].[Players] ON 
GO
INSERT [dbo].[Players] ([PlayerId], [PlayerNum], [Position], [FirstName], [LastName], [Picture], [Team_TeamId]) VALUES (1, N'99', N'Center', N'Joey', N'Balogney', N'To Be Added', 1)
GO
INSERT [dbo].[Players] ([PlayerId], [PlayerNum], [Position], [FirstName], [LastName], [Picture], [Team_TeamId]) VALUES (2, N'01', N'Point Guard', N'Tony', N'Taco', N'To Be Added', 2)
GO
SET IDENTITY_INSERT [dbo].[Players] OFF
GO
SET IDENTITY_INSERT [dbo].[ScoringLogs] ON 
GO
INSERT [dbo].[ScoringLogs] ([ScoringLogId], [GameTime], [Points], [Player_PlayerId], [Game_GameId]) VALUES (2, CAST(N'00:01:11' AS Time), 2, 1, 3)
GO
INSERT [dbo].[ScoringLogs] ([ScoringLogId], [GameTime], [Points], [Player_PlayerId], [Game_GameId]) VALUES (3, CAST(N'00:01:39' AS Time), 2, 1, 3)
GO
INSERT [dbo].[ScoringLogs] ([ScoringLogId], [GameTime], [Points], [Player_PlayerId], [Game_GameId]) VALUES (4, CAST(N'00:02:20' AS Time), 3, 2, 3)
GO
INSERT [dbo].[ScoringLogs] ([ScoringLogId], [GameTime], [Points], [Player_PlayerId], [Game_GameId]) VALUES (5, CAST(N'00:02:59' AS Time), 3, 1, 3)
GO
INSERT [dbo].[ScoringLogs] ([ScoringLogId], [GameTime], [Points], [Player_PlayerId], [Game_GameId]) VALUES (6, CAST(N'00:03:25' AS Time), 2, 1, 3)
GO
INSERT [dbo].[ScoringLogs] ([ScoringLogId], [GameTime], [Points], [Player_PlayerId], [Game_GameId]) VALUES (7, CAST(N'00:04:00' AS Time), 2, 2, 3)
GO
INSERT [dbo].[ScoringLogs] ([ScoringLogId], [GameTime], [Points], [Player_PlayerId], [Game_GameId]) VALUES (8, CAST(N'00:04:51' AS Time), 2, 2, 3)
GO
INSERT [dbo].[ScoringLogs] ([ScoringLogId], [GameTime], [Points], [Player_PlayerId], [Game_GameId]) VALUES (9, CAST(N'06:00:22' AS Time), 3, 1, 3)
GO
SET IDENTITY_INSERT [dbo].[ScoringLogs] OFF
GO
SET IDENTITY_INSERT [dbo].[Seasons] ON 
GO
INSERT [dbo].[Seasons] ([SeasonId], [SeasonStart], [SeasonEnd], [League_LeagueId]) VALUES (6, CAST(N'2019-01-01T00:00:00.000' AS DateTime), CAST(N'2019-04-01T00:00:00.000' AS DateTime), 1)
GO
SET IDENTITY_INSERT [dbo].[Seasons] OFF
GO
SET IDENTITY_INSERT [dbo].[Teams] ON 
GO
INSERT [dbo].[Teams] ([TeamId], [TeamName], [CoachName], [Logo], [League_LeagueId]) VALUES (1, N'TestTeamA', N'Bob Cole', N'To Be Added', 1)
GO
INSERT [dbo].[Teams] ([TeamId], [TeamName], [CoachName], [Logo], [League_LeagueId]) VALUES (2, N'TestTeamB', N'John Douglas', N'To Be Added', 1)
GO
SET IDENTITY_INSERT [dbo].[Teams] OFF
GO
ALTER TABLE [dbo].[FoulLogs]  WITH CHECK ADD  CONSTRAINT [FK_GameFoulLog] FOREIGN KEY([Game_GameId])
REFERENCES [dbo].[Games] ([GameId])
GO
ALTER TABLE [dbo].[FoulLogs] CHECK CONSTRAINT [FK_GameFoulLog]
GO
ALTER TABLE [dbo].[FoulLogs]  WITH CHECK ADD  CONSTRAINT [FK_PlayerFoulLog] FOREIGN KEY([Player_PlayerId])
REFERENCES [dbo].[Players] ([PlayerId])
GO
ALTER TABLE [dbo].[FoulLogs] CHECK CONSTRAINT [FK_PlayerFoulLog]
GO
ALTER TABLE [dbo].[Games]  WITH CHECK ADD  CONSTRAINT [FK_SeasonGame] FOREIGN KEY([Season_SeasonId])
REFERENCES [dbo].[Seasons] ([SeasonId])
GO
ALTER TABLE [dbo].[Games] CHECK CONSTRAINT [FK_SeasonGame]
GO
ALTER TABLE [dbo].[MediaLogs]  WITH CHECK ADD  CONSTRAINT [FK_GameMediaLog] FOREIGN KEY([Game_GameId])
REFERENCES [dbo].[Games] ([GameId])
GO
ALTER TABLE [dbo].[MediaLogs] CHECK CONSTRAINT [FK_GameMediaLog]
GO
ALTER TABLE [dbo].[Players]  WITH CHECK ADD  CONSTRAINT [FK_TeamPlayer] FOREIGN KEY([Team_TeamId])
REFERENCES [dbo].[Teams] ([TeamId])
GO
ALTER TABLE [dbo].[Players] CHECK CONSTRAINT [FK_TeamPlayer]
GO
ALTER TABLE [dbo].[ScoringLogs]  WITH CHECK ADD  CONSTRAINT [FK_GameScoringLog] FOREIGN KEY([Game_GameId])
REFERENCES [dbo].[Games] ([GameId])
GO
ALTER TABLE [dbo].[ScoringLogs] CHECK CONSTRAINT [FK_GameScoringLog]
GO
ALTER TABLE [dbo].[ScoringLogs]  WITH CHECK ADD  CONSTRAINT [FK_PlayerScoringLog] FOREIGN KEY([Player_PlayerId])
REFERENCES [dbo].[Players] ([PlayerId])
GO
ALTER TABLE [dbo].[ScoringLogs] CHECK CONSTRAINT [FK_PlayerScoringLog]
GO
ALTER TABLE [dbo].[Seasons]  WITH CHECK ADD  CONSTRAINT [FK_LeagueSeason] FOREIGN KEY([League_LeagueId])
REFERENCES [dbo].[Leagues] ([LeagueId])
GO
ALTER TABLE [dbo].[Seasons] CHECK CONSTRAINT [FK_LeagueSeason]
GO
ALTER TABLE [dbo].[Teams]  WITH CHECK ADD  CONSTRAINT [FK_LeagueTeam] FOREIGN KEY([League_LeagueId])
REFERENCES [dbo].[Leagues] ([LeagueId])
GO
ALTER TABLE [dbo].[Teams] CHECK CONSTRAINT [FK_LeagueTeam]
GO
