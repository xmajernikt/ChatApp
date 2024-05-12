using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatAppServer.IO
{
    internal class PacketBuilder
    {

        public byte Opcode { get; set; }
        public string? Username { get; set; }
        public object AdditionalData { get; set; }
        public string? SenderUsername { get; set; }

        public byte[] CreatePacket(byte opcode, string data)
        {
            UnicodeEncoding unicodeEncoding = new UnicodeEncoding();
            byte[] firstString = unicodeEncoding.GetBytes(data);
            using (MemoryStream memoryStream = new MemoryStream())
            {
                memoryStream.WriteByte(opcode);
                memoryStream.Write(BitConverter.GetBytes(firstString.Length), 0, BitConverter.GetBytes(firstString.Length).Length);

                memoryStream.Write(firstString, 0, unicodeEncoding.GetBytes(data).Length);
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

       
    }
}
