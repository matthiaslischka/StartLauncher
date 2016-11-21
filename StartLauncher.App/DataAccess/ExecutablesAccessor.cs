﻿using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using IWshRuntimeLibrary;
using StartLauncher.App.Core;

namespace StartLauncher.App.DataAccess
{
    public class ExecutablesAccessor
    {
        private ExecutablesAccessor()
        {
        }

        public static ExecutablesAccessor Current { get; } = new ExecutablesAccessor();

        private static DirectoryInfo CommandsDirectory { get; } = new DirectoryInfo("Commands");

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
            CommandsDataAccessor.Current.ReloadCommands();

            ClearFolder(CommandsDirectory);
            ClearFolder(AppStartMenuDirectory);

            foreach (var command in CommandsDataAccessor.Current.Commands)
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

        private static void AddShortcutForCommand(CommandDto command)
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

                    var iconUrl = IconResolver.TryResolveIconUrl(command);
                    if (!string.IsNullOrEmpty(iconUrl))
                        shortcut.IconLocation = iconUrl;

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