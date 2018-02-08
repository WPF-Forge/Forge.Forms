using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Windows.Input;

namespace FancyGrid
{
    /// <summary>
    /// A hack to let you use functions or anonymous methods as commands.
    /// </summary>
    public class FunctionRunnerCommand : ICommand
    {
        /// <summary>
        /// Convert the code to a command
        /// </summary>
        /// <param name="function">A method, function, or lambda to be treated as a command.</param>
        public FunctionRunnerCommand(Action<object> function)
        {
            _function = function;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            _function(parameter);
        }

        private Action<object> _function;
    }
}
