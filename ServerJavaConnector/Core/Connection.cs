using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerJavaConnector.Core
{
    public class Connection
    {
        private static String serverIP = "91.230.204.135";
        private static String localIP = "127.0.0.1";
        private static int MIN_PORT = 4343;
        private static int MAX_PORT = 4360;
        private Socket clientSocket;
        private Boolean _connected = false;
        private MessageListener listener;
        private IPEndPoint serverAddress;

        public Connection()
        {
            listener = new MessageListener(this);
        }

        public void Connect()
        {
            for (Port = MIN_PORT; Port < MAX_PORT && !Connected; Port++)
            {
                serverAddress = new IPEndPoint(IPAddress.Parse(localIP), Port);
                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                try
                {
                    clientSocket.Connect(serverAddress);
                    Connected = true;
                    listener.startListening();
                }
                catch (SocketException ex)
                {
                    Console.Out.WriteLine(Port + " not found");
                    Connected = false;
                }
            }
        }

        public string receivePacket()
        {
            byte[] rcvLenBytes = new byte[4];
            clientSocket.Receive(rcvLenBytes);
            int rcvLen = System.BitConverter.ToInt32(rcvLenBytes, 0);
            byte[] rcvBytes = new byte[rcvLen];
            clientSocket.Receive(rcvBytes);
            String rcv = System.Text.Encoding.ASCII.GetString(rcvBytes);
            return rcv;
        }

        public void sendPacket(String msg)
        {
            int msgLen = System.Text.Encoding.ASCII.GetByteCount(msg);
            byte[] msgBytes = System.Text.Encoding.ASCII.GetBytes(msg);
            byte[] msgLenBytes = System.BitConverter.GetBytes(msgLen);
            clientSocket.Send(msgLenBytes);
            clientSocket.Send(msgBytes);
        }

        public bool Connected
        {
            get { return _connected; }
            set
            {
                _connected = value;
                MainWindow.OnPropertySChanged("Connected");
            }
        }

        public int Port { get; private set; }

        public void Disconnect()
        {
            if (clientSocket != null)
            {
                listener.stopListening();
                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();
            }
            Connected = false;
            Thread.Sleep(100);
        }
    }
}
