using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Material.Application.Infrastructure;

namespace Material.Application.Routing
{
    public interface IRouteStack : INotifyPropertyChanged
    {
        IRouteErrorListener RouteErrorListener { get; }

        IRouteFactory RouteFactory { get; }

        IContext SynchronizationContext { get; }

        object MenuHeader { get; set; }

        ObservableCollection<Route> MenuRoutes { get; }

        int Count { get; }

        Route Current { get; }

        object CurrentView { get; }

        event EventHandler RouteHeadChanged;

        Task<object> Push(Route route, bool cacheCurrentView);

        Task Change(Route route);

        Task<Route> Pop(object result);

        void ReloadView(Route route);
    }

    public static class RouteStackExtensions
    {
        public static Route FindMenuRoute(this IRouteStack routeStack, Func<Route, bool> filter)
            => routeStack.MenuRoutes.First(route => route != null && filter(route));

        public static TRoute FindMenuRoute<TRoute>(this IRouteStack routeStack) where TRoute : Route
            => (TRoute)routeStack.MenuRoutes.First(route => route is TRoute);

        public static TRoute FindMenuRoute<TRoute>(this IRouteStack routeStack, Func<TRoute, bool> filter)
            where TRoute : Route => (TRoute)routeStack.MenuRoutes.First(route =>
            {
                var tRoute = route as TRoute;
                return tRoute != null && filter(tRoute);
            });
    }
}
