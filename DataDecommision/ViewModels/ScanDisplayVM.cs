using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Threading;

namespace DataDecommision
{
    internal class ScanDisplayVM : INotifyPropertyChanged
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
        private static ScanDisplayVM _instance;


        public static ScanDisplayVM Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ScanDisplayVM();
                }
                return _instance;
            }
        }
        private ScanDisplayVM()
        {
            SelectedDate = null;
            ButtonBackgroundColor = Brushes.DarkGreen;

            ButtonScanClick = new ButtonCommandBinding(ButtonScan)
            {
                IsEnabled = true
            };


            ButtonDisplayClick = new ButtonCommandBinding(ButtonDisplay)
            {
                IsEnabled = true
            };

            ButtonAddClick = new ButtonCommandBinding(ButtonAdd)
            {
                IsEnabled = true
            };

            ButtonDeleteClick = new ButtonCommandBinding(ButtonDelete)
            {
                IsEnabled = true
            };

            List<ScanData> lstsd = new List<ScanData>();
            ScanData sd1 = new ScanData("2025-03-03", "A1234", "12345", "B1234");
            ScanData sd2 = new ScanData("2025-10-03", "D1234", "12345", "C1234");
            _lstScanData.Add(sd1);
            _lstScanData.Add(sd2);
            LstScanData = new ObservableCollection<ScanData>(_lstScanData);
            IsDisplayGridVisible = false;
            IsScanGridVisible = true;
            IsAddGridVisible = false;

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
        private string scannedBottleCount;
        public string ScannedBottleCount
        {
            get { return scannedBottleCount; }
            set { scannedBottleCount = value; NotifyPropertyChanged("ScannedBottleCount"); }
        }

        private string textEXP;
        public string TextEXP
        {
            get { return textEXP; }
            set { textEXP = value; NotifyPropertyChanged("TextEXP"); }
        }

        private bool addPopup;
        public bool AddPopup
        {
            get { return addPopup; }
            set { addPopup = value; NotifyPropertyChanged("AddPopup"); }
        }

        private Brush _buttonBackgroundColor;
        public Brush ButtonBackgroundColor
        {
            get { return _buttonBackgroundColor; }
            set
            {
                _buttonBackgroundColor = value;
                NotifyPropertyChanged(nameof(ButtonBackgroundColor));
            }
        }

        private bool isDisplayGridVisible;
        public bool IsDisplayGridVisible
        {
            get { return isDisplayGridVisible; }
            set
            {
                isDisplayGridVisible = value;
                NotifyPropertyChanged(nameof(IsDisplayGridVisible));
            }
        }

        private bool isScanGridVisible;
        public bool IsScanGridVisible
        {
            get { return isScanGridVisible; }
            set
            {
                isScanGridVisible = value;
                NotifyPropertyChanged(nameof(IsScanGridVisible));
            }
        }

        private bool isAddGridVisible;
        public bool IsAddGridVisible
        {
            get { return isAddGridVisible; }
            set
            {
                isAddGridVisible = value;
                NotifyPropertyChanged(nameof(IsAddGridVisible));
            }
        }
        public ButtonCommandBinding ButtonDisplayClick { get; set; }

        public ButtonCommandBinding ButtonAddClick { get; set; }

        public ButtonCommandBinding ButtonDeleteClick { get; set; }
        public ButtonCommandBinding ButtonScanClick { get; set; }

        private ObservableCollection<ScanData> _lstScanData = new ObservableCollection<ScanData>();
        public ObservableCollection<ScanData> LstScanData
        {
            get => _lstScanData;
            set
            {
                _lstScanData = value;
                ScannedBottleCount = _lstScanData.Count.ToString();
                NotifyPropertyChanged("LstScanData");
            }
        }

        private List<ScanData> lstSelectedItems = null;
        public List<ScanData> LstSelectedItems
        {
            get => lstSelectedItems;
            set
            {
                lstSelectedItems = value;
                
                NotifyPropertyChanged("LstSelectedItems");
            }

        }
        private DispatcherTimer _timer;
        public void ButtonScan()
        {

            ButtonBackgroundColor = Brushes.MidnightBlue;
            IsDisplayGridVisible = false;
            IsScanGridVisible = true;
            IsAddGridVisible = false;
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(0.5); // set the timer interval to 30 seconds
            _timer.Tick += Timer_Tick;
            _timer.Start();


        }

        public void ButtonDisplay()
        {
            if (!IsDisplayGridVisible)
            {
                IsDisplayGridVisible = true;
                IsScanGridVisible = false;
                IsAddGridVisible = false;
            }
            else
            {
                IsDisplayGridVisible = false;
                IsScanGridVisible = true;
                IsAddGridVisible = false;
            }
        }

        public void ButtonAdd()
        {
            if (!IsAddGridVisible)
            {
                IsDisplayGridVisible = false;
                IsScanGridVisible = false;
                IsAddGridVisible = true;
            }
            else
            {
                IsDisplayGridVisible = false;
                IsScanGridVisible = true;
                IsAddGridVisible = false;
            }
        }

        public void ButtonDelete()
        {
            foreach (var item in LstSelectedItems)
            {
                LstScanData.Remove(item);
            }

            LstScanData = new ObservableCollection<ScanData>(LstScanData);
             //ScannedBottleCount = _lstScanData.Count.ToString();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            ScannerDecoder.FindBarcodeScanner(ScannerDecoder.userSelectedPort, true);
        }
    }
   
    
}
