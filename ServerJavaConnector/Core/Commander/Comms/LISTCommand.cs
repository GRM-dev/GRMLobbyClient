using System;
using GRMLobbyClient.Core.Connection;

namespace GRMLobbyClient.Core.Commander.Comms
{
    internal class LISTCommand : Command
    {
        public LISTCommand(Commands name, CommandType type, bool requireConnection) : base(name, type, requireConnection)
        {
        }

        public override bool executeCommand(string args = null, Connection.Connection conn = null, bool invokedByServer = false)
        {
            throw new NotImplementedException();
        }
    }
}