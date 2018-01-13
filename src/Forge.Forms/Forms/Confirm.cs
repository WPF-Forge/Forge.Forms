using Forge.Forms.Annotations;
using Forge.Forms.Annotations.Content;
using Forge.Forms.Forms.Base;

namespace Forge.Forms.Forms
{
    [Form(Mode = DefaultFields.None)]
    [Title("{Binding Title}", IsVisible = "{Binding Title|IsNotEmpty}")]
    [Text("{Binding Message}", IsVisible = "{Binding Message|IsNotEmpty}")]
    [Action("negative", "{Binding NegativeAction}", IsVisible = "{Binding NegativeAction|IsNotEmpty}")]
    [Action("positive", "{Binding PositiveAction}", IsVisible = "{Binding PositiveAction|IsNotEmpty}")]
    public sealed class Confirm : DialogBase
    {
    }
}
