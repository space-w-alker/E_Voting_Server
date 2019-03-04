using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCP_Election_Server.Models;
using Newtonsoft.Json;

namespace TCP_Election_Server.Controllers
{
    public class PingController
    {
        
        public static string PingNetwork(ActiveUser UserPing)
        {
            UnicodeEncoding ByteConverter = new UnicodeEncoding();

            byte[] Decrypted = Security.RSAImplementation.Decrypt(UserPing.EncryptedMessage, Security.RSAImplementation.LoadParameters());
            if (ByteConverter.GetString(Decrypted) != "APPLICATION")
            {
                return JsonConvert.SerializeObject(new { Error = true, Message = "Invalid Signature" });
            }

            List<ActiveUser> activeUsers = new List<ActiveUser>();
            List<ActiveUser> removeUsers = new List<ActiveUser>();
            var conn = Connection.Connect();
            conn.Open();
            var getActiveCommand = conn.CreateCommand();
            getActiveCommand.CommandText = "select * from active_user";
            using (var reader = getActiveCommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    ActiveUser user = new ActiveUser();
                    user.Id = (int)reader["id"];
                    user.IpAddress = (string)reader["ip_address"];
                    user.Ticks = (long)reader["time_stamp"];
                    activeUsers.Add(user);
                }
            }
            var command = conn.CreateCommand();
            command.CommandText = String.Format("UPDATE `active_user` SET `time_stamp` = '{0}' WHERE `active_user`.`ip_address` = '{1}';", DateTime.UtcNow.Ticks, UserPing.IpAddress);
            int rowsAffected = command.ExecuteNonQuery();
            if (rowsAffected == 0)
            {
                command.CommandText = String.Format("insert into active_user (`id`, `ip_address`, `time_stamp`) values (NULL, '{0}', '{1}')", UserPing.IpAddress, DateTime.UtcNow.Ticks);
                command.ExecuteNonQuery();
            }
            Security.Randomizer.Randomize(activeUsers);
            int countMax = 0;
            List<string> usersToReturn = new List<string>();
            foreach (ActiveUser user in activeUsers)
            {
                if (user.IpAddress == UserPing.IpAddress)
                {
                    continue;
                }
                if (DateTime.UtcNow.Ticks - user.Ticks > TimeSpan.TicksPerSecond * 20)
                {
                    //removeUsers.Add(user);
                    continue;
                }
                usersToReturn.Add(user.IpAddress);
                countMax += 1;
                if (countMax == 6) break;
            }
            DeleteItems(removeUsers);
            return JsonConvert.SerializeObject(new { ActiveUsers = usersToReturn });
        }

        public static async Task DeleteItems(IEnumerable<ActiveUser> users)
        {
            var conn = Connection.Connect();
            conn.Open();
            var command = conn.CreateCommand();
            foreach (ActiveUser user in users)
            {
                command.CommandText = String.Format("delete from active_user where `id` = '{0}'", user.Id);
                await command.ExecuteNonQueryAsync();
            }
            conn.Close();
        }
    }
}