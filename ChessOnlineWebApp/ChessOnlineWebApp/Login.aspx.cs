using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using ChessOnlineWebAPI.Models;
using System.Web.Configuration;
namespace ChessOnlineWebApp
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void LoginButton_Click(object sender, EventArgs e)
        {
            LoginParams login = new LoginParams();
            login.Username = Username.Text;
            login.Password = Password.Text;
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(WebConfigurationManager.AppSettings.Get("api-base-url"));
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
                Task<HttpResponseMessage> task = client.PostAsJsonAsync("login", login);
                HttpResponseMessage response = task.Result;
                if (response.IsSuccessStatusCode)
                {
                    Task<string> token_task = response.Content.ReadAsStringAsync();
                    string token = token_task.Result;
                    token = token.Substring(1, token.Length - 2);
                    Session["username"] = login.Username;
                    HttpCookie token_cookie = new HttpCookie("token_cookie");
                    token_cookie.HttpOnly = true;
                    token_cookie.Value = token;
                    token_cookie.Expires = DateTime.Now.AddDays(15).AddSeconds(-1);
                    Response.Cookies.Add(token_cookie);
                    Response.Redirect("~/Home.aspx");
                }
                else
                {
                    Task<string> error_task = response.Content.ReadAsStringAsync(); ;
                    string error = error_task.Result;
                    ErrorLabel.Text = error;
                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = ex.ToString(); 
                ////ErrorLabel.Text = "Username or Password is incorrect :/\n";
            }
        }
    }
}