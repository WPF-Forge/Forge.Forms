using System;

namespace Forge.Forms.Annotations
{
    /// <summary>
    /// Indicates that a string property should be rendered as a password field.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class PasswordAttribute : Attribute
    {
    }
}
