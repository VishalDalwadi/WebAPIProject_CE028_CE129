using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChessOnlineWebAPI.Models;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Configuration;
namespace ChessOnlineWebApp
{
    public partial class Register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void Register_Click(object sender, EventArgs e)
        {
            RegisterParams register = new RegisterParams();
            register.Username = Username.Text;
            register.Email = EmailID.Text;
            register.Password = Password.Text;
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(WebConfigurationManager.AppSettings.Get("api-base-url"));
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                Task<HttpResponseMessage> task = client.PostAsJsonAsync("register", register);
                HttpResponseMessage response = task.Result;
                if (response.IsSuccessStatusCode)
                {
                    Task<string> token_task = response.Content.ReadAsStringAsync(); ;
                    string token = token_task.Result;
                    StatusMsg.ForeColor = System.Drawing.Color.Green;
                    StatusMsg.Text = "Registration Successful! Redirecting you to Login page ...";
                    Response.AddHeader("REFRESH", "3;URL=Login.aspx");
                }
                else
                {
                    Task<string> error_task = response.Content.ReadAsStringAsync(); ;
                    string error = error_task.Result;
                    StatusMsg.ForeColor = System.Drawing.Color.Red;
                    StatusMsg.Text = error;
                }
            }
            catch (Exception ex)
            {
                StatusMsg.ForeColor = System.Drawing.Color.Red;
                StatusMsg.Text = ex.Message + " " + ex.GetBaseException() + " " + ex.ToString();
            }
        }
    }
}