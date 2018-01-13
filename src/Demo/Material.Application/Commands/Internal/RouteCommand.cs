using System;
using System.Threading;
using Material.Application.Routing;

namespace Material.Application.Commands
{
    internal class RouteCommand<TParameter> : IRefreshableCommand where TParameter : class
    {
        private readonly Predicate<TParameter> canExecute;
        private readonly Action<TParameter> execute;
        private readonly bool ignoreNullParameters;

        public RouteCommand(Route route, Action<TParameter> execute,
            Predicate<TParameter> canExecute, bool ignoreNullParameters)
        {
            if (route == null)
            {
                throw new ArgumentNullException(nameof(route));
            }

            Route = route;
            this.execute = execute;
            this.canExecute = canExecute;
            this.ignoreNullParameters = ignoreNullParameters;
        }

        public Route Route { get; }

        public event EventHandler CanExecuteChanged;

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public bool CanExecute(object parameter)
        {
            return canExecute == null || canExecute(parameter as TParameter);
        }

        public void Execute(object parameter)
        {
            if (Route.IsTransitioning || !Route.EnableCommands)
            {
                return;
            }

            try
            {
                Interlocked.Increment(ref Route.CommandCounter);
                var param = parameter as TParameter;
                if (ignoreNullParameters && param == null)
                {
                    return;
                }

                execute(param);
            }
            catch (Exception e)
            {
                if (Route.Routes?.RouteErrorListener != null)
                {
                    Route.Routes.RouteErrorListener.OnRouteCommandException(Route, this, e);
                }
                else
                {
                    throw;
                }
            }
            finally
            {
                Interlocked.Decrement(ref Route.CommandCounter);
            }
        }
    }
}
