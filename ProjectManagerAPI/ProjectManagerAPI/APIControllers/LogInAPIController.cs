using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using ProjectManagerAPI.DataAccessLayer;
using ProjectManagerAPI.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Protocols.WSTrust;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using System.IO;
using Wolf.Assembly.Logging;

namespace ProjectManagerAPI.APIControllers
{
    public class LogInAPIController : ApiController
    {
        string errorString = "Error";
        string authenticatedString = "Authenticated";
        string notAuthenticatedString = "NonAuthenticated";

        [HttpPost]
        [Route("api/Login/authoriseCredentials")]
        public JObject returnJWT([FromBody]JObject inputDetails)
        {
            using (new MethodLogging())
            {
                try
                {
                    JObject userPass = inputDetails;

                    UserPassPair currentUser = userPass.ToObject<UserPassPair>();

                    LoginDataAccess loginDataAccess = new LoginDataAccess();
                    string noValidation = "";

                    LoginExistPassword doesUserExist = new LoginExistPassword() { Exists = 0, Password = "", Role = false, UserID = 0 };

                    doesUserExist = loginDataAccess.DoesUserExist(currentUser.User);

                    if (doesUserExist.Exists == 2 && doesUserExist.Password.Length > 0)
                    {
                        string passwordToCheckAgainst = doesUserExist.Password;

                        bool validPassword = false;
                        validPassword = BCrypt.Net.BCrypt.Verify(currentUser.Password, passwordToCheckAgainst);

                        Console.WriteLine("validPassword?: " + validPassword.ToString());

                        if (validPassword)
                        {
                            KeyDataAccess keyDataAccess = new KeyDataAccess();
                            var plainTextSecurityKey = keyDataAccess.GetKey();
                            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(plainTextSecurityKey));
                            var signingCredentials = new SigningCredentials(signingKey,
                                SecurityAlgorithms.HmacSha256Signature);

                            var claimsIdentity = new ClaimsIdentity();

                            if (doesUserExist.Role == true)
                            {
                                //Admin
                                claimsIdentity = new ClaimsIdentity(new List<Claim> {
                            
                                new Claim(ClaimTypes.Role, "Administrator"),
                                new Claim(ClaimTypes.Name, doesUserExist.UserID.ToString())

                            });
                            }
                            else if (doesUserExist.Role == false)
                            {
                                //User
                                claimsIdentity = new ClaimsIdentity(new List<Claim> { 
                            
                                new Claim(ClaimTypes.Role, "User"),
                                new Claim(ClaimTypes.Name, doesUserExist.UserID.ToString())

                            });

                            }
                            else
                            {
                                throw new Exception("Not a valid user");
                            }

                            var securityTokenDescriptor = new SecurityTokenDescriptor()
                            {
                                Issuer = "Self",
                                Audience = "http://wolfwebtest1:2099",
                                Expires = DateTime.Now.AddMinutes(5),
                                Subject = claimsIdentity,
                                SigningCredentials = signingCredentials,
                            };

                            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                            var plainToken = tokenHandler.CreateToken(securityTokenDescriptor);
                            var signedAndEncodedToken = tokenHandler.WriteToken(plainToken);

                            #region Another way of Generating JWT
                            //var tokenHandler = new JwtSecurityTokenHandler();
                            //var input = "anyoldrandomtext";
                            //var securityKey = new byte[input.Length * sizeof(char)];
                            //Buffer.BlockCopy(input.ToCharArray(), 0, securityKey, 0, securityKey.Length);
                            //var now = DateTime.UtcNow;
                            //var tokenDescriptor = new SecurityTokenDescriptor
                            //{
                            //    Subject = new ClaimsIdentity(new[]
                            //{
                            //    new Claim( ClaimTypes.UserData,
                            //    "IsValid", ClaimValueTypes.String, "(local)" )
                            //}),
                            //    Issuer = "self",
                            //    Audience = "https://www.mywebsite.com",
                            //    Expires = now.AddMinutes(60),
                            //    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(securityKey), SecurityAlgorithms.HmacSha256),
                            //};

                            //var token = tokenHandler.CreateToken(tokenDescriptor);
                            //var tokenString = tokenHandler.WriteToken(token);
                            #endregion

                            LoginMessage authenticatedReturnString = new LoginMessage() { Message = signedAndEncodedToken, Type = authenticatedString, UserID = doesUserExist.UserID.ToString() };

                            return JObject.FromObject(authenticatedReturnString);
                        }

                    }
                    else if (doesUserExist.Exists == 2)
                    {
                        noValidation = "Incorrect Username of Password";
                    }
                    else if (doesUserExist.Exists == 1)
                    {
                        noValidation = "User exists but has no password, please create a password";
                    }
                    else
                    {
                        noValidation = "User does not exist";
                    }

                    LoginMessage noValidationReturnString = new LoginMessage() { Message = noValidation, Type = notAuthenticatedString };

                    return JObject.FromObject(noValidationReturnString);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        [HttpPost]
        [Route("api/Login/createNew")]
        public JObject CreateNewPassword([FromBody] JToken userPass)
        {
            UserPassPair newUser = userPass.ToObject<UserPassPair>();
            JObject returnJSON = new JObject();
            LoginDataAccess loginDataAccess = new LoginDataAccess();
            string returnString = "";
            int wasAdded = 0;
            LoginExistPassword doesUserExist = new LoginExistPassword() { Exists = 0, Password = "" };
            doesUserExist = loginDataAccess.DoesUserExist(newUser.User);

            if (doesUserExist.Exists == 1)
            {
                string hashPass = BCrypt.Net.BCrypt.HashPassword(newUser.Password);
                wasAdded = loginDataAccess.AddNewPassword(newUser.User, hashPass);

                if (wasAdded == 3)
                {
                    returnString = "Password Added Successfully, please log in with your details";
                }
                else if (wasAdded == 2)
                {
                    returnString = "User exists but is innactive";
                }
                else if (wasAdded == 1)
                {
                    returnString = "User already has a password";
                }
                else if (wasAdded == 0)
                {
                    returnString = "Error";
                }
            }
            else
            {
                returnString = "User does not exist";

            }

            returnJSON = JObject.FromObject(returnString);
            return returnJSON;
        }

        [HttpPost]
        [Route("api/Login/authoriseToken")]
        public JObject authoriseToken([FromBody]string tokenData)
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
            
                            if((string)claim["Type"] == "role")
                            {
                                role = claim["Value"].ToString();
                            }

                            if((string)claim["Type"] == "unique_name")
                            {
                                id = claim["Value"].ToString();
                            }
                            //if ((string)claim["Value"] == "Administrator")
                            //{
                            //    isAuthorised = 2;
                            //    tokenReturnString = new LoginMessage() {Message = isAuthorised.ToString(), Type = authenticatedString, UserID=};
                            //    return JObject.FromObject(tokenReturnString);
                            //}
                        }
                        if(role == "Administrator")
                        {
                            isAuthorised = 2;
                            tokenReturnString = new LoginMessage() {Message = isAuthorised.ToString(), Type = authenticatedString, UserID=id};
                            return JObject.FromObject(tokenReturnString);

                        }
                        else if(role == "User")
                        {
                            isAuthorised = 1;
                            tokenReturnString = new LoginMessage() { Message = isAuthorised.ToString(), Type = authenticatedString, UserID = id };
                            return JObject.FromObject(tokenReturnString);
                        }
                        else
                        {
                            isAuthorised = 0;
                            tokenReturnString = new LoginMessage() { Message = isAuthorised.ToString(), Type = notAuthenticatedString, UserID = "0" };
                            return JObject.FromObject(tokenReturnString);
                        }

                    }
                    catch (SecurityTokenExpiredException)
                    {
                        tokenReturnString = new LoginMessage() { Message = "Expired", Type = notAuthenticatedString };
                        return JObject.FromObject(tokenReturnString);
                    }
                    catch (SecurityTokenInvalidAudienceException)
                    {
                        tokenReturnString = new LoginMessage() { Message = "Invalid Audience", Type = notAuthenticatedString };
                        return JObject.FromObject(tokenReturnString);
                    }
                    catch (SecurityTokenInvalidIssuerException)
                    {
                       tokenReturnString = new LoginMessage() { Message = "Invalid Issuer", Type = notAuthenticatedString };
                        return JObject.FromObject(tokenReturnString);
                    }
                    catch (SecurityTokenInvalidSigningKeyException)
                    {
                        tokenReturnString = new LoginMessage() { Message = "Invalid Key", Type = notAuthenticatedString };
                        return JObject.FromObject(tokenReturnString);
                    }

                }
                catch(Exception e)
                {
                    throw e;
                }
                return JObject.FromObject(new LoginMessage() {Message = "Hit the end", Type = notAuthenticatedString });
            }
        }
    }
}
