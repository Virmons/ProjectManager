using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectManagerAPI.Models
{
    public class IDValuePair
    {
        public int ID { get; set; }
        public string Value { get; set; }
        public bool Active { get; set; }
    }
}