
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DataDecommision
{
    /// <summary>
    /// Interaction logic for BulkItemPage.xaml
    /// </summary>
    public partial class BulkItemPage : Page
    {
       
        private static BulkItemPage _instance;

        private BulkItemPage()
        {

            InitializeComponent();
            this.DataContext = new BulkItemVM();
            
        }

        public static BulkItemPage Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new BulkItemPage();
                }
                return _instance;
            }
        }

     

        private void PreviewTextInput1(object sender, TextCompositionEventArgs e)
        {
            // Define a regular expression pattern that matches alphanumeric characters
            Regex regex = new Regex("^[a-zA-Z0-9-]*$");

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

            //Define a regular expression pattern that matches alphanumeric characters
           //Regex regex = new Regex("^[0-9-]*$");

           // // Test the input against the regular expression pattern
           // if (!regex.IsMatch(e.Text))
           // {
           //     //If the input doesn't match, mark the event as handled to prevent it from being entered
                e.Handled = true;
           // }
        }

        private void Expdate_SelectedDateChanged(object sender, RoutedEventArgs e)
        {
            //var bulkItemVM = this.DataContext as BulkItemVM;
            //DatePicker datePicker = sender as DatePicker;

            //if (Validation.GetHasError(datePicker))
            //{
            //    // The entered date is invalid, display an error message
            //    MessageBox.Show("Invalid date entered. Please enter a valid date.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            //    // Set the focus back to the DatePicker so the user can correct their input
            //    datePicker.Focus();
            //    bulkItemVM.IsDateValid = false;
            //}
            //else
            //{
            //    // The entered date is invalid
            //    bulkItemVM.IsDateValid = true;
            //}
        }
    }


    public static class LabelBlinkBehavior
    {
        public static bool GetIsBlinking(Label label)
        {
            return (bool)label.GetValue(IsBlinkingProperty);
        }

        public static void SetIsBlinking(Label label, bool value)
        {
            label.SetValue(IsBlinkingProperty, value);
        }

        public static readonly DependencyProperty IsBlinkingProperty =
            DependencyProperty.RegisterAttached(
                "IsBlinking", typeof(bool), typeof(LabelBlinkBehavior),
                new UIPropertyMetadata(false, OnIsBlinkingChanged));

        private static void OnIsBlinkingChanged(
            DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (!(obj is Label label))
                return;

            bool isBlinking = (bool)e.NewValue;
            if (isBlinking)
            {
                Storyboard storyboard = new Storyboard();
                DoubleAnimation animation = new DoubleAnimation
                {
                    From = 0,
                    To = 1,
                    Duration = TimeSpan.FromSeconds(0.5),
                    AutoReverse = true,
                    RepeatBehavior = RepeatBehavior.Forever
                };
                Storyboard.SetTarget(animation, label);
                Storyboard.SetTargetProperty(animation,
                    new PropertyPath(UIElement.OpacityProperty));
                storyboard.Children.Add(animation);
                storyboard.Begin();
            }
        }


    }

}
