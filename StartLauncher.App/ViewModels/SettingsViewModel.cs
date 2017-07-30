using System.Windows;
using System.Windows.Input;
using StartLauncher.App.DataAccess;

namespace StartLauncher.App.ViewModels
{
  public class SettingsViewModel : ObservableObject
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
        RaisePropertyChangedEvent(nameof(CommandsJsonFilePath));
      }
    }

    public ICommand SaveCommand => new DelegateCommand<Window>(window =>
      {
        _commandsDataAccessor.ChangeCommandsJsonFilePath(CommandsJsonFilePath);
        window.Close();
      }
    );

    public ICommand CancelCommand => new DelegateCommand<Window>(window => window.Close());
  }
}