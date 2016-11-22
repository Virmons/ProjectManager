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
using System.Web.Services.Description;

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
                TokenAuthenticator tokenAuthenticator = new TokenAuthenticator();
                UserRolePair userRole = tokenAuthenticator.authoriseToken(this.Request.Headers.Authorization.ToString());
                int authorised = userRole.Role;
                //int authorised = 2;
                if (authorised == 2 || authorised == 1)
                {
                    try
                    {
                        List<Project> projectList = new List<Project>();

                        ProjectDataAccess projectDataAccess = new ProjectDataAccess();
                        StoryDataAccess storyDataAccess = new StoryDataAccess();
                        TaskDataAccess taskDataAccess = new TaskDataAccess();
                        WorkLogDataAccess workLogDataAccess = new WorkLogDataAccess();
                        PersonDataAccess personDataAccess = new PersonDataAccess();

                        //Get a list of projects the user is a personnel of
                        projectList = projectDataAccess.getAllProjects(userID);

                        foreach (Project project in projectList)
                        {
                            List<Story> projectStories = storyDataAccess.getStoriesByProjectID(project.ID);
                            List<Person> projectPersonnel = personDataAccess.getPersonnelByProjectID(project.ID);

                            foreach (Story story in projectStories)
                            {
                                List<Task> storyTasks = taskDataAccess.getTasksByStoryID(story.ID);

                                foreach (Task task in storyTasks)
                                {
                                    task.WorkLogs = workLogDataAccess.getWorkLogByTaskID(task.ID);
                                }

                                story.Tasks = storyTasks;
                            }

                            project.Personnel = projectPersonnel;

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

        [HttpPost]
        [Route("api/Projects/updateProject")]
        public LoginMessage updateProject([FromBody]JObject project)
        {
            using (new MethodLogging())
            {
                bool wasAdded = false;
                LoginMessage message = new LoginMessage();
                try
                {
                    JArray returnProjectList = new JArray();
                    TokenAuthenticator tokenAuthenticator = new TokenAuthenticator();
                    UserRolePair userRole = tokenAuthenticator.authoriseToken(this.Request.Headers.Authorization.ToString());
                    int authorised = userRole.Role;
                    int userID = userRole.UserID;
                    Project project1 = project.ToObject<Project>();
                    
                    //LoginMessage messasge = new LoginMessage() { Message = authorised.ToString() + " " + userID.ToString() + " " + project1.ProjectName , Type = "Success", UserID = userID.ToString() };
                    //return messasge;
                    //int authorised = 2;
                    if (authorised == 2)
                    {
                        try
                        {
                            ProjectDataAccess projectDataAccess = new ProjectDataAccess();
                            StoryDataAccess storyDataAccess = new StoryDataAccess();
                            TaskDataAccess taskDataAccess = new TaskDataAccess();
                            PersonDataAccess personDataAccess = new PersonDataAccess();
                            string user = User.Identity.Name;
                            Project newProject = project.ToObject<Project>();

                            foreach (Story story in newProject.Stories)
                            {
                                storyDataAccess.UpdateProjectStories(story, userID);
                                foreach(Task task in story.Tasks)
                                {
                                    taskDataAccess.UpdateStoryTasks(task, userID);
                                }
                            }

                            foreach (Person person in newProject.Personnel)
                            {
                                personDataAccess.UpdatePersonnelIntermediate(newProject, person, userID);
                            }
                            
                            wasAdded = projectDataAccess.UpdateProject(newProject, userID);

                            message = new LoginMessage() { Message = wasAdded.ToString(), Type = "Success", UserID = userID.ToString() };
                        }
                        catch (WebException ex)
                        {
                            string a = ex.StackTrace.ToString();
                            message = new LoginMessage() { Message = a, Type = "Fail", UserID = userID.ToString() };
                            return message;
                        }
                    }
                }
                catch (WebException ex)
                {
                    string a = ex.StackTrace.ToString();
                    message = new LoginMessage() { Message = a, Type = "Fail", UserID = "1" };
                    return message;
                }

                
                return message;
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
