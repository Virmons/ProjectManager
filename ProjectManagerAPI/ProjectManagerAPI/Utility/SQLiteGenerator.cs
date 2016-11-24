using ProjectManagerAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Web;

namespace ProjectManagerAPI.Utility
{
    public class SQLiteGenerator
    {
        public void GenerateSqlite(int userID, List<Project> projects, List<IntermediateTable> projectPeople, List<Person> people, List<Story> stories, List<IDValuePair> actors, List<IDValuePair> themes, List<Task> tasks, List<IntermediateTable> sprintTasks, List<IDValuePair> sprints, List<WorkLog> workLogs)
        {
            
            string sqliteFile = AppDomain.CurrentDomain.BaseDirectory + "Sqlite\\" + userID.ToString() + ".sqlite";

            SQLiteConnection.CreateFile(sqliteFile);

            SQLiteConnection sqliteConnection = new SQLiteConnection("Data Source=" + sqliteFile + ";Version=3;");
            sqliteConnection.Open();

            List<string> sqlList = new List<string>();

            sqlList.Add("CREATE TABLE ProjectPerson(ID int IDENTITY NOT NULL, ProjectID int NOT NULL, PersonID int NOT NULL, Active bit NOT NULL, PRIMARY KEY ID)");
            sqlList.Add("CREATE TABLE Sprint(	ID int IDENTITY(1,1) NOT NULL,	[Description] [varchar](50) NOT NULL,	[Active] [bit] NOT NULL, CONSTRAINT [PK_ProjectPerson] PRIMARY KEY CLUSTERED (	[ID] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");
            sqlList.Add("CREATE TABLE SprintTask(	[ID] [int] IDENTITY(1,1) NOT NULL,	[SprintID] [int] NOT NULL,	[TaskID] [int] NOT NULL,	[Active] [bit] NOT NULL, CONSTRAINT [PK_SprintTask] PRIMARY KEY CLUSTERED (	[ID] ASC )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");
            sqlList.Add("CREATE TABLE Story(	[ID] [int] IDENTITY(1,1) NOT NULL,	[Name] [varchar](50) NOT NULL,	[ThemeID] [int] NOT NULL,	[ActorID] [int] NOT NULL,	[IWantTo] [varchar](max) NOT NULL,	[SoThat] [varchar](max) NOT NULL,	[Notes] [varchar](max) NULL,	[Priority] [int] NOT NULL,	[Estimate] [varchar](8) NOT NULL,	[TimeEstimate] [decimal](7, 3) NOT NULL,	[Status] [int] NOT NULL,	[ProjectID] [int] NOT NULL,	[Active] [bit] NOT NULL,	[PercentageCompletion] [decimal](6, 3) NOT NULL, CONSTRAINT [PK_Story] PRIMARY KEY CLUSTERED (	[ID] ASC )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]");
            sqlList.Add("CREATE TABLE Task(	[ID] [int] IDENTITY(1,1) NOT NULL,	[Description] [varchar](max) NOT NULL,	[TimeEstimate] [int] NOT NULL,	[StoryID] [int] NOT NULL,	[Active] [bit] NOT NULL,	[Complete] [bit] NOT NULL, CONSTRAINT [PK_Task] PRIMARY KEY CLUSTERED (	[ID] ASC )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]");
            sqlList.Add("CREATE TABLE Theme(	[ID] [int] IDENTITY(1,1) NOT NULL,	[Name] [varchar](50) NOT NULL,	[Active] [bit] NOT NULL, CONSTRAINT [PK_Theme] PRIMARY KEY CLUSTERED (	[ID] ASC )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");
            sqlList.Add("CREATE TABLE WorkLog(	[ID] [int] IDENTITY(1,1) NOT NULL,	[TaskID] [int] NOT NULL,	[Time] [int] NOT NULL, CONSTRAINT [PK_WorkLog] PRIMARY KEY CLUSTERED (	[ID] ASC )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");
            sqlList.Add("CREATE TABLE Actor(	[ID] [int] IDENTITY(1,1) NOT NULL,	[Name] [varchar](50) NOT NULL,	[Active] [bit] NOT NULL, CONSTRAINT [PK_Actor] PRIMARY KEY CLUSTERED (	[ID] ASC )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");
            sqlList.Add("CREATE TABLE Person(	[ID] [int] IDENTITY(1,1) NOT NULL,	[Name] [varchar](50) NOT NULL,	[Initials] [varchar](5) NOT NULL,	[Administrator] [bit] NOT NULL,	[Active] [bit] NOT NULL, CONSTRAINT [PK_Person] PRIMARY KEY CLUSTERED (	[ID] ASC )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");
            sqlList.Add("CREATE TABLE Project(	[ID] [int] IDENTITY(1,1) NOT NULL,	[Name] [varchar](50) NOT NULL,	[Active] [bit] NOT NULL, CONSTRAINT [PK_Project] PRIMARY KEY CLUSTERED (	[ID] ASC )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

            try
            {
                foreach (string sql in sqlList)
                {
                    executeNonQuery(sql, sqliteConnection);
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }


            Console.WriteLine("Aaah");

        }

        public void executeNonQuery(string sql, SQLiteConnection sqliteConnection)
        {
            SQLiteCommand command = new SQLiteCommand(sql, sqliteConnection);
            command.ExecuteNonQuery();
        }
    }
}