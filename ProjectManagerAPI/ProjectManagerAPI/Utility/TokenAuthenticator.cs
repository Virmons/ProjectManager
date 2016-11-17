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
        public int authoriseToken(string tokenData)
        {
            using (new MethodLogging())
            {
                LoginMessage tokenReturnString = new LoginMessage();
                int isAuthorised = 0;
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
                        isAuthorised = 1;
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
                            isAuthorised = 2;
                            return isAuthorised;

                        }
                        else if (role == "User")
                        {
                            isAuthorised = 1;
                            return isAuthorised;
                        }
                        else
                        {
                            isAuthorised = 0;
                            return isAuthorised;
                        }

                    }
                    catch (SecurityTokenExpiredException)
                    {
                        isAuthorised = 0;
                        return isAuthorised;
                    }
                    catch (SecurityTokenInvalidAudienceException)
                    {
                        isAuthorised = 0;
                        return isAuthorised;
                    }
                    catch (SecurityTokenInvalidIssuerException)
                    {
                        isAuthorised = 0;
                        return isAuthorised;
                    }
                    catch (SecurityTokenInvalidSigningKeyException)
                    {
                        isAuthorised = 0;
                        return isAuthorised;
                    }

                }
                catch (Exception e)
                {
                    isAuthorised = 0;
                    return isAuthorised;
                }

            }
        }
    }
}