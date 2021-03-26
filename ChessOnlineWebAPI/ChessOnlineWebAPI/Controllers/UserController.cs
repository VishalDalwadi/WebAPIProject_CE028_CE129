using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.ServiceModel;
using ChessOnlineWebAPI.Models;
namespace ChessOnlineWebAPI.Controllers
{
    public class UserController : ApiController
    {
        private HttpResponseMessage SetHttpErrorMsg(string error, HttpStatusCode error_code)
        {
            var error_resp = new HttpResponseMessage();
            error_resp.StatusCode = error_code;
            error_resp.Content = new StringContent(error);
            return error_resp;
        }
        // GET api/values
        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        [Route("login")]
        public string Post([FromBody] LoginParams login)
        {
            string error = null;
			string username = login.Username;
			string password = login.Password;
            using (AuthenticationServiceReference.AuthenticationServiceClient client = new AuthenticationServiceReference.AuthenticationServiceClient())
            {
                string token = null;
                try
                { 
                    token = client.AreCorrectCredentials(username, password);                    
                }
                catch (FaultException<AuthenticationServiceReference.AuthenticationFault> ex)
                {
                    var error_resp = new HttpResponseMessage();
                    if (ex.Detail.FaultType == AuthenticationServiceReference.AuthenticationFault.AuthenticationFaultType.NoSuchUser)
                    {
						error = "User with the name " + username + " does not exist :/ Check that you've entered the name correctly.";
                        throw new HttpResponseException(SetHttpErrorMsg(error, HttpStatusCode.NotFound));
                    }
                    if (ex.Detail.FaultType == AuthenticationServiceReference.AuthenticationFault.AuthenticationFaultType.InvalidPassword)
                    {
						error = "The password entered is incorrect :/";
                        throw new HttpResponseException(SetHttpErrorMsg(error, HttpStatusCode.NotFound));
                    }
                    if (ex.Detail.FaultType == AuthenticationServiceReference.AuthenticationFault.AuthenticationFaultType.ServerFault)
                    {
                        error = "The server encountered an error while processing your request. Please try again.";
                        throw new HttpResponseException(SetHttpErrorMsg(error, HttpStatusCode.InternalServerError));
                    }
                }
                return token;
            }
        }

        // PUT api/values/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
