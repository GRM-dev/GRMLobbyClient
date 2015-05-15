using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerJavaConnector.Core.Commands
{
    public class Commands
    {
        private Command _comm;
        private CommandType type;

        private Commands(Command name, CommandType type)
        {
            this._comm = name;
            this.type = type;
        }

        public static void Init()
        {
            CommandsList = new Dictionary<Command, Commands>();
            Add(NONE = new Commands(Command.NONE, CommandType.NONE));
            Add(ERROR = new Commands(Command.ERROR, CommandType.NONE));
            Add(MSG = new Commands(Command.MSG, CommandType.SERVER));
            Add(JSON = new Commands(Command.JSON, CommandType.BOTH));
            Add(CLOSECONN = new Commands(Command.CLOSECONN, CommandType.BOTH));
            Add(CLOSE = new Commands(Command.CLOSE, CommandType.CLIENT));
            Add(SAY = new Commands(Command.SAY, CommandType.CLIENT));
            Add(LIST = new Commands(Command.LIST, CommandType.CLIENT));
            Add(GETUSERDATA = new Commands(Command.GETUSERDATA, CommandType.SERVER));
        }

        private static void Add(Commands cmm)
        {
            CommandsList.Add(cmm.Comm, cmm);
        }

        public static Commands getCommand(String commS)
        {
            foreach (KeyValuePair<Command, Commands> commPair in CommandsList)
            {
                Commands commT = commPair.Value;
                if (commT == NONE || commT == ERROR)
                {
                    continue;
                }
                if (commS.ToLower().StartsWith(commT.getCommandString())) { return commT; }
            }
            return NONE;
        }

        public static String getOffset(String commS)
        {
            Commands comm = getCommand(commS);
            if (comm == NONE) { return commS; }
            commS = commS.Replace(comm.getCommandString(), "");
            return commS;
        }

        public String getCommandString()
        {
            return "!" + Comm.ToString().ToLower();
        }

        public CommandType getType()
        {
            return type;
        }

        public static Dictionary<Command, Commands> CommandsList { get; private set; }
        public static Commands NONE { get; private set; }
        public static Commands ERROR { get; private set; }
        public static Commands MSG { get; private set; }
        public static Commands JSON { get; private set; }
        public static Commands CLOSECONN { get; private set; }
        public static Commands CLOSE { get; private set; }
        public static Commands SAY { get; private set; }
        public static Commands LIST { get; private set; }
        public static Commands GETUSERDATA { get; private set; }

        public Command Comm
        {
            get { return _comm; }
        }
    }

    public enum Command
    {
        NONE, ERROR, MSG, JSON, CLOSECONN, CLOSE, SAY, LIST, GETUSERDATA
    }
}
