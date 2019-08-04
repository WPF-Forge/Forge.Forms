using System;
using System.Windows.Data;
using Forge.Forms.DynamicExpressions;

namespace Forge.Forms.Validation
{
    public abstract class ComparisonValidator : FieldValidator
    {
        protected ComparisonValidator(
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
                errorProvider, 
                isEnforced, 
                valueConverter, 
                strictValidation, 
                validatesOnTargetUpdated,
                ignoreNullOrEmpty)
        {
            Argument = argument ?? throw new ArgumentNullException(nameof(argument));
        }

        public IProxy Argument { get; }
    }
}
