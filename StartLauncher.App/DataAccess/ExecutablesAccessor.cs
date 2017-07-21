using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Windows.Forms;
using IWshRuntimeLibrary;
using StartLauncher.App.Core;
using StartLauncher.App.Models;

namespace StartLauncher.App.DataAccess
{
    public interface IExecutablesAccessor
    {
        void EnsureCommands();
    }

    public class ExecutablesAccessor : IExecutablesAccessor
    {
        private readonly IIconResolver _iconResolver;
        private readonly ICommandsDataAccessor _commandsDataAccessor;

        public ExecutablesAccessor(IIconResolver iconResolver, ICommandsDataAccessor commandsDataAccessor)
        {
            _iconResolver = iconResolver;
            _commandsDataAccessor = commandsDataAccessor;
        }

        private static DirectoryInfo CommandsDirectory
            => new DirectoryInfo(Path.Combine(Application.UserAppDataPath, "Commands"));

        private static DirectoryInfo AppStartMenuDirectory
        {
            get
            {
                var commonStartMenuPath = Environment.GetFolderPath(Environment.SpecialFolder.StartMenu);
                return new DirectoryInfo(Path.Combine(commonStartMenuPath, "Programs", "Start Launcher", "Commands"));
            }
        }

        public void EnsureCommands()
        {
            _commandsDataAccessor.ReloadCommands();

            ClearFolder(CommandsDirectory);
            ClearFolder(AppStartMenuDirectory);

            foreach (var command in _commandsDataAccessor.Commands)
            {
                var commandFileInfo = GetCommandFileInfo(command);

                if (commandFileInfo.Exists)
                    commandFileInfo.Delete();

                Directory.CreateDirectory(CommandsDirectory.FullName);

                var streamWriter = new StreamWriter(commandFileInfo.FullName);
                streamWriter.WriteLine("echo off");
                streamWriter.WriteLine("start \"\" /B " + command.Command);
                streamWriter.Close();

                AddShortcutForCommand(command);
            }
        }

        public static FileInfo GetCommandFileInfo(CommandDto command)
        {
            var commandFileName = command.Name + ".bat";
            var commandRelativePath = Path.Combine(CommandsDirectory.FullName, commandFileName);
            return new FileInfo(commandRelativePath);
        }

        private static void ClearFolder(DirectoryInfo directory)
        {
            if (!directory.Exists)
                return;

            foreach (var file in directory.GetFiles())
                file.Delete();

            foreach (var dir in directory.GetDirectories())
                dir.Delete(true);
        }

        private void AddShortcutForCommand(CommandDto command)
        {
            if (!AppStartMenuDirectory.Exists)
            {
                var securityRules = new DirectorySecurity();
                securityRules.AddAccessRule(new FileSystemAccessRule(Environment.UserName, FileSystemRights.FullControl,
                    AccessControlType.Allow));
                AppStartMenuDirectory.Create(securityRules);
            }

            var shortcutFileInfo = new FileInfo(Path.Combine(AppStartMenuDirectory.FullName, command.Name + ".lnk"));

            var shell = new WshShell();
            try
            {
                var shortcut = shell.CreateShortcut(shortcutFileInfo.FullName);
                try
                {
                    shortcut.Description = command.Description;
                    var commandFileInfo = GetCommandFileInfo(command);
                    shortcut.TargetPath = commandFileInfo.FullName;
                    var iconUrl = _iconResolver.TryResolveIconUrl(command.Command);
                    iconUrl.IfSome(s => shortcut.IconLocation = s);

                    shortcut.Save();

                    if (command.RunAsAdmin)
                        SetShortcutToRunAsAdministrator(shortcutFileInfo);
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

        private static void SetShortcutToRunAsAdministrator(FileInfo shortcutFileInfo)
        {
            using (var fs = new FileStream(shortcutFileInfo.FullName, FileMode.Open, FileAccess.ReadWrite))
            {
                fs.Seek(21, SeekOrigin.Begin);
                fs.WriteByte(0x22);
            }
        }
    }
}