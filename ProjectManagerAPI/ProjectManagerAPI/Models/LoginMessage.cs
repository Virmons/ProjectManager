using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectManagerAPI.Models
{
    public class LoginMessage
    {
        public string Message { get; set; }
        public string Type {get; set;}
        public string UserID { get; set; }
    }
}