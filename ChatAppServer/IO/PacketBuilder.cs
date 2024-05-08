using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatAppServer.IO
{
    internal class PacketBuilder
    {
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
    }
}
