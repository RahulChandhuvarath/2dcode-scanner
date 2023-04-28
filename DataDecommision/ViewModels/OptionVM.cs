using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataDecommision
{
    internal class OptionVM
    {
        public OptionVM() 
        {
            ButtonDecomClick = new ButtonCommandBinding(ButtonDecom)
            {
                IsEnabled = true
            };

            ButtonCheckClick = new ButtonCommandBinding(ButtonCheck)
            {
                IsEnabled = true
            };
        }

        public ButtonCommandBinding ButtonDecomClick { get; set; }

        public ButtonCommandBinding ButtonCheckClick { get; set; }
        public void ButtonDecom()
        {
           
            NavigationService navigationService = (NavigationService)App.Current.MainWindow.Resources["NavigationService"];
            navigationService.CurrentPage = new BulkItemPage();
        }

        public void ButtonCheck()
        {

            //NavigationService navigationService = (NavigationService)App.Current.MainWindow.Resources["NavigationService"];
            //navigationService.CurrentPage = new BulkItemPage();
        }
    }
}
