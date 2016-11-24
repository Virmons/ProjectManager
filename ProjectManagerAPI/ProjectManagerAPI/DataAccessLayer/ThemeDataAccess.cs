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
    public class ThemeDataAccess
    {
        public string connectionString = ConfigurationManager.ConnectionStrings["DBConstr"].ConnectionString;

        public List<IDValuePair> getAllThemes()
        {
            using (new MethodLogging())
            {
                List<IDValuePair> themes = new List<IDValuePair>();
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        using (SqlCommand command = new SqlCommand("usp_ThemeGetAll", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            connection.Open();
                            SqlDataReader reader = command.ExecuteReader();
                            while (reader.Read())
                            {
                                themes.Add(new IDValuePair
                                {
                                    ID = reader.GetValueOrDefault<int>("ID"),
                                    Value = reader.GetValueOrDefault<string>("Theme"),
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
                return themes;
            }
        }
    }
}