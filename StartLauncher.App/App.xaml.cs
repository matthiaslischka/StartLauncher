using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Autofac;
using StartLauncher.App.DataAccess;
using StartLauncher.App.ViewModels;
using StartLauncher.App.Views;
using Velopack;
using Velopack.Sources;

namespace StartLauncher.App
{
	public partial class App
	{
		private readonly ICommandsDataAccessor _commandsDataAccessor;
		private readonly IExecutablesAccessor _executablesAccessor;

		public App(IExecutablesAccessor executablesAccessor, ICommandsDataAccessor commandsDataAccessor)
		{
			Startup += App_Startup;

			_executablesAccessor = executablesAccessor;
			_commandsDataAccessor = commandsDataAccessor;
		}

		[STAThread]
		public static void Main()
		{
			VelopackApp.Build().Run();

			EnsureAppUpToDate().ContinueWith(t => Console.Error.WriteLine(t.Exception),
				TaskContinuationOptions.OnlyOnFaulted);

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
				DataContext = new MainViewModel(_commandsDataAccessor.Commands, _commandsDataAccessor, _executablesAccessor)
			}.Show();
		}

		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);
			_executablesAccessor.EnsureCommands();
		}

		private static async Task EnsureAppUpToDate()
		{
			var mgr = new UpdateManager(new GithubSource("https://github.com/matthiaslischka/StartLauncher", null, false));

			//so you can run app from Dev Environment
			if (!mgr.IsInstalled)
				return;

			var updateInfo = await mgr.CheckForUpdatesAsync();
			if (updateInfo == null)
				return;

			Console.Out.WriteLine($"Found Update {updateInfo.TargetFullRelease.Version}.");
			await mgr.DownloadUpdatesAsync(updateInfo, i => Console.Out.WriteLine($"Updating: {i}"));
			Console.Out.WriteLine("Update Finished. Restarting to launch new Version.");
			mgr.ApplyUpdatesAndRestart(updateInfo.TargetFullRelease);
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

		private void App_OnExit(object sender, ExitEventArgs e)
		{
		}
	}
}