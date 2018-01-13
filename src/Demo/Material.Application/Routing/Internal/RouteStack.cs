using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using Material.Application.Helpers;
using Material.Application.Infrastructure;
using Material.Application.Properties;

namespace Material.Application.Routing
{
    internal class RouteStack : IRouteStack
    {
        private readonly Stack<RouteItem> stack;
        private readonly object syncRoot = new object();
        private bool locked;

        private RouteItem routeHead;
        private object menuHeader;

        public RouteStack(ObservableCollection<Route> menuRoutes, IRouteFactory routeFactory,
            IRouteErrorListener routeErrorListener, IContext synchronizationContext)
        {
            if (routeFactory == null || routeErrorListener == null || synchronizationContext == null)
            {
                throw new ArgumentNullException();
            }

            MenuRoutes = menuRoutes ?? new ObservableCollection<Route>();
            RouteFactory = routeFactory;
            RouteErrorListener = routeErrorListener;
            SynchronizationContext = synchronizationContext;
            stack = new Stack<RouteItem>();
        }

        private RouteItem RouteHead
        {
            set
            {
                if (routeHead == value) return;
                var previous = routeHead;
                routeHead = value;
                if (value.Route != null)
                {
                    value.Route.IsTransitioning = false;
                }

                OnPropertyChanged(nameof(Current));
                OnPropertyChanged(nameof(CurrentView));
                OnHeadChanged();
                previous?.Route.NotifyPropertyChanged(nameof(Route.IsActive));
                routeHead?.Route.NotifyPropertyChanged(nameof(Route.IsActive));
            }
        }

        private LockManager Lock => new LockManager(this, true);

        private LockManager SoftLock => new LockManager(this, false);

        public event EventHandler RouteHeadChanged;

        public IRouteFactory RouteFactory { get; }

        public object MenuHeader
        {
            get { return menuHeader; }
            set
            {
                if (Equals(value, menuHeader)) return;
                menuHeader = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Route> MenuRoutes { get; }

        public IRouteErrorListener RouteErrorListener { get; }

        public IContext SynchronizationContext { get; }

        public int Count => stack.Count;

        public Route Current => routeHead?.Route;

        public object CurrentView
        {
            get
            {
                if (routeHead == null)
                {
                    return null;
                }

                return routeHead.CachedView ?? routeHead.Route;
            }
        }

        public async Task<object> Push(Route route, bool cacheCurrentView)
        {
            SynchronizationContext.VerifyAccess();

            if (route == null)
            {
                throw new ArgumentNullException(nameof(route));
            }

            if (route.Routes != null && route.Routes != this)
            {
                throw new ArgumentException(ErrorMessages.RoutesAssociatedWithOtherStack);
            }

            if (route is TransientRoute)
            {
                throw new ArgumentException("Cannot push a transient route.");
            }

            if (stack.Any(item => item.Route == route))
            {
                throw new InvalidOperationException("Cannot push same route instance multiple times.");
            }

            List<RouteEventError> errors;
            Task<object> result;
            using (Lock)
            {
                errors = new List<RouteEventError>();
                result = (await PushRoute(route, cacheCurrentView, false, errors)).Task;
            }

            await OnRouteReady(route, RouteActivationMethod.Pushed, errors);
            return await result;
        }

        public async Task Change(Route route)
        {
            SynchronizationContext.VerifyAccess();

            if (route == null)
            {
                throw new ArgumentNullException(nameof(route));
            }

            if (route == Current)
            {
                return;
            }

            if (route.Routes != null && route.Routes != this)
            {
                throw new ArgumentException(ErrorMessages.RoutesAssociatedWithOtherStack);
            }

            var completionSources = new List<TaskCompletionSource<object>>(stack.Count);
            var errors = new List<RouteEventError>();
            using (Lock)
            {
                await ChangeRoute(route, completionSources, errors);
            }

            await OnRouteReady(route, RouteActivationMethod.Changed, errors);
            foreach (var completionSource in completionSources)
            {
                completionSource.SetCanceled();
            }
        }

        public async Task<Route> Pop(object result)
        {
            SynchronizationContext.VerifyAccess();

            if (stack.Count <= 1)
            {
                throw new InvalidOperationException("Cannot pop base route.");
            }

            RouteItem poppedRoute;
            RouteItem nextRoute;
            List<RouteEventError> errors;
            using (Lock)
            {
                poppedRoute = stack.Pop();
                nextRoute = stack.Peek();
                errors = new List<RouteEventError>();

                try
                {
                    await poppedRoute.Route.OnRouteDeactivating(false);
                }
                catch (Exception ex)
                {
                    RouteErrorListener.TryOnRouteEventException(poppedRoute.Route, RouteEventType.Deactivating, ex);
                }

                try
                {
                    await nextRoute.Route.OnRouteRestoring(result);
                }
                catch (Exception ex)
                {
                    errors.Add(new RouteEventError(RouteEventType.Restoring, ex));
                    RouteErrorListener.TryOnRouteEventException(nextRoute.Route, RouteEventType.Restoring, ex);
                }

                poppedRoute.CachedView = null;
                var route = nextRoute.Route;
                if (nextRoute.CachedView == null)
                {
                    try
                    {
                        var view = route.CreateView(false);
                        nextRoute.CachedView = view;
                        var frameworkElement = view as FrameworkElement;
                        if (frameworkElement != null && frameworkElement.DataContext == null)
                        {
                            frameworkElement.DataContext = route;
                        }
                    }
                    catch (Exception ex)
                    {
                        errors.Add(new RouteEventError(RouteEventType.ViewCreation, ex));
                        RouteErrorListener.TryOnRouteEventException(route, RouteEventType.ViewCreation, ex);
                    }
                }

                RouteHead = nextRoute;

                try
                {
                    await poppedRoute.Route.OnRouteDeactivated(false);
                }
                catch (Exception ex)
                {
                    RouteErrorListener.TryOnRouteEventException(poppedRoute.Route, RouteEventType.Deactivated, ex);
                }

                try
                {
                    await nextRoute.Route.OnRouteRestored(result);
                }
                catch (Exception ex)
                {
                    errors.Add(new RouteEventError(RouteEventType.Restored, ex));
                    RouteErrorListener.TryOnRouteEventException(nextRoute.Route, RouteEventType.Restored, ex);
                }
            }

            await OnRouteReady(nextRoute.Route, RouteActivationMethod.Restored, errors);
            poppedRoute.CompletionSource.SetResult(result);
            return nextRoute.Route;
        }

        public void ReloadView(Route route)
        {
            if (route == null)
            {
                return;
            }

            using (SoftLock)
            {
                if (routeHead == null)
                {
                    return;
                }

                if (SynchronizationContext.IsSynchronized)
                {
                    ReloadViewInternal(route);
                }
                else
                {
                    SynchronizationContext.Invoke(() => ReloadViewInternal(route));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void ReloadViewInternal(Route route)
        {
            if (routeHead.Route == route)
            {
                try
                {
                    var view = route.CreateView(true);
                    routeHead.CachedView = view;
                    var frameworkElement = view as FrameworkElement;
                    if (frameworkElement != null && frameworkElement.DataContext == null)
                    {
                        frameworkElement.DataContext = route;
                    }

                    OnPropertyChanged(nameof(CurrentView));
                }
                catch (Exception ex)
                {
                    RouteErrorListener.TryOnRouteEventException(route, RouteEventType.ViewCreation, ex);
                }
            }
            else
            {
                var routeItem = stack.FirstOrDefault(item => item.Route == route);
                if (routeItem != null)
                {
                    routeItem.CachedView = null;
                }
            }
        }

        private async Task ChangeRoute(Route route, List<TaskCompletionSource<object>> completionSources,
            List<RouteEventError> errors)
        {
            var sentinel = 0;
            while (true)
            {
                if (++sentinel == 32)
                {
                    throw new RouteTransitionException("Detected possible loop with transient routes.");
                }

                var transientRoute = route as TransientRoute;
                if (transientRoute != null)
                {
                    transientRoute.Routes = this;
                    var transientRouteErrors = new List<RouteEventError>();
                    try
                    {
                        await transientRoute.OnRouteInitializing();
                    }
                    catch (Exception ex)
                    {
                        transientRouteErrors.Add(new RouteEventError(RouteEventType.Initializing, ex));
                    }

                    try
                    {
                        await transientRoute.OnRouteActivating();
                    }
                    catch (Exception ex)
                    {
                        transientRouteErrors.Add(new RouteEventError(RouteEventType.Activating, ex));
                    }

                    try
                    {
                        route = transientRoute.GetNextRoute(transientRouteErrors);
                    }
                    catch (Exception ex)
                    {
                        throw new RouteTransitionException(
                            "A transient route threw an exception while switching to next route.", ex);
                    }

                    if (route == null)
                    {
                        throw new RouteTransitionException("A transient route resulted in a dead end.");
                    }

                    if (route.Routes != null && route.Routes != this)
                    {
                        throw new RouteTransitionException(ErrorMessages.RoutesAssociatedWithOtherStack);
                    }

                    continue;
                }

                while (stack.Count != 0)
                {
                    var item = stack.Pop();
                    completionSources.Add(item.CompletionSource);
                    try
                    {
                        await item.Route.OnRouteDeactivating(true);
                    }
                    catch (Exception ex)
                    {
                        RouteErrorListener.TryOnRouteEventException(item.Route, RouteEventType.Deactivating, ex);
                    }

                    item.CachedView = null;

                    try
                    {
                        await item.Route.OnRouteDeactivated(true);
                    }
                    catch (Exception ex)
                    {
                        RouteErrorListener.TryOnRouteEventException(item.Route, RouteEventType.Deactivated, ex);
                    }
                }

                await PushRoute(route, false, true, errors);
                break;
            }
        }

        private async Task<TaskCompletionSource<object>> PushRoute(Route route, bool cacheCurrentView,
            bool ignoreCurrent, List<RouteEventError> errors)
        {
            route.Routes = this;

            var currentRoute = Current;
            if (currentRoute != null && !cacheCurrentView)
            {
                routeHead.CachedView = null;
            }

            if (!ignoreCurrent && currentRoute != null)
            {
                try
                {
                    await currentRoute.OnRouteHiding();
                }
                catch (Exception ex)
                {
                    RouteErrorListener.TryOnRouteEventException(currentRoute, RouteEventType.Hiding, ex);
                }
            }

            try
            {
                await route.OnRouteInitializing();
            }
            catch (Exception ex)
            {
                errors.Add(new RouteEventError(RouteEventType.Initializing, ex));
                RouteErrorListener.TryOnRouteEventException(route, RouteEventType.Initializing, ex);
            }

            try
            {
                await route.OnRouteActivating();
            }
            catch (Exception ex)
            {
                errors.Add(new RouteEventError(RouteEventType.Activating, ex));
                RouteErrorListener.TryOnRouteEventException(route, RouteEventType.Activating, ex);
            }

            var item = new RouteItem(route);
            try
            {
                var view = route.CreateView(false);
                item.CachedView = view;
                var frameworkElement = view as FrameworkElement;
                if (frameworkElement != null && frameworkElement.DataContext == null)
                {
                    frameworkElement.DataContext = route;
                }
            }
            catch (Exception ex)
            {
                errors.Add(new RouteEventError(RouteEventType.ViewCreation, ex));
                RouteErrorListener.TryOnRouteEventException(route, RouteEventType.ViewCreation, ex);
            }

            stack.Push(item);
            RouteHead = item;

            if (!ignoreCurrent && currentRoute != null)
            {
                try
                {
                    await currentRoute.OnRouteHidden();
                }
                catch (Exception ex)
                {
                    RouteErrorListener.TryOnRouteEventException(currentRoute, RouteEventType.Hidden, ex);
                }
            }

            try
            {
                await route.OnRouteInitialized();
            }
            catch (Exception ex)
            {
                errors.Add(new RouteEventError(RouteEventType.Initialized, ex));
                RouteErrorListener.TryOnRouteEventException(route, RouteEventType.Initialized, ex);
            }

            try
            {
                await route.OnRouteActivated();
            }
            catch (Exception ex)
            {
                errors.Add(new RouteEventError(RouteEventType.Activated, ex));
                RouteErrorListener.TryOnRouteEventException(route, RouteEventType.Activated, ex);
            }

            return item.CompletionSource;
        }

        private async Task OnRouteReady(Route route, RouteActivationMethod method, List<RouteEventError> errors)
        {
            try
            {
                await route.OnRouteReady(method, errors);
            }
            catch (Exception ex)
            {
                RouteErrorListener.TryOnRouteEventException(route, RouteEventType.Ready, ex);
            }
        }

        private void EnterLock(bool hardLock)
        {
            // This is old code because locking isn't necessary since we're synchronized.
            lock (syncRoot)
            {
                if (locked)
                {
                    throw new InvalidOperationException("A route change is already in progress.");
                }

                if (hardLock)
                {
                    Current?.BeginTransition();
                }

                locked = true;
            }
        }

        private void ExitLock()
        {
            lock (syncRoot)
            {
                locked = false;
            }
        }

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void OnHeadChanged()
        {
            try
            {
                RouteHeadChanged?.Invoke(this, EventArgs.Empty);
            }
            catch
            {
                // ignored
            }
        }

        private class LockManager : IDisposable
        {
            private readonly RouteStack routeStack;

            public LockManager(RouteStack routeStack, bool hardLock)
            {
                routeStack.EnterLock(hardLock);
                this.routeStack = routeStack;
            }

            public void Dispose()
            {
                routeStack.ExitLock();
            }
        }
    }
}
