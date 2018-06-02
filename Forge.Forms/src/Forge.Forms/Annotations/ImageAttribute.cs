using System;
using System.Runtime.CompilerServices;
using Forge.Forms.FormBuilding;
using Forge.Forms.FormBuilding.Defaults;

namespace Forge.Forms.Annotations
{
    /// <summary>
    /// Draws images from ImageSources or string paths.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = true)]
    public sealed class ImageAttribute : FormContentAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Forge.Forms.Annotations.ImageAttribute"/> class.
        /// </summary>
        /// <param name="source">ImageSource value. Accepts a string or a dynamic resource.</param>
        /// <param name="position">Do not provide a value for this argument.</param>
        public ImageAttribute(string source, [CallerLineNumber] int position = 0)
            : base(position)
        {
            Source = source;
        }

        /// <summary>
        /// Gets the image source path or dynamic resource that resolves to the image source.
        /// </summary>
        public string Source { get; }

        protected override FormElement CreateElement()
        {
            return new ImageElement
            {
                Source = Utilities.GetResource<object>(Source, null, x => x)
            };
        }
    }
}
