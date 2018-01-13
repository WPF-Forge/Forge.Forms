using System.Runtime.CompilerServices;
using Forge.Forms.Annotations.Content;
using Forge.Forms.Components.Fields;
using Forge.Forms.Components.Fields.Defaults;

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
