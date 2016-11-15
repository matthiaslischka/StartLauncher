using System.Collections.Generic;
using System.IO;
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

        public List<CommandDto> LoadCommands()
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
                    new JsonSerializer().Serialize(textWriter, commands);
                }
            }
        }
    }
}