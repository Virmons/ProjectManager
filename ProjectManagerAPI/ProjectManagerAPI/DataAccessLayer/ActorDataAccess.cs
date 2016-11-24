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
    public class ActorDataAccess
    {
        public string connectionString = ConfigurationManager.ConnectionStrings["DBConstr"].ConnectionString;

        public List<IDValuePair> getAllActors()
        {
            using (new MethodLogging())
            {
                List<IDValuePair> actors = new List<IDValuePair>();
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        using (SqlCommand command = new SqlCommand("usp_ActorGetAll", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            connection.Open();
                            SqlDataReader reader = command.ExecuteReader();
                            while (reader.Read())
                            {
                                actors.Add(new IDValuePair
                                {
                                    ID = reader.GetValueOrDefault<int>("ID"),
                                    Value = reader.GetValueOrDefault<string>("Actor"),
                                    Active = reader.GetValueOrDefault<bool>("Active")


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
                return actors;
            }
        }

    }
}