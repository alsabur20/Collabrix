USE [master]
GO
/****** Object:  Database [Colabrix]    Script Date: 10/13/2024 9:46:11 PM ******/
CREATE DATABASE [Colabrix]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Colabrix', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\Colabrix.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Colabrix_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\Colabrix_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [Colabrix] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Colabrix].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Colabrix] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Colabrix] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Colabrix] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Colabrix] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Colabrix] SET ARITHABORT OFF 
GO
ALTER DATABASE [Colabrix] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Colabrix] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Colabrix] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Colabrix] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Colabrix] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Colabrix] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Colabrix] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Colabrix] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Colabrix] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Colabrix] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Colabrix] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Colabrix] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Colabrix] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Colabrix] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Colabrix] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Colabrix] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Colabrix] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Colabrix] SET RECOVERY FULL 
GO
ALTER DATABASE [Colabrix] SET  MULTI_USER 
GO
ALTER DATABASE [Colabrix] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Colabrix] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Colabrix] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Colabrix] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Colabrix] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [Colabrix] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'Colabrix', N'ON'
GO
ALTER DATABASE [Colabrix] SET QUERY_STORE = ON
GO
ALTER DATABASE [Colabrix] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [Colabrix]
GO
/****** Object:  Table [dbo].[ChatMessages]    Script Date: 10/13/2024 9:46:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChatMessages](
	[message_id] [uniqueidentifier] NOT NULL,
	[chat_room_id] [uniqueidentifier] NULL,
	[user_id] [uniqueidentifier] NULL,
	[message_content] [nvarchar](max) NOT NULL,
	[created_at] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[message_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ChatRooms]    Script Date: 10/13/2024 9:46:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChatRooms](
	[chat_room_id] [uniqueidentifier] NOT NULL,
	[project_id] [uniqueidentifier] NULL,
	[created_at] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[chat_room_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FileAttachments]    Script Date: 10/13/2024 9:46:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FileAttachments](
	[attachment_id] [uniqueidentifier] NOT NULL,
	[task_id] [uniqueidentifier] NULL,
	[message_id] [uniqueidentifier] NULL,
	[file_path] [nvarchar](255) NOT NULL,
	[uploaded_by] [uniqueidentifier] NULL,
	[uploaded_at] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[attachment_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProjectMembers]    Script Date: 10/13/2024 9:46:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProjectMembers](
	[project_member_id] [uniqueidentifier] NOT NULL,
	[project_id] [uniqueidentifier] NULL,
	[user_id] [uniqueidentifier] NULL,
	[role] [nvarchar](50) NULL,
	[joined_at] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[project_member_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Projects]    Script Date: 10/13/2024 9:46:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Projects](
	[project_id] [uniqueidentifier] NOT NULL,
	[project_name] [nvarchar](255) NOT NULL,
	[description] [nvarchar](max) NULL,
	[start_date] [date] NULL,
	[end_date] [date] NULL,
	[type] [nvarchar](50) NULL,
	[status] [nvarchar](50) NULL,
	[created_by] [uniqueidentifier] NULL,
	[created_at] [datetime] NULL,
	[updated_at] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[project_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProjectTaskStage]    Script Date: 10/13/2024 9:46:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProjectTaskStage](
	[id] [uniqueidentifier] NOT NULL,
	[project_id] [uniqueidentifier] NULL,
	[stage_id] [uniqueidentifier] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TaskComments]    Script Date: 10/13/2024 9:46:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TaskComments](
	[comment_id] [uniqueidentifier] NOT NULL,
	[task_id] [uniqueidentifier] NULL,
	[user_id] [uniqueidentifier] NULL,
	[content] [nvarchar](max) NOT NULL,
	[created_at] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[comment_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tasks]    Script Date: 10/13/2024 9:46:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tasks](
	[task_id] [uniqueidentifier] NOT NULL,
	[project_id] [uniqueidentifier] NULL,
	[title] [nvarchar](255) NOT NULL,
	[description] [nvarchar](max) NULL,
	[assigned_to] [uniqueidentifier] NULL,
	[stage_id] [uniqueidentifier] NULL,
	[due_date] [date] NULL,
	[created_at] [datetime] NULL,
	[updated_at] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[task_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TaskStages]    Script Date: 10/13/2024 9:46:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TaskStages](
	[stage_id] [uniqueidentifier] NOT NULL,
	[stage_name] [nvarchar](255) NOT NULL,
	[stage_description] [nvarchar](max) NULL,
	[created_by] [uniqueidentifier] NULL,
	[created_at] [datetime] NULL,
	[updated_at] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[stage_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 10/13/2024 9:46:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[user_id] [uniqueidentifier] NOT NULL,
	[username] [nvarchar](255) NOT NULL,
	[email] [nvarchar](255) NOT NULL,
	[password_hash] [nvarchar](255) NOT NULL,
	[profile_picture] [nvarchar](255) NULL,
	[created_at] [datetime] NULL,
	[updated_at] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[user_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ChatMessages] ADD  DEFAULT (newid()) FOR [message_id]
GO
ALTER TABLE [dbo].[ChatMessages] ADD  DEFAULT (getdate()) FOR [created_at]
GO
ALTER TABLE [dbo].[ChatRooms] ADD  DEFAULT (newid()) FOR [chat_room_id]
GO
ALTER TABLE [dbo].[ChatRooms] ADD  DEFAULT (getdate()) FOR [created_at]
GO
ALTER TABLE [dbo].[FileAttachments] ADD  DEFAULT (newid()) FOR [attachment_id]
GO
ALTER TABLE [dbo].[FileAttachments] ADD  DEFAULT (getdate()) FOR [uploaded_at]
GO
ALTER TABLE [dbo].[ProjectMembers] ADD  DEFAULT (newid()) FOR [project_member_id]
GO
ALTER TABLE [dbo].[ProjectMembers] ADD  DEFAULT (getdate()) FOR [joined_at]
GO
ALTER TABLE [dbo].[Projects] ADD  DEFAULT (newid()) FOR [project_id]
GO
ALTER TABLE [dbo].[Projects] ADD  DEFAULT (getdate()) FOR [created_at]
GO
ALTER TABLE [dbo].[Projects] ADD  DEFAULT (getdate()) FOR [updated_at]
GO
ALTER TABLE [dbo].[ProjectTaskStage] ADD  DEFAULT (newid()) FOR [id]
GO
ALTER TABLE [dbo].[TaskComments] ADD  DEFAULT (newid()) FOR [comment_id]
GO
ALTER TABLE [dbo].[TaskComments] ADD  DEFAULT (getdate()) FOR [created_at]
GO
ALTER TABLE [dbo].[Tasks] ADD  DEFAULT (newid()) FOR [task_id]
GO
ALTER TABLE [dbo].[Tasks] ADD  DEFAULT (getdate()) FOR [created_at]
GO
ALTER TABLE [dbo].[Tasks] ADD  DEFAULT (getdate()) FOR [updated_at]
GO
ALTER TABLE [dbo].[TaskStages] ADD  DEFAULT (newid()) FOR [stage_id]
GO
ALTER TABLE [dbo].[TaskStages] ADD  DEFAULT (getdate()) FOR [created_at]
GO
ALTER TABLE [dbo].[TaskStages] ADD  DEFAULT (getdate()) FOR [updated_at]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT (newid()) FOR [user_id]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT (getdate()) FOR [created_at]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT (getdate()) FOR [updated_at]
GO
ALTER TABLE [dbo].[ChatMessages]  WITH CHECK ADD FOREIGN KEY([chat_room_id])
REFERENCES [dbo].[ChatRooms] ([chat_room_id])
GO
ALTER TABLE [dbo].[ChatMessages]  WITH CHECK ADD FOREIGN KEY([user_id])
REFERENCES [dbo].[Users] ([user_id])
GO
ALTER TABLE [dbo].[ChatRooms]  WITH CHECK ADD FOREIGN KEY([project_id])
REFERENCES [dbo].[Projects] ([project_id])
GO
ALTER TABLE [dbo].[FileAttachments]  WITH CHECK ADD FOREIGN KEY([message_id])
REFERENCES [dbo].[ChatMessages] ([message_id])
GO
ALTER TABLE [dbo].[FileAttachments]  WITH CHECK ADD FOREIGN KEY([task_id])
REFERENCES [dbo].[Tasks] ([task_id])
GO
ALTER TABLE [dbo].[FileAttachments]  WITH CHECK ADD FOREIGN KEY([uploaded_by])
REFERENCES [dbo].[Users] ([user_id])
GO
ALTER TABLE [dbo].[ProjectMembers]  WITH CHECK ADD FOREIGN KEY([project_id])
REFERENCES [dbo].[Projects] ([project_id])
GO
ALTER TABLE [dbo].[ProjectMembers]  WITH CHECK ADD FOREIGN KEY([user_id])
REFERENCES [dbo].[Users] ([user_id])
GO
ALTER TABLE [dbo].[Projects]  WITH CHECK ADD FOREIGN KEY([created_by])
REFERENCES [dbo].[Users] ([user_id])
GO
ALTER TABLE [dbo].[ProjectTaskStage]  WITH CHECK ADD FOREIGN KEY([project_id])
REFERENCES [dbo].[Projects] ([project_id])
GO
ALTER TABLE [dbo].[ProjectTaskStage]  WITH CHECK ADD FOREIGN KEY([stage_id])
REFERENCES [dbo].[TaskStages] ([stage_id])
GO
ALTER TABLE [dbo].[TaskComments]  WITH CHECK ADD FOREIGN KEY([task_id])
REFERENCES [dbo].[Tasks] ([task_id])
GO
ALTER TABLE [dbo].[TaskComments]  WITH CHECK ADD FOREIGN KEY([user_id])
REFERENCES [dbo].[Users] ([user_id])
GO
ALTER TABLE [dbo].[Tasks]  WITH CHECK ADD FOREIGN KEY([assigned_to])
REFERENCES [dbo].[Users] ([user_id])
GO
ALTER TABLE [dbo].[Tasks]  WITH CHECK ADD FOREIGN KEY([project_id])
REFERENCES [dbo].[Projects] ([project_id])
GO
ALTER TABLE [dbo].[Tasks]  WITH CHECK ADD FOREIGN KEY([stage_id])
REFERENCES [dbo].[ProjectTaskStage] ([id])
GO
ALTER TABLE [dbo].[TaskStages]  WITH CHECK ADD FOREIGN KEY([created_by])
REFERENCES [dbo].[Users] ([user_id])
GO
ALTER TABLE [dbo].[ProjectMembers]  WITH CHECK ADD CHECK  (([role]='Other' OR [role]='Member' OR [role]='Team Leader'))
GO
ALTER TABLE [dbo].[Projects]  WITH CHECK ADD CHECK  (([status]='Archived' OR [status]='Completed' OR [status]='Active'))
GO
ALTER TABLE [dbo].[Projects]  WITH CHECK ADD CHECK  (([type]='Other' OR [type]='Marketing' OR [type]='Software'))
GO
USE [master]
GO
ALTER DATABASE [Colabrix] SET  READ_WRITE 
GO
