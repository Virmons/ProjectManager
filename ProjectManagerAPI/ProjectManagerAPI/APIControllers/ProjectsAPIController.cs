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
        [Route("api/Projects/getAllProjects")]
        public List<Project> getAllProjects(string user)
        {
            List<Project> projectList = new List<Project>();
            ProjectDataAccess projectDataAccess = new ProjectDataAccess();

            projectList = projectDataAccess.getAllProjects(user);


            return projectList;
        }
        
    }
}
