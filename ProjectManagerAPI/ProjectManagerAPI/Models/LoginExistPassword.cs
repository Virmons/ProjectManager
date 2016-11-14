using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectManagerAPI.Models
{
    public class LoginExistPassword
    {
        public int Exists { get; set; }
        public string Password { get; set; }
        public bool Role { get; set; }
    }
}