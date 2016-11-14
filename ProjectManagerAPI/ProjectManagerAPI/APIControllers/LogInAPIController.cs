using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using ProjectManagerAPI.Models;
using ProjectManagerAPI.DataAccessLayer;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using ProjectManagerAPI.Utility;

namespace ProjectManagerAPI.APIControllers
{
    public class LogInAPIController : ApiController
    {
        [HttpGet]
        [Route("Login/authoriseCredentials/{user}/{password}")]
        public JObject returnJWT(string user, string password)
        {
            JObject returnJSON = new JObject();
            LoginDataAccess loginDataAccess = new LoginDataAccess();
            string noValidation = "";
            LoginExistPassword doesUserExist = new LoginExistPassword() { Exists = 0, Password = "" };

            doesUserExist = loginDataAccess.DoesUserExist(user);

            if (doesUserExist.Exists == 2 && doesUserExist.Password.Length > 0)
            {
                string passwordToCheckAgainst = doesUserExist.Password;
                
                bool validPassword = false;
                validPassword = BCrypt.Net.BCrypt.Verify(password, passwordToCheckAgainst);

                if (validPassword)
                {
                    KeyDataAccess keyDataAccess = new KeyDataAccess();
                    RSAParameters keyParams = RSAKey.GetKeyParameters(user);

                    RsaSecurityKey key = new RsaSecurityKey(keyParams);
                    var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
                    var claimsIdentity = new ClaimsIdentity();

                    if (doesUserExist.Role == true)
                    {
                        //Admin
                        claimsIdentity = new ClaimsIdentity(new List<Claim> {
                            
                                new Claim(ClaimTypes.Role, "Administrator")

                            });
                    }
                    else if (doesUserExist.Role == false)
                    {
                        //User
                        claimsIdentity = new ClaimsIdentity(new List<Claim> { 
                            
                                new Claim(ClaimTypes.Role, "User")

                            });

                    }
                    else
                    {
                        throw new Exception("Not a valid user");
                    }

                    var securityTokenDescriptor = new SecurityTokenDescriptor()
                    {
                        Issuer = "WolfAPI",
                        IssuedAt = DateTime.Now,
                        Expires = DateTime.Now.AddDays(1),
                        Subject = claimsIdentity,
                        SigningCredentials = signingCredentials,
                    };

                    var tokenHandler = new JwtSecurityTokenHandler();
                    var plainToken = tokenHandler.CreateToken(securityTokenDescriptor);
                    var signedAndEncodedToken = tokenHandler.WriteToken(plainToken);

                    returnJSON = JObject.FromObject(signedAndEncodedToken);
                }

            }
            else if (doesUserExist.Exists == 1)
            {
                noValidation = "User exists but has no password, please create a password";
                returnJSON = JObject.FromObject(noValidation);
            }
            else
            {
                noValidation = "User does not exist";
                returnJSON = JObject.FromObject(noValidation);
            }

            return returnJSON;
        }

        [HttpGet]
        [Route("Login/createNew/{user}/{password}")]
        public JObject CreateNewPassword(string user, string password)
        {

            JObject returnJSON = new JObject();
            LoginDataAccess loginDataAccess = new LoginDataAccess();
            string returnString = "";
            int wasAdded = 0;
            LoginExistPassword doesUserExist = new LoginExistPassword() { Exists = 0, Password = "" };
            doesUserExist = loginDataAccess.DoesUserExist(user);

            if(doesUserExist.Exists == 1)
            {
                string hashPass = BCrypt.Net.BCrypt.HashPassword(password);
                wasAdded = loginDataAccess.AddNewPassword(user, hashPass);

                if(wasAdded == 3)
                {
                    returnString = "Password Added Successfully, please log in with your details";
                }
                else if(wasAdded == 2)
                {
                    returnString = "User exists but is innactive";
                }
                else if(wasAdded == 1)
                {
                    returnString = "User already has a password";
                }
                else if(wasAdded == 0)
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

    }
}
