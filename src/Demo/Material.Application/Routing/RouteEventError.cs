using System;

namespace Material.Application.Routing
{
    public class RouteEventError
    {
        public RouteEventError(RouteEventType routeEventType, Exception exception)
        {
            RouteEventType = routeEventType;
            Exception = exception;
        }

        public RouteEventType RouteEventType { get; }

        public Exception Exception { get; }
    }
}
