using System.Globalization;
using System.Windows.Data;
using Forge.Forms.Utils;
using Forge.Forms.Utils.ValueConverters;

namespace Forge.Forms.Interfaces
{
    public interface IValueProvider
    {
        BindingBase ProvideBinding(IResourceContext context);

        object ProvideValue(IResourceContext context);
    }

    public static class ValueProviderExtensions
    {
        public static IProxy GetBestMatchingProxy(this IValueProvider valueProvider, IResourceContext context)
        {
            switch (valueProvider)
            {
                case LiteralValue v:
                    return new PlainObject(v.Value);
                case BoundExpression b:
                    return b.GetProxy(context);
                default:
                    return valueProvider.GetValue(context);
            }
        }

        public static BindingProxy GetValue(this IValueProvider valueProvider, IResourceContext context)
        {
            var proxy = new BindingProxy();
            var value = valueProvider.ProvideValue(context);
            if (value is BindingBase binding)
            {
                BindingOperations.SetBinding(proxy, BindingProxy.ValueProperty, binding);
            }
            else
            {
                proxy.Value = value;
            }

            return proxy;
        }

        public static StringProxy GetStringValue(this IValueProvider valueProvider, IResourceContext context)
        {
            return GetStringValue(valueProvider, context, false);
        }

        public static StringProxy GetStringValue(this IValueProvider valueProvider, IResourceContext context, bool setKey)
        {
            var proxy = new StringProxy();
            var value = valueProvider.ProvideValue(context);
            if (value is BindingBase binding)
            {
                BindingOperations.SetBinding(proxy, StringProxy.ValueProperty, binding);
                if (setKey)
                {
                    BindingOperations.SetBinding(proxy, StringProxy.KeyProperty, binding);
                }
            }
            else
            {
                proxy.Value = value?.ToString();
                if (setKey)
                {
                    proxy.Key = value;
                }
            }

            return proxy;
        }

        public static BoolProxy GetBoolValue(this IValueProvider valueProvider, IResourceContext context)
        {
            var proxy = new BoolProxy();
            var value = valueProvider.ProvideValue(context);
            if (value is BindingBase binding)
            {
                BindingOperations.SetBinding(proxy, BoolProxy.ValueProperty, binding);
            }
            else
            {
                proxy.Value = value is bool b && b;
            }

            return proxy;
        }

        public static IValueProvider Wrap(this IValueProvider valueProvider, string valueConverter)
        {
            return valueConverter == null
                ? valueProvider
                : new ValueProviderWrapper(valueProvider, valueConverter);
        }

        private class ValueProviderWrapper : IValueProvider
        {
            private readonly IValueProvider innerProvider;
            private readonly string valueConverter;

            public ValueProviderWrapper(IValueProvider innerProvider, string valueConverter)
            {
                this.innerProvider = innerProvider;
                this.valueConverter = valueConverter;
            }

            public BindingBase ProvideBinding(IResourceContext context)
            {
                var bindingBase = innerProvider.ProvideBinding(context);
                if (bindingBase is Binding binding)
                {
                    var converter = Resource.GetValueConverter(context, valueConverter);
                    binding.Converter = binding.Converter == null
                        ? converter
                        : new ConverterWrapper(converter, binding.Converter);
                }

                return bindingBase;
            }

            public object ProvideValue(IResourceContext context)
            {
                var value = innerProvider.ProvideValue(context);
                var converter = Resource.GetValueConverter(context, valueConverter);
                if (value is Binding binding)
                {
                    binding.Converter = binding.Converter == null
                        ? converter
                        : new ConverterWrapper(converter, binding.Converter);
                    return binding;
                }

                if (value is BindingBase)
                {
                    return value;
                }

                return converter.Convert(value, typeof(object), null, CultureInfo.CurrentCulture);
            }
        }
    }
}
