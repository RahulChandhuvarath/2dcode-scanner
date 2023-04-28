using System;
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

namespace DataDecommision
{
    /// <summary>
    /// Interaction logic for RePackingPage.xaml
    /// </summary>
    public partial class RePackingPage : Page
    {
        public RePackingPage()
        {
            InitializeComponent();
            this.DataContext = new RepackingVM();
        }
    }
}
