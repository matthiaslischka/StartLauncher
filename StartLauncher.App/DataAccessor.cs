using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace StartLauncher.App
{
    public class DataAccessor
    {
        public const string CommandsJsonFile = @"commands.json";

        private static DataAccessor _dataAccessor;

        public static DataAccessor GetInstance()
        {
            return _dataAccessor ?? (_dataAccessor = new DataAccessor());
        }

        public List<CommandDto> GetCommands()
        {
            if (!File.Exists(CommandsJsonFile)) return new List<CommandDto>();

            using (var streamReader = new StreamReader(CommandsJsonFile))
            {
                using (var jsonTextReader = new JsonTextReader(streamReader))
                {
                    try
                    {
                        return new JsonSerializer().Deserialize<List<CommandDto>>(jsonTextReader);
                    }
                    catch (JsonReaderException)
                    {
                    }
                    catch (JsonSerializationException)
                    {
                    }
                }
            }
            return new List<CommandDto>();
        }

        public void SaveCommands(List<CommandDto> commands)
        {
            using (var streamWriter = new StreamWriter(CommandsJsonFile))
            {
                using (var textWriter = new JsonTextWriter(streamWriter))
                {
                    textWriter.Formatting = Formatting.Indented;
                    new JsonSerializer().Serialize(textWriter, commands);
                }
            }

            new CommandsPopulator().EnsureCommands();
        }

        public void DeleteCommand(CommandDto command)
        {
            var commands = GetCommands();
            commands.RemoveAll(c => c.Id == command.Id);
            SaveCommands(commands);

            new CommandsPopulator().EnsureCommands();
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

            new CommandsPopulator().EnsureCommands();
        }
    }
}