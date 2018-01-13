using Forge.Forms.Annotations;
using Forge.Forms.Annotations.Content;
using Forge.Forms.Forms.Base;
using MaterialDesignThemes.Wpf;

namespace Forge.Forms.Forms
{
    [Form(Mode = DefaultFields.None)]
    [Title("{Binding Title}", IsVisible = "{Binding Title|IsNotEmpty}")]
    [Text("{Binding Message}", IsVisible = "{Binding Message|IsNotEmpty}")]
    [Action("negative", "{Binding NegativeAction}", IsVisible = "{Binding NegativeAction|IsNotEmpty}")]
    [Action("positive", "{Binding PositiveAction}", IsVisible = "{Binding PositiveAction|IsNotEmpty}")]
    public sealed class Prompt<T> : DialogBase
    {
        private PackIconKind? icon;
        private string name;
        private string toolTip;
        private T value;

        [Field(Name = "{Binding Name}",
            ToolTip = "{Binding ToolTip}",
            Icon = "{Binding Icon}")]
        public T Value
        {
            get => value;
            set
            {
                if (Equals(value, this.value))
                {
                    return;
                }

                this.value = value;
                OnPropertyChanged();
            }
        }

        public string Name
        {
            get => name;
            set
            {
                if (value == name)
                {
                    return;
                }

                name = value;
                OnPropertyChanged();
            }
        }

        public string ToolTip
        {
            get => toolTip;
            set
            {
                if (value == toolTip)
                {
                    return;
                }

                toolTip = value;
                OnPropertyChanged();
            }
        }

        public PackIconKind? Icon
        {
            get => icon;
            set
            {
                if (value == icon)
                {
                    return;
                }

                icon = value;
                OnPropertyChanged();
            }
        }
    }
}
