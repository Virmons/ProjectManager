using Newtonsoft.Json.Linq;
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
    public class KeyDataAccess
    {
        public string connectionString = ConfigurationManager.ConnectionStrings["DBConstr"].ConnectionString;

        public string GetKey(string user)
        {
            using(new MethodLogging())
            {
                string returnKey = "";
                try
                {
                    using(SqlConnection connection = new SqlConnection(connectionString))
                    {
                        using(SqlCommand command = new SqlCommand("usp_KeyGetKey",connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@User", user).Direction = ParameterDirection.Input;
                            command.Parameters.Add("@Key", SqlDbType.VarChar, -1).Value = returnKey;
                            command.Parameters["@Key"].Direction = ParameterDirection.Output;
                            connection.Open();
                            command.ExecuteNonQuery();
                            returnKey = command.Parameters["@Key"].Value.ToString();
                            connection.Close();
                        }
                    }
                }
                catch (Exception e)
                {
                    
                    throw e;
                }

                return returnKey;
            }
        }

        public bool CreateKey(JObject key)
        {
            using (new MethodLogging())
            {
                bool wasAdded = false;
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        using (SqlCommand command = new SqlCommand("usp_KeyCreateKey", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@Key", key.ToString()).Direction = ParameterDirection.Input;
                            command.Parameters.AddWithValue("@WasAdded", wasAdded).Direction = ParameterDirection.Output;
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