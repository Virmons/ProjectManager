using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectManagerAPI.Models
{
    public class Task
    {
        public int? ID { get; set; }
        public string Description { get; set; }
        public int TimeTaken { get; set; }
        public int TimeEstimate { get; set; }
        public int StoryID { get; set; }
        public bool Active { get; set; }
        public bool Complete { get; set; }
        public List<WorkLog> WorkLogs { get; set; }
    }
}