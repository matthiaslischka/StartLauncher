using System;
using System.ComponentModel;
using System.IO;
using StartLauncher.App.DataAccess;
using StartLauncher.App.Properties;

namespace StartLauncher.App.Core
{
    public class CommandsDataFileWatcher
    {
        private FileSystemWatcher _watcher;

        private CommandsDataFileWatcher()
        {
            Settings.Default.PropertyChanged += SettingsPropertyChanged;
        }

        public static CommandsDataFileWatcher Current { get; } = new CommandsDataFileWatcher();

        private void SettingsPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            CreateFileWatcher();
        }

        public void CreateFileWatcher()
        {
            var commandsFile = CommandsDataAccessor.Current.GetCommandsFile();

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

        private static void OnChanged(FileInfo commandsFile)
        {
            if (FileHelper.IsFileLocked(commandsFile))
                return;

            ExecutablesAccessor.Current.EnsureCommands();
        }
    }
}