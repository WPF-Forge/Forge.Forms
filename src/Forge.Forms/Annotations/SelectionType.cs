namespace Forge.Forms.Annotations
{
    /// <summary>
    /// Determines how a selection field should be displayed.
    /// </summary>
    public enum SelectionType
    {
        /// <summary>
        /// A selectable ComboBox is displayed.
        /// </summary>
        ComboBox,

        /// <summary>
        /// An editable ComboBox is displayed.
        /// Falls back to a non-editable ComboBox if editing is not applicable for a property type.
        /// </summary>
        ComboBoxEditable,

        /// <summary>
        /// A list of radio buttons is displayed.
        /// </summary>
        RadioButtons
    }
}
