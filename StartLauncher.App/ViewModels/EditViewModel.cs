using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using StartLauncher.App.DataAccess;
using StartLauncher.App.Models;

namespace StartLauncher.App.ViewModels
{
  public class EditViewModel : ObservableObject
  {
    private readonly ICommandsDataAccessor _commandsDataAccessor;
    private string _command;
    private string _description;
    private Guid _id;
    private string _name;
    private bool _runAsAdmin;

    public EditViewModel(CommandDto commandDto, ICommandsDataAccessor commandsDataAccessor)
    {
      _commandsDataAccessor = commandsDataAccessor;

      _id = commandDto.Id;
      _command = commandDto.Command;
      _description = commandDto.Description;
      _name = commandDto.Name;
      _runAsAdmin = commandDto.RunAsAdmin;
    }

    public Guid Id
    {
      get => _id;
      set
      {
        _id = value;
        RaisePropertyChangedEvent(nameof(Id));
      }
    }

    public string Command
    {
      get => _command;
      set
      {
        _command = value;
        RaisePropertyChangedEvent(nameof(Command));
      }
    }

    public string Name
    {
      get => _name;
      set
      {
        _name = value;
        RaisePropertyChangedEvent(nameof(Name));
      }
    }

    public string Description
    {
      get => _description;
      set
      {
        _description = value;
        RaisePropertyChangedEvent(nameof(Description));
      }
    }

    public bool RunAsAdmin
    {
      get => _runAsAdmin;
      set
      {
        _runAsAdmin = value;
        RaisePropertyChangedEvent(nameof(RunAsAdmin));
      }
    }

    public ICommand SaveCommand => new DelegateCommand<Window>(window =>
      {
        _commandsDataAccessor.SaveCommand(ToModel());
        window.Close();
      }
    );

    public ICommand TestCommand => new DelegateCommand<Window>(window =>
      {
        var process = new Process();
        var startInfo = new ProcessStartInfo
        {
          WindowStyle = ProcessWindowStyle.Hidden,
          FileName = "cmd.exe",
          Arguments = "/C start \"\" /B " + Command,
          UseShellExecute = false
        };
        process.StartInfo = startInfo;
        process.Start();
      }
    );

    public ICommand CancelCommand => new DelegateCommand<Window>(window => window.Close());

    private CommandDto ToModel()
    {
      return new CommandDto
      {
        Id = _id,
        Command = _command,
        Name = _name,
        Description = _description,
        RunAsAdmin = _runAsAdmin
      };
    }
  }
}