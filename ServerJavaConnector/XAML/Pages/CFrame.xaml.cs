using System;
using System.Collections;
using System.Collections.Generic;
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

namespace ServerJavaConnector.XAML.Pages
{
    /// <summary>
    /// Interaction logic for CFrame.xaml
    /// </summary>
    public partial class CFrame : Frame
    {
        public CFrame()
        {
            InitializeComponent();
            pageHistory = new Dictionary<int,Page>();
            CurrentPageID = 0;
        }

        public void AddAndChangePage(Page page)
        {
            CurrentPage = page;
            Navigate(CurrentPage);
            pageHistory[++CurrentPageID]=page;
        }

        public void goBack()
        {
            if (CurrentPageID > 1)
            {
                Page prevPage;
                pageHistory.TryGetValue(--CurrentPageID, out prevPage);
                CurrentPage = prevPage;
                Navigate(CurrentPage);
            }
        }

        public int CurrentPageID { get; private set; }
        public Dictionary<Int32,Page> pageHistory { get; private set; }
        public Page CurrentPage { get; private set; }
    }
}
