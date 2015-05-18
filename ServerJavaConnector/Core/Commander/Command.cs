using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerJavaConnector.Core.Commander
{
    public abstract class Command
    {
        protected Commands _comm;
        protected CommandType _type;
        protected bool _requireConnection;

        protected Command(Commands name, CommandType type, bool requireConnection)
        {
            this._comm = name;
            this._type = type;
            this._requireConnection = requireConnection;
        }

        public abstract Boolean executeCommand(String args = null, Connection.Connection conn = null, bool invokedByServer = false);

        public static Command GetCommand(String commS)
        {
            foreach (KeyValuePair<Commands, Command> commPair in CommandManager.CommandsList)
            {
                Command commT = commPair.Value;
                if (commT.Comm == Commands.NONE || commT.Comm == Commands.ERROR)
                {
                    continue;
                }
                if (commS.ToLower().StartsWith(commT.CommandString))
                {
                    return commT;
                }
            }
            return null;
        }

        public static Command GetCommand(Commands command)
        {
            foreach (KeyValuePair<Commands, Command> commPair in CommandManager.CommandsList)
            {
                Command commT = commPair.Value;
                if (commT.Comm == Commands.NONE || commT.Comm == Commands.ERROR)
                {
                    continue;
                }
                if (command.Equals(commT.Comm))
                {
                    return commT;
                }
            }
            return null;
        }

        public static Commands GetEnumCommand(string command)
        {
            if (command.StartsWith("!"))
            {
                command = command.Substring(1);
            }
            command = command.ToUpper();
            if(command.Contains(" "))
            {
                int i = command.IndexOf(" ");
                command = command.Substring(0, i);
            }
            return (Commands)Enum.Parse(typeof(Commands), command);
        }

        public static String Offset(String commS)
        {
            Command comm = GetCommand(commS);
            if (comm.Comm == Commands.NONE) { return commS; }
            commS = commS.Replace(comm.CommandString, "");
            if (commS.Length > 1 && commS[2] == ' ')
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

        public Commands Comm
        {
            get { return _comm; }
        }

        public bool RequireConnection
        {
            get { return _requireConnection; }
        }
    }
}
