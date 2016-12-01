using Autofac;
using StartLauncher.App.Core;
using StartLauncher.App.DataAccess;
using StartLauncher.App.UI;

namespace StartLauncher.App
{
    public class AppContainerBuilder : ContainerBuilder
    {
        public AppContainerBuilder()
        {
            this.RegisterType<App>().SingleInstance();
            this.RegisterType<SimpleIconResolver>().As<IIconResolver>().SingleInstance();
            this.RegisterType<ExecutablesAccessor>().As<IExecutablesAccessor>().SingleInstance();
            this.RegisterType<CommandsDataFileWatcher>().As<ICommandsDataFileWatcher>().SingleInstance();
            this.RegisterType<CommandsDataAccessor>().As<ICommandsDataAccessor>().SingleInstance();

        }
    }
}