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

        public void SendMessageToContact(string UsernameId, string Username, string message) 
        {
            if (_tcpClient.Connected)
            {
                PacketBuilder packet = new PacketBuilder
                {
                    Opcode = 2,
                    Username = UsernameId,
                    SenderUsername = Username,
                    AdditionalData = message
                };
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
                                JObject additionalData = (JObject)packet.AdditionalData;

                                // Check if the AdditionalData contains a 'Contacts' property
                                if (additionalData.TryGetValue("Contacts", out JToken contactsToken))
                                {
                                    // Extract the 'Contacts' property and deserialize it into a list of ContactModel
                                    List<ContactModel> receivedContacts = contactsToken.ToObject<List<ContactModel>>();
                                    foreach (var contact in receivedContacts)
                                    {
                                        contact.Messages = new ObservableCollection<MessageModel>();
                                    }
                                    MainViewModel.UpdateContacts(receivedContacts);
                                }

                                // Check if the AdditionalData contains a 'Messages' property
                                if (additionalData.TryGetValue("Message", out JToken messagesToken))
                                {
                                    Dictionary<Guid, List<string>> contactToMessages = new Dictionary<Guid, List<string>>();

                                    // Extract the 'Messages' property and deserialize it into a list of MessageModel
                                    List<MessageModel> messages = messagesToken.ToObject<List<MessageModel>>();
                                    foreach (var message in messages)
                                    {
                                        contactToMessages[(Guid)message.ContactId].Add(message.Message); 
                                    }
                                    MainViewModel.GetAllMessges(messages);
                                    // Do something with the messages, such as updating the UI or storing them
                                    // MainViewModel.HandleMessages(messages);
                                }

                                //foreach (var item in additionalData)
                                //{
                                //    // Convert the JToken to a string and deserialize it into a ContactModel

                                //    ContactModel contact = item.ToObject<ContactModel>();
                                //    contact.Messages = new ObservableCollection<MessageModel>();
                                //    receivedContacts.Add(contact);
                                //}

                                // Update MainViewModel.Contacts with the received contacts
                                //MainViewModel.UpdateContacts(receivedContacts);
                                //MainViewModel.GetAllMessges()
                                break;


                            case 1:
                                List<ContactModel> receivedContact = new List<ContactModel>
                                {
                                    new ContactModel
                                    {
                                        Username = packet.Username,
                                        ImageSrc = "C:\\Users\\admin\\source\\repos\\ChatApp\\ChatApp\\Icons\\no_profile.png",
                                        Messages = new ObservableCollection<MessageModel>()
                                    }
                                };

                                MainViewModel.UpdateContacts(receivedContact);
                                
                                break;
                            
                            case 2:
                                MainViewModel.GetContact(packet.SenderUsername, (string)packet.AdditionalData);
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
