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

        public bool ShareLine { get; set; }

        public Position LinePosition { get; set; }

        protected internal int RowSpan { get; protected set; } = 1;

        protected internal bool StartsNewRow { get; protected set; } = true;

        /// <summary>
        /// Create a form element corresponding to this object.
        /// </summary>
        /// <returns></returns>
        protected abstract FormElement CreateElement();

        internal FormElement GetElement()
        {
            var element = CreateElement();
            element.IsVisible = Utilities.GetResource<bool>(IsVisible, true, Deserializers.Boolean);
            element.LinePosition = LinePosition;
            InitializeElement(element);
            return element;
        }

        protected virtual void InitializeElement(FormElement element)
        {
        }
    }
}
