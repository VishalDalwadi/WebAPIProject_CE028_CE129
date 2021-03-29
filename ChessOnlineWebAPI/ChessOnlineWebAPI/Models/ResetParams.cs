using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChessOnlineWebAPI.Models
{
    public class ResetParams
    {
        public string NewPassword { get; set; }
        public string Token { get; set; }
        public string Email { get; set; }
    }
}