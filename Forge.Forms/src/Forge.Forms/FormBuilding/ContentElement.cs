using System.Windows;
using Forge.Forms.DynamicExpressions;
using MaterialDesignThemes.Wpf;

namespace Forge.Forms.FormBuilding
{
    public abstract class ContentElement : FormElement
    {
        public IValueProvider Content { get; set; }

        public IValueProvider Icon { get; set; }

        public IValueProvider IconPadding { get; set; }

        protected internal override void Freeze()
        {
            const string iconVisibility = "IconVisibility";

            base.Freeze();
            Resources.Add(nameof(Content), Content ?? LiteralValue.Null);
            Resources.Add(nameof(IconPadding), IconPadding ?? LiteralValue.False);

            if (Icon != null && !(Icon is LiteralValue v && v.Value == null))
            {
                Resources.Add(iconVisibility, Icon.Wrap("ToVisibility"));
                Resources.Add(nameof(Icon), Icon);
            }
            else
            {
                Resources.Add(iconVisibility, new LiteralValue(Visibility.Collapsed));
                Resources.Add(nameof(Icon), new LiteralValue((PackIconKind)(-2)));
            }
        }
    }
}
