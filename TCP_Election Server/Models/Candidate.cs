using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCP_Election_Server.Models
{
    public class Candidate
    {
        public string CandidateName { get; set; }
        public Int64 UniqueId { get; set; }
        public int CatId { get; set; }
        public Stack<Vote> VoteList { get; set;}

        public Candidate()
        {
            VoteList = new Stack<Vote>();
        }
    }
}
