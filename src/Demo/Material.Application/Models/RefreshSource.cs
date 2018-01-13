using Material.Application.Commands;

namespace Material.Application.Models
{
    public class RefreshSource
    {
        public RefreshSource(Model model)
        {
            Properties = new PropertyRefreshSource(model);
            Commands = new CommandRefreshSource();
        }

        public PropertyRefreshSource Properties { get; }

        public CommandRefreshSource Commands { get; }

        public void Refresh()
        {
            Properties.Refresh();
            Commands.Refresh();
        }

        public RefreshSource WithProperties(params string[] properties)
        {
            if (properties != null)
            {
                foreach (var property in properties)
                {
                    Properties.Add(property);
                }
            }

            return this;
        }

        public RefreshSource WithCommands(params IRefreshableCommand[] commands)
        {
            if (commands != null)
            {
                foreach (var command in commands)
                {
                    Commands.Add(command);
                }
            }

            return this;
        }
    }
}
