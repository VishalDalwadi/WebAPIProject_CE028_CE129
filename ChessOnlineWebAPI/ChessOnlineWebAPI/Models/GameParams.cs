using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChessOnlineWebAPI.Models
{
    public class GameParams
    {
        public string GameString { get; set; }
        public char PlayedAs { get; set; }
        public string Token { get; set; }
    }
}