using System;
using Material.Application.Routing;

namespace Material.Application.Helpers
{
    internal static class RouteErrorListenerExtensions
    {
        public static void TryOnRouteEventException(this IRouteErrorListener listener, Route route,
            RouteEventType eventType, Exception exception)
        {
            if (listener == null)
            {
                return;
            }

            try
            {
                listener.OnRouteEventException(route, eventType, exception);
            }
            catch
            {
                // ignored
            }
        }
    }
}
