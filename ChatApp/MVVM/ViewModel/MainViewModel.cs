using ChatApp.Core;
using ChatApp.MVVM.Model;
using ChatApp.Network;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ChatApp.MVVM.ViewModel
{
    internal class MainViewModel : ObservableObject
    {
        public static ObservableCollection<ContactModel> _contacts;
        public ObservableCollection<ContactModel> Contacts
        {
            get { return _contacts; }
            set
            {
                _contacts = value;
                OnPropertyChanged(nameof(Contacts));
            }
        }
        public static User user { get; set; }

        private static ObservableCollection<MessageModel> _messages;
        public ObservableCollection<MessageModel> Messages
        {
            get { return _messages; }
            set
            {
                _messages = value;
                OnPropertyChanged(nameof(Messages));
            }
        }
        public static event EventHandler<User> UserLoggedIn;

        private ContactModel _selecteContact;

        public ContactModel SelectedContact
        {
            get { return _selecteContact; }
            set { _selecteContact = value;
                OnPropertyChanged();
            }
        }

        private string username;

        public string Username
        {
            get { return username; }
            set
            {
                username = value;
                OnPropertyChanged();
            }
        }

        private string imageSrc;

        public string ImageSrc
        {
            get { return imageSrc; }
            set
            {
                imageSrc = value;
                OnPropertyChanged();
            }
        }


        public RelayCommand SendCommand { get; set; }

        private string _message;

        public string Message
        {
            get { return _message; }
            set 
            { 
                _message = value;
                OnPropertyChanged();
            }
        }

        public static ServerUtils serverUtils { get; set; }

        private void OnUserLoggedIn(object sender, User e)
        {
            // This method will be called when the UserLoggedIn event is raised
            Console.WriteLine($"{e.Username}");
            user = new User { Username = e.Username, ImageSrc = e.ImageSrc };
            //serverUtils = new ServerUtils(new TcpClient());
            //serverUtils.ConnectToServer("127.0.0.1", 55400, e.Username);
            serverUtils.SendInitialPacket(e.Username);
        }

        public class UserLoggedInEventArgs
        {
            public string Username { get; set; }
            public string ImageSrc { get; set; }
            public Guid UserId { get; set; }
        }

        public MainViewModel() 
        {
            LoginViewModel.UserLoggedIn += OnUserLoggedIn;

            
            Contacts = new ObservableCollection<ContactModel>();


            Messages = new ObservableCollection<MessageModel>();
            

            SendCommand = new RelayCommand(o =>
            {
                //Messages.Add(new MessageModel
                //{
                //    Message = Message,
                //    IsFirstMessage = false          
                //});
                GetContact(SelectedContact.Username, Message, user.Username);
                serverUtils.SendMessageToContact(SelectedContact.Username, user.Username, Message);
                Message = "";
            });
            

           
            
        }

        public static void GetContact(string Username, string Message, string NativeSender = null)
        {
            string senderUsername = NativeSender is null ? Username : NativeSender;
            Application.Current.Dispatcher.Invoke(() =>
            {
                foreach (var contact in _contacts)
                {
                    if (contact.Username == Username)
                    {
                        contact.Messages.Add(new MessageModel
                        {
                            Message = Message,
                            Username = senderUsername,
                            Time = DateTime.Now,
                            IsFirstMessage = true,
                            IsNativeOrigined = false,
                            ImageSrc = "C:\\Users\\admin\\source\\repos\\ChatApp\\ChatApp\\Icons\\no_profile.png",


                        });
                        break;
                    }
                }
            });
            
        }

        public static void UpdateContacts(IEnumerable<ContactModel> receivedContacts)
        {

            Application.Current.Dispatcher.Invoke(() =>
            {
                foreach (var contact in receivedContacts)
                {
                    _contacts.Add(contact); // Add each received contact
                }
            });
        }

        public static void GetAllMessges(IEnumerable<MessageModel> receivedMessages)
        {

            Application.Current.Dispatcher.Invoke(() =>
            {
                foreach (var message in receivedMessages)
                {
                    _messages.Add(message); // Add each received contact
                }

                
            });
        }


    }

    public static class BindableProperty
    {
        public static readonly DependencyProperty VariableProperty =
            DependencyProperty.RegisterAttached(
                "Variable",
                typeof(string),
                typeof(BindableProperty),
                new PropertyMetadata(null));

        public static string GetVariable(DependencyObject obj)
        {
            return (string)obj.GetValue(VariableProperty);
        }

        public static void SetVariable(DependencyObject obj, string value)
        {
            obj.SetValue(VariableProperty, value);
        }

        public static readonly DependencyProperty ImageSrcProperty =
            DependencyProperty.RegisterAttached(
                "ImageSrc",
                typeof(string),
                typeof(BindableProperty),
                new PropertyMetadata(null));

        public static string GetImageSrc(DependencyObject obj)
        {
            return (string)obj.GetValue(ImageSrcProperty);
        }

        public static void SetImageSrc(DependencyObject obj, string value)
        {
            obj.SetValue(ImageSrcProperty, value);
            
        }

        
    }


}
