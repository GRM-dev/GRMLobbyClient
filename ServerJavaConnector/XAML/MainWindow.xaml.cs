using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using ServerJavaConnector.Core;
using ServerJavaConnector.Core.Connection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace ServerJavaConnector
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public static MainWindow instance;

        public MainWindow()
        {
            this.Conn = new Connection();
            instance = this;
            InitializeComponent();
            this.ConsoleOutput = ConsoleBoxV;
            this.ConsoleInput = ConsoleInputV;
        }

        private void Send_Button_Click(object sender, RoutedEventArgs e)
        {
            if (!Conn.Connected) { return; }
            try
            {
                PacketParser.sendPacket(ConsoleInput.Text,Conn.ClientSocket);
                ConsoleInput.Text = "";
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine(ex.ToString());
                Conn.Connected = false;
            }
        }

        private void Close_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            Application.Current.Shutdown();
        }

        private async void Connect_Button_Click(object sender, RoutedEventArgs e)
        {
            String name= await DialogManager.ShowInputAsync(this, "Podaj swoje dane", "imie");
            ConsoleOutput.Text += "Hello " + name + "\n";
            Conn.Connect();
            if (Conn.Connected)
            {
                ConsoleOutput.Text += "Connected with server on port: " + Conn.Port + "\n";
            }
            else
            {
                ConsoleOutput.Text += "There were problems while connecting\n";
            }
        }
        
        private void Disconnect_Button_Click(object sender, RoutedEventArgs e)
        {
            Conn.Disconnect();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
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

        internal static void OnPropertySChanged(string p)
        {
            if (instance != null)
            {
                instance.OnPropertyChanged(p);
            }
        }

        public Connection Conn { get; set; }
        public TextBox ConsoleInput {get; set; }
        public TextBlock ConsoleOutput { get; set; }
    }
}
