using System.Globalization;
using System.Windows.Data;
using Forge.Forms.Interfaces;

namespace Forge.Forms.Validation
{
    public class TrueValidator : FieldValidator
    {
        public TrueValidator(ValidationPipe pipe, IErrorStringProvider errorProvider, IBoolProxy isEnforced, IValueConverter valueConverter,
            bool strictValidation, bool validatesOnTargetUpdated)
            : base(pipe, errorProvider, isEnforced, valueConverter, strictValidation, validatesOnTargetUpdated)
        {
        }

        protected override bool ValidateValue(object value, CultureInfo cultureInfo)
        {
            return value is true;
        }
    }
}
