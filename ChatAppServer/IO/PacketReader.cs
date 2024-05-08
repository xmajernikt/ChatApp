using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ChatAppServer.IO
{
    internal class PacketReader : BinaryReader
    {
        private readonly NetworkStream _stream;
        private readonly PacketBuilder _packetBuilder = new PacketBuilder();
        
        public PacketReader(NetworkStream input) : base(input)
        {

            _stream = input;
        }

        public string DecodePacket()
        {
            UnicodeEncoding unicode = new UnicodeEncoding();
            byte[] bytes;
            var lenght = ReadInt32();
            bytes = new byte[lenght];
            Console.WriteLine(_stream);
            _stream.Read(bytes, 0, lenght);
            string msg = unicode.GetString(bytes);
            return msg;
        }

        public async Task ManageClientAsync(TcpClient tcpClient, List<Client> clients)
        {
            try
            {
                NetworkStream stream = tcpClient.GetStream();
                var lenght = ReadInt32();
                byte[] data = new byte[lenght + 1];
                int bytesRead;
                while ((bytesRead = await stream.ReadAsync(data.AsMemory(0, lenght))) > 0)
                {
                    string receivedData = Encoding.UTF8.GetString(data);
                    await Console.Out.WriteLineAsync($"Received data: {receivedData}");
                    Client? client = GetClient(clients, receivedData);

                    if (client != null)
                    {
                        tcpClient.Client.Send(_packetBuilder.CreatePacket(2, GetClientInfo(client)));

                    }
                    else
                    {
                        Console.WriteLine("Client does not exits");
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

        private string GetClientInfo(Client client)
        {
            var clientInfo = new
            {
                client.Username,
                client.ImageSrc,
                client.Id,
            };

            return JsonSerializer.Serialize(clientInfo);
        }

        private Client? GetClient(List<Client> clients ,string Username) 
        {
            foreach (Client client in clients)
            {
                if (client.Username == Username)
                {
                    return client;
                }
            }
            return null;
        }
    }
}
