using Material.Application.Models;

namespace Forge.Forms.Demo.Models
{
    public class ExamplePresenter : ObjectPresenter
    {
        public ExamplePresenter(object instance, string displayString, double preferredWidth) 
            : base(instance, displayString)
        {
            PreferredWidth = preferredWidth;
        }

        public double PreferredWidth { get; }
    }
}