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
    public class PersonDataAccess
    {
        public string connectionString = ConfigurationManager.ConnectionStrings["DBConstr"].ConnectionString;

        public List<Person> getPersonnelByProjectID(int? projectID)
        {
            using (new MethodLogging())
            {
                List<Person> projectPersonnel = new List<Person>();
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        using (SqlCommand command = new SqlCommand("usp_PersonGetByProjectID", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@ProjectID", projectID).Direction = ParameterDirection.Input;
                            connection.Open();
                            SqlDataReader reader = command.ExecuteReader();
                            while (reader.Read())
                            {
                                projectPersonnel.Add(new Person
                                {
                                    ID = reader.GetValueOrDefault<int>("ID"),
                                    DateCreated = reader.GetValueOrDefault<DateTime>("DateCreated"),
                                    Active = reader.GetValueOrDefault<bool>("Active"),
                                    Administrator = reader.GetValueOrDefault<bool>("Administrator"),
                                    Initials = reader.GetValueOrDefault<string>("Initials"),
                                    Name = reader.GetValueOrDefault<string>("Name")

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

        public bool UpdatePersonnelIntermediate(Project project, Person person, int userID)
        {
            using (new MethodLogging())
            {
                bool wasAdded = false;

                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        using (SqlCommand command = new SqlCommand("usp_ProjectPersonAddNewLink", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@UserID", userID).Direction = ParameterDirection.Input;
                            command.Parameters.AddWithValue("@ProjectID", project.ID).Direction = ParameterDirection.Input;
                            command.Parameters.AddWithValue("@PersonID", person.ID).Direction = ParameterDirection.Input;
                            command.Parameters.AddWithValue("@wasAdded", wasAdded).Direction = ParameterDirection.Output;
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