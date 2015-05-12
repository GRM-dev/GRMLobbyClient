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

			// Insert code required on object creation below this point.
		}

		private void Login_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			// TODO: Add event handler implementation here.
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