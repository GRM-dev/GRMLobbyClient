using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using ServerJavaConnector.Core;
using ServerJavaConnector.Core.Connection;
using ServerJavaConnector.Pages;
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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ServerJavaConnector
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public static MainWindow instance;

        public MainWindow()
        {
            this.Conn = new Connection();
            instance = this;
            InitializeComponent();
            PageManager pM = new PageManager(getFrames());
            pM.initSetup();
        }

        private Dictionary<FrameType, Frame> getFrames()
        {
            Dictionary<FrameType, Frame> frames = new Dictionary<FrameType,Frame>();
            frames.Add(FrameType.MainFrame, MainFrame);
            frames.Add(FrameType.TopFrame,TopFrame);
            frames.Add(FrameType.BottomFrame,BottomFrame);
            return frames;
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            Conn.Disconnect();
        }

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
            Resources["Connected"] = Conn.Connected;
            Resources["NegConnected"] = !Conn.Connected;
        }

        internal static void OnPropertySChanged(string p)
        {
            if (instance != null)
            {
                instance.OnPropertyChanged(p);
            }
        }

        public Connection Conn { get; private set; }
    }
}
