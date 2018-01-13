using System.Globalization;

namespace Forge.Forms.Validation
{
    /// <summary>
    /// Gets passed to method validators.
    /// </summary>
    public sealed class ValidationContext
    {
        /// <summary>
        /// Creates a new <see cref="ValidationContext" />.
        /// </summary>
        public ValidationContext(object model, object modelContext, string propertyName, object propertyValue,
            CultureInfo cultureInfo, bool willCommit)
        {
            Model = model;
            ModelContext = modelContext;
            PropertyName = propertyName;
            PropertyValue = propertyValue;
            CultureInfo = cultureInfo;
            WillCommit = willCommit;
        }

        /// <summary>
        /// Gets the model that contains the property that is being validated.
        /// </summary>
        public object Model { get; }

        /// <summary>
        /// Gets the model's context at the time of validation.
        /// </summary>
        public object ModelContext { get; }

        /// <summary>
        /// Gets the name of the property that is being validated.
        /// </summary>
        public string PropertyName { get; }

        /// <summary>
        /// Gets the value of the property that is being validated.
        /// </summary>
        public object PropertyValue { get; }

        /// <summary>
        /// Gets the <see cref="CultureInfo" /> in which this validation is performed.
        /// </summary>
        public CultureInfo CultureInfo { get; }

        /// <summary>
        /// Gets whether the value will be written to the
        /// property regardless of validation result.
        /// </summary>
        public bool WillCommit { get; }
    }
}
