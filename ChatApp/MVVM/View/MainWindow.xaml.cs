using ChatApp.Core;
using ChatApp.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ChatApp.MVVM.Model;

namespace ChatApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void Minimize_Button_Click(object sender, RoutedEventArgs e)
        {
            // Assuming you have a reference to the window you want to minimize
            // Replace WindowName with the actual name of the window class
            Window win = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.Name == "MainWindowName");


            if (win != null)
            {
                win.WindowState = WindowState.Minimized;
            }
        }

        private void Maximize_Button_Click(object sender, RoutedEventArgs e)
        {
            Window win = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.Name == "MainWindowName");
            if (win != null)
            {
                if (win.WindowState != WindowState.Maximized)
                {
                    win.WindowState = WindowState.Maximized;
                }
                else
                {
                    win.WindowState = WindowState.Normal;
                }
            }
            
        }

        private void Close_Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void AddContactButton_Click(object sender, RoutedEventArgs e)
        {
            
            AddContactWindow addContactWindow = new AddContactWindow(MainViewModel.serverUtils);
            if (addContactWindow.ShowDialog() == true)
            {
               
            }
            else
            {
                // User clicked "Cancel" or closed the window
            }
        }

        private MainWindow GetWindowFromResourceDictionary(string windowName)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType().Name == windowName)
                {
                    return (MainWindow)window;
                }
            }
            return null;
        }
    }
}
