using System;
using System.Runtime.CompilerServices;
using Forge.Forms.Components.Fields;
using Forge.Forms.Components.Fields.Defaults;
using Forge.Forms.FormBuilding;

namespace Forge.Forms.Annotations.Content
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = true)]
    public class DividerAttribute : FormContentAttribute
    {
        public DividerAttribute([CallerLineNumber] int position = 0)
            : this(true, position)
        {
        }

        public DividerAttribute(bool hasMargin, [CallerLineNumber] int position = 0) : base(position)
        {
            HasMargin = hasMargin;
        }

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
