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
    public class LoginDataAccess
    {
        public string connectionString = ConfigurationManager.ConnectionStrings["DBConstr"].ConnectionString;

        public string getPasswordByUser(string user)
        {
            using(new MethodLogging())
            {
                string hashPass = "";

                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        using(SqlCommand command = new SqlCommand("usp_CredPasswordByUser",connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@User", user).Direction = ParameterDirection.Input;
                            command.Parameters.AddWithValue("@ReturnPass", hashPass).Direction = ParameterDirection.Output;
                            connection.Open();
                            command.ExecuteNonQuery();
                            hashPass = command.Parameters["@ReturnPass"].Value.ToString();
                            connection.Close();
                        }
                    }
                }
                catch(Exception e)
                {
                    throw e;
                }

                return hashPass;
            }
        }

        public int addNewPassword(string user, string password)
        {
            using(new MethodLogging())
            {
                int wasAdded = 0;
                try
                {
                    using(SqlConnection connection = new SqlConnection(connectionString))
                    {
                        using(SqlCommand command = new SqlCommand("usp_CredAddNewPassword", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@User", user).Direction = ParameterDirection.Input;
                            command.Parameters.AddWithValue("@Pass", password).Direction = ParameterDirection.Input;
                            command.Parameters.AddWithValue("@wasAdded", wasAdded).Direction = ParameterDirection.Output;
                            connection.Open();
                            command.ExecuteNonQuery();
                            wasAdded = int.Parse(command.Parameters["@wasAdded"].Value.ToString());
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

        public bool doesUserExist(string user)
        {
            using(new MethodLogging())
            {
                bool exists = false;
                try 
                {
                
                    using(SqlConnection connection = new SqlConnection(connectionString))
                    {
                        using(SqlCommand command = new SqlCommand("usp_CredUserExists",connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@User", user).Direction = ParameterDirection.Input;
                            command.Parameters.AddWithValue("@Exists", exists).Direction = ParameterDirection.Output;
                            connection.Open();
                            command.ExecuteNonQuery();
                            exists = bool.Parse(command.Parameters["@Exists"].Value.ToString());
                            connection.Close();

                        }
                    }

                }
                catch(Exception e)
                {
                    throw e;
                }
                return exists;
            }
        }
    }
}