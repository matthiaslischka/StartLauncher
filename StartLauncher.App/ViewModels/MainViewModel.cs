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

        public MainViewModel(ObservableCollection<CommandDto> commands, ICommandsDataAccessor commandsDataAccessor)
        {
            _commandsDataAccessor = commandsDataAccessor;
            Commands = commands;
        }

        public ObservableCollection<CommandDto> Commands { get; set; }

        public CommandDto SelectedCommand { get; set; }

        public ICommand AddCommand => new DelegateCommand<Window>(window =>
            {
                var editWindow = new EditWindow
                {
                    DataContext = new EditViewModel(new CommandDto(), _commandsDataAccessor)
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
                    DataContext = new EditViewModel(SelectedCommand, _commandsDataAccessor)
                };
                editWindow.ShowDialog();
            }
        );

        public ICommand RemoveCommand => new DelegateCommand<Window>(window =>
            {
                if (SelectedCommand == null)
                    return;

                _commandsDataAccessor.DeleteCommand(SelectedCommand);
            }
        );
    }
}