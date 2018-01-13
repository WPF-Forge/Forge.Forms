using System;
using System.Windows.Threading;

namespace Material.Application.Infrastructure
{
    internal class DispatcherContext : IContext
    {
        private readonly Dispatcher dispatcher;

        public DispatcherContext(Dispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
        }

        public bool IsSynchronized => dispatcher.CheckAccess();

        public void Invoke(Action action) => dispatcher.Invoke(action);
    }
}
