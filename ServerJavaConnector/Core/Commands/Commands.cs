using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerJavaConnector.Core.Commands
{
    public class Commands
    {
        private String commandName;
        private CommandType type;

        private Commands(String name, CommandType type)
        {
            this.commandName = name;
            this.type = type;
        }

        public static void Init()
        {
            CommandsList = new Dictionary<string, Commands>();
            CommandsList.Add("NONE", NONE = new Commands("NONE", CommandType.NONE));
            CommandsList.Add("ERROR", ERROR = new Commands("ERROR", CommandType.NONE));
            CommandsList.Add("MSG", MSG = new Commands("MSG", CommandType.SERVER));
            CommandsList.Add("JSON", JSON = new Commands("JSON", CommandType.BOTH));
            CommandsList.Add("CLOSECONN", CLOSECONN = new Commands("CLOSECONN", CommandType.BOTH));
            CommandsList.Add("CLOSE", CLOSE = new Commands("CLOSE", CommandType.CLIENT));
            CommandsList.Add("SAY", SAY = new Commands("SAY", CommandType.CLIENT));
            CommandsList.Add("LIST", LIST = new Commands("LIST", CommandType.CLIENT));
            CommandsList.Add("USERDATA", USERDATA = new Commands("USERDATA", CommandType.SERVER));
        }

        public static Commands getCommand(String commS)
        {
            foreach (KeyValuePair<String, Commands> commPair in CommandsList)
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
            commS=commS.Replace(comm.getCommandString(), "");
            return commS;
        }

        public String getCommandString()
        {
            return "!" + commandName.ToLower();
        }

        public CommandType getType()
        {
            return type;
        }

        public static Dictionary<String, Commands> CommandsList { get; private set; }
        public static Commands NONE { get; private set; }
        public static Commands ERROR { get; private set; }
        public static Commands MSG { get; set; }
        public static Commands JSON { get; set; }
        public static Commands CLOSECONN { get; set; }
        public static Commands CLOSE { get; set; }
        public static Commands SAY { get; set; }
        public static Commands LIST { get; set; }

        public static Core.Commands.Commands USERDATA { get; set; }
    }
}
