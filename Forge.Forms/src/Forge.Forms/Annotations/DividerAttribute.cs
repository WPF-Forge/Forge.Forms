using System;
using System.Runtime.CompilerServices;
using Forge.Forms.FormBuilding;
using Forge.Forms.FormBuilding.Defaults;

namespace Forge.Forms.Annotations
{
    /// <summary>
    /// Adds a divider line between form rows.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = true)]
    public class DividerAttribute : FormContentAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DividerAttribute"/> class.
        /// </summary>
        /// <param name="position">Do not provide a value for this argument.</param>
        public DividerAttribute([CallerLineNumber] int position = 0)
            : this(true, position)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DividerAttribute"/> class.
        /// </summary>
        /// <param name="hasMargin">Determines whether the divider adds a top and bottom margin. Accepts a boolean or a dynamic resource.</param>
        /// <param name="position">Do not provide a value for this argument.</param>
        public DividerAttribute(bool hasMargin, [CallerLineNumber] int position = 0) : base(position)
        {
            HasMargin = hasMargin;
        }

        /// <summary>
        /// Determines whether the divider adds a top and bottom margin.
        /// Accepts a boolean or a dynamic resource.
        /// </summary>
        public object HasMargin { get; set; }

        protected override FormElement CreateElement()
        {
            return new DividerElement
            {
                HasMargin = Utilities.GetResource<bool>(HasMargin, true, Deserializers.Boolean)
            };
        }
    }
}
