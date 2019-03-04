using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCP_Election_Server.Models;

namespace TCP_Election_Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting Server...");
            Controllers.VoteController.BallotBox = new List<Category>(Connection.LoadData());
            ServerRoutine.StartRoutine();
            
        }
    }
}
