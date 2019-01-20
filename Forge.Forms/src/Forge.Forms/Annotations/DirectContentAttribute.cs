using System;

namespace Forge.Forms.Annotations
{
    /// <summary>
    /// Specifies that content rendering should be handled by WPF.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class DirectContentAttribute : Attribute
    {
    }
}
