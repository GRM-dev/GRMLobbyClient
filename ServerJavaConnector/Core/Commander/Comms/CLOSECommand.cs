using System;
using ServerJavaConnector.Core.Connection;
using ServerJavaConnector.XAML.Dialogs;

namespace ServerJavaConnector.Core.Commander.Comms
{
    internal class CLOSECommand : Command
    {
        public CLOSECommand(Commands name, CommandType type, bool requireConnection) : base(name, type, requireConnection)
        {
        }

        public override bool executeCommand(string args = null, Connection.Connection conn = null, bool invokedByServer = false)
        {
            CDialogManager.ShowClosingDialog();
            return true;
        }
    }
}