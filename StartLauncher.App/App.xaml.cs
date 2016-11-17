using System.Windows;

namespace StartLauncher.App
{
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            CommandsPopulator.GetInstance().EnsureCommands();
        }
    }
}