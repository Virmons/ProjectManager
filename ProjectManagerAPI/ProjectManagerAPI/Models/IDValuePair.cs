using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectManagerAPI.Models
{
    public class Actor 
    {
        public int ID { get; set; }
        public bool Active { get; set; }
        public string Name { get; set; }
    }

    public class Theme
    {
        public int ID { get; set; }
        public bool Active { get; set; }
        public string Name { get; set; }
    }

    public class Sprint
    {
        public int ID { get; set; }
        public bool Active { get; set; }
        public string Description { get; set; }
    }
}