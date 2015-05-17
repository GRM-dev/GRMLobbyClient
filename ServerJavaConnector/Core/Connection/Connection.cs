using ServerJavaConnector.Core.Commands;
using ServerJavaConnector.XAML;
using ServerJavaConnector.XAML.Dialogs;
using ServerJavaConnector.XAML.Pages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerJavaConnector.Core.Connection
{
    public class Connection
    {
        private static String serverIP = "91.230.204.135";
        private static String localIP = "127.0.0.1";
        private static int EST_PORT = 4342;
        private Socket _clientSocket;
        private Boolean _connected = false;
        private WebMessageListener listener;
        private IPEndPoint serverAddress;
        private User _user;

        public Connection()
        {
            listener = new WebMessageListener(this);
            UserData = new User();
        }

        public void Connect()
        {
            int Port = EST_PORT;
            serverAddress = new IPEndPoint(IPAddress.Parse(localIP), Port);
            _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                _clientSocket.Connect(serverAddress);
                Connected = true;
                listener.startListening();
            }
            catch (SocketException ex)
            {
                CDialogManager.ShowExceptionDialog(ex, "Connection on address " + serverAddress.Address.ToString() + ":" + Port + " not found.");
                Connected = false;
            }
        }

        public void Disconnect()
        {
            if (_clientSocket != null)
            {
                listener.stopListening();

                if (_clientSocket.Connected == true)
                {
                    MainWindow.instance.CommandManager.executeCommand(Command.CLOSECONN, this);
                    _clientSocket.Shutdown(SocketShutdown.Both);
                }
                _clientSocket.Close();
                _clientSocket = null;
            }
            Connected = false;
            Thread.Sleep(100);
        }

        public bool Connected
        {
            get { return _connected; }
            set
            {
                _connected = value;
                MainPage.OnPropertySChanged("Connected");
            }
        }

        public Socket ClientSocket
        {
            get { return _clientSocket; }
            private set { _clientSocket = value; }
        }

        public int Port { get; private set; }

        public User UserData
        {
            get { return _user; }
            private set { _user = value; }
        }
    }
}