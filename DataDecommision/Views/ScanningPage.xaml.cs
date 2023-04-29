
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
    /// Interaction logic for ScanningPage.xaml
    /// </summary>
    public partial class ScanningPage : Page
    {
        public ScanningPage()
        {
            InitializeComponent();
            this.DataContext = new ScanningVM();
        }
        private void Expdate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            var bulkItemVM = this.DataContext as BulkItemVM;
            DateTime selectedDate;
            if (DateTime.TryParseExact(expdate.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out selectedDate))
            {
                // The entered date is valid
                bulkItemVM.SelectedDate = selectedDate;
                bulkItemVM.IsDateValid = true;
            }
            else
            {
                // The entered date is invalid
                bulkItemVM.IsDateValid = false;
            }
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

        private void PreviewTextInput2(object sender, TextCompositionEventArgs e)
        {
            // Define a regular expression pattern that matches alphanumeric characters
            Regex regex = new Regex("^[0-9]*$");

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
