using System.Collections.Generic;
using System.Windows;

namespace StartLauncher.App
{
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

# if DEBUG

            var commandDto = new CommandDto
            {
                Name = "New Google",
                Command = @"""C:\Program Files\Internet Explorer\iexplore.exe"" -noframemerging https://www.google.com",
                Description = @"Open Google.com in new Internet Explorer session"
            };
            DataAccessor.GetInstance().SaveCommands(new List<CommandDto> {commandDto});

#endif

            var commandsPopulator = new CommandsPopulator();
            commandsPopulator.EnsureCommands();
        }
    }
}