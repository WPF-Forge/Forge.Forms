using System;
using System.Runtime.CompilerServices;
using Forge.Forms.FormBuilding;
using Forge.Forms.FormBuilding.Defaults;

namespace Forge.Forms.Annotations
{
    /// <summary>
    /// Adds a small distance between form rows.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = true)]
    public class BreakAttribute : FormContentAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BreakAttribute"/> class.
        /// </summary>
        /// <param name="position">Do not provide a value for this argument.</param>
        public BreakAttribute([CallerLineNumber] int position = 0) : base(position)
        {
        }

        /// <summary>
        /// Height of the break. Accepts a double or a dynamic resource.
        /// </summary>
        public object Height { get; set; } = 8d;

        protected override FormElement CreateElement()
        {
            return new BreakElement
            {
                Height = Utilities.GetResource<double>(Height, 8d, Deserializers.Double)
            };
        }
    }
}
