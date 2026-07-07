using Autofac;
using StartLauncher.App.DataAccess;

namespace StartLauncher.App
{
    public class AppContainerBuilder
    {
        private readonly ContainerBuilder _containerBuilder = new ContainerBuilder();

        public AppContainerBuilder()
        {
            _containerBuilder.RegisterType<App>().SingleInstance();
            _containerBuilder.RegisterType<SimpleIconResolver>().As<IIconResolver>().SingleInstance();
            _containerBuilder.RegisterType<ExecutablesAccessor>().As<IExecutablesAccessor>().SingleInstance();
            _containerBuilder.RegisterType<CommandsDataAccessor>().As<ICommandsDataAccessor>().SingleInstance();
        }

        public IContainer Build()
        {
            return _containerBuilder.Build();
        }
    }
}