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
        private CommandType _type;
        private bool _requireConnection;

        private Commands(Command name, CommandType type, bool requireConnection)
        {
            this._comm = name;
            this._type = type;
            this._requireConnection = requireConnection;
        }

        public static void Init()
        {
            CommandsList = new Dictionary<Command, Commands>();
            Add(NONE = new Commands(Command.NONE, CommandType.NONE, false));
            Add(ERROR = new Commands(Command.ERROR, CommandType.NONE, false));
            Add(MSG = new Commands(Command.MSG, CommandType.SERVER, true));
            Add(JSON = new Commands(Command.JSON, CommandType.BOTH, true));
            Add(CLOSECONN = new Commands(Command.CLOSECONN, CommandType.BOTH, true));
            Add(CLOSE = new Commands(Command.CLOSE, CommandType.CLIENT, false));
            Add(SAY = new Commands(Command.SAY, CommandType.CLIENT, true));
            Add(LIST = new Commands(Command.LIST, CommandType.BOTH, true));
            Add(GETUSERDATA = new Commands(Command.GETUSERDATA, CommandType.SERVER, true));
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
                if (commS.ToLower().StartsWith(commT.CommandString)) { return commT; }
            }
            return NONE;
        }

        public static String Offset(String commS)
        {
            Commands comm = getCommand(commS);
            if (comm == NONE) { return commS; }
            commS = commS.Replace(comm.CommandString, "");
            if (commS.Length > 1&&commS[2]==' ')
            {
                commS = commS.Substring(1);
            }
            return commS;
        }

        public String CommandString
        {
            get { return "!" + Comm.ToString().ToLower(); }
        }

        public CommandType Type
        {
            get { return _type; }
        }

        public Command Comm
        {
            get { return _comm; }
        }

        public bool RequireConnection
        {
            get { return _requireConnection; }
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
    }

    public enum Command
    {
        NONE, ERROR, MSG, JSON, CLOSECONN, CLOSE, SAY, LIST, GETUSERDATA
    }
}
