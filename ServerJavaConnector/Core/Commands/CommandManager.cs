using ServerJavaConnector.Pages;
using ServerJavaConnector.XAML.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServerJavaConnector.Core;
using ServerJavaConnector.Core.Connection;

namespace ServerJavaConnector.Core.Commands
{
    public class CommandManager
    {
        private MainWindow mainWindow;

        public CommandManager(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            Commands.Init();
        }

        public Boolean executeCommand(String command, Connection.Connection conn=null)
        {
            Console.WriteLine("...............");
            Commands comm = Commands.getCommand(command);
            String args = Commands.getOffset(command);
            return executeCommand(comm, args, conn);
        }

        public Boolean executeCommand(Commands comm, String args=null, Connection.Connection conn=null)
        {
            if (comm == Commands.NONE)
            {
                return true;
            }
            if (comm == Commands.ERROR)
            {
                return false;
            }
            if (comm == Commands.MSG)
            {
                MainWindow.instance.Dispatcher.BeginInvoke(new Action(() =>
                {
                    ((MainPage)PageManager.instance.getPage(PageType.MainPage)).WriteLine(args);
                }));
                return true;
            }
            if (comm == Commands.SAY)
            {
                PacketParser.sendPacket(Commands.SAY.getCommandString() + args, conn.ClientSocket);
            }
            if (comm == Commands.USERDATA)
            {
                PacketParser.sendUserData(conn.UserData, conn.ClientSocket);
                return true;
            }
            return false;
        }
    }

}
