using ChatApp.Core;
using ChatApp.MVVM.Model;
using ChatApp.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace ChatApp.MVVM.ViewModel
{
    internal class LoginViewModel : ObservableObject
    {
        public RelayCommand loginCommand { get; set; }
        public static event EventHandler<User> UserLoggedIn;
        private ServerUtils serverUtils;

        public string username;

        public string Username
        {
            get { return username; }
            set { username = value;
                OnPropertyChanged();
            }
        }

        public string imageSrc;

        public string ImageSrc
        {
            get { return imageSrc; }
            set { imageSrc = value;
                OnPropertyChanged();
            }
        }
        public LoginViewModel() 
        {
            serverUtils = new ServerUtils(new TcpClient());
            serverUtils.ConnectToServer("127.0.0.1", 55400);
            loginCommand = new RelayCommand(o => LoginToApp(username, imageSrc));
        }

        private void LoginToApp(string usrname, string imgSrc)
        {
            Console.WriteLine($"{username}");
            PacketBuilder packet = new PacketBuilder
            {
                Username = usrname,
                Opcode = 9
            };
            ServerUtils._tcpClient.Client.Send(packet.Serialize());
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            MainViewModel.serverUtils = serverUtils;
            UserLoggedIn?.Invoke(this, new User { Username = usrname, ImageSrc = imgSrc, UserId = Guid.NewGuid() });
            Window win = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.Name == "LoginWindowName");
            win.Close();






        }

    }
}
