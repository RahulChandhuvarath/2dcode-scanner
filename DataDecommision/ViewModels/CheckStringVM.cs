using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Threading;

namespace DataDecommision
{
    internal class CheckStringVM : INotifyPropertyChanged
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

        private static CheckStringVM _instance;


        public static CheckStringVM Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new CheckStringVM();
                }
                return _instance;
            }
        }
        private CheckStringVM()
        {
            ButtonCheckClick = new ButtonCommandBinding(ButtonCheck)
            {
                IsEnabled = true
            };
          
            ButtonBackClick = new ButtonCommandBinding(ButtonBack)
            {
                IsEnabled = true
            };

            TextMatching = "0";
            TextNotMatching = "0";
            ButtonBackgroundColor = Brushes.DarkGreen;
        }

        public void Clear()
        {
            TextMatching = "0";
            TextNotMatching = "0";
            TextString = "";
            TextLength = "";
            ButtonBackgroundColor = Brushes.DarkGreen;
    }
        private string textString;
        public string TextString
        {
            get { return textString; }
            set { textString = value; NotifyPropertyChanged("TextString"); }
        }

        private string textLength;
        public string TextLength
        {
            get { return textLength; }
            set { textLength = value; NotifyPropertyChanged("TextLength"); }
        }

        private string textMatching;
        public string TextMatching
        {
            get { return textMatching; }
            set { textMatching = value; NotifyPropertyChanged("TextMatching"); }
        }

        private string textNotMatching;
        public string TextNotMatching
        {
            get { return textNotMatching; }
            set { textNotMatching = value; NotifyPropertyChanged("TextNotMatching"); }
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

        public ButtonCommandBinding ButtonBackClick { get; set; }
        public ButtonCommandBinding ButtonCheckClick { get; set; }

        private DispatcherTimer _timer;
        public void ButtonCheck()
        {
            ScanStart = true;
            ScannerDecoder.serialportOpen = true;
            ButtonBackgroundColor = Brushes.MidnightBlue;
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(5); // set the timer interval to 5 milliseconds
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        private bool ScanStart = false;
        public void ButtonBack()
        {

            NavigationService navigationService = (NavigationService)App.Current.MainWindow.Resources["NavigationService"];
            navigationService.CurrentPage = new OptionPage();
            CheckStringVM.Instance.Clear();
            ScanStart = false;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (ScanStart)
            {
                if(ScannerDecoder.serialportOpen)
                    ScannerDecoder.FindBarcodeScanner(ScannerDecoder.userSelectedPort, 2);
            }

        }
    }
}
