using System;
using ServerJavaConnector.Core.Connection;

namespace ServerJavaConnector.Core.Commander.Comms
{
    internal class ERRORCommand : Command
    {
        public ERRORCommand(Commands name, CommandType type, bool requireConnection) : base(name, type, requireConnection)
        {
        }

        public override bool executeCommand(string args = null, Connection.Connection conn = null, bool invokedByServer = false)
        {
            return false;
        }
    }
}