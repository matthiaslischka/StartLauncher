using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Autofac;
using Squirrel;
using StartLauncher.App.DataAccess;
using StartLauncher.App.ViewModels;
using StartLauncher.App.Views;

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
			var assembly = Assembly.GetEntryAssembly();
			var updateDotExe = Path.Combine(Path.GetDirectoryName(assembly.Location), "..", "Update.exe");
			var isInstalled = File.Exists(updateDotExe);

			//so you can run app from Dev Environment
			if (!isInstalled)
				return;

			var updated = false;

			using (var mgr = UpdateManager.GitHubUpdateManager("https://github.com/myuser/myapp"))
			{
				await mgr.Result.UpdateApp(i => Console.Out.WriteLine($"Updating: {i}"));
			}

			using (var mgr = UpdateManager.GitHubUpdateManager("https://github.com/matthiaslischka/StartLauncher").Result)
			{
				var updateInfo = await mgr.CheckForUpdate();
				if (updateInfo.ReleasesToApply.Any())
				{
					Console.Out.WriteLine($"Found Update {updateInfo.FutureReleaseEntry.Version}.");
					await mgr.UpdateApp(i => Console.Out.WriteLine($"Updating: {i}"));
					Console.Out.WriteLine("Update Finished.");
					updated = true;
				}
			}

			if (updated)
			{
				Console.Out.WriteLine("Restarting to launch new Version.");
				UpdateManager.RestartApp();
			}
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