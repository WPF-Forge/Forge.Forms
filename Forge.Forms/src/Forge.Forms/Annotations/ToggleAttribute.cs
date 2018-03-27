using System;

namespace Forge.Forms.Annotations
{
    /// <summary>
    /// Indicates that a boolean property should be rendered as a toggle button.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class ToggleAttribute : Attribute
    {
    }
}
