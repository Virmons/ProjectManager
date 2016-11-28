using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectManagerAPI.Models
{
    public class SqlArray
    {
        public JArray SQL { get; set; }
        public JArray Array { get; set; }
    }
}