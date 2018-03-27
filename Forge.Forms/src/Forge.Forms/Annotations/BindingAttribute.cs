using System;
using System.Windows.Data;
using Forge.Forms.FormBuilding;

namespace Forge.Forms.Annotations
{
    /// <summary>
    /// Specifies additional information about a field's data binding.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class BindingAttribute : Attribute
    {
        /// <summary>
        /// Indicates the culture name to use if this field uses
        /// data conversion. A null value indicates UI culture;
        /// an empty value indicates invariant culture; a string
        /// value will retrieve the culture by name.
        /// </summary>
        public string ConverterCulture { get; set; } = "";

        /// <summary>
        /// Error message that will be displayed in case the conversion fails.
        /// Accepts a string or a dynamic expression.
        /// </summary>
        public string ConversionErrorMessage { get; set; }

        /// <summary>
        /// String format of the data binding.
        /// Accepts a string only.
        /// </summary>
        public string StringFormat { get; set; }

        /// <summary>
        /// The <see cref="UpdateSourceTrigger"/> that specifies how the binding will update.
        /// </summary>
        public UpdateSourceTrigger UpdateSourceTrigger { get; set; }

        /// <summary>
        /// Binding delay in milliseconds.
        /// </summary>
        public int Delay { get; set; }

        /// <summary>
        /// Specifies the value of <see cref="Binding.ValidatesOnDataErrors"/> property.
        /// </summary>
        public bool ValidatesOnDataErrors { get; set; } = true;

        /// <summary>
        /// Specifies the value of <see cref="Binding.ValidatesOnExceptions"/> property.
        /// </summary>
        public bool ValidatesOnExceptions { get; set; } = false;

        /// <summary>
        /// Specifies the value of <see cref="Binding.ValidatesOnNotifyDataErrors"/> property.
        /// </summary>
        public bool ValidatesOnNotifyDataErrors { get; set; } = true;

        internal void Apply(BindingOptions bindingOptions)
        {
            bindingOptions.StringFormat = StringFormat;
            bindingOptions.UpdateSourceTrigger = UpdateSourceTrigger;
            bindingOptions.ConverterCulture = ConversionCulture.Get(ConverterCulture);
            bindingOptions.Delay = Delay;
            bindingOptions.ValidatesOnExceptions = ValidatesOnExceptions;
            bindingOptions.ValidatesOnDataErrors = ValidatesOnDataErrors;
            bindingOptions.ValidatesOnNotifyDataErrors = ValidatesOnNotifyDataErrors;
        }
    }
}
