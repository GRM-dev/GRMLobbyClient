using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using ServerJavaConnector.Core.Commands;
using ServerJavaConnector.Core.Connection;
using ServerJavaConnector.XAML.Dialogs;
using ServerJavaConnector.XAML.Pages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ServerJavaConnector.XAML.Pages
{
    public partial class MainPage : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private static MainPage instance;

        public MainPage()
        {
            instance = this;
            InitializeComponent();
            this.ConsoleOutput = ConsoleBoxV;
            this.ConsoleInput = ConsoleInputV;
            this.Conn = MainWindow.instance.Conn;
        }

        private void Send_Button_Click(object sender, RoutedEventArgs e)
        {
            if (!Conn.Connected) { return; }
            try
            {
                String input = ConsoleInput.Text;
                if (!input.StartsWith("!"))
                {
                    input = "!say " + input;
                }
                if (MainWindow.instance.CommandManager.executeCommand(input, Conn))
                {
                    ConsoleInput.Text = "";
                    MainWindow.instance.CommandManager.AddCommandToList(input.Substring(5,input.Length-5));
                    WriteLine("Command executed");
                }
                else
                {
                    WriteLine("Command not executed!");
                }
            }
            catch (Exception ex)
            {
                CDialogManager.ShowExceptionDialog(ex, "While sending");
                Conn.Disconnect();
            }
        }

        private void Close_Button_Click(object sender, RoutedEventArgs e)
        {
            CDialogManager.ShowClosingDialog();
        }

        private void Connect_Button_Click(object sender, RoutedEventArgs e)
        {
            PageManager.instance.changePage(FrameType.MainFrame, PageType.LoginPage);
        }

        private void Disconnect_Button_Click(object sender, RoutedEventArgs e)
        {
            Conn.Disconnect();
        }

        private void Button_Press(object sender, KeyEventArgs e)
        {
            ServerJavaConnector.Core.Commands.CommandManager commandManager = MainWindow.instance.CommandManager;
            if (e.Key == Key.Up)
            {
                Console.WriteLine("Up");
                String input = ConsoleInput.Text;
                if (input == null || input == ""
                        || !commandManager.wasExecuted(input))
                {
                    ConsoleInput.Text = commandManager.getLastCommand();
                }
                else
                {
                    String previousCommand = commandManager.getPreviousCommand(input);
                    if (previousCommand != "")
                    {
                        ConsoleInput.Text = previousCommand;
                    }
                }
            }
            else if (e.Key == Key.Down)
            {
                Console.WriteLine("Down");
                String input = ConsoleInput.Text;
                String nextCommand = commandManager.getNextCommand(input);
                if (nextCommand != ""
                        || input.Equals(commandManager.getLastCommand()))
                {
                    ConsoleInput.Text=nextCommand;
                }
            }
        }

        public void WriteLine(String msg)
        {
            ConsoleOutput.Text += msg + "\n";
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
        }

        public Connection Conn { get; private set; }
        public TextBox ConsoleInput { get; private set; }
        public TextBlock ConsoleOutput { get; private set; }
    }
}