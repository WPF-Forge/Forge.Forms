using System;
using System.Globalization;
using System.Windows.Controls;

namespace Forge.Forms.Validation
{
    public sealed class ConversionValidator : ValidationRule
    {
        private readonly Func<string, CultureInfo, object> deserializer;
        private readonly IErrorStringProvider errorProvider;
        private readonly CultureInfo overrideCulture;

        public ConversionValidator(Func<string, CultureInfo, object> deserializer, IErrorStringProvider errorProvider,
            CultureInfo overrideCulture)
            : base(ValidationStep.RawProposedValue, false)
        {
            this.deserializer = deserializer;
            this.errorProvider = errorProvider;
            this.overrideCulture = overrideCulture;
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                deserializer(value as string, overrideCulture ?? cultureInfo);
                return new ValidationResult(true, null);
            }
            catch
            {
                return new ValidationResult(false, errorProvider.GetErrorMessage(value));
            }
        }
    }
}
