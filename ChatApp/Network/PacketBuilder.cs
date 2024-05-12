using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Network
{
    internal class PacketBuilder
    {
        public byte Opcode {  get; set; }
        public string Username { get; set; }
        public string SenderUsername { get; set; }
        public object AdditionalData { get; set; }
        
        public byte[] CreatePacket(byte opcode, string username, string additionalData)
        {
            UnicodeEncoding unicodeEncoding = new UnicodeEncoding();
            byte[] firstString = unicodeEncoding.GetBytes(username);
            using (MemoryStream memoryStream = new MemoryStream())
            {
                memoryStream.WriteByte(opcode);
                memoryStream.Write(BitConverter.GetBytes(firstString.Length), 0, BitConverter.GetBytes(firstString.Length).Length);

                memoryStream.Write(firstString, 0, unicodeEncoding.GetBytes(username).Length);
                return memoryStream.ToArray();
            }
        }

        public byte[] Serialize()
        {
            string json = JsonConvert.SerializeObject(this);
            byte[] jsonData = Encoding.UTF8.GetBytes(json);

            // Prepend the length of the packet to the data
            byte[] lengthBytes = BitConverter.GetBytes(jsonData.Length);
            byte[] packetData = new byte[lengthBytes.Length + jsonData.Length];
            lengthBytes.CopyTo(packetData, 0);
            jsonData.CopyTo(packetData, lengthBytes.Length);

            return packetData;
        }

        public static PacketBuilder Deserialize(byte[] data)
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
