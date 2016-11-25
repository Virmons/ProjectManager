using ProjectManagerAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;

namespace ProjectManagerAPI.Utility
{
    public class SQLiteGenerator
    {
        public bool GenerateSqlite(int userID, List<Project> projects, List<ProjectPerson> projectPeople, List<Person> people, List<Story> stories, List<Actor> actors, List<Theme> themes, List<Task> tasks, List<SprintTask> sprintTasks, List<Sprint> sprints, List<WorkLog> workLogs)
        {


            string sqliteFile = AppDomain.CurrentDomain.BaseDirectory + "Sqlite\\" + userID.ToString() + ".sqlite";

            File.Delete(sqliteFile);
            if(File.Exists(sqliteFile))
            {
                return false;
            }
            SQLiteConnection.CreateFile(sqliteFile);

            SQLiteConnection sqliteConnection = new SQLiteConnection("Data Source=" + sqliteFile + ";Version=3;");
            sqliteConnection.Open();

            List<string> sqlList = new List<string>();

            List<ParameterInfo> parameters = new List<ParameterInfo>();

            foreach (ParameterInfo info in this.GetType().GetMethod("GenerateSqlite").GetParameters())
            {
                if (info.Name != "userID")
                {
                    parameters.Add(info);
                }
            }

            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic |
                 BindingFlags.Static | BindingFlags.Instance |
                 BindingFlags.DeclaredOnly;

            foreach (ParameterInfo param in parameters)
            {
                try
                {
                    Type paramType = param.ParameterType.GenericTypeArguments[0];
                    FieldInfo[] fields = paramType.GetFields(flags);
                    string tableName = paramType.Name.ToString();
                    string sql = ("CREATE TABLE IF NOT EXISTS " + tableName + "(ID int UNIQUE PRIMARY KEY NOT NULL");
                    foreach (FieldInfo field in fields)
                    {
                        string fieldName = Regex.Match(field.Name, @"\<([^)]*)\>").Groups[1].Value;
                        if (fieldName != "ID")
                        {
                            switch (field.FieldType.Name)
                            {
                                case "Int32":
                                    sql += ("," + fieldName + " INT NOT NULL");
                                    break;
                                case "Boolean":
                                    sql += ("," + fieldName + " BIT NOT NULL");
                                    break;
                                case "String":
                                    sql += ("," + fieldName + " VARCHAR(0) NOT NULL");
                                    break;
                                case "Decimal":
                                    sql += ("," + fieldName + " NUMERIC NOT NULL");
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    sql += ")";

                    sqlList.Add(sql);
                }
                catch(Exception e)
                {
                    throw e;
                }
            }

            try
            {
                foreach (string sql in sqlList)
                {
                    executeNonQuery(sql, sqliteConnection);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


            try
            {
                foreach(Project project in projects)
                {
                    Create(project, sqliteConnection);
                }
                foreach(ProjectPerson projectPerson in projectPeople)
                {
                    Create(projectPerson, sqliteConnection);
                }
                foreach (Person person in people)
                {
                    Create(person, sqliteConnection);
                }
                foreach (Story story in stories)
                {
                    Create(story, sqliteConnection);
                }
                foreach (Actor actor in actors)
                {
                    Create(actor, sqliteConnection);
                }
                foreach (Theme theme in themes)
                {
                    Create(theme, sqliteConnection);
                }
                foreach (Task task in tasks)
                {
                    Create(task, sqliteConnection);
                }
                foreach (SprintTask sprintTask in sprintTasks)
                {
                    Create(sprintTask, sqliteConnection);
                }
                foreach (Sprint sprint in sprints)
                {
                    Create(sprint, sqliteConnection);
                }
                foreach(WorkLog workLog in workLogs)
                {
                    Create(workLog, sqliteConnection);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return true;

        }

        public void executeNonQuery(string sql, SQLiteConnection sqliteConnection)
        {
            SQLiteCommand command = new SQLiteCommand(sql, sqliteConnection);
            command.ExecuteNonQuery();
        }

        public void Create(Object currentItem, SQLiteConnection sqliteConnection)
        {
            Type type = currentItem.GetType();

            string sql = "";

            SQLiteCommand insertSQL = new SQLiteCommand(sql, sqliteConnection);

            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic |
                 BindingFlags.Static | BindingFlags.Instance |
                 BindingFlags.DeclaredOnly;

            var fields = type.GetFields(flags);

            string parameters = " (";
            string values = parameters;


            foreach (FieldInfo field in fields)
            {
                string fieldName = Regex.Match(field.Name, @"\<([^)]*)\>").Groups[1].Value;

                var fieldValue = field.GetValue(currentItem);

                parameters += fieldName + ",";

                string valueName = "@" + fieldName + ",";

                values += "?,";

                insertSQL.Parameters.AddWithValue(valueName, fieldValue);
            }

            parameters = parameters.Substring(0, parameters.Length - 1);
            parameters += ")";

            values = values.Substring(0, values.Length - 1);
            values += ")";

            sql = "INSERT INTO " + type.Name + parameters + " VALUES" + values;

            insertSQL.CommandText = sql;

            insertSQL.ExecuteNonQuery();
        }
    }
}