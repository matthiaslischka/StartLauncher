using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using Newtonsoft.Json;

namespace StartLauncher.App
{
    public class DataAccessor
    {
        private DataAccessor()
        {
            Commands = new ObservableCollection<CommandDto>();
        }

        public ObservableCollection<CommandDto> Commands { get; }

        public static DataAccessor Current { get; } = new DataAccessor();

        public FileInfo GetCommandsFile()
        {
            return new FileInfo(@"commands.json");
        }

        public void ReloadCommands()
        {
            if (!GetCommandsFile().Exists)
            {
                Commands.Clear();
                return;
            }

            using (var streamReader = new StreamReader(GetCommandsFile().FullName))
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
            SaveCommands(Commands);
        }
    }
}