using System;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using Autofac;
using StartLauncher.App.Core;
using StartLauncher.App.DataAccess;
using StartLauncher.App.UI;

namespace StartLauncher.App
{
    public partial class App
    {
        private static readonly Mutex SingleInstanceApplicationMutex = new Mutex(true,
            "{333499E4-B949-48F2-8C7C-6DFBF11ED9E1}");

        private readonly ICommandsDataAccessor _commandsDataAccessor;
        private readonly ICommandsDataFileWatcher _commandsDataFileWatcher;
        private readonly IExecutablesAccessor _executablesAccessor;

        public App(IExecutablesAccessor executablesAccessor, ICommandsDataFileWatcher commandsDataFileWatcher,
            ICommandsDataAccessor commandsDataAccessor)
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

            _executablesAccessor = executablesAccessor;
            _commandsDataFileWatcher = commandsDataFileWatcher;
            _commandsDataAccessor = commandsDataAccessor;
        }

        [STAThread]
        public static void Main()
        {
            var appContainerBuilder = new AppContainerBuilder();
            var appContainer = appContainerBuilder.Build();
            var application = appContainer.Resolve<App>();
            application.InitializeComponent();
            application.Run();
        }

        private void App_Startup(object sender, StartupEventArgs e)
        {
            new MainWindow(_commandsDataAccessor).Show();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            _executablesAccessor.EnsureCommands();
            _commandsDataFileWatcher.CreateFileWatcher();
        }

        private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
#if DEBUG
            return;
#endif

            MessageBox.Show(
                "An unhandled exception just occurred.\nPlease report this issue to https://github.com/matthiaslischka/StartLauncher\nThx for contributing.\n\nFind the logfile in the application folder.\n\n:)",
                "Unhandled Exception", MessageBoxButton.OK, MessageBoxImage.Warning);
            FileLogger.Current.Log(e.Exception);
            e.Handled = true;
            Current.Shutdown();
        }
    }
}