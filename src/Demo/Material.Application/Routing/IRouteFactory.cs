using System;
using System.Collections.Generic;

namespace Material.Application.Routing
{
    public interface IRouteFactory
    {
        Route Get(Type routeType, IDictionary<string, object> parameters);

        IRouteWrapper<Route> Get(Route caller, Type routeType, IDictionary<string, object> parameters);

        TRoute Get<TRoute>(IDictionary<string, object> parameters) where TRoute : Route;

        IRouteWrapper<TRoute> Get<TRoute>(Route caller, IDictionary<string, object> parameters) where TRoute : Route;
    }

    public static class RouteFactoryExtensions
    {
        public static Route Get(this IRouteFactory routeFactory, Type routeType)
            => routeFactory.Get(routeType, null);

        public static TRoute Get<TRoute>(this IRouteFactory routeFactory) where TRoute : Route
            => routeFactory.Get<TRoute>(null);

        public static TRoute Get<TRoute>(this IRouteFactory routeFactory,
            Action<IDictionary<string, object>> parametersInitializer) where TRoute : Route
        {
            var parameters = new Dictionary<string, object>();
            parametersInitializer?.Invoke(parameters);
            return routeFactory.Get<TRoute>(parameters);
        }

        public static TRoute Get<TRoute>(this IRouteFactory routeFactory, string parameterName, object parameterValue)
            where TRoute : Route
            => routeFactory.Get<TRoute>(new Dictionary<string, object>
            {
                [parameterName] = parameterValue
            });

        public static TRoute Get<TRoute>(this IRouteFactory routeFactory, string parameter1Name, object parameter1Value,
            string parameter2Name, object parameter2Value) where TRoute : Route
            => routeFactory.Get<TRoute>(new Dictionary<string, object>
            {
                [parameter1Name] = parameter1Value,
                [parameter2Name] = parameter2Value
            });

        public static TRoute Get<TRoute>(this IRouteFactory routeFactory, string parameter1Name, object parameter1Value,
            string parameter2Name, object parameter2Value, string parameter3Name, object parameter3Value)
            where TRoute : Route
            => routeFactory.Get<TRoute>(new Dictionary<string, object>
            {
                [parameter1Name] = parameter1Value,
                [parameter2Name] = parameter2Value,
                [parameter3Name] = parameter3Value
            });

        public static TRoute Get<TRoute>(this IRouteFactory routeFactory, string parameter1Name, object parameter1Value,
            string parameter2Name, object parameter2Value, string parameter3Name, object parameter3Value,
            string parameter4Name,
            object parameter4Value) where TRoute : Route => routeFactory.Get<TRoute>(new Dictionary<string, object>
            {
                [parameter1Name] = parameter1Value,
                [parameter2Name] = parameter2Value,
                [parameter3Name] = parameter3Value,
                [parameter4Name] = parameter4Value
            });

        public static TRoute Get<TRoute>(this IRouteFactory routeFactory, string parameter1Name, object parameter1Value,
            string parameter2Name, object parameter2Value, string parameter3Name, object parameter3Value,
            string parameter4Name,
            object parameter4Value, string parameter5Name, object parameter5Value) where TRoute : Route
            => routeFactory.Get<TRoute>(new Dictionary<string, object>
            {
                [parameter1Name] = parameter1Value,
                [parameter2Name] = parameter2Value,
                [parameter3Name] = parameter3Value,
                [parameter4Name] = parameter4Value,
                [parameter5Name] = parameter5Value
            });

        public static TRoute Get<TRoute>(this IRouteFactory routeFactory, string parameter1Name, object parameter1Value,
            string parameter2Name, object parameter2Value, string parameter3Name, object parameter3Value,
            string parameter4Name, object parameter4Value, string parameter5Name, object parameter5Value,
            string parameter6Name,
            object parameter6Value) where TRoute : Route => routeFactory.Get<TRoute>(new Dictionary<string, object>
            {
                [parameter1Name] = parameter1Value,
                [parameter2Name] = parameter2Value,
                [parameter3Name] = parameter3Value,
                [parameter4Name] = parameter4Value,
                [parameter5Name] = parameter5Value,
                [parameter6Name] = parameter6Value
            });
    }
}
