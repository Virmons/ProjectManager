USE [SSTest]
GO
/****** Object:  Table [dbo].[Actor]    Script Date: 23/11/2016 13:08:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Actor](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Actor] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Event]    Script Date: 23/11/2016 13:08:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Event](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[DateCreated] [datetime2](3) NOT NULL,
	[UserID] [int] NOT NULL,
	[EventType] [int] NOT NULL,
	[EventItemType] [int] NULL,
	[EventItemID] [int] NOT NULL,
 CONSTRAINT [PK_Event] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[EventItemType]    Script Date: 23/11/2016 13:08:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EventItemType](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_EventItemType] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[EventType]    Script Date: 23/11/2016 13:08:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EventType](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Event] [varchar](50) NOT NULL,
 CONSTRAINT [PK_EventType] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Person]    Script Date: 23/11/2016 13:08:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Person](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Initials] [varchar](5) NOT NULL,
	[DateCreated] [datetime2](3) NOT NULL,
	[Administrator] [bit] NOT NULL,
	[Active] [bit] NOT NULL,
	[CreatedBy] [int] NOT NULL,
 CONSTRAINT [PK_Person] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Project]    Script Date: 23/11/2016 13:08:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Project](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[DateCreated] [datetime2](3) NOT NULL,
	[CreatedByPersonID] [int] NOT NULL,
	[Active] [bit] NOT NULL,
 CONSTRAINT [PK_Project] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ProjectPerson]    Script Date: 23/11/2016 13:08:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProjectPerson](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ProjectID] [int] NOT NULL,
	[PersonID] [int] NOT NULL,
	[Active] [bit] NOT NULL,
 CONSTRAINT [PK_ProjectPerson] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Sprint]    Script Date: 23/11/2016 13:08:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sprint](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Number] [int] NOT NULL,
	[DateCreated] [datetime2](3) NOT NULL,
	[DateStarted] [datetime2](3) NULL,
	[DateEnded] [datetime2](3) NULL,
	[CreatedByPersonID] [int] NOT NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SprintTask]    Script Date: 23/11/2016 13:08:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SprintTask](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[SprintID] [int] NOT NULL,
	[TaskID] [int] NOT NULL,
	[Active] [bit] NOT NULL,
 CONSTRAINT [PK_SprintTask] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Story]    Script Date: 23/11/2016 13:08:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Story](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[DateCreated] [datetime2](3) NOT NULL,
	[ThemeID] [int] NOT NULL,
	[ActorID] [int] NOT NULL,
	[IWantTo] [varchar](max) NOT NULL,
	[SoThat] [varchar](max) NOT NULL,
	[Notes] [varchar](max) NULL,
	[Priority] [int] NOT NULL,
	[Estimate] [varchar](8) NOT NULL,
	[TimeEstimate] [decimal](7, 3) NOT NULL,
	[Status] [int] NOT NULL,
	[CreatedByPersonID] [int] NOT NULL,
	[ProjectID] [int] NOT NULL,
	[Active] [bit] NOT NULL,
	[PercentageCompletion] [decimal](6, 3) NOT NULL,
 CONSTRAINT [PK_Story] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Task]    Script Date: 23/11/2016 13:08:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Task](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Description] [varchar](max) NOT NULL,
	[DateCreated] [datetime2](3) NOT NULL,
	[CreatedByID] [int] NOT NULL,
	[TimeEstimate] [int] NOT NULL,
	[StoryID] [int] NOT NULL,
	[Active] [bit] NOT NULL,
	[Complete] [bit] NOT NULL,
 CONSTRAINT [PK_Task] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Theme]    Script Date: 23/11/2016 13:08:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Theme](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Theme] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[WorkLog]    Script Date: 23/11/2016 13:08:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WorkLog](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[TaskID] [int] NOT NULL,
	[Time] [int] NOT NULL,
	[DateCreated] [datetime2](3) NOT NULL,
	[PersonID] [int] NOT NULL,
 CONSTRAINT [PK_WorkLog] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET IDENTITY_INSERT [dbo].[Actor] ON 

INSERT [dbo].[Actor] ([ID], [Name]) VALUES (1, N'All')
INSERT [dbo].[Actor] ([ID], [Name]) VALUES (2, N'User')
INSERT [dbo].[Actor] ([ID], [Name]) VALUES (3, N'System')
INSERT [dbo].[Actor] ([ID], [Name]) VALUES (4, N'Administrator')
SET IDENTITY_INSERT [dbo].[Actor] OFF
SET IDENTITY_INSERT [dbo].[Event] ON 

INSERT [dbo].[Event] ([ID], [DateCreated], [UserID], [EventType], [EventItemType], [EventItemID]) VALUES (2, CAST(N'2016-11-18T09:32:30.2500000' AS DateTime2), 1, 1, 3, 1)
INSERT [dbo].[Event] ([ID], [DateCreated], [UserID], [EventType], [EventItemType], [EventItemID]) VALUES (3, CAST(N'2016-11-18T09:41:03.5130000' AS DateTime2), 1, 2, 3, 1)
INSERT [dbo].[Event] ([ID], [DateCreated], [UserID], [EventType], [EventItemType], [EventItemID]) VALUES (4, CAST(N'2016-11-18T11:57:08.5100000' AS DateTime2), 1, 1, 4, 1)
INSERT [dbo].[Event] ([ID], [DateCreated], [UserID], [EventType], [EventItemType], [EventItemID]) VALUES (5, CAST(N'2016-11-18T11:57:16.4930000' AS DateTime2), 1, 1, 4, 2)
INSERT [dbo].[Event] ([ID], [DateCreated], [UserID], [EventType], [EventItemType], [EventItemID]) VALUES (6, CAST(N'2016-11-18T11:57:22.0270000' AS DateTime2), 1, 1, 4, 3)
INSERT [dbo].[Event] ([ID], [DateCreated], [UserID], [EventType], [EventItemType], [EventItemID]) VALUES (7, CAST(N'2016-11-22T13:23:02.6500000' AS DateTime2), 1, 2, 1, 1)
INSERT [dbo].[Event] ([ID], [DateCreated], [UserID], [EventType], [EventItemType], [EventItemID]) VALUES (8, CAST(N'2016-11-22T13:35:55.1400000' AS DateTime2), 1, 2, 1, 1)
INSERT [dbo].[Event] ([ID], [DateCreated], [UserID], [EventType], [EventItemType], [EventItemID]) VALUES (9, CAST(N'2016-11-22T14:57:14.7200000' AS DateTime2), 1, 3, 2, 1)
INSERT [dbo].[Event] ([ID], [DateCreated], [UserID], [EventType], [EventItemType], [EventItemID]) VALUES (10, CAST(N'2016-11-22T15:02:31.7000000' AS DateTime2), 1, 3, 2, 1)
INSERT [dbo].[Event] ([ID], [DateCreated], [UserID], [EventType], [EventItemType], [EventItemID]) VALUES (11, CAST(N'2016-11-22T15:02:35.3400000' AS DateTime2), 1, 3, 2, 1)
INSERT [dbo].[Event] ([ID], [DateCreated], [UserID], [EventType], [EventItemType], [EventItemID]) VALUES (12, CAST(N'2016-11-22T15:02:47.3000000' AS DateTime2), 1, 3, 2, 1)
INSERT [dbo].[Event] ([ID], [DateCreated], [UserID], [EventType], [EventItemType], [EventItemID]) VALUES (13, CAST(N'2016-11-22T15:03:18.4730000' AS DateTime2), 1, 3, 2, 1)
INSERT [dbo].[Event] ([ID], [DateCreated], [UserID], [EventType], [EventItemType], [EventItemID]) VALUES (14, CAST(N'2016-11-22T15:03:29.3000000' AS DateTime2), 1, 3, 2, 1)
INSERT [dbo].[Event] ([ID], [DateCreated], [UserID], [EventType], [EventItemType], [EventItemID]) VALUES (15, CAST(N'2016-11-22T15:03:56.2970000' AS DateTime2), 1, 3, 2, 1)
INSERT [dbo].[Event] ([ID], [DateCreated], [UserID], [EventType], [EventItemType], [EventItemID]) VALUES (16, CAST(N'2016-11-22T15:04:01.3900000' AS DateTime2), 1, 3, 2, 1)
INSERT [dbo].[Event] ([ID], [DateCreated], [UserID], [EventType], [EventItemType], [EventItemID]) VALUES (17, CAST(N'2016-11-22T15:53:59.1170000' AS DateTime2), 1, 2, 3, 1)
INSERT [dbo].[Event] ([ID], [DateCreated], [UserID], [EventType], [EventItemType], [EventItemID]) VALUES (18, CAST(N'2016-11-22T15:53:59.1400000' AS DateTime2), 1, 2, 4, 1)
INSERT [dbo].[Event] ([ID], [DateCreated], [UserID], [EventType], [EventItemType], [EventItemID]) VALUES (19, CAST(N'2016-11-22T15:53:59.1500000' AS DateTime2), 1, 2, 4, 2)
INSERT [dbo].[Event] ([ID], [DateCreated], [UserID], [EventType], [EventItemType], [EventItemID]) VALUES (20, CAST(N'2016-11-22T15:53:59.1570000' AS DateTime2), 1, 2, 4, 3)
INSERT [dbo].[Event] ([ID], [DateCreated], [UserID], [EventType], [EventItemType], [EventItemID]) VALUES (21, CAST(N'2016-11-22T16:02:53.7630000' AS DateTime2), 1, 2, 1, 1)
INSERT [dbo].[Event] ([ID], [DateCreated], [UserID], [EventType], [EventItemType], [EventItemID]) VALUES (22, CAST(N'2016-11-22T16:08:37.7770000' AS DateTime2), 1, 2, 3, 1)
INSERT [dbo].[Event] ([ID], [DateCreated], [UserID], [EventType], [EventItemType], [EventItemID]) VALUES (23, CAST(N'2016-11-22T16:08:37.7900000' AS DateTime2), 1, 2, 4, 1)
INSERT [dbo].[Event] ([ID], [DateCreated], [UserID], [EventType], [EventItemType], [EventItemID]) VALUES (24, CAST(N'2016-11-22T16:08:37.8000000' AS DateTime2), 1, 2, 4, 2)
INSERT [dbo].[Event] ([ID], [DateCreated], [UserID], [EventType], [EventItemType], [EventItemID]) VALUES (25, CAST(N'2016-11-22T16:08:37.8070000' AS DateTime2), 1, 2, 4, 3)
INSERT [dbo].[Event] ([ID], [DateCreated], [UserID], [EventType], [EventItemType], [EventItemID]) VALUES (26, CAST(N'2016-11-22T16:11:24.4530000' AS DateTime2), 1, 2, 3, 1)
INSERT [dbo].[Event] ([ID], [DateCreated], [UserID], [EventType], [EventItemType], [EventItemID]) VALUES (27, CAST(N'2016-11-22T16:11:24.4630000' AS DateTime2), 1, 2, 4, 1)
INSERT [dbo].[Event] ([ID], [DateCreated], [UserID], [EventType], [EventItemType], [EventItemID]) VALUES (28, CAST(N'2016-11-22T16:11:24.4700000' AS DateTime2), 1, 2, 4, 2)
INSERT [dbo].[Event] ([ID], [DateCreated], [UserID], [EventType], [EventItemType], [EventItemID]) VALUES (29, CAST(N'2016-11-22T16:11:24.4800000' AS DateTime2), 1, 2, 4, 3)
INSERT [dbo].[Event] ([ID], [DateCreated], [UserID], [EventType], [EventItemType], [EventItemID]) VALUES (30, CAST(N'2016-11-22T16:13:20.9370000' AS DateTime2), 1, 2, 3, 1)
INSERT [dbo].[Event] ([ID], [DateCreated], [UserID], [EventType], [EventItemType], [EventItemID]) VALUES (31, CAST(N'2016-11-22T16:13:20.9500000' AS DateTime2), 1, 2, 4, 1)
INSERT [dbo].[Event] ([ID], [DateCreated], [UserID], [EventType], [EventItemType], [EventItemID]) VALUES (32, CAST(N'2016-11-22T16:13:20.9600000' AS DateTime2), 1, 2, 4, 2)
INSERT [dbo].[Event] ([ID], [DateCreated], [UserID], [EventType], [EventItemType], [EventItemID]) VALUES (33, CAST(N'2016-11-22T16:13:20.9630000' AS DateTime2), 1, 2, 4, 3)
INSERT [dbo].[Event] ([ID], [DateCreated], [UserID], [EventType], [EventItemType], [EventItemID]) VALUES (34, CAST(N'2016-11-22T16:13:20.9800000' AS DateTime2), 1, 2, 1, 1)
INSERT [dbo].[Event] ([ID], [DateCreated], [UserID], [EventType], [EventItemType], [EventItemID]) VALUES (35, CAST(N'2016-11-22T16:19:04.8070000' AS DateTime2), 1, 2, 3, 1)
INSERT [dbo].[Event] ([ID], [DateCreated], [UserID], [EventType], [EventItemType], [EventItemID]) VALUES (36, CAST(N'2016-11-22T16:19:04.8270000' AS DateTime2), 1, 2, 4, 1)
INSERT [dbo].[Event] ([ID], [DateCreated], [UserID], [EventType], [EventItemType], [EventItemID]) VALUES (37, CAST(N'2016-11-22T16:19:04.8330000' AS DateTime2), 1, 2, 4, 2)
INSERT [dbo].[Event] ([ID], [DateCreated], [UserID], [EventType], [EventItemType], [EventItemID]) VALUES (38, CAST(N'2016-11-22T16:19:04.8400000' AS DateTime2), 1, 2, 4, 3)
INSERT [dbo].[Event] ([ID], [DateCreated], [UserID], [EventType], [EventItemType], [EventItemID]) VALUES (39, CAST(N'2016-11-22T16:19:04.8700000' AS DateTime2), 1, 2, 1, 1)
SET IDENTITY_INSERT [dbo].[Event] OFF
SET IDENTITY_INSERT [dbo].[EventItemType] ON 

INSERT [dbo].[EventItemType] ([ID], [Name]) VALUES (1, N'Project')
INSERT [dbo].[EventItemType] ([ID], [Name]) VALUES (2, N'Person')
INSERT [dbo].[EventItemType] ([ID], [Name]) VALUES (3, N'Story')
INSERT [dbo].[EventItemType] ([ID], [Name]) VALUES (4, N'Task')
INSERT [dbo].[EventItemType] ([ID], [Name]) VALUES (5, N'Sprint')
INSERT [dbo].[EventItemType] ([ID], [Name]) VALUES (6, N'Theme')
INSERT [dbo].[EventItemType] ([ID], [Name]) VALUES (7, N'Actor')
INSERT [dbo].[EventItemType] ([ID], [Name]) VALUES (8, N'ProjectPerson')
SET IDENTITY_INSERT [dbo].[EventItemType] OFF
SET IDENTITY_INSERT [dbo].[EventType] ON 

INSERT [dbo].[EventType] ([ID], [Event]) VALUES (1, N'Add')
INSERT [dbo].[EventType] ([ID], [Event]) VALUES (2, N'Update')
INSERT [dbo].[EventType] ([ID], [Event]) VALUES (3, N'Delete')
INSERT [dbo].[EventType] ([ID], [Event]) VALUES (4, N'Started')
INSERT [dbo].[EventType] ([ID], [Event]) VALUES (5, N'Ended')
SET IDENTITY_INSERT [dbo].[EventType] OFF
SET IDENTITY_INSERT [dbo].[Person] ON 

INSERT [dbo].[Person] ([ID], [Name], [Initials], [DateCreated], [Administrator], [Active], [CreatedBy]) VALUES (1, N'SimeonSidey', N'SS', CAST(N'2016-10-28T13:13:37.1770000' AS DateTime2), 1, 1, 1)
INSERT [dbo].[Person] ([ID], [Name], [Initials], [DateCreated], [Administrator], [Active], [CreatedBy]) VALUES (2, N'James Sharp', N'JS', CAST(N'2016-11-18T13:55:59.3800000' AS DateTime2), 1, 1, 2)
INSERT [dbo].[Person] ([ID], [Name], [Initials], [DateCreated], [Administrator], [Active], [CreatedBy]) VALUES (3, N'TestUser', N'TU', CAST(N'2016-11-21T10:18:32.8700000' AS DateTime2), 0, 1, 1)
SET IDENTITY_INSERT [dbo].[Person] OFF
SET IDENTITY_INSERT [dbo].[Project] ON 

INSERT [dbo].[Project] ([ID], [Name], [DateCreated], [CreatedByPersonID], [Active]) VALUES (1, N'Apcoa ANPRUKRec', CAST(N'2016-10-28T13:14:09.4200000' AS DateTime2), 1, 1)
INSERT [dbo].[Project] ([ID], [Name], [DateCreated], [CreatedByPersonID], [Active]) VALUES (2, N'CSC', CAST(N'2016-10-31T09:58:59.1770000' AS DateTime2), 2, 1)
SET IDENTITY_INSERT [dbo].[Project] OFF
SET IDENTITY_INSERT [dbo].[ProjectPerson] ON 

INSERT [dbo].[ProjectPerson] ([ID], [ProjectID], [PersonID], [Active]) VALUES (1, 1, 1, 1)
INSERT [dbo].[ProjectPerson] ([ID], [ProjectID], [PersonID], [Active]) VALUES (2, 1, 2, 1)
INSERT [dbo].[ProjectPerson] ([ID], [ProjectID], [PersonID], [Active]) VALUES (3, 2, 3, 0)
INSERT [dbo].[ProjectPerson] ([ID], [ProjectID], [PersonID], [Active]) VALUES (4, 1, 3, 1)
SET IDENTITY_INSERT [dbo].[ProjectPerson] OFF
SET IDENTITY_INSERT [dbo].[Sprint] ON 

INSERT [dbo].[Sprint] ([ID], [Number], [DateCreated], [DateStarted], [DateEnded], [CreatedByPersonID]) VALUES (1, 1, CAST(N'2016-10-31T09:37:27.4600000' AS DateTime2), CAST(N'2016-10-31T09:36:00.0000000' AS DateTime2), CAST(N'2016-11-04T10:30:00.0000000' AS DateTime2), 1)
SET IDENTITY_INSERT [dbo].[Sprint] OFF
SET IDENTITY_INSERT [dbo].[SprintTask] ON 

INSERT [dbo].[SprintTask] ([ID], [SprintID], [TaskID], [Active]) VALUES (1, 1, 1, 1)
SET IDENTITY_INSERT [dbo].[SprintTask] OFF
SET IDENTITY_INSERT [dbo].[Story] ON 

INSERT [dbo].[Story] ([ID], [Name], [DateCreated], [ThemeID], [ActorID], [IWantTo], [SoThat], [Notes], [Priority], [Estimate], [TimeEstimate], [Status], [CreatedByPersonID], [ProjectID], [Active], [PercentageCompletion]) VALUES (1, N'Authorisation', CAST(N'2016-10-28T13:27:40.3370000' AS DateTime2), 2, 2, N'Change the want to', N'Change the so that', N'Notes part 2', 1, N'12', CAST(100.000 AS Decimal(7, 3)), 1, 1, 1, 1, CAST(100.000 AS Decimal(6, 3)))
SET IDENTITY_INSERT [dbo].[Story] OFF
SET IDENTITY_INSERT [dbo].[Task] ON 

INSERT [dbo].[Task] ([ID], [Description], [DateCreated], [CreatedByID], [TimeEstimate], [StoryID], [Active], [Complete]) VALUES (1, N'Create UI', CAST(N'2016-11-11T11:49:44.1770000' AS DateTime2), 1, 7200, 1, 1, 0)
INSERT [dbo].[Task] ([ID], [Description], [DateCreated], [CreatedByID], [TimeEstimate], [StoryID], [Active], [Complete]) VALUES (2, N'Documentation', CAST(N'2016-11-18T11:31:33.9100000' AS DateTime2), 1, 3600, 1, 1, 0)
INSERT [dbo].[Task] ([ID], [Description], [DateCreated], [CreatedByID], [TimeEstimate], [StoryID], [Active], [Complete]) VALUES (3, N'Test', CAST(N'2016-11-18T11:32:10.4100000' AS DateTime2), 1, 1800, 1, 1, 0)
SET IDENTITY_INSERT [dbo].[Task] OFF
SET IDENTITY_INSERT [dbo].[Theme] ON 

INSERT [dbo].[Theme] ([ID], [Name]) VALUES (1, N'Interface')
INSERT [dbo].[Theme] ([ID], [Name]) VALUES (2, N'Service')
SET IDENTITY_INSERT [dbo].[Theme] OFF
SET IDENTITY_INSERT [dbo].[WorkLog] ON 

INSERT [dbo].[WorkLog] ([ID], [TaskID], [Time], [DateCreated], [PersonID]) VALUES (1, 1, 3600, CAST(N'2016-11-18T15:42:03.2700000' AS DateTime2), 1)
INSERT [dbo].[WorkLog] ([ID], [TaskID], [Time], [DateCreated], [PersonID]) VALUES (2, 1, 3600, CAST(N'2016-11-18T15:42:03.2700000' AS DateTime2), 2)
INSERT [dbo].[WorkLog] ([ID], [TaskID], [Time], [DateCreated], [PersonID]) VALUES (3, 2, 4000, CAST(N'2016-11-18T15:42:03.2700000' AS DateTime2), 1)
SET IDENTITY_INSERT [dbo].[WorkLog] OFF
ALTER TABLE [dbo].[Event] ADD  CONSTRAINT [DF_Event_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]
GO
ALTER TABLE [dbo].[Person] ADD  CONSTRAINT [DF_Person_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]
GO
ALTER TABLE [dbo].[Project] ADD  CONSTRAINT [DF_Project_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]
GO
ALTER TABLE [dbo].[Sprint] ADD  CONSTRAINT [DF_Sprint_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]
GO
ALTER TABLE [dbo].[SprintTask] ADD  CONSTRAINT [DF_SprintTask_Active]  DEFAULT ((0)) FOR [Active]
GO
ALTER TABLE [dbo].[Story] ADD  CONSTRAINT [DF_Story_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]
GO
ALTER TABLE [dbo].[Story] ADD  CONSTRAINT [DF_Story_Completion]  DEFAULT ((0)) FOR [PercentageCompletion]
GO
ALTER TABLE [dbo].[Task] ADD  CONSTRAINT [DF_Task_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]
GO
ALTER TABLE [dbo].[WorkLog] ADD  CONSTRAINT [DF_WorkLog_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]
GO
