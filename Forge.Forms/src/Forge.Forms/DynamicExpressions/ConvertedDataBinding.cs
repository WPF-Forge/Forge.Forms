using System;
using System.Collections.Generic;
using System.Windows.Data;
using Forge.Forms.FormBuilding;
using Forge.Forms.Validation;

namespace Forge.Forms.DynamicExpressions
{
    public sealed class ConvertedDataBinding : IValueProvider
    {
        public ConvertedDataBinding(string propertyPath, BindingOptions bindingOptions,
            List<IValidatorProvider> validationRules, ReplacementPipe replacementPipe,
            Func<IResourceContext, IErrorStringProvider> conversionErrorStringProvider)
            : this(propertyPath, bindingOptions, validationRules,
                replacementPipe, conversionErrorStringProvider, false)
        {
        }

        public ConvertedDataBinding(string propertyPath, BindingOptions bindingOptions,
            List<IValidatorProvider> validationRules, ReplacementPipe replacementPipe,
            Func<IResourceContext, IErrorStringProvider> conversionErrorStringProvider,
            bool oneWay)
        {
            PropertyPath = propertyPath;
            BindingOptions = bindingOptions ?? throw new ArgumentNullException(nameof(bindingOptions));
            ReplacementPipe = replacementPipe ?? throw new ArgumentNullException(nameof(replacementPipe));
            ValidationRules = validationRules ?? new List<IValidatorProvider>();
            ConversionErrorStringProvider = conversionErrorStringProvider;
            OneWay = oneWay;
        }

        public string PropertyPath { get; }

        public BindingOptions BindingOptions { get; }

        public List<IValidatorProvider> ValidationRules { get; }

        public ReplacementPipe ReplacementPipe { get; }

        public Func<IResourceContext, IErrorStringProvider> ConversionErrorStringProvider { get; }

        public bool OneWay { get; }

        public BindingBase ProvideBinding(IResourceContext context)
        {
            var binding = context.CreateModelBinding(PropertyPath);
            if (OneWay)
            {
                binding.Mode = BindingMode.OneWay;
            }

            BindingOptions.Apply(binding);
            var deserializer = ReplacementPipe.CreateDeserializer(context);
            binding.Converter = new StringTypeConverter(deserializer);
            binding.ValidationRules.Add(new ConversionValidator(deserializer, ConversionErrorStringProvider(context),
                binding.ConverterCulture));
            var pipe = new ValidationPipe();
            foreach (var validatorProvider in ValidationRules)
            {
                binding.ValidationRules.Add(validatorProvider.GetValidator(context, pipe));
            }

            binding.ValidationRules.Add(pipe);
            return binding;
        }

        public object ProvideValue(IResourceContext context)
        {
            return ProvideBinding(context);
        }
    }
}
