
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DataDecommision
{
  

    public class NavigationService : INotifyPropertyChanged
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
    
        private static readonly NavigationService instance = new NavigationService();

        public static NavigationService Instance
        {
            get { return instance; }
        }

        private object currentPage;

        public object CurrentPage
        {
            get { return currentPage; }
            set { currentPage = value; NotifyPropertyChanged(nameof(CurrentPage)); }
        }

        public void Navigate(object page)
        {
            CurrentPage = page;
            // Do the actual navigation here, for example:
            // Application.Current.MainWindow.Content = page;
        }
    }
}
