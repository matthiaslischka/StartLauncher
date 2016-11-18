using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace StartLauncher.App
{
    public class DataAccessor
    {
        private ObservableCollection<CommandDto> Commands { get; set; }

        private DataAccessor()
        {
        }

        public static DataAccessor Current => new DataAccessor();

        public FileInfo GetCommandsFile()
        {
            return new FileInfo(@"commands.json");
        }

        public List<CommandDto> GetCommands()
        {
            if (!GetCommandsFile().Exists) return new List<CommandDto>();

            using (var streamReader = new StreamReader(GetCommandsFile().FullName))
            {
                using (var jsonTextReader = new JsonTextReader(streamReader))
                {
                    return new JsonSerializer().Deserialize<List<CommandDto>>(jsonTextReader) ?? new List<CommandDto>();
                }
            }
        }

        public void SaveCommands(List<CommandDto> commands)
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
            var commands = GetCommands();
            commands.RemoveAll(c => c.Id == command.Id);
            SaveCommands(commands);
        }

        public void SaveCommand(CommandDto command)
        {
            var commands = GetCommands();
            var persistingCommand = commands.SingleOrDefault(c => c.Id == command.Id);
            if (persistingCommand == null)
            {
                persistingCommand = new CommandDto();
                commands.Add(persistingCommand);
            }
            persistingCommand.Name = command.Name;
            persistingCommand.Command = command.Command;
            persistingCommand.Description = command.Description;
            SaveCommands(commands);
        }
    }
}