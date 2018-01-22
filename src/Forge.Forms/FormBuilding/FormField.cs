using System.Windows;
using Forge.Forms.DynamicExpressions;
using MaterialDesignThemes.Wpf;

namespace Forge.Forms.FormBuilding
{
    /// <summary>
    /// Base class for all form field definitions.
    /// </summary>
    public abstract class FormField : FormElement
    {
        /// <summary>
        /// Gets or sets the unique name of this field.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the string expression of the field's title.
        /// </summary>
        public IValueProvider Name { get; set; }

        /// <summary>
        /// Gets or sets the string expression of the field's tooltip.
        /// </summary>
        public IValueProvider ToolTip { get; set; }

        /// <summary>
        /// Gets or sets the field's PackIconKind resource. Not all controls may display an icon.
        /// </summary>
        public IValueProvider Icon { get; set; }

        /// <summary>
        /// Finalizes the field state by adding all appropriate values as resources.
        /// Changing properties after this method has been called is strongly discouraged.
        /// </summary>
        protected internal override void Freeze()
        {
            const string iconVisibility = "IconVisibility";

            base.Freeze();
            Resources.Add(nameof(Name), Name ?? LiteralValue.Null);
            Resources.Add(nameof(ToolTip), ToolTip ?? LiteralValue.Null);

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
