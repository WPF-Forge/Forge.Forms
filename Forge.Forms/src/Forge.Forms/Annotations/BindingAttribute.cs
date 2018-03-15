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

        public string ConversionErrorMessage { get; set; }

        public string StringFormat { get; set; }

        public UpdateSourceTrigger UpdateSourceTrigger { get; set; }

        public int Delay { get; set; }

        public bool ValidatesOnDataErrors { get; set; } = true;

        public bool ValidatesOnExceptions { get; set; } = false;

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
