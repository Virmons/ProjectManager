using ProjectManagerAPI.Models;
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
            using(new MethodLogging())
            {
                List<Story> projectStories = new List<Story>();
                try 
                {
                    using(SqlConnection connection = new SqlConnection(connectionString))
                    {
                        using(SqlCommand command = new SqlCommand("usp_StoryGetByProjectID", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@ProjectID", projectID).Direction = ParameterDirection.Input;
                            connection.Open();
                            SqlDataReader reader = command.ExecuteReader();
                            while (reader.Read())
                            {
                                projectStories.Add(new Story
                                {
                                    ID = int.Parse(reader["ID"].ToString()),
                                    Actor = reader["Actor"].ToString(),
                                    DateCreated = DateTime.Parse(reader["DateCreated"].ToString()),
                                    Active = Boolean.Parse(reader["Active"].ToString()),
                                    CreatedBy = reader["CreatedBy"].ToString(),
                                    Estimate = reader["Estimate"].ToString(),
                                    IWantTo = reader["IWantTo"].ToString(),
                                    LastEdited = DateTime.Parse(reader["LastEdited"].ToString()),
                                    LastEditedBy = reader["LastEditedBy"].ToString(),
                                    Notes = reader["Notes"].ToString(),
                                    PercentageCompletion = float.Parse(reader["PercentageCompletion"].ToString()),
                                    Priority = 1,
                                    ProjectID = int.Parse(reader["ProjectID"].ToString()),
                                    SoThat = reader["SoThat"].ToString(),
                                    SprintID = int.Parse(reader["SprintID"].ToString()),
                                    Status = int.Parse(reader["Status"].ToString()),
                                    StoryName = reader["Name"].ToString(),
                                    Theme = reader["Theme"].ToString(),
                                    TimeEstimate = reader["TimeEstimate"].ToString()
                                });
                            }
                            connection.Close();
                        }
                    }
                }
                catch(Exception e)
                {
                    throw e;
                }
                return projectStories;
            }
        }


        public bool AddProject(Project data, string user)
        {
            using(new MethodLogging())
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
                catch(Exception e)
                {
                    throw e;
                }

                return wasAdded;
            }
        }
    }
}