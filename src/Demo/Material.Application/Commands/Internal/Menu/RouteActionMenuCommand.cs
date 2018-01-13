using System;
using Material.Application.Routing;
using MaterialDesignThemes.Wpf;

namespace Material.Application.Commands
{
    internal class RouteActionMenuCommand : RouteActionCommand, IMenuCommand
    {
        public RouteActionMenuCommand(Route route, string commandText, PackIconKind? iconKind, Action execute,
            Func<bool> canExecute)
            : base(route, execute, canExecute)
        {
            CommandText = commandText;
            IconKind = iconKind;
        }

        public string CommandText { get; }

        public PackIconKind? IconKind { get; }
    }
}
