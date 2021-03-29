using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ServiceModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Configuration;
using System.Threading.Tasks;
using ChessOnlineWebAPI.Models;
namespace ChessOnlineWebApp
{
    public partial class ResetPassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ResetPass_Click(object sender, EventArgs e)
        {
            ResetParams reset = new ResetParams();
            reset.NewPassword = NewPassword.Text;
            reset.Token = Token.Text;
            reset.Email = EmailID.Text;
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(WebConfigurationManager.AppSettings.Get("api-base-url"));
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                Task<HttpResponseMessage> task = client.PostAsJsonAsync("resetpassword", reset);
                HttpResponseMessage response = task.Result;
                if (response.IsSuccessStatusCode)
                {
                    ErrorLabel.ForeColor = System.Drawing.Color.Green;
                    ErrorLabel.Text = "Password Reset Successfully! Redirecting you to Login page ...";
                    Response.AddHeader("REFRESH", "2;URL=Login.aspx");
                }
            }
            catch (FaultException ex)
            {
                ErrorLabel.Text = ex.Reason.ToString();
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "Some unexpected error occurred :/";
                ErrorLabel.Text += ex.Message + ex.ToString();
            }
        }
    }
}