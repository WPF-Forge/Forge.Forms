using System;

namespace Forge.Forms.Annotations
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class TimeAttribute : Attribute
    {
        /// <summary>
        /// Determines whether the time is displayed in 24-hour format.
        /// Accepts a boolean or a dynamic resource.
        /// </summary>
        public object Is24Hours { get; set; } = false;
    }
}
