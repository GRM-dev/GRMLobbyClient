using GRMLobbyClient.Core.Connection;
using GRMLobbyClient.XAML.Dialogs;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace GRMLobbyClient.XAML.Pages
{
    public partial class MainPage : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private static MainPage instance;

        public MainPage()
        {
            instance = this;
            InitializeComponent();
            this.Conn = MainWindow.instance.Conn;
        }



        private void Close_Button_Click(object sender, RoutedEventArgs e)
        {
            CDialogManager.ShowClosingDialog();
        }

        private void Connect_Button_Click(object sender, RoutedEventArgs e)
        {
            PageManager.Instance.ChangePage(FrameType.MainFrame, PageType.LoginPage);
        }

        private void Disconnect_Button_Click(object sender, RoutedEventArgs e)
        {
            Conn.Disconnect();
        }

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
            Resources["Connected"] = Conn.Connected;
            Resources["NegConnected"] = !Conn.Connected;
        }

        public static void OnPropertySChanged(string p)
        {
            if (instance != null)
            {
                instance.OnPropertyChanged(p);
            }
            ChatPage.OnPropertySChanged(p);
        }

        public Connection Conn { get; private set; }
    }
}