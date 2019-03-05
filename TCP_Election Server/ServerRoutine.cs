using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TCP_Election_Server.Models;

namespace TCP_Election_Server
{
    public static class ServerRoutine
    {
        private static TcpListener server;

        public static void StartRoutine()
        {
            Int32 port = 13002;
            IPAddress localAddress = IPAddress.Parse("127.0.0.1");

            server = new TcpListener(localAddress, port);
            server.Start();

            while (true)
            {
                //Console.WriteLine("Waiting for connection...");
                TcpClient client = server.AcceptTcpClient();
                Thread thread = new Thread(() => ClientHandler(client));
                thread.Start();
                Thread.Sleep(0);
            }
        }

        public static void ClientHandler(TcpClient client)
        {
            string requestString = "";
            //Console.WriteLine("Other Thread");
            try
            {
                NetworkStream networkStream = client.GetStream();
                StreamReader reader = new StreamReader(networkStream);
                
                requestString = reader.ReadLine();
                    
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                
            }
            var action = JsonConvert.DeserializeAnonymousType(requestString, new { Action = "" });
            string response = "";

            if(action.Action == "LOGIN")
            {
                LoginDetails loginDetails = JsonConvert.DeserializeObject<LoginDetails>(requestString);
                Console.WriteLine(String.Format("This is the Ip Address: {0}", loginDetails.IpAddress));
                response = Controllers.LoginController.Login(loginDetails);
                using(StreamWriter writer = new StreamWriter(client.GetStream()))
                {
                    writer.WriteLine(response);
                    writer.Flush();
                }
            }
            if(action.Action == "PING")
            {
                ActiveUser user = JsonConvert.DeserializeObject<ActiveUser>(requestString);
                response = Controllers.PingController.PingNetwork(user);
                using (StreamWriter writer = new StreamWriter(client.GetStream()))
                {
                    writer.Write(response);
                    writer.Flush();
                }
            }
            if(action.Action == "VOTE")
            {
                Controllers.VoteController.Voter(JsonConvert.DeserializeObject<Vote>(requestString));
            }
            client.Close();
            return;
        }
    }
}
