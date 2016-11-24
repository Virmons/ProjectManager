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
    public class SprintDataAccess
    {
        public string connectionString = ConfigurationManager.ConnectionStrings["DBConstr"].ConnectionString;

        public List<IDValuePair> getSprintsByTaskID(string taskIDList)
        {
            using (new MethodLogging())
            {
                List<IDValuePair> sprints = new List<IDValuePair>();
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        using (SqlCommand command = new SqlCommand("usp_SprintsGetByTaskIDList", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@TaskIDList", taskIDList).Direction = ParameterDirection.Input;
                            connection.Open();
                            SqlDataReader reader = command.ExecuteReader();
                            while (reader.Read())
                            {
                                sprints.Add(new IDValuePair
                                {
                                    ID = reader.GetValueOrDefault<int>("ID"),
                                    Active = reader.GetValueOrDefault<bool>("Active"),
                                    Value = reader.GetValueOrDefault<string>("Description")

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
                return sprints;
            }
        }
    }
}