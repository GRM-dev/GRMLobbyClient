using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerJavaConnector.Core
{
    class MessageListener
    {
        private Thread listenerThread;

        public MessageListener(Connection conn)
        {
            this.conn = conn;
            listenerThread = new Thread(() =>
                {
                    MainWindow mwindow = MainWindow.instance;
                    while (mwindow.Conn.Connected)
                    {
                        String msg = conn.receivePacket();
                        if (!msg.Equals(""))
                        {
                            mwindow.Dispatcher.BeginInvoke(new Action(() =>
                            {
                                   mwindow.ConsoleOutput.Text += msg+"\n";
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
                });
        }

        public void startListening()
        {
            if (listenerThread != null)
            {
                listenerThread.Start();
            }
        }

        public void stopListening()
        {
            if (listenerThread != null)
            {
                listenerThread.Interrupt();
            }
        }

        public Connection conn { get; private set; }
    }
}
