using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for ScanDisplayPage.xaml
    /// </summary>
    public partial class ScanDisplayPage : Page
    {
        public ScanDisplayPage()
        {
            InitializeComponent();
            this.DataContext = ScanDisplayVM.Instance;
        }
        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ListView listView)
            {
                var selectedItems = listView.SelectedItems.Cast<ScanData>().ToList();
                ScanDisplayVM.Instance.LstSelectedItems = selectedItems;
            }
        }

        private void Expdate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            var scanDisplayVM = this.DataContext as ScanDisplayVM;
            DateTime selectedDate;
            if (DateTime.TryParseExact(expdate.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out selectedDate))
            {
                // The entered date is valid
                scanDisplayVM.SelectedDate = selectedDate;
                scanDisplayVM.IsDateValid = true;
            }
            else
            {
                // The entered date is invalid
                scanDisplayVM.IsDateValid = false;
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

    public class SerialNumberConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is ListViewItem item))
                return null;

            var listView = ItemsControl.ItemsControlFromItemContainer(item) as ListView;
            if (listView == null)
                return null;

            var index = listView.ItemContainerGenerator.IndexFromContainer(item);
            return index + 1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class DivideByConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double actualWidth && double.TryParse(parameter?.ToString(), out double divisor))
            {
                return new GridLength(actualWidth / divisor);
            }

            return new GridLength(1, GridUnitType.Auto);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
   

}
