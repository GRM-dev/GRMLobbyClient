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

        public Boolean executeCommand(String command, Connection.Connection conn = null)
        {
            Console.WriteLine("......|executing|.........");
            Commands comm = Commands.getCommand(command);
            String args = Commands.getOffset(command);
            return executeCommand(comm, args, conn);
        }

        public Boolean executeCommand(Commands comm, String args = null, Connection.Connection conn = null)
        {
            if (comm == Commands.NONE)
            {
                return false;
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
                return true;
            }
            if (comm == Commands.USERDATA)
            {
                JsonParser.sendUserData(conn.UserData, conn.ClientSocket);
                return true;
            }
            return false;
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
