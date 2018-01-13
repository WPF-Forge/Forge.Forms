using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Material.Application.Commands;
using Material.Application.Controls;
using Material.Application.Properties;
using Material.Application.Routing;
using MaterialDesignThemes.Wpf;
using Ninject;
using Ninject.Modules;

namespace Material.Application.Infrastructure
{
    public abstract class AppController : INotifyPropertyChanged, IRouteErrorListener,
        IMainWindowLocator, IMainWindowController
    {
        private static int dialogId;

        private readonly int id;

        private double fontSize = 13d;
        private bool initialized;
        private bool isMenuOpen;
        private bool lockToggle;
        private ICommand menuCommand;
        private string title;
        private bool toggleState;

        protected AppController()
        {
            id = Interlocked.Increment(ref dialogId);
            MenuCommand = new UntrackedCommand(async parameter =>
            {
                if (Routes.Count <= 1)
                {
                    IsMenuOpen = !IsMenuOpen;
                }
                else
                {
                    var current = Routes.Current;
                    if (current.IsTransitioning || !current.IsOutsideTransitionReady)
                    {
                        return;
                    }

                    var handler = current.DeactivateRequested;
                    var popRoute = handler == null || await handler();
                    if (popRoute)
                    {
                        await Routes.Pop(null);
                    }
                }
            });

            SnackbarMessageQueue = new SnackbarMessageQueue();
            Kernel = new StandardKernel();
        }

        protected internal IKernel Kernel { get; }

        protected Route InitialRoute { get; set; }

        public IRouteStack Routes { get; private set; }

        public ISnackbarMessageQueue SnackbarMessageQueue { get; }

        public bool LockToggle
        {
            get { return lockToggle; }
            private set
            {
                if (value == lockToggle) return;
                lockToggle = value;
                OnPropertyChanged();
            }
        }

        public bool ToggleState
        {
            get { return toggleState; }
            private set
            {
                if (value == toggleState) return;
                toggleState = value;
                OnPropertyChanged();
            }
        }

        public ICommand MenuCommand
        {
            get { return menuCommand; }
            private set
            {
                if (Equals(value, menuCommand)) return;
                menuCommand = value;
                OnPropertyChanged();
            }
        }

        public MaterialRoutesWindow Window { get; private set; }

        protected internal string HostIdentifer => "RouteController" + id;

        public bool IsMenuOpen
        {
            get { return isMenuOpen; }
            set
            {
                if (value == isMenuOpen) return;
                isMenuOpen = value;
                ToggleState = value;
                OnPropertyChanged();
            }
        }

        public string Title
        {
            get { return title; }
            set
            {
                if (value == title) return;
                title = value;
                OnPropertyChanged();
            }
        }

        public double FontSize
        {
            get { return fontSize; }
            set
            {
                if (value.Equals(fontSize)) return;
                fontSize = value;
                OnPropertyChanged();
            }
        }

        public MaterialRoutesWindow GetMainWindow() => Window;

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnRouteEventException(Route route, RouteEventType eventType, Exception exception)
        {
        }

        public virtual void OnRouteCommandException(Route route, ICommand command, Exception exception)
        {
        }

        public void Exit()
        {
            Window?.Close();
        }

        public void ShowApplicationWindow()
        {
            if (Window != null)
            {
                throw new InvalidOperationException("Application window has already been shown for current controller.");
            }

            Initialize();
            if (InitialRoute == null)
            {
                throw new InvalidOperationException("No initial route has been specified.");
            }

            Window = new MaterialRoutesWindow(this)
            {
                RootDialog = { Identifier = HostIdentifer }
            };

            Window.Closing += OnWindowClosing;
            Window.Show();
            var initialRoute = InitialRoute;
            InitialRoute = null;
            Routes.Change(initialRoute);
        }

        private async void OnWindowClosing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            var close = await CloseRequested();
            if (close)
            {
                try
                {
                    Routes.Current.ApplicationShuttingDown();
                }
                catch
                {
                    // ignored
                }

                System.Windows.Application.Current.Shutdown();
            }
        }

        protected virtual Task<bool> CloseRequested()
        {
            return Task.FromResult(true);
        }

        protected virtual void OnInitializing()
        {
        }

        protected virtual void OnInitialized()
        {
        }

        protected virtual Route GetInitialRoute() => InitialRoute ?? Routes.MenuRoutes.First();

        protected virtual IEnumerable<INinjectModule> LoadModules()
        {
            return null;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Initialize()
        {
            if (initialized)
            {
                return;
            }

            Kernel.Load(new DefaultAppModule(this));
            var modules = LoadModules();
            if (modules != null)
            {
                Kernel.Load(modules);
            }

            Routes = Kernel.Get<IRouteStack>();
            Routes.RouteHeadChanged += (sender, args) =>
            {
                IsMenuOpen = false;
                ToggleState = LockToggle = Routes.Count > 1;
            };

            Kernel.Inject(this);
            OnInitializing();
            foreach (var route in Routes.MenuRoutes)
            {
                if (route == null)
                {
                    continue;
                }

                route.Routes = Routes;
            }

            InitialRoute = GetInitialRoute();
            InitialRoute.Routes = Routes;
            OnInitialized();
            initialized = true;
        }
    }
}
