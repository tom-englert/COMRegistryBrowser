using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace COMRegistryBrowser
{
    internal class DelegateCommand : ICommand
    {
        public Predicate<object> CanExecuteCallback { get; set; }
        public Action<object> ExecuteCallback { get; set; }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        public DelegateCommand()
            : this(null, null)
        {
        }

        public DelegateCommand(Action<object> execute)
            : this(execute, null)
        {
        }

        public DelegateCommand(Action<object> execute, Predicate<object> canExecute)
        {
            this.ExecuteCallback = execute;
            this.CanExecuteCallback = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            if (CanExecuteCallback == null)
            {
                return true;
            }

            return CanExecuteCallback(parameter);
        }

        public void Execute(object parameter)
        {
            if (ExecuteCallback != null)
            {
                ExecuteCallback(parameter);
            }
        }
    }
}
