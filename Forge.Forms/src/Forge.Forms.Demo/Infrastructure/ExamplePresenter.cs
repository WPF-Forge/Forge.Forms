using Material.Application.Models;

namespace Forge.Forms.Demo.Infrastructure
{
    public class ExamplePresenter : ObjectPresenter
    {
        public ExamplePresenter(object instance, string displayString, double preferredWidth)
            : base(instance, displayString)
        {
            PreferredWidth = preferredWidth;
        }

        public double PreferredWidth { get; }

        public string Source { get; set; }
    }
}
