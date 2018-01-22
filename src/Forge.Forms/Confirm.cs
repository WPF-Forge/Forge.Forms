using Forge.Forms.Annotations;
using Forge.Forms.Base;

namespace Forge.Forms
{
    [Form(Mode = DefaultFields.None)]
    [Title("{Binding Title}", IsVisible = "{Binding Title|IsNotEmpty}")]
    [Text("{Binding Message}", IsVisible = "{Binding Message|IsNotEmpty}")]
    [Action("negative", "{Binding NegativeAction}", IsCancel = true, IsVisible = "{Binding NegativeAction|IsNotEmpty}")]
    [Action("positive", "{Binding PositiveAction}", IsDefault = true,
        IsVisible = "{Binding PositiveAction|IsNotEmpty}")]
    public sealed class Confirm : DialogBase
    {
        public Confirm()
        {
        }

        public Confirm(string message)
        {
            Message = message;
        }

        public Confirm(string message, string title)
        {
            Message = message;
            Title = title;
        }

        public Confirm(string message, string title, string positiveAction)
        {
            Message = message;
            Title = title;
            PositiveAction = positiveAction;
        }

        public Confirm(string message, string title, string positiveAction, string negativeAction)
        {
            Message = message;
            Title = title;
            PositiveAction = positiveAction;
            NegativeAction = negativeAction;
        }
    }
}
