using System;
using ServerJavaConnector.Core.Connection;
using ServerJavaConnector.Core.JSON;

namespace ServerJavaConnector.Core.Commander.Comms
{
    internal class GETUSERDATACommand : Command
    {
        public GETUSERDATACommand(Commands name, CommandType type, bool requireConnection) : base(name, type, requireConnection)
        {
        }

        public override bool executeCommand(string args = null, Connection.Connection conn = null, bool invokedByServer = false)
        {
            JsonParser.sendUserData(conn.UserData, conn.ClientSocket);
            return true;
        }
    }
}