using System;
using System.Collections.Generic;

namespace TCP_Election_Server.Models
{
    public class Category
    {
        public string CatName { get; set; }
        public Int64 Id { get; set; }
        public List<Candidate> Candidates { get; set; }
        

        public Category()
        {
            Candidates = new List<Candidate>();
        }
    }
}
