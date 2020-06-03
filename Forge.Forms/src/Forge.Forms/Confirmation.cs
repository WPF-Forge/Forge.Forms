using Forge.Forms.Annotations;

namespace Forge.Forms
{
    [Form(Mode = DefaultFields.None)]
    [Title("{Binding Title}", IsVisible = "{Binding Title|IsNotEmpty}")]
    [Text("{Binding Message}", IsVisible = "{Binding Message|IsNotEmpty}")]
    [Action("{Binding NegativeActionName}", "{Binding NegativeAction}",
        IsCancel = true,
        ClosesDialog = true,
        IsVisible = "{Binding NegativeAction|IsNotEmpty}",
        Icon = "{Binding NegativeActionIcon}")]
    [Action("{Binding PositiveActionName}", "{Binding PositiveAction}",
        IsDefault = true,
        ClosesDialog = true,
        IsVisible = "{Binding PositiveAction|IsNotEmpty}",
        Icon = "{Binding PositiveActionIcon}")]
    public sealed class Confirmation : DialogBase
    {
        [FieldIgnore]
        public string PositiveActionName { get; set; } = "positive";
        
        [FieldIgnore]
        public string NegativeActionName { get; set; } = "negative";
        
        public object Parameter { get; set; }
        
        public Confirmation()
        {
        }

        public Confirmation(string message)
        {
            Message = message;
        }

        public Confirmation(string message, string title)
        {
            Message = message;
            Title = title;
        }

        public Confirmation(string message, string title, string positiveAction)
        {
            Message = message;
            Title = title;
            PositiveAction = positiveAction;
        }

        public Confirmation(string message, string title, string positiveAction, string negativeAction)
        {
            Message = message;
            Title = title;
            PositiveAction = positiveAction;
            NegativeAction = negativeAction;
        }
    }
}
