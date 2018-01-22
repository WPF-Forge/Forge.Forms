using System;
using System.Runtime.CompilerServices;
using Forge.Forms.FormBuilding;
using Forge.Forms.FormBuilding.Defaults;

namespace Forge.Forms.Annotations
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = true)]
    public class BreakAttribute : FormContentAttribute
    {
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
