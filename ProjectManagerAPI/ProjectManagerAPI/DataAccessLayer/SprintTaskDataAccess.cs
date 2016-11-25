using ProjectManagerAPI.Models;
using ProjectManagerAPI.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Wolf.Assembly.Logging;

namespace ProjectManagerAPI.DataAccessLayer
{
    public class SprintTaskDataAccess
    {
        public string connectionString = ConfigurationManager.ConnectionStrings["DBConstr"].ConnectionString;

        public List<SprintTask> getSprintTasksByTaskID(int? taskID, out string taskIDList)
        {
            using (new MethodLogging())
            {
                List<SprintTask> sprintTasks = new List<SprintTask>();
                taskIDList = "";
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        using (SqlCommand command = new SqlCommand("usp_SprintTasksGetByTaskID", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@TaskID", taskID).Direction = ParameterDirection.Input;
                            connection.Open();
                            SqlDataReader reader = command.ExecuteReader();
                            while (reader.Read())
                            {
                                sprintTasks.Add(new SprintTask
                                {
                                    ID = reader.GetValueOrDefault<int>("ID"),
                                    Active = reader.GetValueOrDefault<bool>("Active"),
                                    SprintID = reader.GetValueOrDefault<int>("SprintID"),
                                    TaskID = reader.GetValueOrDefault<int>("TaskID")
                                                                       
                                });

                                taskIDList += (sprintTasks.Last<SprintTask>().ID.ToString() + ",");
                            }
                            connection.Close();
                        }
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
                return sprintTasks;
            }
        }
    }
}