using DataDecommision.ViewModels;
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

namespace DataDecommision.Views
{
    /// <summary>
    /// Interaction logic for BulkItemPage.xaml
    /// </summary>
    public partial class BulkItemPage : Page
    {
        public BulkItemPage()
        {
            InitializeComponent();
            this.DataContext = new BulkItemVM();
        }
    }
}
