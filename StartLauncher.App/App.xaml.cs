using System.Windows;
using System.Windows.Threading;
using StartLauncher.App.Core;
using StartLauncher.App.DataAccess;

namespace StartLauncher.App
{
    public partial class App
    {
        public App()
        {
            Startup += App_Startup;
        }

        private void App_Startup(object sender, StartupEventArgs e)
        {
            UI.MainWindow.Current.Show();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ExecutablesAccessor.Current.EnsureCommands();
            CommandsDataFileWatcher.Current.CreateFileWatcher();
        }

        private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
#if DEBUG
            return;
#endif

            MessageBox.Show(
                "An unhandled exception just occurred.\nPlease report this issue to https://github.com/matthiaslischka/StartLauncher\nThx for contributing.\n\nFind the logfile in the application folder.\n\n:)",
                "Unhandled Exception", MessageBoxButton.OK, MessageBoxImage.Warning);
            FileLogger.GetInstance().Log(e.Exception);
            e.Handled = true;
            Current.Shutdown();
        }
    }
}