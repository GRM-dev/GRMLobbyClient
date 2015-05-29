using System;
using GRMLobbyClient.Core.Connection;
using GRMLobbyClient.Core.JSON;

namespace GRMLobbyClient.Core.Commander.Comms
{
    internal class GETUSERDATACommand : Command
    {
        public GETUSERDATACommand(Commands name, CommandType type, bool requireConnection) : base(name, type, requireConnection)
        {
        }

        public override bool executeCommand(string args = null, Connection.Connection conn = null, bool invokedByServer = false)
        {
            JsonParser.sendData(conn.UserData, conn.ClientSocket);
            return true;
        }
    }
}