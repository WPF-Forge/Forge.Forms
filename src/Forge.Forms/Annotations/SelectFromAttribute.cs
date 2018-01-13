using System;

namespace Forge.Forms.Annotations
{
    /// <summary>
    /// Specifies that a field can have values from a collection.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class SelectFromAttribute : Attribute
    {
        public SelectFromAttribute(object itemsSource)
        {
            ItemsSource = itemsSource;
        }

        /// <summary>
        /// Selection items source. Accepts an array, an enum type, or a dynamic resource.
        /// </summary>
        public object ItemsSource { get; set; }

        /// <summary>
        /// Display member path. Accepts a string or a dynamic expression.
        /// </summary>
        public string DisplayPath { get; set; }

        /// <summary>
        /// Display value path. Accepts a string or a dynamic expression.
        /// </summary>
        public string ValuePath { get; set; }

        /// <summary>
        /// Item string format. Accepts a string or a dynamic expression.
        /// </summary>
        public string ItemStringFormat { get; set; }

        /// <summary>
        /// Field selection type. Accepts a <see cref="SelectionType"/> or a dynamic resource.
        /// </summary>
        public object SelectionType { get; set; }
    }
}
