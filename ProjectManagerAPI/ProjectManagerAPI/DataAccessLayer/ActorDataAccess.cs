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

        public List<Actor> getAllActors()
        {
            using (new MethodLogging())
            {
                List<Actor> actors = new List<Actor>();
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
                                actors.Add(new Actor
                                {
                                    ID = reader.GetValueOrDefault<int>("ID"),
                                    Name = reader.GetValueOrDefault<string>("Actor"),
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