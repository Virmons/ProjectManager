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
    public class TaskDataAccess
    {
        public string connectionString = ConfigurationManager.ConnectionStrings["DBConstr"].ConnectionString;

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

        public bool UpdateStoryTasks(Task task, int userID)
        {
            using (new MethodLogging())
            {
                bool wasAdded = false;

                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        using (SqlCommand command = new SqlCommand("usp_TaskUpdateTask", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@UserID", userID).Direction = ParameterDirection.Input;
                            command.Parameters.AddWithValue("@TaskID", task.ID).Direction = ParameterDirection.Input;
                            command.Parameters.AddWithValue("@TaskDescription", task.Description).Direction = ParameterDirection.Input;
                            command.Parameters.AddWithValue("@TaskTimeEstimate", task.TimeEstimate).Direction = ParameterDirection.Input;
                            command.Parameters.AddWithValue("@TaskStoryID", task.StoryID).Direction = ParameterDirection.Input;
                            command.Parameters.AddWithValue("@TaskComplete", task.Complete).Direction = ParameterDirection.Input;
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