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
        /// A vertical list of radio buttons is displayed.
        /// </summary>
        RadioButtons,

        /// <summary>
        /// A horizontal list of radio buttons is displayed.
        /// </summary>
        RadioButtonsInline,

        /// <summary>
        /// A horizontal list of radio buttons is displayed right aligned.
        /// </summary>
        RadioButtonsInlineRightAligned
    }
}
