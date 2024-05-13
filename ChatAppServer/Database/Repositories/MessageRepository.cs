using ChatAppServer.Database.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatAppServer.Database.Repositories
{
    internal class MessageRepository
    {
        private NpgsqlConnection _connection;

        public MessageRepository(NpgsqlConnection connection) 
        {
            _connection = connection;
        }

        public void InsertMessage(Message message)
        {
            try
            {

                string insertionString =
                    """
                    INSERT INTO messages 
                    (id, message, created_at, isnativeoriginated, isfirstmessage, usersusername, contactsid) 
                    VALUES (@p1, @p2, @p3, @p4, @p5, @p6, @p7)
                    """;

                using (NpgsqlCommand insertionCommand = new NpgsqlCommand(insertionString, _connection))
                {
                    insertionCommand.Parameters.Add(new NpgsqlParameter("p1", NpgsqlTypes.NpgsqlDbType.Uuid) { Value = message.Id });
                    insertionCommand.Parameters.Add(new NpgsqlParameter("p2", NpgsqlTypes.NpgsqlDbType.Text) { Value = message.MessageText });
                    insertionCommand.Parameters.Add(new NpgsqlParameter("p3", NpgsqlTypes.NpgsqlDbType.TimestampTz) { Value = message.CreatedAt });
                    insertionCommand.Parameters.Add(new NpgsqlParameter("p4", NpgsqlTypes.NpgsqlDbType.Boolean) { Value = message.IsNativeOriginated });
                    insertionCommand.Parameters.Add(new NpgsqlParameter("p5", NpgsqlTypes.NpgsqlDbType.Boolean) { Value = message.IsFirstMessage });
                    insertionCommand.Parameters.Add(new NpgsqlParameter("p6", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = message.UsernameID });
                    insertionCommand.Parameters.Add(new NpgsqlParameter("p7", NpgsqlTypes.NpgsqlDbType.Uuid) { Value = message.ContactId });

                    int rowsAffected = insertionCommand.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Message inserted successfully.");

                    }
                    else
                    {
                        Console.WriteLine("No rows were inserted.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while inserting messgae: {ex.Message}");
            }
        }

        public List<Message> GetMessages(string UsernameId)
        {
            List<Message> messages = [];
            string getMessagesQuery =
                """
                SELECT * FROM messages WHERE usersusername = @p1   
                """;

            try
            {
                using (NpgsqlCommand getMessagesCommand = new NpgsqlCommand(getMessagesQuery, _connection))
                {
                    getMessagesCommand.Parameters.Add(new NpgsqlParameter("p1", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = UsernameId });

                    using (NpgsqlDataReader reader = getMessagesCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            messages.Add(new Message
                            {
                                Id = (Guid)reader["id"],
                                UsernameID = (string)reader["usersusername"],
                                ContactId = (Guid)reader["contactsid"],
                                MessageText = reader["message"].ToString(),
                                IsFirstMessage = (bool)reader["isfirstmessage"],
                                IsNativeOriginated = (bool)reader["isnativeoriginated"]
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while retrieving messages: {ex.Message}");
            }
            return messages;
        }
    }
}
