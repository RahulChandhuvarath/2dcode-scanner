using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        private static RePackingPage _instance;
        private RePackingPage()
        {
            InitializeComponent();
            this.DataContext = new RepackingVM();
        }
        public static RePackingPage Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new RePackingPage();
                }
                return _instance;
            }
        }
        private void Expdate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            //var repackingVM = this.DataContext as RepackingVM;
            //DateTime selectedDate;
            //if (DateTime.TryParseExact(expdate.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out selectedDate))
            //{
            //    // The entered date is valid
            //    repackingVM.SelectedDate = selectedDate;
            //    repackingVM.IsDateValid = true;
            //}
            //else
            //{
            //    // The entered date is invalid
            //    repackingVM.IsDateValid = false;
            //}
        }

        private void PreviewTextInput1(object sender, TextCompositionEventArgs e)
        {
            // Define a regular expression pattern that matches alphanumeric characters
            Regex regex = new Regex("^[a-zA-Z0-9]*$");

            // Test the input against the regular expression pattern
            if (!regex.IsMatch(e.Text))
            {
                // If the input doesn't match, mark the event as handled to prevent it from being entered
                e.Handled = true;
            }
        }

        private void PreviewTextInput3(object sender, TextCompositionEventArgs e)
        {
            // Define a regular expression pattern that matches alphanumeric characters
            //Regex regex = new Regex("^[0-9-]*$");

            //// Test the input against the regular expression pattern
            //if (!regex.IsMatch(e.Text))
            //{
            //    // If the input doesn't match, mark the event as handled to prevent it from being entered
            e.Handled = true;
            //}
        }

        private void PreviewTextInput4(object sender, TextCompositionEventArgs e)
        {
            // Define a regular expression pattern that matches alphanumeric characters
            Regex regex = new Regex("^[0-9-]*$");

            // Test the input against the regular expression pattern
            if (!regex.IsMatch(e.Text))
            {
                // If the input doesn't match, mark the event as handled to prevent it from being entered
                e.Handled = true;
            }
        }
    }
}
