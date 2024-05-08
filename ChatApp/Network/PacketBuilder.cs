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
        public byte[] CreatePacket(byte opcode, string username)
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
    }
}
