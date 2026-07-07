using System;
using System.Windows.Input;

namespace StartLauncher.App.ViewModels
{
  public class DelegateCommand<T> : ICommand
  {
    private readonly Action<T> _action;
    private readonly Func<T, bool> _canExecute;

    public DelegateCommand(Action<T> action) : this(action, null)
    {
    }

    public DelegateCommand(Action<T> action, Func<T, bool> canExecute)
    {
      _action = action;
      _canExecute = canExecute;
    }

    public void Execute(object parameter)
    {
      _action((T)parameter);
    }

    public bool CanExecute(object parameter)
    {
      return _canExecute == null || _canExecute((T)parameter);
    }

    public event EventHandler CanExecuteChanged
    {
      add { CommandManager.RequerySuggested += value; }
      remove { CommandManager.RequerySuggested -= value; }
    }
  }
}
