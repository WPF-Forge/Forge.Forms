using System;
using System.Windows.Input;

namespace FancyGrid
{
    /// <summary>
    /// A hack to let you use functions or anonymous methods as commands.
    /// </summary>
    public class FunctionRunnerCommand : ICommand
    {
        private readonly Action<object> _function;

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _function(parameter);
        }

        /// <summary>
        /// Convert the code to a command
        /// </summary>
        /// <param name="function">A method, function, or lambda to be treated as a command.</param>
        public FunctionRunnerCommand(Action<object> function)
        {
            _function = function;
        }
    }
}
