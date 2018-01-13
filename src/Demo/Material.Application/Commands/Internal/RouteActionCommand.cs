using System;
using System.Threading;
using Material.Application.Routing;

namespace Material.Application.Commands
{
    internal class RouteActionCommand : IRefreshableCommand
    {
        private readonly Func<bool> canExecute;
        private readonly Action execute;

        public RouteActionCommand(Route route, Action execute, Func<bool> canExecute)
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

        public void Execute(object parameter)
        {
            if (Route.IsTransitioning || !Route.EnableCommands)
            {
                return;
            }

            try
            {
                Interlocked.Increment(ref Route.CommandCounter);
                execute();
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
