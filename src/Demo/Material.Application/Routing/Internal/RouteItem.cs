using System.Threading.Tasks;

namespace Material.Application.Routing
{
    internal class RouteItem
    {
        public RouteItem(Route route)
        {
            Route = route;
            CompletionSource = new TaskCompletionSource<object>();
        }

        public Route Route { get; }

        public TaskCompletionSource<object> CompletionSource { get; }

        public object CachedView { get; set; }
    }
}
