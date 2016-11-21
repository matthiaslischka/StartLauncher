using System;
using System.IO;
using System.Linq;
using Microsoft.Win32;
using StartLauncher.App.Core;

namespace StartLauncher.App.DataAccess
{
    public class IconResolver
    {
        public static string TryResolveIconUrl(CommandDto command)
        {
            var firstWord = command.Command?.Split(' ').FirstOrDefault();

            if (string.IsNullOrEmpty(firstWord))
                return null;

            var firstWordWithExeEnding = firstWord.ToLower().EndsWith(".exe") ? firstWord : firstWord + ".exe";

            var commandPathFromRegistry = GetCommandPathFromRegistry(firstWordWithExeEnding);
            if (!string.IsNullOrEmpty(commandPathFromRegistry))
                return commandPathFromRegistry;

            var commandPathFromEnvironmentVariables = GetCommandPathFromEnvironmentVariables(firstWordWithExeEnding);
            if (!string.IsNullOrEmpty(commandPathFromEnvironmentVariables))
                return commandPathFromEnvironmentVariables;

            return null;
        }

        private static string GetCommandPathFromRegistry(string commandName)
        {
            commandName = EnsureExeEnding(commandName);
            var localMachineRegistryKey = Registry.LocalMachine;
            localMachineRegistryKey =
                localMachineRegistryKey.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths", true);
            var commandSubKey = localMachineRegistryKey.OpenSubKey(commandName);
            return commandSubKey?.GetValue("")?.ToString();
        }

        private static string GetCommandPathFromEnvironmentVariables(string commandName)
        {
            commandName = EnsureExeEnding(commandName);
            if (File.Exists(commandName))
                return Path.GetFullPath(commandName);

            var values = Environment.GetEnvironmentVariable("PATH");
            foreach (var path in values.Split(';'))
            {
                var fullPath = Path.Combine(path, commandName);
                if (File.Exists(fullPath))
                    return fullPath;
            }
            return null;
        }

        private static string EnsureExeEnding(string commandName)
        {
            if (commandName.ToLower().EndsWith(".exe"))
                return commandName;
            return commandName + ".exe";
        }
    }
}