using System;
using System.Collections.Generic;
using System.ComponentModel;
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
            loginCredentials = Common.GetCredentials();
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

        private string userStatus;
      

        private string textPassword;
        public string TextPassword
        {
            get { return textPassword; }
            set { textPassword = value; NotifyPropertyChanged("TextPassword"); }
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
        

            NavigationService navigationService = (NavigationService)App.Current.MainWindow.Resources["NavigationService"];
            navigationService.CurrentPage = new BulkItemPage();
        }


    }


}
