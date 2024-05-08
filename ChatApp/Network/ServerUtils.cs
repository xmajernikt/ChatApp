using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Network
{
    public class ServerUtils
    {
        public static TcpClient _tcpClient;
        private static PacketBuilder _packetBuilder;
        private byte[] _receiveBuffer = new byte[1024];
        public ServerUtils(TcpClient tcpClient)
        {
            _packetBuilder = new PacketBuilder();
            _tcpClient = tcpClient;
        }

        public void ConnectToServer(string host, int port, string Username)
        {
            if (!_tcpClient.Connected)
            {
                _tcpClient.Connect(host, port);
                _tcpClient.Client.Send(_packetBuilder.CreatePacket(0, Username));
                //_tcpClient.GetStream().Flush();
                _ = Task.Run(() => ReceivePackets());
            }
        }

        private void ReceiveCallback(IAsyncResult result)
        {
            try
            {
                int bytesRead = _tcpClient.GetStream().EndRead(result);
                if (bytesRead > 0)
                {
                    string response = Encoding.UTF8.GetString(_receiveBuffer, 0, bytesRead);
                    Console.WriteLine("Received response: " + response);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error receiving data: " + ex.Message);
            }
        }

        public static void AddContact(string contactId)
        {
            if (_tcpClient.Connected)
            {
                _tcpClient.Client.Send(_packetBuilder.CreatePacket(1, contactId));
            }
        }

        private async Task ReceivePackets()
        {
            
            await Console.Out.WriteLineAsync("KURVCA");
            PacketReader packetReader = new PacketReader(_tcpClient.GetStream());

            while (true)
            {
                await packetReader.ReadPacket(_tcpClient);
            }
        }

    }
}
