using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Data;
using Forge.Forms.Annotations;
using Forge.Forms.DynamicExpressions;

namespace Forge.Forms.Validation
{
    public class MatchPatternValidator : ComparisonValidator
    {
        public MatchPatternValidator(ValidationPipe pipe, IProxy argument, IErrorStringProvider errorProvider,
            IBoolProxy isEnforced,
            IValueConverter valueConverter, bool strictValidation, bool validatesOnTargetUpdated,
            NullValueValidateAction nullValueValidateAction = NullValueValidateAction.Default)
            : base(pipe, argument, errorProvider, isEnforced, valueConverter, strictValidation,
                validatesOnTargetUpdated,nullValueValidateAction)
        {
        }

        protected override bool ValidateValue(object value, CultureInfo cultureInfo)
        {
            if (!(Argument.Value is string pattern))
            {
                return true;
            }

            if (value is string s)
            {
                return Regex.IsMatch(s, pattern);
            }

            return false;
        }
    }
}
