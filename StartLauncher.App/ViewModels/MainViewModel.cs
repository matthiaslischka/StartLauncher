using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using StartLauncher.App.DataAccess;
using StartLauncher.App.Models;
using StartLauncher.App.Views;

namespace StartLauncher.App.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        private readonly ICommandsDataAccessor _commandsDataAccessor;
        private readonly IExecutablesAccessor _executablesAccessor;

        public MainViewModel(ObservableCollection<CommandDto> commands, ICommandsDataAccessor commandsDataAccessor, IExecutablesAccessor executablesAccessor)
        {
            _commandsDataAccessor = commandsDataAccessor;
            _executablesAccessor = executablesAccessor;
            Commands = commands;
        }

        public ObservableCollection<CommandDto> Commands { get; set; }

        private CommandDto _selectedCommand;

        public CommandDto SelectedCommand
        {
            get => _selectedCommand;
            set
            {
                _selectedCommand = value;
                RaisePropertyChangedEvent(nameof(SelectedCommand));
            }
        }

        public ICommand AddCommand => new DelegateCommand<Window>(window =>
            {
                var editWindow = new EditWindow
                {
                    Owner = window,
                    DataContext = new EditViewModel(new CommandDto(), _commandsDataAccessor, _executablesAccessor)
                };
                editWindow.ShowDialog();
            }
        );

        public ICommand EditCommand => new DelegateCommand<Window>(window =>
            {
                if (SelectedCommand == null)
                    return;

                var editWindow = new EditWindow
                {
                    Owner = window,
                    DataContext = new EditViewModel(SelectedCommand, _commandsDataAccessor, _executablesAccessor)
                };
                editWindow.ShowDialog();
            },
            window => SelectedCommand != null
        );

        public ICommand RemoveCommand => new DelegateCommand<Window>(window =>
            {
                if (SelectedCommand == null)
                    return;

                var result = MessageBox.Show(window,
                    $"Remove \"{SelectedCommand.Name}\"? This deletes its shortcut and batch file.",
                    "Remove Command",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question,
                    MessageBoxResult.No);

                if (result != MessageBoxResult.Yes)
                    return;

                _commandsDataAccessor.DeleteCommand(SelectedCommand);
            },
            window => SelectedCommand != null
        );
    }
}