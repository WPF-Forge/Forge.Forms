using System;
using System.Threading.Tasks;
using Material.Application.Helpers;

namespace Material.Application.Routing
{
    internal class RouteWrapperInternal<TRoute> : IRouteWrapper<TRoute> where TRoute : Route
    {
        public RouteWrapperInternal(Route caller, TRoute route)
        {
            if (caller == null)
            {
                throw new ArgumentNullException(nameof(caller));
            }

            if (caller.Routes == null)
            {
                throw new ArgumentException(ErrorMessages.MustHaveRoutes);
            }

            if (route == null)
            {
                throw new ArgumentNullException(nameof(route));
            }

            Caller = caller;
            Route = route;
            if (route.Routes == null)
            {
                route.Routes = caller.Routes;
            }
        }

        internal Route Caller { get; }

        public TRoute Route { get; }

        public Task<object> Push(bool cacheCurrentView)
        {
            if (Caller.Routes.Current != Caller)
            {
                throw new InvalidOperationException(ErrorMessages.MustBeActiveRoute);
            }

            return Caller.Routes.Push(Route, cacheCurrentView);
        }

        public Task Change()
        {
            if (Caller.Routes.Current != Caller)
            {
                throw new InvalidOperationException(ErrorMessages.MustBeActiveRoute);
            }

            return Caller.Routes.Change(Route);
        }
    }
}
