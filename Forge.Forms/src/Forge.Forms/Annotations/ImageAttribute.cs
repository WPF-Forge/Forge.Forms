using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using Forge.Forms.FormBuilding;
using Forge.Forms.FormBuilding.Defaults;
using Vertical = System.Windows.VerticalAlignment;
using Horizontal = System.Windows.HorizontalAlignment;
using WpfStretch = System.Windows.Media.Stretch;
using WpfStretchDirection = System.Windows.Controls.StretchDirection;

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

        /// <summary>
        /// Gets or sets the image width. Accepts "auto", a double, or a dynamic resource resolving to one of those.
        /// Defaults to auto.
        /// </summary>
        public object Width { get; set; }

        /// <summary>
        /// Gets or sets the image height. Accepts "auto", a double, or a dynamic resource resolving to one of those.
        /// Defaults to auto.
        /// </summary>
        public object Height { get; set; }

        /// <summary>
        /// Gets or sets the image horizontal alignment. Accepts a <see cref="System.Windows.HorizontalAlignment"/>, string, or a dynamic resource.
        /// Defaults to <see cref="Horizontal.Stretch"/>.
        /// </summary>
        public object HorizontalAlignment { get; set; }

        /// <summary>
        /// Gets or sets the image horizontal alignment. Accepts a <see cref="System.Windows.VerticalAlignment"/>, string, or a dynamic resource.
        /// Defaults to <see cref="Vertical.Center"/>.
        /// </summary>
        public object VerticalAlignment { get; set; }

        /// <summary>
        /// Gets or sets the image stretch behavior. Accepts a <see cref="System.Windows.Media.Stretch"/>, string, or a dynamic resource.
        /// Defaults to <see cref="WpfStretch.Uniform"/>.
        /// </summary>
        public object Stretch { get; set; }

        /// <summary>
        /// Gets or sets the image stretch direction. Accepts a <see cref="System.Windows.Controls.StretchDirection"/>, string, or a dynamic resource.
        /// Defaults to <see cref="WpfStretchDirection.DownOnly"/>.
        /// </summary>
        public object StretchDirection { get; set; }

        protected override FormElement CreateElement()
        {
            return new ImageElement
            {
                Source = Utilities.GetResource<object>(Source, null, x => x),
                Width = Utilities.GetResource<double>(Width, double.NaN, SizeDeserializer),
                Height = Utilities.GetResource<double>(Height, double.NaN, SizeDeserializer),
                HorizontalAlignment = Utilities.GetResource<Horizontal>(HorizontalAlignment, Horizontal.Stretch, Deserializers.Enum<Horizontal>()),
                VerticalAlignment = Utilities.GetResource<Vertical>(VerticalAlignment, Vertical.Center, Deserializers.Enum<Vertical>()),
                Stretch = Utilities.GetResource<WpfStretch>(Stretch, WpfStretch.Uniform, Deserializers.Enum<WpfStretch>()),
                StretchDirection = Utilities.GetResource<WpfStretchDirection>(StretchDirection, WpfStretchDirection.DownOnly, Deserializers.Enum<WpfStretchDirection>())
            };
        }

        private static object SizeDeserializer(string arg)
        {
            return string.Equals(arg, "auto", StringComparison.OrdinalIgnoreCase) ? double.NaN : double.Parse(arg, CultureInfo.InvariantCulture);
        }
    }
}
