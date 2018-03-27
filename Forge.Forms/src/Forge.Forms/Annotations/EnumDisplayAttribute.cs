using System;

namespace Forge.Forms.Annotations
{
    /// <summary>
    /// Allows specifying enum display text.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class EnumDisplayAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnumDisplayAttribute"/> class.
        /// </summary>
        /// <param name="name">Enumeration member name. Accepts a string or a dynamic expression.</param>
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
