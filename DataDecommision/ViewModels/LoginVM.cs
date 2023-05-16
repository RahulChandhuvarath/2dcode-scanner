using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace DataDecommision
{
    internal class LoginVM : INotifyPropertyChanged
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
        public LoginVM()
        {
            loginCredentials = Common.GetCredentialsExcel();
            AllPorts = SerialPort.GetPortNames();
            if (AllPorts.Count() > 0)
                SelectedPort = AllPorts[0];
            else
                PortStatus = "No Active Ports Found!!";
            ButtonLoginClick = new ButtonCommandBinding(ButtonLogin)
            {
                IsEnabled = true
            };
        }

        public string UserStatus
        {
            get { return userStatus; }
            set { userStatus = value; NotifyPropertyChanged("UserStatus"); }
        }

        private string textUserName;
        public string TextUserName
        {
            get { return textUserName; }
            set { textUserName = value; NotifyPropertyChanged("TextUserName"); }
        }

        private string passStatus;
        public string PassStatus
        {
            get { return passStatus; }
            set { passStatus = value; NotifyPropertyChanged("PassStatus"); }
        }

        private string portStatus;
        public string PortStatus
        {
            get { return portStatus; }
            set { portStatus = value; NotifyPropertyChanged("PortStatus"); }
        }

        private string userStatus;
      

        private string textPassword;
        public string TextPassword
        {
            get { return textPassword; }
            set { textPassword = value; NotifyPropertyChanged("TextPassword"); }
        }

        private string selectedPort;
        public string SelectedPort
        {
            get { return selectedPort; }
            set { selectedPort = value; NotifyPropertyChanged("SelectedPort"); }
        }


        private string[] allPorts;
        public string[] AllPorts
        {
            get { return allPorts; }
            set { allPorts = value; NotifyPropertyChanged("AllPorts"); }
        }

        public ButtonCommandBinding ButtonLoginClick { get; set; }

        private static Dictionary<string, string> loginCredentials = new Dictionary<string, string>();
        public void ButtonLogin()
        {
           
            if (TextUserName ==null || !loginCredentials.Keys.Contains(TextUserName))
            {
                UserStatus = "Invalid Username!!";
                return;
            }
            else
            {
                UserStatus = "";
            }

            if (TextPassword == null  || loginCredentials[TextUserName] != TextPassword)
            {
                PassStatus = "Wrong Password!!";
                return;
            }
            else
            {
                PassStatus = "";
            }
            if (SelectedPort == null || SelectedPort == "")
                return;

            DecomData.UserName = TextUserName;
            DecomData.Password = TextPassword;
            ScannerDecoder.userSelectedPort = SelectedPort;
            NavigationService navigationService = (NavigationService)App.Current.MainWindow.Resources["NavigationService"];
            navigationService.CurrentPage = new OptionPage();
        }


    }


}
