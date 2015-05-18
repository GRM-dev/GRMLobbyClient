using System;
using ServerJavaConnector.Core.Connection;

namespace ServerJavaConnector.Core.Commander.Comms
{
    internal class SAYCommand : Command
    {
        public SAYCommand(Commands name, CommandType type, bool requireConnection) : base(name, type, requireConnection)
        {
        }

        public override bool executeCommand(string args = null, Connection.Connection conn = null, bool invokedByServer = false)
        {
            PacketParser.sendPacket(Command.GetCommand(Commands.SAY).CommandString + args, conn.ClientSocket);
            return true;
        }
    }
}