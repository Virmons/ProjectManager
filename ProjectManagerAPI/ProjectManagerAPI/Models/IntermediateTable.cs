using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectManagerAPI.Models
{
    public class ProjectPerson
    {
        public int ID { get; set; }
        public bool Active { get; set; }
        public int ProjectID { get; set; }
        public int PersonID { get; set; }
    }

    public class SprintTask
    {
        public int ID { get; set; }
        public bool Active { get; set; }
        public int SprintID { get; set; }
        public int TaskID { get; set; }
    }
}