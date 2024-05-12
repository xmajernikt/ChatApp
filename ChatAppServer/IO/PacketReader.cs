using ChatAppServer.Database;
using ChatAppServer.Database.Models;
using ChatAppServer.Database.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ChatAppServer.IO
{
    internal class PacketReader : BinaryReader
    {
        private NetworkStream _stream;
        private readonly PacketBuilder _packetBuilder = new PacketBuilder();
        
        public PacketReader(NetworkStream input) : base(input)
        {

            _stream = input;
        }

        private byte[] DecodePacket()
        {
            byte[] lengthBuffer = new byte[4];
            _stream.Read(lengthBuffer, 0, 4); // Read the length of the packet

            int packetLength = BitConverter.ToInt32(lengthBuffer, 0);

            byte[] packetData = new byte[packetLength];
            _stream.Read(packetData, 0, packetLength); // Read the entire packet

            return packetData;
        }

        public async Task ManageClientAsync(TcpClient tcpClient)
        {
            try
            {
                UsersRepository usersRepository = new UsersRepository(DbContext.Connection);
                ContactRepository contactRepository = new ContactRepository(DbContext.Connection);

                while (true)
                {
                    _stream = tcpClient.GetStream();
                    //var lenght = stream;
                    byte[] data = DecodePacket();
                   
                    string receivedData = Encoding.UTF8.GetString(data);
                    PacketBuilder? deserializedPacket = Deserialize(data);
                    if (deserializedPacket != null)
                    {
                        switch (deserializedPacket.Opcode)
                        {
                            case 0:
                                Console.WriteLine($"[{DateTime.Now}] Client {deserializedPacket.Username} has connected");

                                usersRepository.InsertUser(new Database.Models.User
                                {
                                    Username = deserializedPacket.Username,
                                    ImageSrc = (string)deserializedPacket.AdditionalData,
                                    CreatedAt = DateTime.UtcNow,
                                });

                                List<Contact> contacts = contactRepository.GetContacts(deserializedPacket.Username);
                                PacketBuilder contactsPacket = new PacketBuilder
                                {
                                    Username = deserializedPacket.Username,
                                    Opcode = 0,
                                    SenderUsername = "server",
                                    AdditionalData = contacts
                                };
                                tcpClient.Client.Send(contactsPacket.Serialize());
                                break;
                            case 1:
                                Console.WriteLine($"Information about {deserializedPacket.Username} has been requested");

                                User? user = usersRepository.GetUser(deserializedPacket.Username);
                                Contact contact = new Contact
                                    {
                                    Id = Guid.NewGuid(),
                                    Username = user.Username, 
                                    UsernameId = deserializedPacket.SenderUsername, 
                                    ImageSrc = "C:\\Users\\admin\\source\\repos\\ChatApp\\ChatApp\\Icons\\no_profile.png", 
                                    LastMessage = "", 
                                    CreatedAt = DateTime.UtcNow };
                                contactRepository.AddContact(contact);

                                PacketBuilder packet = new PacketBuilder
                                {
                                    Opcode = 1,
                                    Username = user.Username,
                                    AdditionalData = user.ImageSrc
                                };

                                byte[] packetData = packet.Serialize();
                                tcpClient.Client.Send(packetData);
                                break;
                            case 3:
                                break;
                        }
                    }
                       

                }
                
                


            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error handling client: {ex.Message}");
            }
            finally
            {
                tcpClient.Close();

            }
        }
    

        private static PacketBuilder? Deserialize(byte[] data)
        {
            using (MemoryStream stream = new MemoryStream(data))
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            using (JsonTextReader jsonReader = new JsonTextReader(reader))
            {
                JsonSerializer serializer = new JsonSerializer();
                return serializer.Deserialize<PacketBuilder>(jsonReader);
            }
        }
    }
}
