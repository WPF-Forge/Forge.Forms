using System;

namespace Forge.Forms.Annotations
{
    /// <summary>
    /// Allows configuring generated forms.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class FormAttribute : Attribute
    {
        /// <summary>
        /// Specifies default field generation behavior.
        /// </summary>
        public DefaultFields Mode { get; set; }

        /// <summary>
        /// Specifies grid column widths. Positive values indicate star widths, negative values indicate pixel widths.
        /// </summary>
        public double[] Grid { get; set; }
    }
}
