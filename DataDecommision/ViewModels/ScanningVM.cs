using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DataDecommision
{
    internal class ScanningVM : INotifyPropertyChanged
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
        public ScanningVM()
        {
            SelectedDate = null;
            ButtonScanClick = new ButtonCommandBinding(ButtonScan)
            {
                IsEnabled = true
            };
            ButtonAddClick = new ButtonCommandBinding(ButtonAdd)
            {
                IsEnabled = true
            };
            ButtonDisplayClick = new ButtonCommandBinding(ButtonDisplay)
            {
                IsEnabled = true
            };
            ButtonFinishClick = new ButtonCommandBinding(ButtonFinish)
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

        public ButtonCommandBinding ButtonScanClick { get; set; }
        public ButtonCommandBinding ButtonAddClick { get; set; }

        public ButtonCommandBinding ButtonDisplayClick { get; set; }

        public ButtonCommandBinding ButtonFinishClick { get; set; }

        private string textSerial;
        public string TextSerial
        {
            get { return textSerial; }
            set { textSerial = value; NotifyPropertyChanged("TextSerial"); }
        }

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
                TextSerial = code.Item4;

            });
            Mouse.OverrideCursor = null;
            ScaningPopup = false;
        }
        public void ButtonAdd()
        {

        }
        public void ButtonDisplay()
        {

        }
        public void ButtonFinish()
        {

        }


    }

}
