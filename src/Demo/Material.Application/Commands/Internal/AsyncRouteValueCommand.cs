using System;
using System.Threading;
using System.Threading.Tasks;
using Material.Application.Routing;

namespace Material.Application.Commands
{
    internal class AsyncRouteValueCommand<TParameter> : IRefreshableCommand where TParameter : struct
    {
        private readonly Predicate<object> canExecute;
        private readonly Func<TParameter, Task> execute;

        public AsyncRouteValueCommand(Route route, Func<TParameter, Task> execute,
            Predicate<object> canExecute)
        {
            if (route == null)
            {
                throw new ArgumentNullException(nameof(route));
            }

            Route = route;
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public Route Route { get; }

        public event EventHandler CanExecuteChanged;

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public bool CanExecute(object parameter)
        {
            return canExecute == null || canExecute(parameter);
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
                if (!(parameter is TParameter))
                {
                    return;
                }

                await execute((TParameter)parameter);
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
