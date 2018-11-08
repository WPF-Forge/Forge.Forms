using System;

namespace Forge.Forms.Annotations
{
    /// <summary>
    /// Allows attaching custom data to fields or forms.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class, AllowMultiple = true)]
    public class MetaAttribute : Attribute
    {
        public MetaAttribute(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; }

        public string Value { get; }
    }
}
