using System.Windows.Input;

namespace Material.Application.Commands
{
    public interface IRefreshableCommand : ICommand
    {
        void RaiseCanExecuteChanged();
    }
}
