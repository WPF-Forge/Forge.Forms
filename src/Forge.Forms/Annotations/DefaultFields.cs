namespace Forge.Forms.Annotations
{
    /// <summary>
    /// Specifies which fields are displayed by default.
    /// </summary>
    public enum DefaultFields
    {
        /// <summary>
        /// Properties with public getters and setters are displayed by default.
        /// </summary>
        AllExcludingReadonly,

        /// <summary>
        /// All public properties are displayed by default.
        /// </summary>
        AllIncludingReadonly,

        /// <summary>
        /// No properties are displayed by default. Use <see cref="FieldAttribute" /> to add properties.
        /// </summary>
        None
    }
}
