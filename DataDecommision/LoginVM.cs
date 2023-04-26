using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
            ButtonLoginClick = new ButtonCommandBinding(ButtonLogin)
            {
                IsEnabled = true
            };
        }

        public ButtonCommandBinding ButtonLoginClick { get; set; }

        public void ButtonLogin()
        {
            NavigationService navigationService = (NavigationService)App.Current.MainWindow.Resources["NavigationService"];
            navigationService.CurrentPage = new BulkItemPage();
        }


    }
}
