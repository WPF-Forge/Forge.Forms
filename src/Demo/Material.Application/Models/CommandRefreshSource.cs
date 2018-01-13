using System.Collections;
using System.Collections.Generic;
using Material.Application.Commands;

namespace Material.Application.Models
{
    public class CommandRefreshSource : IEnumerable<IRefreshableCommand>
    {
        private readonly List<IRefreshableCommand> commands = new List<IRefreshableCommand>();

        public IEnumerator<IRefreshableCommand> GetEnumerator() => commands.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Add(IRefreshableCommand command)
        {
            if (command != null && !commands.Contains(command))
            {
                commands.Add(command);
            }
        }

        public bool Remove(IRefreshableCommand command) => commands.Remove(command);

        public void Refresh()
        {
            foreach (var command in commands)
            {
                command.RaiseCanExecuteChanged();
            }
        }
    }
}
