using System;
using System.Windows.Input;

namespace StartLauncher.App.ViewModels
{
  public class DelegateCommand<T> : ICommand
  {
    private readonly Action<T> _action;

    public DelegateCommand(Action<T> action)
    {
      _action = action;
    }

    public void Execute(object parameter)
    {
      _action((T)parameter);
    }

    public bool CanExecute(object parameter)
    {
      return true;
    }

    public event EventHandler CanExecuteChanged
    {
      add { }
      remove { }
    }
  }
}