using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public CheckStringVM()
        {
            ButtonCheckClick = new ButtonCommandBinding(ButtonCheck)
            {
                IsEnabled = true
            };
          
            ButtonBackClick = new ButtonCommandBinding(ButtonBack)
            {
                IsEnabled = true
            };

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

        private string textResult;
        public string TextResult
        {
            get { return textResult; }
            set { textResult = value; NotifyPropertyChanged("TextResult"); }
        }
        public ButtonCommandBinding ButtonBackClick { get; set; }
        public ButtonCommandBinding ButtonCheckClick { get; set; }
        public void ButtonCheck()
        {

        }
        public void ButtonBack()
        {

            NavigationService navigationService = (NavigationService)App.Current.MainWindow.Resources["NavigationService"];
            navigationService.CurrentPage = new OptionPage();
        }
    }
}
