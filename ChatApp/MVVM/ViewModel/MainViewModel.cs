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
        public ObservableCollection<MessageModel> Messages { get; set; }
        public static User user { get; set; }
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
            

            Messages = new ObservableCollection<MessageModel>
            {
                new MessageModel
                {
                    Message = $"Test -1",
                    Username = $"TEST -1",
                    UsernameColor = "White",
                    ImageSrc = "C:\\Obrázky\\Snímky obrazovky\\cv_photo.png",
                    IsFirstMessage = true,
                    IsNativeOrigined = false,
                    Time = DateTime.Now
                }
            };

            SendCommand = new RelayCommand(o =>
            {
                Messages.Add(new MessageModel
                {
                    Message = Message,
                    IsFirstMessage = false          
                });
                Message = "";
            });
            for (int i = 0; i < 10; i++)
            {
                Messages.Add(new MessageModel
                {
                     Message = $"Test {i}", Username = $"TEST {i}", UsernameColor = "White", IsFirstMessage = false, IsNativeOrigined = false, Time = DateTime.Now
                });
            }

           
            
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
