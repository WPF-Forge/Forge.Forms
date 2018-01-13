using System;
using System.Threading.Tasks;
using Material.Application.Routing;
using MaterialDesignThemes.Wpf;

namespace Material.Application.Commands
{
    internal class AsyncRouteValueMenuCommand<TParameter> : AsyncRouteValueCommand<TParameter>, IMenuCommand
        where TParameter : struct
    {
        public AsyncRouteValueMenuCommand(Route route, string commandText, PackIconKind? iconKind,
            Func<TParameter, Task> execute, Predicate<object> canExecute)
            : base(route, execute, canExecute)
        {
            CommandText = commandText;
            IconKind = iconKind;
        }

        public string CommandText { get; }

        public PackIconKind? IconKind { get; }
    }
}
