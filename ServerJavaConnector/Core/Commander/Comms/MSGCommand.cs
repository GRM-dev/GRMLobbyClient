﻿using System;
using GRMLobbyClient.Core.Connection;
using GRMLobbyClient.XAML.Pages;

namespace GRMLobbyClient.Core.Commander.Comms
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
                ((ChatPage)PageManager.Instance.GetPage(PageType.ChatPage)).WriteLine(args);
            }));
            return true;
        }
    }
}