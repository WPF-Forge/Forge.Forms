using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using Forge.Forms.Interfaces;

namespace Forge.Forms.Validation
{
    public class NotExistsInValidator : ComparisonValidator
    {
        public NotExistsInValidator(ValidationPipe pipe, IProxy argument, IErrorStringProvider errorProvider, IBoolProxy isEnforced,
            IValueConverter valueConverter, bool strictValidation, bool validatesOnTargetUpdated)
            : base(pipe, argument, errorProvider, isEnforced, valueConverter, strictValidation, validatesOnTargetUpdated)
        {
        }

        protected override bool ValidateValue(object value, CultureInfo cultureInfo)
        {
            if (Argument.Value is IEnumerable<object> e)
            {
                return !e.Contains(value);
            }

            return true;
        }
    }
}
