using System;
using StartLauncher.App.Models;

namespace StartLauncher.App.ViewModels
{
    public class CommandViewModel : ObservableObject
    {
        private string _command;
        private string _description;
        private Guid _id;
        private string _name;
        private bool _runAsAdmin;

        public CommandViewModel(CommandDto commandDto)
        {
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
    }
}