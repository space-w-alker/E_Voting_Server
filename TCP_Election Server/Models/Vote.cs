

namespace TCP_Election_Server.Models
{
    public class Vote
    {       
        public int CandidateId { get; set; }
        public byte[] EncryptedVote { get; set; }
        public byte[] HashTitle;
    }
}
