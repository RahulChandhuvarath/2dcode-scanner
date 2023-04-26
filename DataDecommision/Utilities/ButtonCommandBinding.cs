using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DataDecommision
{
    internal class ButtonCommandBinding : ICommand
    {
        //Delegate command to register method to be executed
        private readonly Action handler;
        private bool isEnabled;
        public event EventHandler CanExecuteChanged;
        private readonly Func<bool> _canExecute;
        /// <summary>
        /// Bind method to execute the handler
        /// </summary>
        /// <param name="handler"></param>
        public ButtonCommandBinding(Action handler, Func<bool> canExecute = null)
        {
            this.handler = handler;
            _canExecute = canExecute;
        }

        //Enable property
        public  bool IsEnabled
        {
            get { return isEnabled; }
            set
            {
                if(value != isEnabled)
                {
                    isEnabled = value;
                    CanExecuteChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Method to specify if the event will execute
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            return _canExecute?.Invoke() ?? true;
        }

        //calls the repective method that registered with handler
        public void Execute(object parameter)
        {
            //calls the repective method that registered with handler
            handler();
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
