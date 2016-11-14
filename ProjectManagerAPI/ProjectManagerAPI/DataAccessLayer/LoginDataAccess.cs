using ProjectManagerAPI.Models;
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

        public int AddNewPassword(string user, string password)
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

        public LoginExistPassword DoesUserExist(string user)
        {
            using(new MethodLogging())
            {

                LoginExistPassword data = new LoginExistPassword() { Exists = 0, Password = ""};
                
                try 
                {
                
                    using(SqlConnection connection = new SqlConnection(connectionString))
                    {
                        using(SqlCommand command = new SqlCommand("usp_CredUserExists",connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@User", user).Direction = ParameterDirection.Input;
                            command.Parameters.AddWithValue("@Exists", data.Exists).Direction = ParameterDirection.Output;
                            command.Parameters.Add("@ReturnPass", SqlDbType.VarChar, -1).Value = data.Password;
                            command.Parameters["@ReturnPass"].Direction = ParameterDirection.Output;
                            command.Parameters.AddWithValue("@Role", data.Role).Direction = ParameterDirection.Output;
                            connection.Open();
                            command.ExecuteNonQuery();
                            data.Exists = int.Parse(command.Parameters["@Exists"].Value.ToString());
                            data.Password = command.Parameters["@ReturnPass"].Value.ToString();
                            data.Role = bool.Parse(command.Parameters["@Role"].Value.ToString());
                            connection.Close();

                        }
                    }

                }
                catch(Exception e)
                {
                    throw e;
                }
                return data;
            }
        }
    }
}