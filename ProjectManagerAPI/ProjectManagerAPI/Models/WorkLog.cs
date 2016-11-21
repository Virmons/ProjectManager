using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectManagerAPI.Models
{
    public class WorkLog
    {
        public int? ID { get; set; }
        public int? TaskID { get; set; }
        public int? PersonID { get; set; }
        public string Person { get; set; }
        public int? Time { get; set; }
        public DateTime? DateCreated { get; set; }
    }
}