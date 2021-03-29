using ChessOnlineWebAPI.GamesManagementServiceReference;
using ChessOnlineWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel;
using System.Web.Http;

namespace ChessOnlineWebAPI.Controllers
{
    public class GamesController : ApiController
    {
        private HttpResponseMessage SetHttpErrorMsg(string error, HttpStatusCode error_code)
        {
            var error_resp = new HttpResponseMessage();
            error_resp.StatusCode = error_code;
            error_resp.Content = new StringContent(error);
            return error_resp;
        }

        // GET /games/saved
        [HttpGet]
        [Route("games/saved")]
        public Game[] GetSavedGames([FromBody] TokenParams token)
        {
            try {
                using (GamesManagementServiceClient gmsClient = new GamesManagementServiceClient())
                {
                    return gmsClient.GetAllSavedGames(token.Token);
                }
            }
            catch (FaultException<GamesManagementFault> ex)
            {
                if (ex.Detail.FaultType == GamesManagementFault.GamesManagementFaultType.TokenExpired)
                {
                    throw new HttpResponseException(SetHttpErrorMsg("Token Expired. Re-login.", HttpStatusCode.Unauthorized));
                }
                else if (ex.Detail.FaultType == GamesManagementFault.GamesManagementFaultType.InvalidSignature)
                {
                    throw new HttpResponseException(SetHttpErrorMsg("Invalid Token Signature.", HttpStatusCode.Unauthorized));
                }
                else
                {
                    throw new HttpResponseException(SetHttpErrorMsg("Internal Server Error.", HttpStatusCode.InternalServerError));
                }
            }
        }

        // POST /games/save
        [HttpPost]
        [Route("games/save")]
        public void Post([FromBody] GameParams gameParams)
        {
            try {
                using (GamesManagementServiceClient gmsClient = new GamesManagementServiceClient())
                {
                    Game game = new Game
                    {
                        GameString = gameParams.GameString,
                        PlayedAs = gameParams.PlayedAs == 'w' ? Game.Player.White : Game.Player.Black
                    };
                    gmsClient.SaveGame(game, gameParams.Token);
                }
            }
            catch (FaultException<GamesManagementFault> ex)
            {
                if (ex.Detail.FaultType == GamesManagementFault.GamesManagementFaultType.TokenExpired)
                {
                    throw new HttpResponseException(SetHttpErrorMsg("Token Expired. Re-login.", HttpStatusCode.Unauthorized));
                } 
                else if (ex.Detail.FaultType == GamesManagementFault.GamesManagementFaultType.InvalidSignature)
                {
                    throw new HttpResponseException(SetHttpErrorMsg("Invalid Token Signature.", HttpStatusCode.Unauthorized));
                }
                else
                {
                    throw new HttpResponseException(SetHttpErrorMsg("Internal Server Error.", HttpStatusCode.InternalServerError));
                }
            }
        }

        // DELETE /games/{gameId}
        [HttpDelete]
        [Route("games/{gameId}")]
        public void Delete([FromBody] TokenParams token, Int64 gameId)
        {
            try
            {
                using (GamesManagementServiceClient gmsClient = new GamesManagementServiceClient())
                {
                    Game game = new Game()
                    {
                        GameId = gameId
                    };
                    gmsClient.DeleteGame(game, token.Token);
                }
            }
            catch (FaultException<GamesManagementFault> ex)
            {
                if (ex.Detail.FaultType == GamesManagementFault.GamesManagementFaultType.TokenExpired)
                {
                    throw new HttpResponseException(SetHttpErrorMsg("Token Expired. Re-login.", HttpStatusCode.Unauthorized));
                } 
                else if (ex.Detail.FaultType == GamesManagementFault.GamesManagementFaultType.InvalidSignature)
                {
                    throw new HttpResponseException(SetHttpErrorMsg("Invalid Token Signature.", HttpStatusCode.Unauthorized));
                }
                else
                {
                    throw new HttpResponseException(SetHttpErrorMsg("Internal Server Error.", HttpStatusCode.InternalServerError));
                }
            }
        }

        // GET /games/setup
        [HttpGet]
        [Route("games/setup")]
        public string Get([FromBody] TokenParams token)
        {
            try
            {
                using (GamesManagementServiceClient gmsClient = new GamesManagementServiceClient())
                {
                    return gmsClient.FindMatch(token.Token);
                }
            }
            catch (FaultException<GamesManagementFault> ex)
            {
                if (ex.Detail.FaultType == GamesManagementFault.GamesManagementFaultType.TokenExpired)
                {
                    throw new HttpResponseException(SetHttpErrorMsg("Token Expired. Re-login.", HttpStatusCode.Unauthorized));
                }
                else if (ex.Detail.FaultType == GamesManagementFault.GamesManagementFaultType.InvalidSignature)
                {
                    throw new HttpResponseException(SetHttpErrorMsg("Invalid Token Signature.", HttpStatusCode.Unauthorized));
                }
                else
                {
                    throw new HttpResponseException(SetHttpErrorMsg("Internal Server Error.", HttpStatusCode.InternalServerError));
                }
            }
        }
    }
}
