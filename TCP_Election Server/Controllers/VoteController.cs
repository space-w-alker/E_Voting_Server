using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TCP_Election_Server.Models;

namespace TCP_Election_Server.Controllers
{
    

    public static class VoteController
    {
        public static List<Category> BallotBox;

        public static void Voter(Vote vote)
        {
            foreach(Category category in BallotBox)
            {
                foreach(Candidate candidate in category.Candidates)
                {
                    if (candidate.UniqueId == vote.CandidateId)
                    {
                        if (candidate.VoteList.Count < 1)
                        {
                            vote.HashTitle = new SHA256Managed().ComputeHash(new UnicodeEncoding().GetBytes("FIRST_VOTE"));
                        }
                        else
                        {
                            vote.HashTitle = new SHA256Managed().ComputeHash(candidate.VoteList.Peek().EncryptedVote);
                        }
                        candidate.VoteList.Push(vote);
                        Console.WriteLine(JsonConvert.SerializeObject(vote, Formatting.Indented));
                        SaveBallotBox();
                    }
                }
            }
        }

        public static void SaveBallotBox()
        {
            File.WriteAllBytes("BallotBox.json", new byte[0]);

            using (FileStream fileStream = File.OpenWrite("BallotBox.json"))
            {
                StreamWriter writer = new StreamWriter(fileStream);
                writer.Write(JsonConvert.SerializeObject(BallotBox, Formatting.Indented));
                writer.Close();
                fileStream.Close();
            }
            return;
        }
    }
}
