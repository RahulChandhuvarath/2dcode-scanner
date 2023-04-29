using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    
        public ScanDisplayVM() 
        {
           

            ButtonScanClick = new ButtonCommandBinding(ButtonScan)
            {
                IsEnabled = true
            };

            List<ScanData> lstsd = new List<ScanData>();
            ScanData sd1 = new ScanData("2025-03-03", "A1234", "12345", "B1234");
            ScanData sd2 = new ScanData("2025-10-03", "D1234", "12345", "C1234");
            _lstScanData.Add(sd1);
            _lstScanData.Add(sd2);
            LstScanData = new ObservableCollection<ScanData>(_lstScanData);
        }
        public ButtonCommandBinding ButtonScanClick { get; set; }

        private ObservableCollection<ScanData> _lstScanData = new ObservableCollection<ScanData>();
        public ObservableCollection<ScanData> LstScanData
        {
            get => _lstScanData;
            set
            {
                _lstScanData = value;
                NotifyPropertyChanged("LstScanData");
            }
        }

        public void ButtonScan() {
          
        }
    }

    public class ScanData
    {
        public ScanData(string strExp, string strLot, string strGtin, string strSerial)
        {
            Expdate = strExp;
            LotNumber = strLot;
            GTIN = strGtin;
            SerialNumber = strSerial;

        }

        public string Expdate { get; set; }
        public string LotNumber { get; set; }
        public string GTIN { get; set; }
        public string SerialNumber { get; set; }

    }
}
