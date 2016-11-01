using Newtonsoft.Json.Linq;
using ProjectManagerAPI.DataAccessLayer;
using ProjectManagerAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Wolf.Assembly.Logging;

namespace ProjectManagerAPI.APIControllers
{
    public class ProjectsAPIController : ApiController
    {
        [HttpGet]
        [Route("api/Projects/getAllProjects/{userInitials}")]
        public JArray getAllProjects(string userInitials)
        {
            using (new MethodLogging())
            {
                JArray returnProjectList = new JArray();

                try
                {
                    List<Project> projectList = new List<Project>();

                    ProjectDataAccess projectDataAccess = new ProjectDataAccess();

                    projectList = projectDataAccess.getAllProjects(userInitials);

                    returnProjectList = JArray.FromObject(projectList);

                }
                catch (Exception e)
                {
                    throw e;
                }
                return returnProjectList;
            }
        }

        [HttpPost]
        [Route("api/Projects/addProject")]
        public bool addProject([FromBody] JToken project)
        {
            using (new MethodLogging())
            {
                bool wasAdded = false;
                try
                {
                    var data = project.ToObject<Project>();

                    ProjectDataAccess projectDataAccess = new ProjectDataAccess();
                    string user = User.Identity.Name;
                    wasAdded = projectDataAccess.AddProject(data, user);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return wasAdded;
            }
        }

    }
}
