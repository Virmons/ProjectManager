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
using ProjectManagerAPI.Utility;
using Newtonsoft.Json;

namespace ProjectManagerAPI.APIControllers
{
    public class ProjectsAPIController : ApiController
    {        
        [HttpGet]
        [Route("api/Projects/getAllProjects/{userID}")]
        public JArray getAllProjects(int userID)
        {
            using (new MethodLogging())
            {
                JArray returnProjectList = new JArray();
                //TokenAuthenticator tokenAuthenticator = new TokenAuthenticator();
                //int authorised = tokenAuthenticator.authoriseToken(this.Request.Headers.Authorization.ToString());
                int authorised = 2;
                if (authorised == 2 || authorised == 1)
                {
                    try
                    {
                        List<Project> projectList = new List<Project>();

                        ProjectDataAccess projectDataAccess = new ProjectDataAccess();

                        //Get a list of projects the user is a personnel of
                        projectList = projectDataAccess.getAllProjects(userID);

                        foreach (Project project in projectList)
                        {
                            List<Story> projectStories = projectDataAccess.getStoriesByProjectID(project.ID);
                            List<Person> projectPersonnel = projectDataAccess.getPersonellByProjectID(project.ID);

                            foreach (Story story in projectStories)
                            {
                                List<Task> storyTasks = projectDataAccess.getTasksByStoryID(story.ID);

                                foreach (Task task in storyTasks)
                                {
                                    task.WorkLogs = projectDataAccess.getWorkLogByTaskID(task.ID);
                                }

                                story.Tasks = storyTasks;
                            }

                            project.Personell = projectPersonnel;

                            project.Stories = projectStories;
                        }

                        returnProjectList = JArray.FromObject(projectList);

                    }
                    catch (Exception e)
                    {
                        throw e;
                    }

                }

                return returnProjectList;
            }
        }

        //[HttpPost]
        //[Route("api/Projects/addProject")]
        //public bool addProject([FromBody]JObject project)
        //{
        //    using (new MethodLogging())
        //    {
        //        bool wasAdded = false;
        //        try
        //        {
        //            var data = project.ToObject<Project>();

        //            ProjectDataAccess projectDataAccess = new ProjectDataAccess();
        //            string user = User.Identity.Name;
        //            wasAdded = projectDataAccess.AddProject(data, user);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }

        //        return wasAdded;
        //    }
        //}

    }
}
