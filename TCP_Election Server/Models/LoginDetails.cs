using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TCP_Election_Server.Models
{
    public class LoginDetails
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string IpAddress { get; set; }
        public string Action { get; set; }
    }
}
