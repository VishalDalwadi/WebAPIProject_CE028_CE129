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
    public partial class ForgotPassword : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void SendToken_Click(object sender, EventArgs e)
        {
            ForgotParams forgot = new ForgotParams();
            forgot.Email = EmailID.Text;
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(WebConfigurationManager.AppSettings.Get("api-base-url"));
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                Task<HttpResponseMessage> task = client.PostAsJsonAsync("forgotpassword", forgot);
                HttpResponseMessage response = task.Result;
                if (response.IsSuccessStatusCode)
                {
                    Response.Redirect("~/ResetPassword.aspx");
                }
            }
            catch (FaultException ex)
            {
                ErrorLabel.Text = ex.Reason.ToString();
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "Some unexpected error occurred :/" + ex.Message + ex.ToString();
            }
        }
    }
}