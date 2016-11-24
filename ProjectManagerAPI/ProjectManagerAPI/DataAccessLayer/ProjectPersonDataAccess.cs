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
    public class ProjectPersonDataAccess
    {
        public string connectionString = ConfigurationManager.ConnectionStrings["DBConstr"].ConnectionString;

        public List<IntermediateTable> getPersonnelOnProjectsByPersonID(int? personID)
        {
            using (new MethodLogging())
            {
                List<IntermediateTable> projectPersonnel = new List<IntermediateTable>();
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        using (SqlCommand command = new SqlCommand("usp_ProjectPersonGetByPersonID", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@PersonID", personID).Direction = ParameterDirection.Input;
                            connection.Open();
                            SqlDataReader reader = command.ExecuteReader();
                            while (reader.Read())
                            {
                                projectPersonnel.Add(new IntermediateTable
                                {
                                    ID = reader.GetValueOrDefault<int>("ID"),
                                    FirstID = reader.GetValueOrDefault<int>("ProjectID"),
                                    SecondID = reader.GetValueOrDefault<int>("PersonID"),
                                    Active = reader.GetValueOrDefault<bool>("Active"),
   

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
                return projectPersonnel;
            }
        }
    }
}