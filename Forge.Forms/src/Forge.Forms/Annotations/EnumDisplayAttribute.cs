using System;

namespace Forge.Forms.Annotations
{
    /// <summary>
    /// Allows specifying enum display text.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class EnumDisplayAttribute : Attribute
    {
        public EnumDisplayAttribute(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Enumeration member name. Accepts a string or a dynamic expression.
        /// </summary>
        public string Name { get; set; }
    }
}
