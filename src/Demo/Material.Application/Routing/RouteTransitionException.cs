using System;

namespace Material.Application.Routing
{
    public class RouteTransitionException : Exception
    {
        public RouteTransitionException(string message)
            : base(message)
        {
        }

        public RouteTransitionException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
