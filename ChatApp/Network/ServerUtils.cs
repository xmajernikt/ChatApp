using ChatApp.MVVM.Model;
using ChatApp.MVVM.ViewModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

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

        public void ConnectToServer(string host, int port)
        {
            if (!_tcpClient.Connected)
            {
                _tcpClient.Connect(host, port);
                _ = Task.Run(() => ReceivePackets());
            }
        }

        public void SendInitialPacket(string Username)
        {
            PacketBuilder packet = new PacketBuilder { Opcode = 0, Username = Username, AdditionalData = "" };
            _tcpClient.Client.Send(packet.Serialize());
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

        public static void AddContact(string contactId, string senderUsername)
        {
            if (_tcpClient.Connected)
            {
                PacketBuilder packet = new PacketBuilder { Opcode = 1, Username = contactId, SenderUsername = senderUsername, AdditionalData = "" };
                _tcpClient.Client.Send(packet.Serialize());
            }
        }

        private async Task ReceivePackets()
        {
            PacketReader packetReader = new PacketReader(_tcpClient.GetStream());

            try
            {
                while (true)
                {
                    byte[] packetData = await packetReader.ReadPacket();
                    PacketBuilder packet = await packetReader.DecodePacketAsync(packetData);
                    if (packet != null)
                    {
                        switch (packet.Opcode)
                        {
                            case 0:
                                Console.Out.WriteLine("Received contacts");
                                JArray additionalData = (JArray)packet.AdditionalData;

                                List<ContactModel> receivedContacts = new List<ContactModel>();
                                foreach (var item in additionalData)
                                {
                                    // Convert the JToken to a string and deserialize it into a ContactModel
                                    ContactModel contact = item.ToObject<ContactModel>();
                                    receivedContacts.Add(contact);
                                }

                                // Update MainViewModel.Contacts with the received contacts
                                MainViewModel.UpdateContacts(receivedContacts);
                                break;


                            case 1:
                                receivedContacts = new List<ContactModel>
                                {
                                    new ContactModel
                                    {
                                        Username = packet.Username,
                                        ImageSrc = "C:\\Users\\admin\\source\\repos\\ChatApp\\ChatApp\\Icons\\no_profile.png",
                                        Messages = new ObservableCollection<MessageModel>()
                                    }
                                };

                                MainViewModel.UpdateContacts(receivedContacts);
                                
                                break;
                            case 9:

                                break;
                        }
                        // Process the received packet
                        Console.WriteLine("Received packet: " + packet.Username);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error receiving packets: " + ex.Message);
            }
        }

    }
}
