using System;
using System.Collections.Generic;
using System.Windows.Data;
using Forge.Forms.FormBuilding;
using Forge.Forms.Validation;

namespace Forge.Forms.DynamicExpressions
{
    public sealed class DataBinding : Resource
    {
        public DataBinding(string propertyPath, BindingOptions bindingOptions)
            : this(propertyPath, bindingOptions, null, null, false)
        {
        }

        public DataBinding(string propertyPath, BindingOptions bindingOptions, List<IValidatorProvider> validationRules)
            : this(propertyPath, bindingOptions, validationRules, null, false)
        {
        }

        public DataBinding(string propertyPath, BindingOptions bindingOptions, 
            List<IValidatorProvider> validationRules, bool oneWay)
            : this(propertyPath, bindingOptions, validationRules, null, oneWay)
        {
        }

        public DataBinding(string propertyPath, BindingOptions bindingOptions, 
            List<IValidatorProvider> validationRules, string valueConverter)
            : this(propertyPath, bindingOptions, validationRules, valueConverter, false)
        {
        }

        public DataBinding(string propertyPath, BindingOptions bindingOptions, List<IValidatorProvider> validationRules,
            string valueConverter, bool oneWay)
            : base(valueConverter)
        {
            PropertyPath = propertyPath;
            BindingOptions = bindingOptions ?? throw new ArgumentNullException(nameof(bindingOptions));
            ValidationRules = validationRules ?? new List<IValidatorProvider>();
            OneWay = oneWay;
        }

        public string PropertyPath { get; }

        public BindingOptions BindingOptions { get; }

        public List<IValidatorProvider> ValidationRules { get; }

        public bool OneWay { get; }

        public override bool IsDynamic => true;

        public override BindingBase ProvideBinding(IResourceContext context)
        {
            var binding = context.CreateModelBinding(PropertyPath);
            if (OneWay)
            {
                binding.Mode = BindingMode.OneWay;
            }

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
            return PropertyPath.GetHashCode();
        }
    }
}
