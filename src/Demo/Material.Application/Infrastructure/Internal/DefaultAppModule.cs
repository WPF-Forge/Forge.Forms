using System.Collections.ObjectModel;
using System.Diagnostics;
using Material.Application.Routing;
using Ninject.Modules;

namespace Material.Application.Infrastructure
{
    internal class DefaultAppModule : NinjectModule
    {
        private readonly AppController appController;

        public DefaultAppModule(AppController appController)
        {
            this.appController = appController;
        }

        public override void Load()
        {
            // Infrastructure
            Bind<IServiceLocator>().ToConstant(new NinjectServiceLocator(appController.Kernel));
            Bind<AppController>().ToConstant(appController);
            Bind<IMainWindowController>().ToConstant(appController);
            Bind<IMainWindowLocator>().ToConstant(appController);

            var dispatcher = System.Windows.Application.Current?.Dispatcher;
            if (dispatcher != null)
            {
                Bind<IContext>().ToConstant(new DispatcherContext(dispatcher));
            }
            else
            {
                Debug.WriteLine("Warning: Using deferred dispatcher resolution.");
                Bind<IContext>().ToMethod(ctx => new DispatcherContext(System.Windows.Application.Current.Dispatcher));
            }

            // Routes
            Bind<IRouteErrorListener>().ToConstant(appController);

            Bind<IRouteFactory>()
                .To<ServiceLocatorRouteFactory>();

            Bind<IRouteStack>()
                .To<RouteStack>()
                .InSingletonScope()
                .WithConstructorArgument("menuRoutes", new ObservableCollection<Route>());

            // Services
            Bind<ILocalizationService>()
                .To<XamlLocalizationService>()
                .InSingletonScope();

            Bind<IPaletteService>()
                .To<PaletteService>()
                .InSingletonScope();

            Bind<INotificationService>()
                .To<SnackbarNotificationService>()
                .InSingletonScope()
                .WithConstructorArgument("snackbarMessageQueue", appController.SnackbarMessageQueue);

            Bind<IDialogService>()
                .To<DialogHostService>();

            Bind<IFilePicker>()
                .To<DialogFilePicker>();
            Bind<IFileSaver>()
                .To<DialogFileSaver>();
        }
    }
}
