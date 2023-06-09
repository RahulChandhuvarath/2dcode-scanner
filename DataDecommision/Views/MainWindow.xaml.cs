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
using DataDecommision;


namespace DataDecommision
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new ViewModel();

            // Handle the application level unhandled exception
            App.Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to Quit?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.No)
            {
                e.Cancel = true; // Cancel the window's closing event
            }
            else
            {
                AccessOperation.CreateCSV();
            }
        }

        private void Current_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            // Your crash handling code here
            MessageBox.Show("Crashed due to unknown error", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            AccessOperation.CreateCSV();
            // Prevent the application from terminating
            e.Handled = false;
        }
    }
}
