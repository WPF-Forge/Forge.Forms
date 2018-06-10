using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using Forge.Forms.DynamicExpressions;
using Forge.Forms.Validation;

namespace Forge.Forms.FormBuilding.Defaults
{
    public sealed class ConvertedField : DataFormField
    {
        public ConvertedField(string key, Type propertyType, ReplacementPipe replacementPipe) : base(key, propertyType)
        {
            ReplacementPipe = replacementPipe;
            CreateBinding = false;
        }

        public ReplacementPipe ReplacementPipe { get; }

        public Func<IResourceContext, IErrorStringProvider> ConversionErrorMessage { get; set; }

        protected internal override void Freeze()
        {
            base.Freeze();
            if (IsDirectBinding)
            {
                Resources.Add("Value",
                    new ConvertedDirectBinding(BindingOptions, Validators, ReplacementPipe,
                        ConversionErrorMessage ?? (ctx => new PlainErrorStringProvider("Invalid value."))));
            }
            else if (string.IsNullOrEmpty(Key))
            {
                Resources.Add("Value", LiteralValue.Null);
            }
            else
            {
                Resources.Add("Value",
                    new ConvertedDataBinding(Key, BindingOptions, Validators, ReplacementPipe,
                        ConversionErrorMessage ?? (ctx => new PlainErrorStringProvider("Invalid value."))));
            }
        }

        protected internal override IBindingProvider CreateBindingProvider(IResourceContext context,
            IDictionary<string, IValueProvider> formResources)
        {
            return new ConvertedPresenter(context, Resources, formResources);
        }
    }

    public sealed class NumericField : DataFormField
    {
        public NumericField(string key, Type propertyType, NumericReplacementPipe replacementPipe) : base(key, propertyType)
        {
            ReplacementPipe = replacementPipe;
            CreateBinding = false;
        }

        public NumericReplacementPipe ReplacementPipe { get; }

        public Func<IResourceContext, IErrorStringProvider> ConversionErrorMessage { get; set; }

        public IValueProvider NumberStyles { get; set; }

        protected internal override void Freeze()
        {
            base.Freeze();
            if (IsDirectBinding)
            {
                Resources.Add("Value",
                    new ConvertedNumericDirectBinding(BindingOptions, Validators, ReplacementPipe,
                        ConversionErrorMessage ?? (ctx => new PlainErrorStringProvider("Invalid value.")),
                        NumberStyles ?? LiteralValue.Null));
            }
            else if (string.IsNullOrEmpty(Key))
            {
                Resources.Add("Value", LiteralValue.Null);
            }
            else
            {
                Resources.Add("Value",
                    new ConvertedNumericDataBinding(Key, BindingOptions, Validators, ReplacementPipe,
                        ConversionErrorMessage ?? (ctx => new PlainErrorStringProvider("Invalid value.")),
                        NumberStyles ?? LiteralValue.Null));
            }

            Resources.Add("NumberStyles", NumberStyles ?? LiteralValue.Null);
        }

        protected internal override IBindingProvider CreateBindingProvider(IResourceContext context,
            IDictionary<string, IValueProvider> formResources)
        {
            return new ConvertedPresenter(context, Resources, formResources);
        }
    }

    public class ConvertedPresenter : ValueBindingProvider
    {
        static ConvertedPresenter()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ConvertedPresenter),
                new FrameworkPropertyMetadata(typeof(ConvertedPresenter)));
        }

        public ConvertedPresenter(IResourceContext context,
            IDictionary<string, IValueProvider> fieldResources,
            IDictionary<string, IValueProvider> formResources)
            : base(context, fieldResources, formResources, true)
        {
        }
    }
}
