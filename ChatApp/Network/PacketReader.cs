using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Network
{
    internal class PacketReader : BinaryReader
    {
        private NetworkStream _stream;
        public PacketReader(NetworkStream stream) : base(stream) 
        {
            _stream = stream;
        }

        public async Task ReadPacket(TcpClient tcpClient)
        {
            NetworkStream networkStream = tcpClient.GetStream();
            var lenght = ReadInt32();
            byte[] data = new byte[lenght];
            int bytesRead;

            while ((bytesRead = await networkStream.ReadAsync(data, 0, lenght)) > 0)
            {
                string receivedData = Encoding.UTF8.GetString(data);
                await Console.Out.WriteLineAsync(receivedData);
            }
        }
    }
}
