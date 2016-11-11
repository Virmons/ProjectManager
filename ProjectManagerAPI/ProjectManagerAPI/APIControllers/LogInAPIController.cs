using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Newtonsoft.Json.Linq;

namespace ProjectManagerAPI.APIControllers
{
    public class LogInAPIController : ApiController
    {
        [HttpPost]
        [Route("/Login/authoriseCredentials/email={email}&pass={password}")]
        public JObject returnJWT(string email, string password)
        {
            JObject returnJSON = new JObject();

            string testString = "Test";

            returnJSON = JObject.FromObject(testString);

            return returnJSON;
        }
    }
}
