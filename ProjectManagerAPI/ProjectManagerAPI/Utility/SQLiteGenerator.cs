using ProjectManagerAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Web;

namespace ProjectManagerAPI.Utility
{
    public class SQLiteGenerator
    {
        public SQLiteGenerator(List<Project> projects, List<ProjectPerson> projectPersonnel, List<Person> personnel, List<Story> stories, List<Task> tasks, List<WorkLog> workLogs, List<Event> events)
        {
            SQLiteConnection.CreateFile("ProjectManager.sqlite");


        }
    }
}