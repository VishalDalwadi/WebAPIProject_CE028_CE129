using ChessOnlineWebApp.GamesManagementServiceReference;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.ServiceModel;
using System.Web;
using System.Web.UI;

namespace ChessOnlineWebApp
{
    public partial class Home : System.Web.UI.Page
    {
        protected bool IsLoggedIn = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            Message.Text = "";
            find_player_button.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(find_player_button, null) + "; game = {}; $('#data').html('');");
            HttpCookie token_cookie = Request.Cookies.Get("token_cookie");
            if (token_cookie != null)
            {
                token.Value = token_cookie.Value;
                try
                {
                    using (AuthorizationServiceReference.AuthorizationServiceClient authZClient =
                        new AuthorizationServiceReference.AuthorizationServiceClient())
                    {
                        AuthorizationServiceReference.User user = authZClient.AuthorizeUser(token_cookie.Value);
                        IsLoggedIn = true;
                        Session["username"] = user.Username;
                    }
                }
                catch (FaultException<AuthorizationServiceReference.AuthorizationFault>)
                {
                    Response.Cookies["token_cookie"].Expires = DateTime.Now.AddDays(-1);
                }
            }
            /*
            if (IsPostBack)
            {
                string parameter = Request["__EVENTARGUMENT"];
                if (parameter == "rldsavedgames")
                {
                    show_saved_games_button_ClickAsync(sender, e);
                }
            }
            */
        }

        /*
        protected override void Render(HtmlTextWriter writer)
        {
            Page.ClientScript.RegisterForEventValidation(show_saved_games_button.UniqueID, "rldsavedgames");
            base.Render(writer);
        }
        */

        protected void logout_button_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Response.Cookies["token_cookie"].Expires = DateTime.Now.AddDays(-1);
            Response.Redirect("~/Home.aspx");
        }

        protected async void find_player_button_ClickAsync(object sender, EventArgs e)
        {
            HttpCookie token_cookie = Request.Cookies.Get("token_cookie");
            if (token_cookie != null)
            {
                try
                {
                    var handler = new WinHttpHandler();
                    using (HttpClient client = new HttpClient(handler))
                    {
                        HttpRequestMessage request = new HttpRequestMessage()
                        {
                            Content = new StringContent("{\"Token\":\"" + token_cookie.Value + "\"}"),
                            Method = HttpMethod.Get,
                            RequestUri = new Uri("https://localhost:44392/games/setup"),
                        };
                        request.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                        HttpResponseMessage response = await client.SendAsync(request);
                        if (response.IsSuccessStatusCode)
                        {
                            game_topic.Value = await response.Content.ReadAsStringAsync();
                            game_topic.Value = JsonConvert.DeserializeObject<string>(game_topic.Value);
                            find_player_button.Enabled = false;
                            show_saved_games_button.Enabled = false;
                            start_game_button.Enabled = true;
                            data.InnerHtml = "";
                        }
                        else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                        {
                            Response.Cookies["token_cookie"].Expires = DateTime.Now.AddDays(-1);
                            Message.ForeColor = System.Drawing.Color.Red;
                            Message.Text = "Sorry for the inconvenience but we need you to login again!";
                            Response.Redirect("~/Login.aspx");
                            Response.AddHeader("REFRESH", "3;URL=Login.aspx");
                        }
                        else
                        {
                            Message.ForeColor = System.Drawing.Color.Red;
                            Message.Text = "Sorry! Couldn't find a player for you. Please try again!";
                        }
                    }
                }
                catch (Exception)
                {
                    //
                }
            }
        }

        protected async void show_saved_games_button_ClickAsync(object sender, EventArgs e)
        {
            try
            {
                var handler = new WinHttpHandler();
                using (HttpClient client = new HttpClient(handler))
                {
                    HttpCookie token_cookie = HttpContext.Current.Request.Cookies.Get("token_cookie");
                    HttpRequestMessage request = new HttpRequestMessage()
                    {
                        Content = new StringContent("{\"Token\":\"" + token_cookie.Value + "\"}"),
                        Method = HttpMethod.Get,
                        RequestUri = new Uri("https://localhost:44392/games/saved")
                    };
                    request.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                    HttpResponseMessage response = await client.SendAsync(request);
                    if (response.IsSuccessStatusCode)
                    {
                        string message = await response.Content.ReadAsStringAsync();
                        Game[] games = JsonConvert.DeserializeObject<Game[]>(message);
                        start_game_button.Enabled = false;
                        find_player_button.Enabled = true;
                        show_saved_games_button.Enabled = true;
                        if (games.Length == 0) return;
                        string html = "<table id='saved_games'><thead><tr><th>No.</th><th></th><th></th></tr></thead>";
                        for (int i = 0; i < games.Length; i++)
                        {
                            html += "<tr><td style='padding-left: 1%; width: 10%;'>" + (i + 1) + "</td><td><input type='button' value='Play Game' onclick='play_game(\"" + games[i].GameString + "\");'></input></td><td><input type='button' value='Delete Game' onclick='delete_game(\"" + games[i].GameId + "\");'></input></td></tr>";
                        }
                        html += "</table>";
                        data.InnerHtml = html;
                    }
                }
            }
            catch (Exception ex)
            {
                Message.Text = ex.Message + "\n" + ex.StackTrace;
            }
        }
    }
}