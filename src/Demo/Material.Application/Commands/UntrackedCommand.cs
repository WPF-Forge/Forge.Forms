using System;

namespace Material.Application.Commands
{
    public class UntrackedCommand : IRefreshableCommand
    {
        private readonly Predicate<object> canExecute;
        private readonly Action<object> execute;

        public UntrackedCommand(Action<object> execute) : this(execute, null)
        {
        }

        public UntrackedCommand(Action<object> execute,
            Predicate<object> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return canExecute == null || canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            execute(parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
