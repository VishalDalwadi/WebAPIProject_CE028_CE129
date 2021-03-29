using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.ServiceModel;
using ChessOnlineWebAPI.Models;
using System.Text;
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
        
        // POST /login
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

        // POST /register
        [HttpPost]
		[Route("register")]
        public string Post([FromBody] RegisterParams register)
        {
			string username = register.Username;
            string password = register.Password;
            string email = register.Email;
			using (UserProfileServiceReference.UserProfileManagementServiceClient client = new UserProfileServiceReference.UserProfileManagementServiceClient())
            {
                string status_msg = null;
                bool username_taken = client.IsUsernameTaken(username);
                bool email_exists = client.UserWithEmailIdExists(email);
                if (username_taken)
                {
                    status_msg = "This username is unavailable :/. ";
					throw new HttpResponseException(SetHttpErrorMsg(status_msg, HttpStatusCode.BadRequest));
                }
                if (email_exists)
                {
                    status_msg = "An account with this email already exists -.- ";
					throw new HttpResponseException(SetHttpErrorMsg(status_msg, HttpStatusCode.BadRequest));
                }
				UserProfileServiceReference.User user = new UserProfileServiceReference.User();
				user.Username = username;
				user.Password = password;
				user.EmailID = email;
				client.RegisterUser(user);
				status_msg = "Registration Successful!";
                return status_msg;
            }
        }

        //POST /forgot
        [Route("forgotpassword")]
        [HttpPost]
        public string Post([FromBody] ForgotParams forgot)
        {
            string emailID = forgot.Email;
            try
            {
                using (UserProfileServiceReference.UserProfileManagementServiceClient client = new UserProfileServiceReference.UserProfileManagementServiceClient())
                {
                    client.SendPasswordResetToken(emailID);
                }
            }
            catch (FaultException ex)
            {
                throw new HttpResponseException(SetHttpErrorMsg(ex.Reason.ToString(), HttpStatusCode.BadRequest));
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(SetHttpErrorMsg("Some unexpected error occurred :/", HttpStatusCode.InternalServerError));
                //throw new HttpResponseException(SetHttpErrorMsg(ex.Message, HttpStatusCode.InternalServerError));
            }
            return "Token sent on email";
        }

        //POST /forgot
        [Route("resetpassword")]
        [HttpPost]
        public string Post([FromBody] ResetParams reset)
        {
            string npwd = reset.NewPassword;
            string token = reset.Token;
            string email_id = reset.Email;
            try
            {
                using (UserProfileServiceReference.UserProfileManagementServiceClient client = new UserProfileServiceReference.UserProfileManagementServiceClient())
                {
                    client.ResetPassword(token, email_id, npwd);
                    return "Password Reset Successfully!";                  
                }
            }
            catch (FaultException ex)
            {
                throw new HttpResponseException(SetHttpErrorMsg(ex.Reason.ToString(), HttpStatusCode.BadRequest));
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(SetHttpErrorMsg("Some unexpected error occurred :/" + ex.Message, HttpStatusCode.InternalServerError));
            }
        }

        [HttpPost]
        [Route("authorize")]
        public bool Post([FromBody]TokenParams token)
        {
            try
            {
                using (AuthorizationServiceReference.AuthorizationServiceClient authZClient =
                    new AuthorizationServiceReference.AuthorizationServiceClient())
                {
                    AuthorizationServiceReference.User user = authZClient.AuthorizeUser(token.Token);
                    return true;
                }
            }
            catch (FaultException<AuthorizationServiceReference.AuthorizationFault> ex)
            {
                throw new HttpResponseException(SetHttpErrorMsg(ex.Reason.ToString(), HttpStatusCode.BadRequest));
            }
            catch (Exception)
            {
                throw new HttpResponseException(SetHttpErrorMsg("Some unexpected error occured, check that you're logged in and try again.", HttpStatusCode.BadRequest));
            }
        }

    }
}
