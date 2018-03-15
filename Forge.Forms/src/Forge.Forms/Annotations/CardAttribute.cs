using System.Runtime.CompilerServices;
using Forge.Forms.FormBuilding;
using Forge.Forms.FormBuilding.Defaults;

namespace Forge.Forms.Annotations
{
    public sealed class CardAttribute : FormContentAttribute
    {
        public CardAttribute(int rows, [CallerLineNumber] int position = 0)
            : base(position)
        {
            StartsNewRow = false;
            RowSpan = rows;
        }

        protected override FormElement CreateElement()
        {
            return new CardElement();
        }
    }
}
