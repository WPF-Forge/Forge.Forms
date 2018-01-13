using System;
using System.Collections.Generic;
using System.Windows.Data;
using Forge.Forms.Interfaces;
using Forge.Forms.Validation;

namespace Forge.Forms.Utils
{
    public sealed class DirectBinding : Resource
    {
        public DirectBinding(BindingOptions bindingOptions)
            : this(bindingOptions, null, null)
        {
        }

        public DirectBinding(BindingOptions bindingOptions, List<IValidatorProvider> validationRules)
            : this(bindingOptions, validationRules, null)
        {
        }

        public DirectBinding(BindingOptions bindingOptions, List<IValidatorProvider> validationRules,
            string valueConverter)
            : base(valueConverter)
        {
            BindingOptions = bindingOptions ?? throw new ArgumentNullException(nameof(bindingOptions));
            ValidationRules = validationRules ?? new List<IValidatorProvider>();
        }

        public BindingOptions BindingOptions { get; }

        public List<IValidatorProvider> ValidationRules { get; }

        public override bool IsDynamic => true;

        public override BindingBase ProvideBinding(IResourceContext context)
        {
            var binding = context.CreateDirectModelBinding();
            binding.Converter = GetValueConverter(context);
            BindingOptions.Apply(binding);
            var pipe = new ValidationPipe();
            foreach (var validatorProvider in ValidationRules)
            {
                binding.ValidationRules.Add(validatorProvider.GetValidator(context, pipe));
            }

            binding.ValidationRules.Add(pipe);
            return binding;
        }

        public override bool Equals(Resource other)
        {
            return ReferenceEquals(this, other);
        }

        public override int GetHashCode()
        {
            return BindingOptions.GetHashCode();
        }
    }
}
