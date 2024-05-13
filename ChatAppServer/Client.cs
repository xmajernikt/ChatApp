using ChatAppServer.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatAppServer
{
    internal class Client
    {
        public string? Username { get; set; }
        public Guid Id { get; set; }
        public TcpClient TcpClient { get; set; }
        public string? ImageSrc { get; set; }

        private PacketReader _packetReader;

        
    }
}
