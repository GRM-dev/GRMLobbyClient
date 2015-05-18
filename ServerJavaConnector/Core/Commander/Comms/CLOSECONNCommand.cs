using System;
using ServerJavaConnector.Core.Connection;

namespace ServerJavaConnector.Core.Commander.Comms
{
    internal class CLOSECONNCommand : Command
    {
        public CLOSECONNCommand(Commands name, CommandType type, bool requireConnection) : base(name, type, requireConnection)
        {
        }

        public override bool executeCommand(string args = null, Connection.Connection conn = null, bool invokedByServer = false)
        {
            PacketParser.sendPacket(Command.GetCommand(Commands.CLOSECONN).CommandString + args, conn.ClientSocket);
            return true;
        }
    }
}