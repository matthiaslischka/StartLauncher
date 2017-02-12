using System;
using System.ComponentModel;
using System.IO;
using StartLauncher.App.DataAccess;
using StartLauncher.App.Properties;

namespace StartLauncher.App.Core
{
    public interface ICommandsDataFileWatcher
    {
        void CreateFileWatcher();
    }

    public class CommandsDataFileWatcher : ICommandsDataFileWatcher
    {
        private readonly IExecutablesAccessor _executablesAccessor;
        private readonly ICommandsDataAccessor _commandsDataAccessor;
        private FileSystemWatcher _watcher;

        public CommandsDataFileWatcher(IExecutablesAccessor executablesAccessor, ICommandsDataAccessor commandsDataAccessor)
        {
            _executablesAccessor = executablesAccessor;
            _commandsDataAccessor = commandsDataAccessor;
            Settings.Default.PropertyChanged += SettingsPropertyChanged;
        }

        private void SettingsPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            CreateFileWatcher();
        }

        public void CreateFileWatcher()
        {
            var commandsFile = _commandsDataAccessor.GetCommandsFile();

            if (commandsFile.Directory == null)
                throw new ArgumentNullException();

            _watcher = new FileSystemWatcher
            {
                Path = commandsFile.Directory.FullName,
                NotifyFilter = NotifyFilters.LastWrite,
                Filter = commandsFile.Name
            };

            _watcher.Changed += (sender, e) => OnChanged(commandsFile);

            _watcher.EnableRaisingEvents = true;
        }

        private void OnChanged(FileInfo commandsFile)
        {
            _executablesAccessor.EnsureCommands();
        }
    }
}