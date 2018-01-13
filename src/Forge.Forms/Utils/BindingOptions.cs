using System.Globalization;
using System.Windows.Data;

namespace Forge.Forms.Utils
{
    public class BindingOptions
    {
        public CultureInfo ConverterCulture { get; set; } = CultureInfo.InvariantCulture;

        public string StringFormat { get; set; }

        public UpdateSourceTrigger UpdateSourceTrigger { get; set; } = UpdateSourceTrigger.Default;

        public int Delay { get; set; }

        public bool ValidatesOnDataErrors { get; set; } = true;

        public bool ValidatesOnExceptions { get; set; } = false;

        public bool ValidatesOnNotifyDataErrors { get; set; } = true;

        internal void Apply(Binding binding)
        {
            binding.ConverterCulture = ConverterCulture;
            binding.StringFormat = StringFormat;
            binding.UpdateSourceTrigger = UpdateSourceTrigger;
            binding.Delay = Delay;
            binding.ValidatesOnDataErrors = ValidatesOnDataErrors;
            binding.ValidatesOnExceptions = ValidatesOnExceptions;
            binding.ValidatesOnNotifyDataErrors = ValidatesOnNotifyDataErrors;
        }
    }
}
