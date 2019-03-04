using MySql.Data.MySqlClient;
using System.Collections.Generic;
using TCP_Election_Server.Models;

namespace TCP_Election_Server
{
    public class Connection
    {
        public static MySqlConnection Connect()
        {
            string conn_string = "server=localhost;port=3306;database=Election;username=root;password=;";
            MySqlConnection conn = new MySqlConnection(conn_string);
            return conn;
        }

        public static Category[] LoadData()
        {
            List<Category> lcat = new List<Category>();
            var conn = Connect();
            var command = conn.CreateCommand();
            conn.Open();
            command.CommandText = "select * from category";
            try
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Category category = new Category();
                        category.CatName = (string)reader["cat_name"];
                        category.Id = (int)reader["id"];
                        lcat.Add(category);
                    }
                }
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine(e);
            }
            command.CommandText = command.CommandText = "select * from candidate";
            try
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Candidate candidate = new Candidate();
                        candidate.CandidateName = (string)reader["name"];
                        candidate.UniqueId = (int)reader["id"];
                        candidate.CatId = (int)reader["cat_id"];
                        foreach (Category cat in lcat)
                        {
                            if (cat.Id == candidate.CatId)
                            {
                                cat.Candidates.Add(candidate);
                            }
                        }
                    }
                }
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine(e);
            }
            conn.Close();
            return lcat.ToArray();
        }
    }
}
