using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace DataDecommision
{
    internal class RepackingVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Method to call when the property value is changed
        /// </summary>
        /// <param name="propertyName"></param>
        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public RepackingVM()
        {
            SelectedDate = null;
            ButtonScanClick = new ButtonCommandBinding(ButtonScan)
            {
                IsEnabled = true
            };
            ButtonNextClick = new ButtonCommandBinding(ButtonNext)
            {
                IsEnabled = true
            };

            ButtonBackClick = new ButtonCommandBinding(ButtonBack)
            {
                IsEnabled = true
            };
        }

        private bool _isDateValid;
        public bool IsDateValid
        {
            get { return _isDateValid; }
            set { _isDateValid = value; NotifyPropertyChanged(nameof(IsDateValid)); }
        }

        private DateTime? _selectedDate;
        public DateTime? SelectedDate
        {
            get { return _selectedDate; }
            set
            {
                if (_selectedDate != value)
                {
                    _selectedDate = value;
                    NotifyPropertyChanged(nameof(SelectedDate));
                }
            }
        }
        public ButtonCommandBinding ButtonBackClick { get; set; }
        public ButtonCommandBinding ButtonScanClick { get; set; }
        public ButtonCommandBinding ButtonNextClick { get; set; }

      
        private string textGtin;
        public string TextGtin
        {
            get { return textGtin; }
            set { textGtin = value; NotifyPropertyChanged("TextGtin"); }
        }

        private string textLot;
        public string TextLot
        {
            get { return textLot; }
            set { textLot = value; NotifyPropertyChanged("TextLot"); }
        }

        private string textEXP;
        public string TextEXP
        {
            get { return textEXP; }
            set { textEXP = value; NotifyPropertyChanged("TextEXP"); }
        }

        private bool scaningPopup;
        public bool ScaningPopup
        {
            get { return scaningPopup; }
            set { scaningPopup = value; NotifyPropertyChanged("ScaningPopup"); }
        }
        private async void ButtonScan()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            ScaningPopup = true;

            await Task.Run(() =>
            {

                var code = ScannerDecoder.ScanAndDecode();
                TextGtin = code.Item1;
                TextLot = code.Item2;
                if (code.Item3 != null && code.Item3 != "")
                {
                    DateTime expResult = DateTime.ParseExact(20 + code.Item3, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                    SelectedDate = expResult;
                }
                else
                {
                    SelectedDate = null;
                }

            });
            Mouse.OverrideCursor = null;
            ScaningPopup = false;
        }
        public void ButtonBack()
        {

            NavigationService navigationService = (NavigationService)App.Current.MainWindow.Resources["NavigationService"];
            navigationService.CurrentPage = BulkItemPage.Instance;
        }
        public void ButtonNext()
        {

            //NavigationService navigationService = (NavigationService)App.Current.MainWindow.Resources["NavigationService"];
            //navigationService.CurrentPage = new RePackingPage();
        }


    }

}
