using System;
using System.Globalization;
using System.Windows.Data;
using Forge.Forms.Interfaces;

namespace Forge.Forms.Validation
{
    public class NotEqualsValidator : ComparisonValidator
    {
        public NotEqualsValidator(ValidationPipe pipe, IProxy argument, IErrorStringProvider errorProvider, IBoolProxy isEnforced,
            IValueConverter valueConverter, bool strictValidation, bool validatesOnTargetUpdated)
            : base(pipe, argument, errorProvider, isEnforced, valueConverter, strictValidation, validatesOnTargetUpdated)
        {
        }

        protected override bool ValidateValue(object value, CultureInfo cultureInfo)
        {
            var comparand = Argument.Value;
            if (value != null && comparand is IConvertible && value.GetType() != comparand.GetType())
            {
                comparand = Convert.ChangeType(comparand, value.GetType(), CultureInfo.InvariantCulture);
            }

            return !Equals(comparand, value);
        }
    }
}
