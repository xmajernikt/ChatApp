using ChatAppServer.Database.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatAppServer.Database.Repositories
{
    internal class ContactRepository
    {
        private readonly NpgsqlConnection _connection;

        public ContactRepository(NpgsqlConnection? connection)
        {
            _connection = connection;
        }

        public void AddContact(Contact contact)
        {
            string InsertionString = """
                INSERT INTO contacts 
                (id, username, imagesrc, created_at, lastmessage, usersusername) 
                VALUES (@p1, @p2, @p3, @p4, @p5, @p6)
                """;
            try
            {
                using (NpgsqlCommand insertCommand = new NpgsqlCommand(InsertionString, _connection))
                {
                    insertCommand.Parameters.Add(new NpgsqlParameter("p1", NpgsqlTypes.NpgsqlDbType.Uuid) { Value = contact.Id });
                    insertCommand.Parameters.Add(new NpgsqlParameter("p2", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = contact.Username });
                    insertCommand.Parameters.Add(new NpgsqlParameter("p3", NpgsqlTypes.NpgsqlDbType.Text) { Value = contact.ImageSrc });
                    insertCommand.Parameters.Add(new NpgsqlParameter("p4", NpgsqlTypes.NpgsqlDbType.TimestampTz) { Value = contact.CreatedAt });
                    insertCommand.Parameters.Add(new NpgsqlParameter("p5", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = contact.LastMessage });
                    insertCommand.Parameters.Add(new NpgsqlParameter("p6", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = contact.UsernameId });

                    if (insertCommand.ExecuteNonQuery() > 0)
                    {
                        Console.WriteLine("Contact inserted successfully");

                    }
                    else
                    {
                        Console.WriteLine("No rows were inserted.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while inserting contact: {ex.Message}");

            }

        }

        public List<Contact> GetContacts(string SenderUsername) 
        {
            List<Contact> contacts = new List<Contact>();
            string queryString =
                """
                SELECT * FROM contacts WHERE usersusername = @p1
                """;
            using (NpgsqlCommand command = new NpgsqlCommand(queryString, _connection))
            {
                command.Parameters.Add(new NpgsqlParameter("p1", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = SenderUsername });

                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        contacts.Add(new Contact
                        {
                            Id = (Guid)reader["id"],
                            Username = (string)reader["username"],
                            UsernameId = (string)reader["usersusername"],
                            CreatedAt = (DateTime)reader["created_at"],
                            ImageSrc = (string)reader["imagesrc"],
                            LastMessage = (string)reader["lastmessage"]
                        });
                    }
                }
            }
            return contacts;
        }
    }
}
