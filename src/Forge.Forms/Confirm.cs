using Forge.Forms.Annotations;
using Forge.Forms.Base;

namespace Forge.Forms
{
    [Form(Mode = DefaultFields.None)]
    [Title("{Binding Title}", IsVisible = "{Binding Title|IsNotEmpty}")]
    [Text("{Binding Message}", IsVisible = "{Binding Message|IsNotEmpty}")]
    [Action("negative", "{Binding NegativeAction}", IsCancel = true, IsVisible = "{Binding NegativeAction|IsNotEmpty}")]
    [Action("positive", "{Binding PositiveAction}", IsDefault = true, IsVisible = "{Binding PositiveAction|IsNotEmpty}")]
    public sealed class Confirm : DialogBase
    {
    }
}
