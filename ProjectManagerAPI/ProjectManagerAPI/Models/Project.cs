using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectManagerAPI.Models
{
    public class Project
    {
        public int ID { get; set; }
        public string ProjectName { get; set; }
        public DateTime DateCreated { get; set; }
        public bool Active { get; set; }
        public List<Story> Stories { get; set; }
        public List<Person> Personell { get; set; }
    }
}