using ServerJavaConnector.Core.Commander.Comms;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ServerJavaConnector.Core.Commander
{
    public class CommandManager
    {
        private MainWindow mainWindow;
        private List<String> lastCommands;

        public CommandManager(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            lastCommands = new List<string>();
            CommandsList = new Dictionary<Commands, Command>();
            Init();
        }

        public static void Init()
        {
            Add(new NONECommand(Commands.NONE, CommandType.NONE, false));
            Add(new ERRORCommand(Commands.ERROR, CommandType.NONE, false));
            Add(new MSGCommand(Commands.MSG, CommandType.SERVER, true));
            Add(new JSONCommand(Commands.JSON, CommandType.BOTH, true));
            Add(new CLOSECONNCommand(Commands.CLOSECONN, CommandType.BOTH, true));
            Add(new CLOSECommand(Commands.CLOSE, CommandType.CLIENT, false));
            Add(new SAYCommand(Commands.SAY, CommandType.CLIENT, true));
            Add(new LISTCommand(Commands.LIST, CommandType.BOTH, true));
            Add(new GETUSERDATACommand(Commands.GETUSERDATA, CommandType.SERVER, true));
        }

        private static void Add(Command cmm)
        {
            CommandsList.Add(cmm.Comm, cmm);
        }

        public Boolean ExecuteCommand(String command, Connection.Connection conn = null, bool invokedByServer = false)
        {
            Console.WriteLine("......|trying to execute: " + command + "|.........");
            Commands comm;
            try {
                 comm = Command.GetEnumCommand(command); }
            catch(Exception e)
            {
                comm = Commands.ERROR;
                Console.WriteLine(e.Message);
            }
            String args = Command.Offset(command);
            return ExecuteCommand(comm, args, conn, false);
        }

        public Boolean ExecuteCommand(Commands command, Connection.Connection connection)
        {
            return ExecuteCommand(command, null, connection);
        }

        public Boolean ExecuteCommand(Commands command, String args = null, Connection.Connection conn = null, bool invokedByServer = false)
        {
            Command comm = Command.GetCommand(command);
            if (comm.RequireConnection && (conn == null || !conn.Connected))
            {
                return false;
            }
            return comm.executeCommand(args,conn,invokedByServer);
        }

        public void AddCommandToList(String input)
        {
            if (lastCommands.Count == 100)
            {
                lastCommands.RemoveAt(0);
            }
            lastCommands.Add(input);
        }

        public String GetLastCommand()
        {
            if (lastCommands.Any())
            {
                return lastCommands[lastCommands.Count - 1];
            }
            return "";
        }

        public bool WasExecuted(String input)
        {
            if (input != null && input != "" && lastCommands.Contains(input)) { return true; }
            return false;
        }

        public String GetPreviousCommand(String input)
        {
            if (WasExecuted(input))
            {
                int i = lastCommands.IndexOf(input);
                if (i != 0) { return lastCommands[i - 1]; }
            }
            return "";
        }

        public bool HasNextCommand(String input)
        {
            if (WasExecuted(input) && lastCommands.IndexOf(input) < lastCommands.Count - 1) { return true; }
            return false;
        }

        public String GetNextCommand(String input)
        {
            if (HasNextCommand(input))
            {
                int i = lastCommands.IndexOf(input);
                return lastCommands[i + 1];
            }
            return "";
        }

        public static Dictionary<Commands, Command> CommandsList { get; private set; }
    }
}
