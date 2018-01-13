using System;
using Material.Application.Routing;
using MaterialDesignThemes.Wpf;

namespace Material.Application.Commands
{
    internal class RouteValueMenuCommand<TParameter> : RouteValueCommand<TParameter>, IMenuCommand
        where TParameter : struct
    {
        public RouteValueMenuCommand(Route route, string commandText, PackIconKind? iconKind, Action<TParameter> execute,
            Predicate<object> canExecute)
            : base(route, execute, canExecute)
        {
            CommandText = commandText;
            IconKind = iconKind;
        }

        public string CommandText { get; }

        public PackIconKind? IconKind { get; }
    }
}
