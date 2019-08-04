using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using Forge.Forms.DynamicExpressions;

namespace Forge.Forms.Validation
{
    public class EmptyValidator : FieldValidator
    {
        public EmptyValidator(
            ValidationPipe pipe,
            IErrorStringProvider errorProvider,
            IBoolProxy isEnforced,
            IValueConverter valueConverter,
            bool strictValidation,
            bool validatesOnTargetUpdated,
            bool ignoreNullOrEmpty)
            : base(
                pipe,
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
            switch (value)
            {
                case null:
                    return true;
                case string s:
                    return s.Length == 0;
                case IEnumerable<object> e:
                    return !e.Any();
                default:
                    return true;
            }
        }
    }
}
