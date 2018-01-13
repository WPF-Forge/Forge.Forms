using System;

namespace Forge.Forms.Annotations
{
    /// <summary>
    /// Allows attaching custom resources to fields or to the model.
    /// These resources become available to generated controls.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = true)]
    public sealed class ResourceAttribute : Attribute
    {
        public ResourceAttribute(string name, object value)
        {
            Name = name;
            Value = value;
        }

        /// <summary>
        /// Resource name. Accepts a string.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Resource value. Accepts an object or a dynamic expresion.
        /// </summary>
        public object Value { get; }
    }
}
