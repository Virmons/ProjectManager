using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectManagerAPI.Models
{
    public class IntermediateTable
    {
        public int ID { get; set; }
        public int FirstID { get; set; }
        public int SecondID { get; set; }
        public bool Active { get; set; }
    }
}