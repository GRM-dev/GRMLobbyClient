using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ServerJavaConnector.XAML.Dialogs
{
    public static class CDialogManager
    {
        public static async void ShowExceptionDialog(Exception ex, String msg){
            var metroWindow = Application.Current.MainWindow as MainWindow;
            if(msg==null){
                msg="";
            }
            msg+= "\r\n" + ex.Message;
            if (metroWindow.IsLoaded)
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
            String msg= "Are you sure to close program?";
            MessageDialogResult result;
            MessageDialogStyle dialogStyle=MessageDialogStyle.AffirmativeAndNegative;
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
