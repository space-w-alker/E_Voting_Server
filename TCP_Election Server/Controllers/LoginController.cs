using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using TCP_Election_Server.Models;

namespace TCP_Election_Server.Controllers
{
    public class LoginController
    {
        
        public static string Login(LoginDetails RequestBody)
        {

            var request = RequestBody;
            var conn = Connection.Connect();
            RSAParameters param = Security.RSAImplementation.LoadParameters();
            RSAParameters publicParam;
            using (RSACryptoServiceProvider p = new RSACryptoServiceProvider())
            {
                p.ImportParameters(param);
                publicParam = p.ExportParameters(false);
            }

            conn.Open();
            try
            {

                var command = conn.CreateCommand();
                command.CommandText = String.Format("select * from User where username = '{0}'", request.Username);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if ((string)reader["password"] == request.Password)
                        {
                            bool ipExists = false;
                            reader.Close();
                            var getActiveCommand = conn.CreateCommand();
                            getActiveCommand.CommandText = "select * from active_user";
                            List<string> ActiveUsers = new List<string>();
                            using (MySqlDataReader otherReader = getActiveCommand.ExecuteReader())
                            {
                                while (otherReader.Read())
                                {
                                    string ip_address = (string)otherReader["ip_address"];
                                    if (ip_address == request.IpAddress)
                                    {
                                        ipExists = true;
                                        continue;
                                    }
                                    ActiveUsers.Add((string)otherReader["ip_address"]);
                                }
                            }
                            if (!ipExists)
                            {
                                var insertCommand = conn.CreateCommand();
                                insertCommand.CommandText = String.Format("INSERT INTO `active_user` (`id`, `ip_address`, `time_stamp`) VALUES (NULL, '{0}', '{1}');", request.IpAddress, DateTime.UtcNow.Ticks);
                                insertCommand.ExecuteNonQuery();
                            }
                            return JsonConvert.SerializeObject(new { Error = false, Key = publicParam });
                        }
                        else
                        {
                            return JsonConvert.SerializeObject(new { Error = true, Message = "Invalid Username or Password" });
                        }
                    }
                    return JsonConvert.SerializeObject(new { Error = true, Message = "Invalid Username or Password" });
                }
            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(new { Error = true, e.Message });
            }
            finally
            {
                conn.Close();
            }
        }
    }
}