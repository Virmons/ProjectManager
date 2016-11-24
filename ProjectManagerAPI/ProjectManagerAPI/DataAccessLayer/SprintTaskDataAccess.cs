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

        public List<IntermediateTable> getSprintTasksByTaskID(int? taskID, out string taskIDList)
        {
            using (new MethodLogging())
            {
                List<IntermediateTable> sprintTasks = new List<IntermediateTable>();
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
                                sprintTasks.Add(new IntermediateTable
                                {
                                    ID = reader.GetValueOrDefault<int>("ID"),
                                    Active = reader.GetValueOrDefault<bool>("Active"),
                                    FirstID = reader.GetValueOrDefault<int>("SprintID"),
                                    SecondID = reader.GetValueOrDefault<int>("TaskID")
                                                                       
                                });

                                taskIDList += (sprintTasks.Last<IntermediateTable>().ID.ToString() + ",");
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