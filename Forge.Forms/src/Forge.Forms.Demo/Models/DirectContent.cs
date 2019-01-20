using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using Forge.Forms.Annotations;

namespace Forge.Forms.Demo.Models
{
    [Form(Mode = DefaultFields.AllIncludingReadonly)]
    public class DirectContent
    {
        [DirectContent]
        public string RawText { get; set; } = "This is a raw string";

        [DirectContent]
        public UIElement RawElement => new Ellipse
        {
            Width = 100d,
            Height = 100d,
            Fill = Brushes.Green
        };

        [Break]

        [DirectContent]
        public CustomContent CustomControl { get; } = new CustomContent
        {
            FirstName = "John",
            LastName = "Doe"
        };
    }

    public class CustomContent
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
