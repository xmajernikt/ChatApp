using Newtonsoft.Json;
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

        public async Task<byte[]> ReadPacket()
        {
            int length = ReadInt32();
            byte[] data = ReadBytes(length);
            return data;
        }

        public async Task<PacketBuilder> DecodePacketAsync(byte[] data)
        {
            try
            {
                return await Task.Run(() => Deserialize(data));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error decoding packet: " + ex.Message);
                return null;
            }
        }

        private static PacketBuilder Deserialize(byte[] data)
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
