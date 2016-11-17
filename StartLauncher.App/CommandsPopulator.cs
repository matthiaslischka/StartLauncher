using System;
using System.IO;
using System.Runtime.InteropServices;
using IWshRuntimeLibrary;

namespace StartLauncher.App
{
    public class CommandsPopulator
    {
        private static CommandsPopulator _commandsPopulator;

        private CommandsPopulator()
        {
        }

        public static CommandsPopulator GetInstance()
        {
            return _commandsPopulator ?? (_commandsPopulator = new CommandsPopulator());
        }

        public void EnsureCommands()
        {
            var commands = DataAccessor.GetInstance().GetCommands();
            foreach (var command in commands)
            {
                var commandFileInfo = command.GetCommandFileInfo();

                if (commandFileInfo.Exists)
                    commandFileInfo.Delete();

                Directory.CreateDirectory(commandFileInfo.DirectoryName);

                var streamWriter = new StreamWriter(commandFileInfo.FullName);
                streamWriter.WriteLine("echo off");
                streamWriter.WriteLine("start \"\" /B " + command.Command);
                streamWriter.Close();

                AddShortcutForCommand(command);
            }
        }

        private static void AddShortcutForCommand(CommandDto command)
        {
            var commonStartMenuPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu);
            var appStartMenuPath = Path.Combine(commonStartMenuPath, "Programs", "Start Launcher");

            if (!Directory.Exists(appStartMenuPath))
                Directory.CreateDirectory(appStartMenuPath);

            var shortcutLocation = Path.Combine(appStartMenuPath, command.Name + ".lnk");

            var shell = new WshShell();
            try
            {
                var shortcut = shell.CreateShortcut(shortcutLocation);
                try
                {
                    shortcut.Description = command.Description;
                    shortcut.TargetPath = command.GetCommandFileInfo().FullName;
                    shortcut.Save();
                }
                finally
                {
                    Marshal.FinalReleaseComObject(shortcut);
                }
            }
            finally
            {
                Marshal.FinalReleaseComObject(shell);
            }
        }
    }
}