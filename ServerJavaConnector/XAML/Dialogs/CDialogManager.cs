using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using ServerJavaConnector.XAML.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace ServerJavaConnector.XAML.Dialogs
{
    public static class CDialogManager
    {

        public static void ShowInfoTop(String title, String message)
        {
            CFrame frame = PageManager.instance.getFrame(FrameType.TopFrame);
            var page = PageManager.instance.getPage(PageType.InfoPage) as InfoPage;
            page.setInfo(title, message);
            PageManager.instance.changePage(FrameType.TopFrame, PageType.InfoPage);
            MainWindow mWindow = (MainWindow)Application.Current.MainWindow;
            var flyout = mWindow.Flyouts.Items[1] as Flyout;
            flyout.IsOpen = true;
        }

        public static void ShowInfoBottom(String title, String message)
        {
            CFrame frame = PageManager.instance.getFrame(FrameType.BottomFrame);
            var page = PageManager.instance.getPage(PageType.InfoPage) as InfoPage;
            page.setInfo(title, message);
            PageManager.instance.changePage(FrameType.BottomFrame, PageType.InfoPage);
            MainWindow mWindow = (MainWindow)Application.Current.MainWindow;
            var flyout = mWindow.Flyouts.Items[0] as Flyout;
            flyout.IsOpen = true;
        }

        public static async void ShowExceptionDialog(Exception ex, String msg)
        {
            if (msg == null)
            {
                msg = "";
            }
            msg += "\r\n" + ex.Message;
            var metroWindow = MainWindow.instance;
            if (metroWindow.WindowLoaded)
            {
                await DialogManager.ShowMessageAsync(metroWindow, "Exception", msg);
            }
            else
            {
                MessageBox.Show(msg);
            }
            Console.WriteLine(msg);
        }

        public static async void ShowClosingDialog()
        {
            var metroWindow = Application.Current.MainWindow as MainWindow;
            if (!metroWindow.IsLoaded)
            {
                return;
            }
            String title = "Closing ..";
            String msg = "Are you sure to close program?";
            MessageDialogResult result;
            MessageDialogStyle dialogStyle = MessageDialogStyle.AffirmativeAndNegative;
            MetroDialogSettings dialogSettings = new MetroDialogSettings();
            dialogSettings.AnimateShow = true;
            dialogSettings.AffirmativeButtonText = "Close";
            dialogSettings.NegativeButtonText = "Cancel";
            result = await metroWindow.ShowMessageAsync(title, msg, dialogStyle, dialogSettings);
            if (result == MessageDialogResult.Affirmative)
            {
                MainWindow.CloseApp();
            }
        }
    }
}
