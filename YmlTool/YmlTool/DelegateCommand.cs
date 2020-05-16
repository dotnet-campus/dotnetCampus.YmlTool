using System;
using System.Windows.Input;

namespace YmlTool
{
    public class DelegateCommand<T> : ICommand
    {
        public DelegateCommand(Action<T> execute, Predicate<T> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }


        public bool CanExecute(T parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public void Execute(T parameter)
        {
            _execute(parameter);
        }

        public bool CanExecute(object parameter)
        {
            return CanExecute((T) parameter);
        }

        public void Execute(object parameter)
        {
            Execute((T) parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        private readonly Predicate<T> _canExecute;
        private readonly Action<T> _execute;
    }

    public class DelegateCommand : ICommand
    {
        public DelegateCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }


        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute();
        }


        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public void Execute(object parameter)
        {
            _execute();
        }

        private readonly Func<bool> _canExecute;
        private readonly Action _execute;
    }
}