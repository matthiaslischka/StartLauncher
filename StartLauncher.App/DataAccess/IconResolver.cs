using System;
using System.IO;
using System.Linq;
using FluentOptionals;
using Microsoft.Win32;
using StartLauncher.App.Core;

namespace StartLauncher.App.DataAccess
{
    public class IconResolver
    {
        public static Optional<string> TryResolveIconUrl(CommandDto command)
        {
            var firstWord = command.Command?.Split(' ').FirstOrDefault();

            if (string.IsNullOrEmpty(firstWord))
                return Optional.None<string>();

            var firstWordWithExeEnding = firstWord.ToLower().EndsWith(".exe") ? firstWord : firstWord + ".exe";

            var commandPathFromRegistry = GetCommandPathFromRegistry(firstWordWithExeEnding);
            if (commandPathFromRegistry.IsSome)
                return commandPathFromRegistry;

            var commandPathFromEnvironmentVariables = GetCommandPathFromEnvironmentVariables(firstWordWithExeEnding);
            return commandPathFromEnvironmentVariables;
        }

        private static Optional<string> GetCommandPathFromRegistry(string commandName)
        {
            commandName = EnsureExeEnding(commandName);
            var localMachineRegistryKey = Registry.LocalMachine;
            localMachineRegistryKey =
                localMachineRegistryKey.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths", true);
            var commandSubKey = localMachineRegistryKey.OpenSubKey(commandName);
            return commandSubKey?.GetValue("")?.ToString() ?? Optional.None<string>();
        }

        private static Optional<string> GetCommandPathFromEnvironmentVariables(string commandName)
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
            return Optional.None<string>();
        }

        private static string EnsureExeEnding(string commandName)
        {
            if (commandName.ToLower().EndsWith(".exe"))
                return commandName;
            return commandName + ".exe";
        }
    }
}