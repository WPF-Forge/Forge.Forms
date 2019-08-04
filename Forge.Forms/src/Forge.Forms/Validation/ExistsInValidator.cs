using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using Forge.Forms.DynamicExpressions;

namespace Forge.Forms.Validation
{
    public class ExistsInValidator : ComparisonValidator
    {
        public ExistsInValidator(
            ValidationPipe pipe,
            IProxy argument,
            IErrorStringProvider errorProvider,
            IBoolProxy isEnforced,
            IValueConverter valueConverter,
            bool strictValidation,
            bool validatesOnTargetUpdated,
            bool ignoreNullOrEmpty)
            : base(
                pipe,
                argument,
                errorProvider,
                isEnforced,
                valueConverter, 
                strictValidation,
                validatesOnTargetUpdated,
                ignoreNullOrEmpty)
        {
        }

        protected override bool ValidateValue(object value, CultureInfo cultureInfo)
        {
            if (Argument.Value is IEnumerable<object> e)
            {
                return e.Contains(value);
            }

            return true;
        }
    }
}
