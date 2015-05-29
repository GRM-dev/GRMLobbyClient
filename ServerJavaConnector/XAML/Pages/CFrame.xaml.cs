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

namespace GRMLobbyClient.XAML.Pages
{
    /// <summary>
    /// Interaction logic for CFrame.xaml
    /// </summary>
    public partial class CFrame : Frame
    {
        public CFrame()
        {
            InitializeComponent();
            PageHistory = new Dictionary<int,Page>();
            CurrentPageID = 0;
        }

        public void AddAndChangePage(Page page)
        {
            CurrentPage = page;
            Navigate(CurrentPage);
            PageHistory[++CurrentPageID]=page;
        }

        public void goBack()
        {
            if (CurrentPageID > 1)
            {
                Page prevPage;
                PageHistory.TryGetValue(--CurrentPageID, out prevPage);
                CurrentPage = prevPage;
                Navigate(CurrentPage);
            }
        }

        public int CurrentPageID { get; private set; }
        public Dictionary<Int32,Page> PageHistory { get; private set; }
        public Page CurrentPage { get; private set; }
    }
}
