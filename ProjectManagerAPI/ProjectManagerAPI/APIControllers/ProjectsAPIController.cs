using Newtonsoft.Json.Linq;
using ProjectManagerAPI.DataAccessLayer;
using ProjectManagerAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ProjectManagerAPI.APIControllers
{
    public class ProjectsAPIController : ApiController
    {
        [HttpGet]
        [Route("api/Projects/getAllProjects/{userInitials}")]
        public JArray getAllProjects(string userInitials)
        {
            List<Project> projectList = new List<Project>();
            ProjectDataAccess projectDataAccess = new ProjectDataAccess();

            projectList = projectDataAccess.getAllProjects(userInitials);

            JArray returnProjectList = JArray.FromObject(projectList);

            return returnProjectList;
        }
        
    }
}
