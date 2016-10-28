using ProjectManagerAPI.Models;
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

        public List<Project> getAllProjects(string user)
        {
            using (new MethodLogging())
            {
                List<Project> projectList = new List<Project>();
                try
                {
                    //using (SqlConnection connection = new SqlConnection(getConnectionString()))
                    //{
                    //    using(SqlCommand command = new SqlCommand("getAllProjects", connection))
                    //    {
                    //        command.CommandType = CommandType.StoredProcedure;
                    //        command.Parameters.AddWithValue("@User", user).Direction = ParameterDirection.Input;
                    //        SqlDataReader reader = command.ExecuteReader();
                    //        while(reader.Read())
                    //        {
                    //            projectList.Add(new Project
                    //            {
                    //                ID = int.Parse(reader["ID"].ToString()),
                    //                ProjectName = reader["ProjectName"].ToString()
                    //            });
                    //        }
                    //    }
                    //}
                    Project newProject = new Project{ID=1, ProjectName="Test", DateCreated=DateTime.Parse("2016-10-26 15:00:00")};
                    projectList.Add(newProject);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return projectList;
            }
        }
    }
}