using System;
using System.Threading;
using System.Threading.Tasks;
using Material.Application.Routing;

namespace Material.Application.Commands
{
    internal class AsyncRouteActionCommand : IRefreshableCommand
    {
        private readonly Func<bool> canExecute;
        private readonly Func<Task> execute;

        public AsyncRouteActionCommand(Route route, Func<Task> execute,
            Func<bool> canExecute)
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

        public bool CanExecute(object parameter)
        {
            return canExecute == null || canExecute();
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
                await execute();
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
