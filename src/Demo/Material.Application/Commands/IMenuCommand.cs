using MaterialDesignThemes.Wpf;

namespace Material.Application.Commands
{
    public interface IMenuCommand : IRefreshableCommand
    {
        string CommandText { get; }

        PackIconKind? IconKind { get; }
    }
}
