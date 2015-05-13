using MahApps.Metro.Controls;
using ServerJavaConnector.Core.Connection;
using ServerJavaConnector.XAML.Pages;
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

namespace ServerJavaConnector.Pages
{
	public partial class LoginPage
	{
		public LoginPage()
		{
			this.InitializeComponent();
		}

		private void Login_Click(object sender, System.Windows.RoutedEventArgs e)
		{
            Connection conn = MainWindow.instance.Conn;
            conn.UserData.Name = InputUsernameBox.Text;

            MainWindow mWindow=(MainWindow)Application.Current.MainWindow;
            var flyout = mWindow.Flyouts.Items[1] as Flyout;
            flyout.IsOpen = true;

            conn.Connect();
            if (conn.Connected)
            {
                ((MainPage)PageManager.instance.getPage(PageType.MainPage)).WriteLine("Connected with server on port: " + conn.Port);
                Back_Click(null,null);
            }
            else
            {
                ((MainPage)PageManager.instance.getPage(PageType.MainPage)).WriteLine("There were problems while connecting");
            }
		}

		private void Register_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			// TODO: Add event handler implementation here.
		}

		private void Back_Click(object sender, System.Windows.RoutedEventArgs e)
		{
            PageManager.instance.getFrame(this).goBack();
		}
	}
}