using System;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using Autofac;
using StartLauncher.App.DataAccess;
using StartLauncher.App.ViewModels;
using StartLauncher.App.Views;

namespace StartLauncher.App
{
    public partial class App
    {
        private static readonly Mutex SingleInstanceApplicationMutex = new Mutex(true,
            "{333499E4-B949-48F2-8C7C-6DFBF11ED9E1}");

        private readonly ICommandsDataAccessor _commandsDataAccessor;
        private readonly IExecutablesAccessor _executablesAccessor;

        public App(IExecutablesAccessor executablesAccessor, ICommandsDataAccessor commandsDataAccessor)
        {
            EnsureApplicationRunsJustOnce();

            _executablesAccessor = executablesAccessor;
            _commandsDataAccessor = commandsDataAccessor;
        }

        private void EnsureApplicationRunsJustOnce()
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
            new MainWindow
            {
                DataContext = new MainViewModel(_commandsDataAccessor.Commands, _commandsDataAccessor)
            }.Show();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            _executablesAccessor.EnsureCommands();
        }

        private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(
                "An unhandled exception just occurred.\nPlease report this issue to https://github.com/matthiaslischka/StartLauncher\nThx for contributing.\n\nSee StdErr output for more information.\n\n:)",
                "Unhandled Exception", MessageBoxButton.OK, MessageBoxImage.Warning);
            Console.Error.WriteLine(e.Exception);
            e.Handled = true;
            Current.Shutdown();
        }
    }
}