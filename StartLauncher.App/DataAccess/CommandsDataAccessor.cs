using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using Newtonsoft.Json;
using StartLauncher.App.Core;
using StartLauncher.App.Models;
using StartLauncher.App.Properties;

namespace StartLauncher.App.DataAccess
{
    public interface ICommandsDataAccessor
    {
        ObservableCollection<CommandDto> Commands { get; }
        FileInfo GetCommandsFile();
        void SaveCommands(ObservableCollection<CommandDto> commands);
        void ChangeCommandsJsonFilePath(string path);
        void ReloadCommands();
        void SaveCommand(CommandDto command);
        void DeleteCommand(CommandDto command);
    }

    public class CommandsDataAccessor : ICommandsDataAccessor
    {
        public CommandsDataAccessor()
        {
            Commands = new ObservableCollection<CommandDto>();
        }

        public ObservableCollection<CommandDto> Commands { get; }

        public FileInfo GetCommandsFile()
        {
            return new FileInfo(Settings.Default.commandsJsonFilePath);
        }

        public void ChangeCommandsJsonFilePath(string path)
        {
            var newCommandsFile = new FileInfo(path);
            var oldCommandsFile = GetCommandsFile();
            Settings.Default.commandsJsonFilePath = newCommandsFile.FullName;
            Settings.Default.Save();
            SaveCommands(Commands);
            oldCommandsFile.Delete();
        }

        public void ReloadCommands()
        {
            var commandsFile = GetCommandsFile();
            if (!commandsFile.Exists)
            {
                Application.Current.Dispatcher.Invoke(delegate { Commands.Clear(); });
                return;
            }

            //todo cleanup
            using (var fs = new FileStream(commandsFile.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (var streamReader = new StreamReader(fs))
                {
                    using (var jsonTextReader = new JsonTextReader(streamReader))
                    {
                        try
                        {
                            Application.Current.Dispatcher.Invoke(delegate { Commands.Clear(); });

                            var commandDtos = new JsonSerializer().Deserialize<List<CommandDto>>(jsonTextReader) ??
                                              new List<CommandDto>();
                            Application.Current.Dispatcher.Invoke(delegate
                            {
                                Commands.Clear();
                                commandDtos.ForEach(Commands.Add);
                            });
                        }
                        catch (JsonReaderException)
                        {
                        }
                    }
                }
            }
        }

        public void SaveCommands(ObservableCollection<CommandDto> commands)
        {
            using (var streamWriter = new StreamWriter(GetCommandsFile().FullName))
            {
                using (var textWriter = new JsonTextWriter(streamWriter))
                {
                    textWriter.Formatting = Formatting.Indented;
                    new JsonSerializer().Serialize(textWriter, commands);
                }
            }
        }

        public void DeleteCommand(CommandDto command)
        {
            Application.Current.Dispatcher.Invoke(delegate { Commands.Remove(c => c.Id == command.Id); });
            SaveCommands(Commands);
        }

        public void SaveCommand(CommandDto command)
        {
            var persistingCommand = Commands.SingleOrDefault(c => c.Id == command.Id);
            if (persistingCommand == null)
            {
                persistingCommand = new CommandDto();
                Application.Current.Dispatcher.Invoke(delegate { Commands.Add(persistingCommand); });
            }
            persistingCommand.Name = command.Name;
            persistingCommand.Command = command.Command;
            persistingCommand.Description = command.Description;
            persistingCommand.RunAsAdmin = command.RunAsAdmin;
            SaveCommands(Commands);
        }
    }
}