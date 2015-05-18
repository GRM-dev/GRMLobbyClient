using ServerJavaConnector.XAML.Dialogs;
using System;
using System.IO;
using System.Threading;

namespace ServerJavaConnector.Core.Connection
{
    public class WebMessageListener
    {
        private Thread listenerThread;
        private Boolean listening;

        public WebMessageListener(Connection conn)
        {
            this.Conn = conn;
        }

        public void run()
        {
            MainWindow MWindow = MainWindow.instance;
            try
            {
                while (MWindow.Conn.Connected && listening)
                {
                    String msg = PacketParser.receivePacket(Conn.ClientSocket);
                    if (!msg.Equals(""))
                    {
                        if (MWindow.CommandManager.executeCommand(msg, Conn, true))
                        {
                            Console.WriteLine("ServerSide Command executed: "+msg);
                        }
                        else
                        {
                            Console.WriteLine("ServerSide Command not executed: "+msg);
                        }
                    }
                    try
                    {
                        Thread.Sleep(1000);
                    }
                    catch (ThreadInterruptedException ex)
                    {
                        CDialogManager.ShowExceptionDialog(ex, null);
                    }
                }
            }
            catch (IOException e)
            {
                CDialogManager.ShowExceptionDialog(e, null);
                Conn.Disconnect();
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
