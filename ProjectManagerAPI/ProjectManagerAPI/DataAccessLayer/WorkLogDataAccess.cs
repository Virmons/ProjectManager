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
    public class WorkLogDataAccess
    {
        public string connectionString = ConfigurationManager.ConnectionStrings["DBConstr"].ConnectionString;

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
                                    TaskID = reader.GetValueOrDefault<int>("TaskID"),
                                    Time = reader.GetValueOrDefault<int>("TimeTaken"),
                                    Person = reader.GetValueOrDefault<string>("Person")

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
    }
}