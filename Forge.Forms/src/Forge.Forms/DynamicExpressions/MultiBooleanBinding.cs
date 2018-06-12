using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;
using Forge.Forms.DynamicExpressions.BooleanExpressions;
using Forge.Forms.FormBuilding;

namespace Forge.Forms.DynamicExpressions
{
    internal class MultiBooleanBinding : IValueProvider
    {
        public MultiBooleanBinding(BooleanExpression ast, IEnumerable<IValueProvider> valueProviders, string converter)
        {
            Ast = ast;
            ValueProviders = valueProviders?.ToArray() ?? throw new ArgumentNullException(nameof(valueProviders));
            Converter = converter;
        }

        public BooleanExpression Ast { get; }

        public IValueProvider[] ValueProviders { get; }

        public string Converter { get; }

        public BindingBase ProvideBinding(IResourceContext context)
        {
            var multiBinding = new MultiBinding
            {
                Converter = new BooleanMultiConverter(Ast, Resource.GetValueConverter(context, Converter))
            };

            foreach (var binding in ValueProviders.Select(provider => provider.ProvideBinding(context)))
            {
                multiBinding.Bindings.Add(binding);
            }

            return multiBinding;
        }

        public object ProvideValue(IResourceContext context)
        {
            return ProvideBinding(context);
        }
    }
}