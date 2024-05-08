using ChatAppServer.Database.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ChatAppServer.Database.Repositories
{
    internal class UsersRepository
    {
        private readonly NpgsqlConnection? _connection;
        public UsersRepository(NpgsqlConnection? connection) 
        {
            _connection = connection;
        }

        public void GetAllUsers()
        {
            using NpgsqlCommand getAllUsersCommand = new NpgsqlCommand("SELCET * FROM users", _connection);
            using NpgsqlDataReader allUsersReader = getAllUsersCommand.ExecuteReader();

            while (allUsersReader.Read())
            {
                Console.WriteLine($"User: {allUsersReader["username"]}");

            }

        }

        public User? GetUser(Guid userId)
        {
            try
            {
                string getUserQuery = "SELECT * FROM users WHERE id = @p1";

                using (NpgsqlCommand getUserCommand = new NpgsqlCommand(getUserQuery, _connection))
                {
                    getUserCommand.Parameters.Add(new NpgsqlParameter("p1", NpgsqlTypes.NpgsqlDbType.Uuid) { Value = userId });

                    using (NpgsqlDataReader getUserReader = getUserCommand.ExecuteReader())
                    {
                        if (getUserReader.Read()) // Check if there are any rows returned
                        {
                            User user = new User
                            {
                                Username = getUserReader["username"].ToString(),
                                ImageSrc = getUserReader["imageSrc"].ToString(),
                                UserId = (Guid)getUserReader["id"],
                                CreatedAt = (DateTime)getUserReader["created_at"]
                            };
                            return user;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., log, throw, etc.)
                Console.WriteLine($"Error while retrieving user: {ex.Message}");
            }

            return null;
        }

        public void InsertUser(User user)
        {
            try
            {
                string insertCommand = "INSERT INTO users (id, username, imagesrc, created_at) VALUES (@p1, @p2, @p3, @p4)";

                using (NpgsqlCommand insertUserCommand = new NpgsqlCommand(insertCommand, _connection))
                {
                    // Add parameters for each value to be inserted
                    insertUserCommand.Parameters.Add(new NpgsqlParameter("p1", NpgsqlTypes.NpgsqlDbType.Uuid) { Value = user.UserId });
                    insertUserCommand.Parameters.Add(new NpgsqlParameter("p2", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = user.Username });
                    insertUserCommand.Parameters.Add(new NpgsqlParameter("p3", NpgsqlTypes.NpgsqlDbType.Text) { Value = user.ImageSrc });
                    insertUserCommand.Parameters.Add(new NpgsqlParameter("p4", NpgsqlTypes.NpgsqlDbType.TimestampTz) { Value = user.CreatedAt });

                    // Execute the command (no need for a reader)
                    int rowsAffected = insertUserCommand.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("User inserted successfully.");
                    }
                    else
                    {
                        Console.WriteLine("No rows were inserted.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while inserting user: {ex.Message}");
            }
        }


    }
}
