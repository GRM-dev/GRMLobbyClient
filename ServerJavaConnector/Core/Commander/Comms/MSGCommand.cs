using System;
using ServerJavaConnector.Core.Connection;
using ServerJavaConnector.XAML.Pages;

namespace ServerJavaConnector.Core.Commander.Comms
{
    internal class MSGCommand : Command
    {
        public MSGCommand(Commands name, CommandType type, bool requireConnection) : base(name, type, requireConnection)
        {
        }

        public override bool executeCommand(string args = null, Connection.Connection conn = null, bool invokedByServer = false)
        {
            MainWindow.instance.Dispatcher.BeginInvoke(new Action(() =>
            {
                ((MainPage)PageManager.instance.getPage(PageType.MainPage)).WriteLine(args);
            }));
            return true;
        }
    }
}