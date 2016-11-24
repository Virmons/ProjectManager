using ProjectManagerAPI.Models;
using ProjectManagerAPI.Utility;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Wolf.Assembly.Logging;
using System.Configuration;
using System.Web.Configuration;
using System.Data;

namespace ProjectManagerAPI.DataAccessLayer
{
    public class ProjectDataAccess
    {
        public string connectionString = ConfigurationManager.ConnectionStrings["DBConstr"].ConnectionString;

        public List<Project> getAllProjects(int userID)
        {
            using (new MethodLogging())
            {
                List<Project> projectList = new List<Project>();
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        using (SqlCommand command = new SqlCommand("usp_ProjectGetByUserID", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@UserID", userID).Direction = ParameterDirection.Input;
                            connection.Open();
                            SqlDataReader reader = command.ExecuteReader();
                            while (reader.Read())
                            {
                                projectList.Add(new Project
                                {
                                    ID = int.Parse(reader["ID"].ToString()),
                                    ProjectName = reader["ProjectName"].ToString(),
                                    Active = Boolean.Parse(reader["Active"].ToString()),
                                });
                            }
                            connection.Close();


                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return projectList;
            }
        }

        public bool UpdateProject(Project data, int userID)
        {
            using (new MethodLogging())
            {
                bool wasAdded = false;

                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        using (SqlCommand command = new SqlCommand("usp_ProjectUpdateProject", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@UserID", userID).Direction = ParameterDirection.Input;
                            command.Parameters.AddWithValue("@ProjectName", data.ProjectName).Direction = ParameterDirection.Input;
                            command.Parameters.AddWithValue("@ProjectID", data.ID).Direction = ParameterDirection.Input;
                            command.Parameters.AddWithValue("@wasUpdated", wasAdded).Direction = ParameterDirection.Output;
                            connection.Open();
                            command.ExecuteNonQuery();
                            wasAdded = bool.Parse(command.Parameters["@wasUpdated"].Value.ToString());
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

        //public bool UpdatePersonnelIntermediate(Project project, Person person, int userID)
        //{
        //    using (new MethodLogging())
        //    {
        //        bool wasAdded = false;

        //        try
        //        {
        //            using (SqlConnection connection = new SqlConnection(connectionString))
        //            {
        //                using (SqlCommand command = new SqlCommand("usp_ProjectUpdateProject", connection))
        //                {
        //                    command.CommandType = CommandType.StoredProcedure;
        //                    command.Parameters.AddWithValue("@UserID", userID).Direction = ParameterDirection.Input;
        //                    command.Parameters.AddWithValue("@ProjectID", project.ProjectName).Direction = ParameterDirection.Input;
        //                    command.Parameters.AddWithValue("@PersonID", person.ID).Direction = ParameterDirection.Input;
        //                    command.Parameters.AddWithValue("@wasUpdated", wasAdded).Direction = ParameterDirection.Output;
        //                    connection.Open();
        //                    command.ExecuteNonQuery();
        //                    wasAdded = bool.Parse(command.Parameters["@wasUpdated"].Value.ToString());
        //                    connection.Close();
        //                }
        //            }
        //        }
        //        catch (Exception e)
        //        {
        //            throw e;
        //        }

        //        return wasAdded;
        //    }
        //}


        public bool AddProject(Project data, string user)
        {
            using (new MethodLogging())
            {
                bool wasAdded = false;

                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        using (SqlCommand command = new SqlCommand("usp_ProjectAddProject", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@User", user).Direction = ParameterDirection.Input;
                            command.Parameters.AddWithValue("@ProjectName", data.ProjectName).Direction = ParameterDirection.Input;
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