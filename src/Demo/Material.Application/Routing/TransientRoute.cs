using System.Collections.Generic;
using System.Threading.Tasks;
using Material.Application.Properties;

namespace Material.Application.Routing
{
    public abstract class TransientRoute : Route
    {
        private Route caller;

        /// <summary>
        /// Gets the route that requested the creation of this object.
        /// </summary>
        public Route Caller
        {
            get { return caller; }
            protected internal set
            {
                if (Equals(value, caller)) return;
                caller = value;
                NotifyPropertyChanged();
            }
        }

        protected internal abstract Route GetNextRoute([NotNull] IEnumerable<RouteEventError> errors);

        protected sealed override void RouteInitialized()
        {
        }

        protected sealed override Task RouteInitializedAsync() => Task.CompletedTask;

        protected sealed override void RouteActivated()
        {
        }

        protected sealed override Task RouteActivatedAsync() => Task.CompletedTask;

        protected sealed override void RouteHiding()
        {
        }

        protected sealed override Task RouteHidingAsync() => Task.CompletedTask;

        protected sealed override void RouteHidden()
        {
        }

        protected sealed override Task RouteHiddenAsync() => Task.CompletedTask;

        protected sealed override void RouteRestoring(object result)
        {
        }

        protected sealed override Task RouteRestoringAsync(object result) => Task.CompletedTask;

        protected sealed override void RouteRestored(object result)
        {
        }

        protected sealed override Task RouteRestoredAsync(object result) => Task.CompletedTask;

        protected sealed override void RouteDeactivating(bool isChanging)
        {
        }

        protected sealed override Task RouteDeactivatingAsync(bool isChanging) => Task.CompletedTask;

        protected sealed override void RouteDeactivated(bool isChanging)
        {
        }

        protected sealed override Task RouteDeactivatedAsync(bool isChanging) => Task.CompletedTask;

        protected sealed override void RouteReady(RouteActivationMethod routeActivationMethod,
            IEnumerable<RouteEventError> errors)
        {
        }

        protected sealed override Task RouteReadyAsync(RouteActivationMethod routeActivationMethod,
            IEnumerable<RouteEventError> errors) => Task.CompletedTask;

        protected internal sealed override object CreateView(bool isReload) => null;
    }
}
