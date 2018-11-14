using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
using Forge.Forms.DynamicExpressions;

namespace Forge.Forms.Validation
{
    public abstract class FieldValidator : ValidationRule
    {
        protected FieldValidator(ValidationPipe pipe, IErrorStringProvider errorProvider, IBoolProxy isEnforced,
            IValueConverter valueConverter, bool strictValidation, bool validatesOnTargetUpdated)
            : base(ValidationStep.ConvertedProposedValue, validatesOnTargetUpdated)
        {
            ValidationPipe = pipe;
            ErrorProvider = errorProvider;
            ValueConverter = valueConverter;
            IsEnforced = isEnforced;
            StrictValidation = strictValidation;
        }

        public IValueConverter ValueConverter { get; }

        public IErrorStringProvider ErrorProvider { get; }

        public IBoolProxy IsEnforced { get; }

        public ValidationPipe ValidationPipe { get; }

        public bool StrictValidation { get; }

        public sealed override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (ValidationPipe != null)
            {
                // Pass if another validator has already reported an error.
                if (ValidationPipe.Error != null)
                {
                    return ValidationResult.ValidResult;
                }

                // Checking this later might be a tiny bit faster.
                if (!IsEnforced.Value)
                {
                    return ValidationResult.ValidResult;
                }

                var isValid = ValidateValue(ValueConverter != null
                    ? ValueConverter.Convert(value, typeof(object), null, cultureInfo)
                    : value, cultureInfo);

                if (!isValid)
                {
                    var error = ErrorProvider.GetErrorMessage(value);
                    if (StrictValidation)
                    {
                        // If we're going to stop propagation, we need to make sure
                        // that the pipe is clean for the next turn.
                        ValidationPipe.Error = null;
                        return new ValidationResult(false, error);
                    }

                    ValidationPipe.Error = error;
                }

                return ValidationResult.ValidResult;
            }
            else
            {
                if (!IsEnforced.Value)
                {
                    return ValidationResult.ValidResult;
                }

                // When there's no pipe, validation must return eagerly.
                // Properties will not be updated this way as validation will stop binding.
                var isValid = ValidateValue(ValueConverter != null
                    ? ValueConverter.Convert(value, typeof(object), null, cultureInfo)
                    : value, cultureInfo);

                return isValid
                    ? ValidationResult.ValidResult
                    : new ValidationResult(false, ErrorProvider.GetErrorMessage(value));
            }
        }

        protected abstract bool ValidateValue(object value, CultureInfo cultureInfo);
    }
}
