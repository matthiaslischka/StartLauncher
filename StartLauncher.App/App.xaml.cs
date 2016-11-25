using System;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using StartLauncher.App.Core;
using StartLauncher.App.DataAccess;

namespace StartLauncher.App
{
    public partial class App
    {
        private static readonly Mutex SingleInstanceApplicationMutex = new Mutex(true,
            "{333499E4-B949-48F2-8C7C-6DFBF11ED9E1}");

        public App()
        {
            if (SingleInstanceApplicationMutex.WaitOne(TimeSpan.Zero, true))
            {
                Startup += App_Startup;
                SingleInstanceApplicationMutex.ReleaseMutex();
            }
            else
            {
                Shutdown();
            }
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