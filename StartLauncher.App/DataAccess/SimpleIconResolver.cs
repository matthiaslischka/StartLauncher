﻿using System;
using System.IO;
using System.Linq;
using FluentOptionals;
using Microsoft.Win32;
using StartLauncher.App.Core;

namespace StartLauncher.App.DataAccess
{
    public interface IIconResolver
    {
        Optional<string> TryResolveIconUrl(string command);
    }

    public class SimpleIconResolver : IIconResolver
    {
        public Optional<string> TryResolveIconUrl(string command)
        {
            var firstWord = command?.Split(' ').FirstOrDefault();

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
                localMachineRegistryKey.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths");
            var commandSubKey = localMachineRegistryKey.OpenSubKey(commandName);

            var commandPath = commandSubKey?.GetValue("")?.ToString();
            return commandPath.ToOptional();
        }

        private static Optional<string> GetCommandPathFromEnvironmentVariables(string commandName)
        {
            commandName = EnsureExeEnding(commandName);
            if (File.Exists(commandName))
                return Path.GetFullPath(commandName);

            var values = Environment.GetEnvironmentVariable("PATH");
            foreach (var path in values.Split(';'))
                try
                {
                    var fullPath = Path.Combine(path, commandName);
                    if (File.Exists(fullPath))
                        return fullPath;
                }
                catch (ArgumentException)
                {
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