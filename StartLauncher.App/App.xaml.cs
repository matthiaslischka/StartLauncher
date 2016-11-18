using System.Windows;
using System.Windows.Threading;

namespace StartLauncher.App
{
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            CommandsPopulator.Current.EnsureCommands();
        }

        private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
#if DEBUG
            return;
#endif

            MessageBox.Show(
                "An unhandled exception just occurred.\nPlease report this issue to https://github.com/matthiaslischka/StartLauncher\nThx for contributing.\n\nFind the logfile in the application folder.\n\n:)",
                "Unhandled Exception", MessageBoxButton.OK, MessageBoxImage.Warning);
            FileLogger.GetInstance().Logger(e.Exception);
            e.Handled = true;
            Current.Shutdown();
        }
    }
}