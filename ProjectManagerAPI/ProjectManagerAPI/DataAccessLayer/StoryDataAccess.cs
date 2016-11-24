using ProjectManagerAPI.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Wolf.Assembly.Logging;
using ProjectManagerAPI.Utility;

namespace ProjectManagerAPI.DataAccessLayer
{
    public class StoryDataAccess
    {
        public string connectionString = ConfigurationManager.ConnectionStrings["DBConstr"].ConnectionString;

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
                                    Actor = reader.GetValueOrDefault<int>("Actor"),
                                    Active = reader.GetValueOrDefault<bool>("Active"),
                                    Estimate = reader.GetValueOrDefault<string>("Estimate"),
                                    IWantTo = reader.GetValueOrDefault<string>("IWantTo"),
                                    Notes = reader.GetValueOrDefault<string>("Notes"),
                                    PercentageCompletion = reader.GetValueOrDefault<decimal>("PercentageCompletion"),
                                    Priority = reader.GetValueOrDefault<int>("Priority"),
                                    ProjectID = reader.GetValueOrDefault<int>("ProjectID"),
                                    SoThat = reader.GetValueOrDefault<string>("SoThat"),
                                    Status = reader.GetValueOrDefault<int>("Status"),
                                    StoryName = reader.GetValueOrDefault<string>("Name"),
                                    Theme = reader.GetValueOrDefault<int>("Theme"),
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

        public bool UpdateProjectStories(Story story, int userID)
        {
            using (new MethodLogging())
            {
                bool wasAdded = false;

                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        using (SqlCommand command = new SqlCommand("usp_StoryUpdateStory", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@UserID", userID).Direction = ParameterDirection.Input;
                            command.Parameters.AddWithValue("@StoryID", story.ID).Direction = ParameterDirection.Input;
                            command.Parameters.AddWithValue("@StoryName", story.StoryName).Direction = ParameterDirection.Input;
                            command.Parameters.AddWithValue("@StoryTheme", story.Theme).Direction = ParameterDirection.Input;
                            command.Parameters.AddWithValue("@StoryActor", story.Actor).Direction = ParameterDirection.Input;
                            command.Parameters.AddWithValue("@StoryIWantTo", story.IWantTo).Direction = ParameterDirection.Input;
                            command.Parameters.AddWithValue("@StorySoThat", story.SoThat).Direction = ParameterDirection.Input;
                            command.Parameters.AddWithValue("@StoryNotes", story.Notes).Direction = ParameterDirection.Input;
                            command.Parameters.AddWithValue("@StoryPriority", story.Priority).Direction = ParameterDirection.Input;
                            command.Parameters.AddWithValue("@StoryEstimate", story.Estimate).Direction = ParameterDirection.Input;
                            command.Parameters.AddWithValue("@StoryTimeEstimate", story.TimeEstimate).Direction = ParameterDirection.Input;
                            command.Parameters.AddWithValue("@StoryStatus", story.Status).Direction = ParameterDirection.Input;
                            command.Parameters.AddWithValue("@StoryProjectID", story.ProjectID).Direction = ParameterDirection.Input;
                            command.Parameters.AddWithValue("@StoryPercentageCompletion", story.PercentageCompletion).Direction = ParameterDirection.Input;
                            command.Parameters.AddWithValue("@wasUpdated", wasAdded).Direction = ParameterDirection.Output;
                            connection.Open();
                            command.ExecuteNonQuery();
                            wasAdded = bool.Parse(command.Parameters["@wasUpdated"].Value.ToString());
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