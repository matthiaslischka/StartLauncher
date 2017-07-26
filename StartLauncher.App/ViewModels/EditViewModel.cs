using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using StartLauncher.App.DataAccess;
using StartLauncher.App.Models;

namespace StartLauncher.App.ViewModels
{
    public class EditViewModel : CommandViewModel
    {
        private readonly ICommandsDataAccessor _commandsDataAccessor;

        public EditViewModel(CommandDto commandDto, ICommandsDataAccessor commandsDataAccessor) : base(commandDto)
        {
            _commandsDataAccessor = commandsDataAccessor;
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
                Id = Id,
                Command = Command,
                Name = Name,
                Description = Description,
                RunAsAdmin = RunAsAdmin
            };
        }
    }
}