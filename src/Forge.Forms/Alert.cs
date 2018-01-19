using Forge.Forms.Annotations;
using Forge.Forms.Base;

namespace Forge.Forms
{
    [Form(Mode = DefaultFields.None)]
    [Title("{Binding Title}", IsVisible = "{Binding Title|IsNotEmpty}")]
    [Text("{Binding Message}", IsVisible = "{Binding Message|IsNotEmpty}")]
    [Action("positive", "{Binding PositiveAction}", IsDefault = true, IsCancel = true, IsVisible = "{Binding PositiveAction|IsNotEmpty}")]
    public sealed class Alert : DialogBase
    {
    }
}
