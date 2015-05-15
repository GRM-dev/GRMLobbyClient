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
                    WriteLine(input);
                    input = "!say " + input;
                }
                else
                {
                    WriteLine(input);
                }
                var CM = MainWindow.instance.CommandManager;
                if (CM.executeCommand(input, Conn))
                {
                    ConsoleInput.Text = "";
                    Commands cmd = Commands.getCommand(input);
                    if (cmd != Commands.NONE && cmd != Commands.ERROR)
                    {
                        int len;
                        if (cmd != Commands.SAY)
                        {
                            len = 0;
                        }
                        else
                        {
                            len = cmd.getCommandString().Length;
                        }
                        CM.AddCommandToList(input.Substring(len, input.Length - len));
                        WriteLine("Command executed");
                    }
                    else
                    {
                        WriteLine("Command executed but there was something wrong", Brushes.Yellow);
                    }
                }
                else
                {
                    WriteLine("Command not executed!", Brushes.Red);
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
                String input = ConsoleInput.Text;
                String nextCommand = commandManager.getNextCommand(input);
                if (nextCommand != ""
                        || input.Equals(commandManager.getLastCommand()))
                {
                    ConsoleInput.Text = nextCommand;
                }
            }
        }

        public void WriteLine(String msg)
        {
            WriteLine(msg, Brushes.LightGreen);
        }
        public void WriteLine(String msg, Brush color)
        {
            if (color == null)
            {
                color = new SolidColorBrush(Colors.LightGreen);
            }
            TextRange tr = new TextRange(ConsoleBoxV.Document.ContentEnd, ConsoleBoxV.Document.ContentEnd);
            tr.Text = msg;
            tr.ApplyPropertyValue(TextElement.ForegroundProperty, color);
            ConsoleBoxV.AppendText("\n");
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
        public RichTextBox ConsoleOutput { get; private set; }
    }
}