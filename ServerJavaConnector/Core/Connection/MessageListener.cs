using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerJavaConnector.Core.Connection
{
    public class MessageListener
    {
        private Thread listenerThread;
        private Boolean listening;

        public MessageListener(Connection conn)
        {
            this.Conn = conn;
        }

        public void run()
        {
            MainWindow MWindow = MainWindow.instance;
            while (MWindow.Conn.Connected && listening)
            {
                String msg = PacketParser.receiveMessage(Conn.ClientSocket);
                if (!msg.Equals(""))
                {
                    if (msg.Contains("!userdata"))
                    {
                        PacketParser.sendUserData(Conn.UserData, Conn.ClientSocket);
                    }
                    MWindow.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        //FIXME: MWindow.MPage.ConsoleOutput.Text += msg + "\n";
                    }));
                }
                try
                {
                    Thread.Sleep(1000);
                }
                catch (ThreadInterruptedException e)
                {
                    Console.Out.WriteLine(e.Message);
                }
            }
        }

        public void startListening()
        {
            if (listenerThread == null && !listening)
            {
                listening = true;
                listenerThread = new Thread(() => run());
                listenerThread.Start();
            }
        }

        public void stopListening()
        {
            if (listenerThread != null && listening)
            {
                listening = false;
                listenerThread.Abort();
                listenerThread = null;
            }
        }

        public Connection Conn { get; private set; }
    }
}
