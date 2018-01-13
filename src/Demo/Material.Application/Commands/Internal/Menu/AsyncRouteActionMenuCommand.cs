using System;
using System.Threading.Tasks;
using Material.Application.Routing;
using MaterialDesignThemes.Wpf;

namespace Material.Application.Commands
{
    internal class AsyncRouteActionMenuCommand : AsyncRouteActionCommand, IMenuCommand
    {
        public AsyncRouteActionMenuCommand(Route route, string commandText, PackIconKind? iconKind, Func<Task> execute,
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
