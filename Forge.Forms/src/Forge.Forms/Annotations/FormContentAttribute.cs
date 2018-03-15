using System;
using Forge.Forms.FormBuilding;

namespace Forge.Forms.Annotations
{
    /// <summary>
    /// Represents content attached before or after form elements.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = true)]
    public abstract class FormContentAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FormContentAttribute"/> class.
        /// </summary>
        /// <param name="position">The relative position of this element.</param>
        protected FormContentAttribute(int position)
        {
            Position = position;
            LinePosition = Annotations.Position.Left;
        }

        /// <summary>
        /// Determines the position relative to other elements added to the attribute target.
        /// </summary>
        public int Position { get; }

        /// <summary>
        /// Determines whether this element will be visible. Accepts a boolean or a dynamic resource.
        /// </summary>
        public object IsVisible { get; set; }

        /// <summary>
        /// If set to true and this attribute is attached to a property, this element will be displayed after the field.
        /// If set to true and this attribute is attached to a class, this element will be displayed after the form contents.
        /// </summary>
        public bool InsertAfter { get; set; }

        /// <summary>
        /// Indicates whether following elements such as actions will be placed in the same line as this element.
        /// Accepts a boolean only.
        /// </summary>
        public bool ShareLine { get; set; }

        /// <summary>
        /// Determines the location of this element inside its row.
        /// </summary>
        public Position LinePosition { get; set; }

        /// <summary>
        /// Determines the row span of this element.
        /// Accepts an integer only. Default is 1.
        /// </summary>
        protected internal int RowSpan { get; protected set; } = 1;

        /// <summary>
        /// Determines whether this element will never share rows with previous elements.
        /// Accepts a boolean only. Default is true.
        /// </summary>
        protected internal bool StartsNewRow { get; protected set; } = true;

        /// <summary>
        /// Create a form element corresponding to this object.
        /// </summary>
        protected abstract FormElement CreateElement();

        internal FormElement GetElement()
        {
            var element = CreateElement();
            element.IsVisible = Utilities.GetResource<bool>(IsVisible, true, Deserializers.Boolean);
            element.LinePosition = LinePosition;
            InitializeElement(element);
            return element;
        }

        /// <summary>
        /// Initializes the element that is created by <see cref="CreateElement"/>.
        /// </summary>
        /// <param name="element">The element that was created.</param>
        protected virtual void InitializeElement(FormElement element)
        {
        }
    }
}
