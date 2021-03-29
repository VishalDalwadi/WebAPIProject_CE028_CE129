using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChessOnlineWebAPI.Models
{
    public class RegisterParams
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}