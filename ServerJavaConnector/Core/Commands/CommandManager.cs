using ServerJavaConnector.XAML;
using ServerJavaConnector.XAML.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServerJavaConnector.Core;
using ServerJavaConnector.Core.Connection;
using ServerJavaConnector.Core.JSON;
using ServerJavaConnector.XAML.Dialogs;

namespace ServerJavaConnector.Core.Commands
{
    public class CommandManager
    {
        private MainWindow mainWindow;
        private List<String> lastCommands;

        public CommandManager(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            lastCommands = new List<string>();
            Commands.Init();
        }

        public Boolean executeCommand(String command, Connection.Connection conn = null, bool invokedByServer = false)
        {
            Console.WriteLine("......|executing: "+command+"|.........");
            Commands comm = Commands.getCommand(command);
            String args = Commands.Offset(command);
            return executeCommand(comm, args, conn, false);
        }

        public Boolean executeCommand(Command command, Connection.Connection connection)
        {
            Commands comm = Commands.getCommand(command);
            return executeCommand(comm, null, connection);
        }

        public Boolean executeCommand(Commands comm, String args = null, Connection.Connection conn = null, bool invokedByServer = false)
        {
            if (comm.RequireConnection && (conn == null || !conn.Connected))
            {
                return false;
            }
            switch (comm.Comm)
            {
                case Command.NONE:
                    return false;
                case Command.ERROR:
                    return false;
                case Command.MSG:
                    MainWindow.instance.Dispatcher.BeginInvoke(new Action(() =>
        {
            ((MainPage)PageManager.instance.getPage(PageType.MainPage)).WriteLine(args);
        }));
                    return true;
                case Command.SAY:
                    PacketParser.sendPacket(Commands.SAY.CommandString + args, conn.ClientSocket);
                    return true;
                case Command.GETUSERDATA:
                    JsonParser.sendUserData(conn.UserData, conn.ClientSocket);
                    return true;
                case Command.CLOSE:
                    CDialogManager.ShowClosingDialog();
                    return true;
                case Command.CLOSECONN:
                    PacketParser.sendPacket(Commands.CLOSECONN.CommandString + args, conn.ClientSocket);
                    return true;
                default:
                    return false;
            }
        }

        public void AddCommandToList(String input)
        {
            if (lastCommands.Count == 100)
            {
                lastCommands.RemoveAt(0);
            }
            lastCommands.Add(input);
        }

        public String getLastCommand()
        {
            if (lastCommands.Any())
            {
                return lastCommands[lastCommands.Count - 1];
            }
            return "";
        }

        public bool wasExecuted(String input)
        {
            if (input != null && input != "" && lastCommands.Contains(input)) { return true; }
            return false;
        }

        public String getPreviousCommand(String input)
        {
            if (wasExecuted(input))
            {
                int i = lastCommands.IndexOf(input);
                if (i != 0) { return lastCommands[i - 1]; }
            }
            return "";
        }

        public bool hasNextCommand(String input)
        {
            if (wasExecuted(input) && lastCommands.IndexOf(input) < lastCommands.Count - 1) { return true; }
            return false;
        }

        public String getNextCommand(String input)
        {
            if (hasNextCommand(input))
            {
                int i = lastCommands.IndexOf(input);
                return lastCommands[i + 1];
            }
            return "";
        }
    }
}
