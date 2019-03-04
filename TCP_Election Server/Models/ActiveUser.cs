using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCP_Election_Server.Models
{
    public class ActiveUser
    {
        public string Action { get; set; }
        public long Id { get; set; }
        public string IpAddress { get; set; }
        public long Ticks { get; set; }
        public byte[] EncryptedMessage { get; set; }
    }
}
