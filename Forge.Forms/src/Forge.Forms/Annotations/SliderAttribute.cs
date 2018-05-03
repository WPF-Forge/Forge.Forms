using System;

namespace Forge.Forms.Annotations
{
    /// <summary>
    /// Indicates that a numeric value should be displayed as a slider.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class SliderAttribute : Attribute
    {
        /// <summary>
        /// Slider minimum value. Accepts a numeric value or a dynamic resource.
        /// </summary>
        public object Minimum { get; set; }

        /// <summary>
        /// Slider maximum value. Accepts a numeric value or a dynamic resource.
        /// </summary>
        public object Maximum { get; set; }
    }
}
