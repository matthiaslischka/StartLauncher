using System;
using System.IO;

namespace StartLauncher.App
{
    public class CommandsFileWatcher
    {
        private CommandsFileWatcher()
        {
        }

        public static CommandsFileWatcher Current { get; } = new CommandsFileWatcher();

        public void CreateFileWatcher(FileInfo commandFile)
        {
            if (commandFile.Directory == null)
                throw new ArgumentNullException();

            var watcher = new FileSystemWatcher
            {
                Path = commandFile.Directory.FullName,
                NotifyFilter = NotifyFilters.LastWrite,
                Filter = commandFile.Name
            };

            watcher.Changed += (sender, e) => OnChanged(commandFile);
            watcher.Created += (sender, e) => OnChanged(commandFile);
            watcher.Deleted += (sender, e) => OnChanged(commandFile);

            watcher.EnableRaisingEvents = true;
        }

        private static void OnChanged(FileInfo commandFile)
        {
            if (IsFileLocked(commandFile))
                return;

            CommandsPopulator.Current.EnsureCommands();
        }

        private static bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (IOException)
            {
                return true;
            }
            finally
            {
                stream?.Close();
            }

            return false;
        }
    }
}