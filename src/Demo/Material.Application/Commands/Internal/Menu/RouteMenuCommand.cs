using System;
using Material.Application.Routing;
using MaterialDesignThemes.Wpf;

namespace Material.Application.Commands
{
    internal class RouteMenuCommand<TParameter> : RouteCommand<TParameter>, IMenuCommand where TParameter : class
    {
        public RouteMenuCommand(Route route, string commandText, PackIconKind? iconKind, Action<TParameter> execute,
            Predicate<TParameter> canExecute, bool ignoreNullParameters)
            : base(route, execute, canExecute, ignoreNullParameters)
        {
            CommandText = commandText;
            IconKind = iconKind;
        }

        public string CommandText { get; }

        public PackIconKind? IconKind { get; }
    }
}
