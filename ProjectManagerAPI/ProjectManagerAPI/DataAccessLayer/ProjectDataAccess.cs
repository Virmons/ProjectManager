using ProjectManagerAPI.Models;
using ProjectManagerAPI.Utility;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Wolf.Assembly.Logging;
using System.Configuration;
using System.Web.Configuration;
using System.Data;

namespace ProjectManagerAPI.DataAccessLayer
{
    public class ProjectDataAccess
    {
        public string connectionString = ConfigurationManager.ConnectionStrings["DBConstr"].ConnectionString;

        public List<Project> getAllProjects(int userID)
        {
            using (new MethodLogging())
            {
                List<Project> projectList = new List<Project>();
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        using (SqlCommand command = new SqlCommand("usp_ProjectGetByUserID", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@UserID", userID).Direction = ParameterDirection.Input;
                            connection.Open();
                            SqlDataReader reader = command.ExecuteReader();
                            while (reader.Read())
                            {
                                projectList.Add(new Project
                                {
                                    ID = int.Parse(reader["ID"].ToString()),
                                    ProjectName = reader["ProjectName"].ToString(),
                                    DateCreated = DateTime.Parse(reader["DateCreated"].ToString()),
                                    Active = Boolean.Parse(reader["Active"].ToString())
                                });
                            }
                            connection.Close();


                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return projectList;
            }
        }

        public List<Story> getStoriesByProjectID(int projectID)
        {
            using (new MethodLogging())
            {
                List<Story> projectStories = new List<Story>();
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        using (SqlCommand command = new SqlCommand("usp_StoryGetByProjectID", connection))
                        {

                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@ProjectID", projectID).Direction = ParameterDirection.Input;
                            connection.Open();
                            SqlDataReader reader = command.ExecuteReader();
                            while (reader.Read())
                            {
                                projectStories.Add(new Story
                                {
                                    ID = reader.GetValueOrDefault<int>("ID"),
                                    Actor = reader.GetValueOrDefault<string>("Actor"),
                                    DateCreated = reader.GetValueOrDefault<DateTime>("DateCreated"),
                                    Active = reader.GetValueOrDefault<bool>("Active"),
                                    CreatedBy = reader.GetValueOrDefault<string>("CreatedBy"),
                                    Estimate = reader.GetValueOrDefault<string>("Estimate"),
                                    IWantTo = reader.GetValueOrDefault<string>("IWantTo"),
                                    LastEdited = reader.GetValueOrDefault<DateTime>("LastEdited"),
                                    LastEditedBy = reader.GetValueOrDefault<string>("LastEditedBy"),
                                    Notes = reader.GetValueOrDefault<string>("Notes"),
                                    PercentageCompletion = reader.GetValueOrDefault<decimal>("PercentageCompletion"),
                                    Priority = reader.GetValueOrDefault<int>("Priority"),
                                    ProjectID = reader.GetValueOrDefault<int>("ProjectID"),
                                    SoThat = reader.GetValueOrDefault<string>("SoThat"),
                                    Status = reader.GetValueOrDefault<int>("Status"),
                                    StoryName = reader.GetValueOrDefault<string>("Name"),
                                    Theme = reader.GetValueOrDefault<string>("Theme"),
                                    TimeEstimate = reader.GetValueOrDefault<decimal>("TimeEstimate")
                                });
                            }
                            connection.Close();
                        }
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
                return projectStories;
            }
        }

        public List<Task> getTasksByStoryID(int? storyID)
        {
            using (new MethodLogging())
            {
                List<Task> storyTasks = new List<Task>();
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        using (SqlCommand command = new SqlCommand("usp_TaskGetByStoryID", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@StoryID", storyID).Direction = ParameterDirection.Input;
                            connection.Open();
                            SqlDataReader reader = command.ExecuteReader();
                            while (reader.Read())
                            {
                                storyTasks.Add(new Task
                                {
                                    ID = reader.GetValueOrDefault<int>("ID"),
                                    DateCreated = reader.GetValueOrDefault<DateTime>("DateCreated"),
                                    Active = reader.GetValueOrDefault<bool>("Active"),
                                    CreatedBy = reader.GetValueOrDefault<string>("CreatedBy"),
                                    Complete = reader.GetValueOrDefault<bool>("Complete"),
                                    Description = reader.GetValueOrDefault<string>("Description"),
                                    StoryID = reader.GetValueOrDefault<int>("StoryID"),
                                    TimeEstimate = reader.GetValueOrDefault<int>("TimeEstimate"),
                                    TimeTaken = reader.GetValueOrDefault<int>("TimeTaken")

                                });
                            }
                            connection.Close();
                        }
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
                return storyTasks;
            }
        }

        public List<WorkLog> getWorkLogByTaskID(int? taskID)
        {
            using (new MethodLogging())
            {
                List<WorkLog> taskWorkLogs = new List<WorkLog>();
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        using (SqlCommand command = new SqlCommand("usp_WorkLogGetByTaskID", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@TaskID", taskID).Direction = ParameterDirection.Input;
                            connection.Open();
                            SqlDataReader reader = command.ExecuteReader();
                            while (reader.Read())
                            {
                                taskWorkLogs.Add(new WorkLog
                                {
                                    ID = reader.GetValueOrDefault<int>("ID"),
                                    DateCreated = reader.GetValueOrDefault<DateTime>("DateCreated"),
                                    Person = reader.GetValueOrDefault<string>("PersonName"),
                                    PersonID = reader.GetValueOrDefault<int>("PersonID"),
                                    TaskID = reader.GetValueOrDefault<int>("TaskID"),
                                    Time = reader.GetValueOrDefault<int>("TimeTaken")

                                });
                            }
                            connection.Close();
                        }
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
                return taskWorkLogs;
            }
        }

        public List<Person> getPersonellByProjectID(int? projectID)
        {
            using (new MethodLogging())
            {
                List<Person> projectPersonell = new List<Person>();
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        using (SqlCommand command = new SqlCommand("usp_PersonGetByProjectID", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@ProjectID", projectID).Direction = ParameterDirection.Input;
                            connection.Open();
                            SqlDataReader reader = command.ExecuteReader();
                            while (reader.Read())
                            {
                                projectPersonell.Add(new Person
                                {
                                    ID = reader.GetValueOrDefault<int>("ID"),
                                    DateCreated = reader.GetValueOrDefault<DateTime>("DateCreated"),
                                    Active = reader.GetValueOrDefault<bool>("Active"),
                                    Administrator = reader.GetValueOrDefault<bool>("Administrator"),
                                    Initials = reader.GetValueOrDefault<string>("Initials"),
                                    Name = reader.GetValueOrDefault<string>("Name")

                                });
                            }
                            connection.Close();
                        }
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
                return projectPersonell;
            }
        }

        public bool AddProject(Project data, string user)
        {
            using (new MethodLogging())
            {
                bool wasAdded = false;

                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        using (SqlCommand command = new SqlCommand("usp_ProjectAddProject", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@User", user).Direction = ParameterDirection.Input;
                            command.Parameters.AddWithValue("@ProjectName", data.ProjectName).Direction = ParameterDirection.Input;
                            command.Parameters.AddWithValue("@wasAdded", wasAdded).Direction = ParameterDirection.Output;
                            connection.Open();
                            command.ExecuteNonQuery();
                            wasAdded = bool.Parse(command.Parameters["@WasAdded"].Value.ToString());
                            connection.Close();
                        }
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }

                return wasAdded;
            }
        }
    }
}