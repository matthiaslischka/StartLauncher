using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using StartLauncher.App.DataAccess;

namespace StartLauncher.App.ViewModels
{
  public class SettingsViewModel : INotifyPropertyChanged
  {
    private readonly ICommandsDataAccessor _commandsDataAccessor;

    private string _commandsJsonFilePath;

    public SettingsViewModel(ICommandsDataAccessor commandsDataAccessor)
    {
      _commandsDataAccessor = commandsDataAccessor;
      _commandsJsonFilePath = commandsDataAccessor.GetCommandsFile().FullName;
    }

    public string CommandsJsonFilePath
    {
      get => _commandsJsonFilePath;
      set
      {
        _commandsJsonFilePath = value;
        RaisePropertyChanged(nameof(CommandsJsonFilePath));
      }
    }

    public ICommand SaveCommand => new DelegateCommand<Window>(window =>
      {
        _commandsDataAccessor.ChangeCommandsJsonFilePath(CommandsJsonFilePath);
        window.Close();
      }
    );

    public ICommand CancelCommand => new DelegateCommand<Window>(window => window.Close());

    public event PropertyChangedEventHandler PropertyChanged;

    protected void RaisePropertyChanged(string propertyName)
    {
      var handler = PropertyChanged;
      handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}