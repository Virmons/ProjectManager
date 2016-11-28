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
using System.Reflection;
using System.Text.RegularExpressions;

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
                List<JObject> returnProjectList = new List<JObject>();
                //TokenAuthenticator tokenAuthenticator = new TokenAuthenticator();
                //UserRolePair userRole = tokenAuthenticator.authoriseToken(this.Request.Headers.Authorization.ToString());
                //int authorised = userRole.Role;     
                                                
                ProjectDataAccess projectDataAccess = new ProjectDataAccess();
                StoryDataAccess storyDataAccess = new StoryDataAccess();
                TaskDataAccess taskDataAccess = new TaskDataAccess();
                WorkLogDataAccess workLogDataAccess = new WorkLogDataAccess();
                PersonDataAccess personDataAccess = new PersonDataAccess();
                ProjectPersonDataAccess projectPersonDataAccess = new ProjectPersonDataAccess();
                SprintTaskDataAccess sprintTaskDataAccess = new SprintTaskDataAccess();
                SprintDataAccess sprintDataAccess = new SprintDataAccess();
                ActorDataAccess actorDataAccess = new ActorDataAccess();
                ThemeDataAccess themeDataAccess = new ThemeDataAccess();

                List<Person> personnel = new List<Person>();
                List<Project> projectList = new List<Project>();
                List<Story> projectStories = new List<Story>();
                List<Task> storyTasks = new List<Task>();
                List<WorkLog> taskWorkLogs = new List<WorkLog>();
                List<Theme> themes = new List<Theme>();
                List<Actor> actors = new List<Actor>();
                List<SprintTask> sprintTasks = new List<SprintTask>();
                List<Sprint> sprints = new List<Sprint>();
                List<ProjectPerson> projectPerson = new List<ProjectPerson>();
                string taskIDList = "";
                string fullTaskIDList = "";

                int  authorised = 2;

                if (authorised == 2 || authorised == 1)
                {
                    try
                    {
                        projectList = projectDataAccess.getAllProjects(userID);
                        projectPerson = projectPersonDataAccess.getPersonnelOnProjectsByPersonID(userID);
                        personnel = personDataAccess.getPersonnelByUserID(userID);
                        actors = actorDataAccess.getAllActors();
                        themes = themeDataAccess.getAllThemes();

                        foreach (Project project in projectList)
                        {
                            projectStories.AddRange(storyDataAccess.getStoriesByProjectID(project.ID));
                           
                        }

                        foreach (Story story in projectStories)
                        {
                            storyTasks.AddRange(taskDataAccess.getTasksByStoryID(story.ID));

                        }

                        foreach (Task task in storyTasks)
                        {
                            taskWorkLogs.AddRange(workLogDataAccess.getWorkLogByTaskID(task.ID));
                            sprintTasks = sprintTaskDataAccess.getSprintTasksByTaskID(task.ID, out taskIDList);
                            fullTaskIDList += taskIDList;
                        }
                        fullTaskIDList = fullTaskIDList.Substring(0, fullTaskIDList.Length - 1);
                        sprints = sprintDataAccess.getSprintsByTaskID(fullTaskIDList);

                        returnProjectList.Add(ToNamedJarray(projectList));
                        returnProjectList.Add(ToNamedJarray(personnel));
                        returnProjectList.Add(ToNamedJarray(projectStories));
                        returnProjectList.Add(ToNamedJarray(storyTasks));
                        returnProjectList.Add(ToNamedJarray(taskWorkLogs));
                        returnProjectList.Add(ToNamedJarray(themes));
                        returnProjectList.Add(ToNamedJarray(actors));
                        returnProjectList.Add(ToNamedJarray(sprintTasks));
                        returnProjectList.Add(ToNamedJarray(sprints));
                        returnProjectList.Add(ToNamedJarray(projectPerson));

                        //SQLiteGenerator sqliteGenerator = new SQLiteGenerator();
                        //sqliteGenerator.GenerateSqlite(userID, projectList, projectPerson, personnel, projectStories, actors, themes, storyTasks, sprintTasks, sprints, taskWorkLogs);
                        
                        
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }

                }

                return JArray.FromObject(returnProjectList);
            }
        }

    public JObject ToNamedJarray(Object inputObject)
    {
        return JObject.FromObject(new SqlArray { SQL = JArray.FromObject(new SQLiteGenerator().GenerateCreateSql(inputObject.GetType().GenericTypeArguments[0].FullName)), Array = JArray.FromObject(inputObject) });
    }

        //[HttpPost]
        //[Route("api/Projects/updateProject")]
        //public LoginMessage updateProject([FromBody]JObject project)
        //{
        //    using (new MethodLogging())
        //    {
        //        bool wasAdded = false;
        //        LoginMessage message = new LoginMessage();
        //        try
        //        {
        //            JArray returnProjectList = new JArray();
        //            TokenAuthenticator tokenAuthenticator = new TokenAuthenticator();
        //            UserRolePair userRole = tokenAuthenticator.authoriseToken(this.Request.Headers.Authorization.ToString());
        //            int authorised = userRole.Role;
        //            int userID = userRole.UserID;
        //            Project project1 = project.ToObject<Project>();
                    
        //            //LoginMessage messasge = new LoginMessage() { Message = authorised.ToString() + " " + userID.ToString() + " " + project1.ProjectName , Type = "Success", UserID = userID.ToString() };
        //            //return messasge;
        //            //int authorised = 2;
        //            if (authorised == 2)
        //            {
        //                try
        //                {
        //                    ProjectDataAccess projectDataAccess = new ProjectDataAccess();
        //                    StoryDataAccess storyDataAccess = new StoryDataAccess();
        //                    TaskDataAccess taskDataAccess = new TaskDataAccess();
        //                    PersonDataAccess personDataAccess = new PersonDataAccess();
        //                    string user = User.Identity.Name;
        //                    Project newProject = project.ToObject<Project>();

        //                    foreach (Story story in newProject.Stories)
        //                    {
        //                        storyDataAccess.UpdateProjectStories(story, userID);
        //                        foreach(Task task in story.Tasks)
        //                        {
        //                            taskDataAccess.UpdateStoryTasks(task, userID);
        //                        }
        //                    }

        //                    foreach (Person person in newProject.Personnel)
        //                    {
        //                        personDataAccess.UpdatePersonnelIntermediate(newProject, person, userID);
        //                    }
                            
        //                    wasAdded = projectDataAccess.UpdateProject(newProject, userID);

        //                    message = new LoginMessage() { Message = wasAdded.ToString(), Type = "Success", UserID = userID.ToString() };
        //                }
        //                catch (WebException ex)
        //                {
        //                    string a = ex.StackTrace.ToString();
        //                    message = new LoginMessage() { Message = a, Type = "Fail", UserID = userID.ToString() };
        //                    return message;
        //                }
        //            }
        //        }
        //        catch (WebException ex)
        //        {
        //            string a = ex.StackTrace.ToString();
        //            message = new LoginMessage() { Message = a, Type = "Fail", UserID = "1" };
        //            return message;
        //        }

                
        //        return message;
        //    }
        //}

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
