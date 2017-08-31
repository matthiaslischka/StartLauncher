using System;
using System.IO;
using System.Windows.Forms;

namespace StartLauncher.App.Properties
{
    internal sealed partial class Settings
    {
        public Settings()
        {
            if (string.IsNullOrEmpty(commandsJsonFilePath))
                commandsJsonFilePath = "../commands.json";
        }
    }
}