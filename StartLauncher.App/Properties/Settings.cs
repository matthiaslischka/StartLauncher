using System.IO;
using System.Windows.Forms;

namespace StartLauncher.App.Properties
{
    internal sealed partial class Settings
    {
        public Settings()
        {
            if (string.IsNullOrEmpty(commandsJsonFilePath))
                commandsJsonFilePath = Path.Combine(Application.UserAppDataPath, "commands.json");
        }
    }
}