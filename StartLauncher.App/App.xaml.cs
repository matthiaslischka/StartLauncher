using System.Collections.Generic;
using System.Windows;

namespace StartLauncher.App
{
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var commandsPopulator = new CommandsPopulator();
            commandsPopulator.EnsureCommands();
        }
    }
}