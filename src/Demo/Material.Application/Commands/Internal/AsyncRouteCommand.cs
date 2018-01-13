using System;
using System.Threading;
using System.Threading.Tasks;
using Material.Application.Routing;

namespace Material.Application.Commands
{
    internal class AsyncRouteCommand<TParameter> : IRefreshableCommand where TParameter : class
    {
        private readonly Predicate<TParameter> canExecute;
        private readonly Func<TParameter, Task> execute;
        private readonly bool ignoreNullParameters;

        public AsyncRouteCommand(Route route, Func<TParameter, Task> execute,
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

        public bool CanExecute(object parameter)
        {
            return canExecute == null || canExecute(parameter as TParameter);
        }

        public async void Execute(object parameter)
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

                await execute(param);
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

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
