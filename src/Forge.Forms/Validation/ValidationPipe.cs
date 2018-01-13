using System.Globalization;
using System.Windows.Controls;

namespace Forge.Forms.Validation
{
    public sealed class ValidationPipe : ValidationRule
    {
        public ValidationPipe()
            : base(ValidationStep.CommittedValue, true)
        {
        }

        public string Error { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var error = Error;
            if (error != null)
            {
                Error = null;
                return new ValidationResult(false, error);
            }

            return ValidationResult.ValidResult; ;
        }
    }
}
