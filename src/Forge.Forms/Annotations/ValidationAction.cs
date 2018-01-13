namespace Forge.Forms.Annotations
{
    /// <summary>
    /// Specifies field validation action when an event occurs.
    /// </summary>
    public enum ValidationAction
    {
        /// <summary>
        /// Does nothing.
        /// </summary>
        DoNothing,
        /// <summary>
        /// Validates the field.
        /// </summary>
        ValidateField,
        /// <summary>
        /// Clears the field of validation errors.
        /// </summary>
        ClearErrors
    }
}
