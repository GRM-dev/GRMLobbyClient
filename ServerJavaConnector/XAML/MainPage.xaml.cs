using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using ServerJavaConnector.Core.Connection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ServerJavaConnector
{
	public partial class MainPage:Page
	{
		public MainPage(Connection conn)
		{
			InitializeComponent();
            this.ConsoleOutput = ConsoleBoxV;
            this.ConsoleInput = ConsoleInputV;
            this.Conn = conn;
		}

        private void Send_Button_Click(object sender, RoutedEventArgs e)
        {
            if (!Conn.Connected) { return; }
            try
            {
                PacketParser.sendMessage(ConsoleInput.Text, Conn.ClientSocket);
                ConsoleInput.Text = "";
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine(ex.ToString());
                Conn.Disconnect();
            }
        }

        private void Close_Button_Click(object sender, RoutedEventArgs e)
        {
            Conn.Disconnect();
            Application.Current.Shutdown();
        }

        private async void Connect_Button_Click(object sender, RoutedEventArgs e)
        {
           /* MainWindow mWindow=(MainWindow)Application.Current.MainWindow;
            var flyout = mWindow.Flyouts.Items[0] as Flyout;
            flyout.IsOpen = true;*/
            
            String name = await DialogManager.ShowInputAsync((MainWindow)Application.Current.MainWindow, "Give your personal data", "Name");
            ConsoleOutput.Text += "Hello " + name + "\n";
            Conn.UserData.Name = name;
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

        public Connection Conn { get;private set; }
        public TextBox ConsoleInput { get; private set; }
        public TextBlock ConsoleOutput { get; private set; }
	}
}