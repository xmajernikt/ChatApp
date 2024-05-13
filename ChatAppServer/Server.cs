using ChatAppServer.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatAppServer
{
    internal class Server(IPAddress address, int port)
    {
        private readonly IPAddress _address = address;
        private readonly int _port = port;
        private TcpListener? Listener { get; set; }
        public static Dictionary<string, TcpClient> UserToSocket = new Dictionary<string, TcpClient>();
        public static object UserToSocketLock = new object();

        public async Task StartServer()
        {
            Listener = new TcpListener(_address, _port);
            Listener.Start();

            try
            {
                while (true)
                {
                    TcpClient client = await Listener.AcceptTcpClientAsync();
                    
                    _ = Task.Run(() => ManageClient(client));
                }
            } catch (Exception ex)
            {
                Console.Out.WriteLine(ex.ToString());
            }
           
           
           
        }

        private async Task ManageClient(TcpClient tcpClient)
        {
            PacketReader packetReader = new(tcpClient.GetStream());
            
            await packetReader.ManageClientAsync(tcpClient);
       
        }
    }
}
