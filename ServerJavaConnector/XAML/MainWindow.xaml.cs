using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using ServerJavaConnector.Core;
using ServerJavaConnector.Core.Commands;
using ServerJavaConnector.Core.Connection;
using ServerJavaConnector.XAML;
using ServerJavaConnector.XAML.Dialogs;
using ServerJavaConnector.XAML.Pages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ServerJavaConnector
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public static MainWindow instance;
        private CommandManager _CM;

        public MainWindow()
        {
            WindowLoaded = false;
            this.Conn = new Connection();
            instance = this;
            InitializeComponent();
            PageManager pM = new PageManager(getFrames());
            pM.initSetup();
            CommandManager = new CommandManager(this);
            this.Loaded += new RoutedEventHandler(MainWindow_Loaded);
            //MainFrame.Navigate(new GLPage());
        }
            
        private Dictionary<FrameType, CFrame> getFrames()
        {
            Dictionary<FrameType, CFrame> frames = new Dictionary<FrameType, CFrame>();
            frames.Add(FrameType.MainFrame, MainFrame);
            frames.Add(FrameType.TopFrame, TopFrame);
            frames.Add(FrameType.BottomFrame, BottomFrame);
            return frames;
        }

        public static void CloseApp()
        {
            if (instance != null)
            {
                instance.Conn.Disconnect();
                Application.Current.Shutdown();
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            CDialogManager.ShowClosingDialog();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            WindowLoaded = true;
        }

        public Connection Conn { get; private set; }

        public CommandManager CommandManager
        {
            get { return _CM; }
            private set { _CM = value; }
        }

        public bool WindowLoaded { get; private set; }

        private void Settings_Button_Click(object sender, RoutedEventArgs e)
        {
            BottomFrame.Navigate(new GLPage());
            var flyout = this.Flyouts.Items[0] as Flyout;
            flyout.IsOpen = !flyout.IsOpen;
        }
    }
}
