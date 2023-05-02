using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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

        public ButtonCommandBinding ButtonDisplayClick { get; set; }

        public ButtonCommandBinding ButtonAddClick { get; set; }

        public ButtonCommandBinding ButtonDeleteClick { get; set; }
        public ButtonCommandBinding ButtonScanClick { get; set; }

        public ButtonCommandBinding ButtonFinishClick { get; set; }

        public ButtonCommandBinding ButtonConfirmClick { get; set; }

        public ButtonCommandBinding ButtonQuitClick { get; set; }

        public ButtonCommandBinding ButtonPassConfirmClick { get; set; }

        public ButtonCommandBinding ButtonPassQuitClick { get; set; }

        public ButtonCommandBinding ButtonModifyClick { get; set; }

        public ButtonCommandBinding ButtonAPHClick { get; set; }

        public ButtonCommandBinding ButtonOtherClick { get; set; }
        private ScanDisplayVM()
        {
            SelectedDate = null;
            ButtonBackgroundColor = Brushes.DarkGreen;
            IsTextReadonly = true;
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

            ButtonFinishClick = new ButtonCommandBinding(ButtonFinish)
            {
                IsEnabled = true
            };

            ButtonConfirmClick = new ButtonCommandBinding(ButtonConfirm)
            {
                IsEnabled = true
            };

            ButtonQuitClick = new ButtonCommandBinding(ButtonQuit)
            {
                IsEnabled = true
            };

            ButtonPassConfirmClick = new ButtonCommandBinding(ButtonPassConfirm)
            {
                IsEnabled = true
            };

            ButtonPassQuitClick = new ButtonCommandBinding(ButtonPassQuit)
            {
                IsEnabled = true
            };

            ButtonModifyClick = new ButtonCommandBinding(ButtonModify)
            {
                IsEnabled = true
            };

            ButtonAPHClick = new ButtonCommandBinding(ButtonAPH)
            {
                IsEnabled = true
            };

            ButtonOtherClick = new ButtonCommandBinding(ButtonOther)
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

        private bool isTextReadonly;
        public bool IsTextReadonly
        {
            get { return isTextReadonly; }
            set
            {
                isTextReadonly = value;
                NotifyPropertyChanged(nameof(IsTextReadonly));
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
        private bool isTextFocus;
        public bool IsTextFocus
        {
            get { return isTextFocus; }
            set { isTextFocus = value; NotifyPropertyChanged("IsTextFocus"); }
        }

        private bool bottlePopup;
        public bool BottlePopup
        {
            get { return bottlePopup; }
            set { bottlePopup = value; NotifyPropertyChanged("BottlePopup"); }
        }

        private bool passwordPopup;
        public bool PasswordPopup
        {
            get { return passwordPopup; }
            set { passwordPopup = value; NotifyPropertyChanged("PasswordPopup"); }
        }

        private bool customerPopup;
        public bool CustomerPopup
        {
            get { return customerPopup; }
            set { customerPopup = value; NotifyPropertyChanged("CustomerPopup"); }
        }
        public void ButtonFinish()
        {
            string match = "NOT Matching";
            if (ScannedBottleCount != DecomData.BulkBottle)
            {
                string msg = "Total Bottles Scanned:" + ScannedBottleCount + "\nTotal Bottles Entered:" + DecomData.BulkBottle + "\nTotal Bottles are:" + match + "\nPlease confirm the total Bottles!";
                MessageBoxResult result = MessageBox.Show(msg, "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    BottlePopup = true;
                    Application.Current.MainWindow.IsEnabled = false;
                }
            }
            else
            {
                CustomerPopup = true;
                Application.Current.MainWindow.IsEnabled = false;

            }
        }

        public void ButtonConfirm()
        {
            BottlePopup = false;
            Application.Current.MainWindow.IsEnabled = true;
            PasswordPopup = true;
            Application.Current.MainWindow.IsEnabled = false;
            PasswordType = 0;
        }
        public void ButtonQuit()
        {
            BottlePopup = false;
            Application.Current.MainWindow.IsEnabled = true;
        }

        private static int PasswordType = 0;
        public void ButtonPassConfirm()
        {
            PasswordPopup = false;
            Application.Current.MainWindow.IsEnabled = true;

            if(PasswordType == 0)
            {
                CustomerPopup = true;
                Application.Current.MainWindow.IsEnabled = false;
            }
        }
        public void ButtonPassQuit()
        {
            PasswordPopup = false;
            Application.Current.MainWindow.IsEnabled = true;
        }

        public void ButtonAPH()
        {
            CustomerPopup = false;
            Application.Current.MainWindow.IsEnabled = true;
            MessageBox.Show("XML file created succesfully!!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        public void ButtonOther()
        {
            CustomerPopup = false;
            Application.Current.MainWindow.IsEnabled = true;
            MessageBox.Show("XML file created succesfully!!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        public void ButtonModify()
        {
            IsTextReadonly = false;
            IsTextFocus = true;
            PasswordPopup = true;
            Application.Current.MainWindow.IsEnabled = false;
            PasswordType = 1;
        }

        public void ButtonDelete()
        {
            if (LstSelectedItems == null || LstSelectedItems.Count() == 0)
                return;
            PasswordPopup = true;
            Application.Current.MainWindow.IsEnabled = false;
            PasswordType = 2;
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
