using System;
using GRMLobbyClient.Core.Connection;
using GRMLobbyClient.XAML.Dialogs;

namespace GRMLobbyClient.Core.Commander.Comms
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