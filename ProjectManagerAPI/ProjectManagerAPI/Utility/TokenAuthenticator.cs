using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using ProjectManagerAPI.DataAccessLayer;
using ProjectManagerAPI.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;
using Wolf.Assembly.Logging;

namespace ProjectManagerAPI.Utility
{
    public class TokenAuthenticator
    {
        public UserRolePair authoriseToken(string tokenData)
        {
            using (new MethodLogging())
            {
                LoginMessage tokenReturnString = new LoginMessage();
                UserRolePair userRole = new UserRolePair();
                userRole.Role = 0;
                userRole.UserID = 0;
                try
                {
                    string token = tokenData.ToString();

                    JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                    KeyDataAccess keyDataAccess = new KeyDataAccess();

                    var plainTextSecurityKey = keyDataAccess.GetKey();
                    var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(plainTextSecurityKey));

                    var tokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidAudiences = new string[]
                    {
                        "http://wolfwebtest1:2099",
                        "http://79.77.23.117:2099"
                    },
                        ValidIssuers = new string[]
                    {
                        "Self"
                    },
                        IssuerSigningKey = signingKey
                    };

                    SecurityToken validatedToken;
                    try
                    {
                        string role = "";
                        string id = "";
                        tokenHandler.ValidateToken(token,
                        tokenValidationParameters, out validatedToken);
                        userRole.Role = 1;
                        JObject jObjectToken = JObject.FromObject(validatedToken);

                        foreach (JObject claim in jObjectToken["Claims"])
                        {

                            if ((string)claim["Type"] == "role")
                            {
                                role = claim["Value"].ToString();
                            }

                            if ((string)claim["Type"] == "unique_name")
                            {
                                id = claim["Value"].ToString();
                            }

                        }
                        if (role == "Administrator")
                        {
                            userRole.Role = 2;
                            userRole.UserID = int.Parse(id);
                            return userRole;

                        }
                        else if (role == "User")
                        {
                            userRole.Role = 1;
                            userRole.UserID = int.Parse(id);
                            return userRole;
                        }
                        else
                        {
                            userRole.Role = 0;
                            return userRole;
                        }

                    }
                    catch (SecurityTokenExpiredException)
                    {
                        userRole.Role = 0;
                        return userRole;
                    }
                    catch (SecurityTokenInvalidAudienceException)
                    {
                        userRole.Role = 0;
                        return userRole;
                    }
                    catch (SecurityTokenInvalidIssuerException)
                    {
                        userRole.Role = 0;
                        return userRole;
                    }
                    catch (SecurityTokenInvalidSigningKeyException)
                    {
                        userRole.Role = 0;
                        return userRole;
                    }

                }
                catch (Exception)
                {
                    userRole.Role = 0;
                    return userRole;
                }

            }
        }
    }
}