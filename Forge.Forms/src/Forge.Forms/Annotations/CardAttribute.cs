using System.Runtime.CompilerServices;
using Forge.Forms.FormBuilding;
using Forge.Forms.FormBuilding.Defaults;

namespace Forge.Forms.Annotations
{
    /// <summary>
    /// Provides a card background for form elements.
    /// </summary>
    public sealed class CardAttribute : FormContentAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CardAttribute"/> class.
        /// </summary>
        /// <param name="rows">Number of following rows that this element should cover.</param>
        /// <param name="position">Do not provide a value for this argument.</param>
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
