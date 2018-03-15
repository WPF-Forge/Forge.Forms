using System;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using Forge.Forms.DynamicExpressions;

namespace Forge.Forms.FormBuilding
{
    /// <summary>
    /// Markup extension for creating deferred bindings.
    /// </summary>
    public class FormBindingExtension : MarkupExtension
    {
        public FormBindingExtension()
        {
        }

        public FormBindingExtension(string name)
        {
            Name = name;
        }

        [ConstructorArgument("name")]
        public string Name { get; set; }

        public string Converter { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var pvt = serviceProvider as IProvideValueTarget;
            if (pvt?.TargetObject == null)
            {
                return null;
            }

            if (pvt.TargetObject is FrameworkElement frameworkElement)
            {
                if (!(frameworkElement.DataContext is IBindingProvider field))
                {
                    return pvt.TargetProperty is DependencyProperty
                        ? DependencyProperty.UnsetValue
                        : null;
                }

                var value = field.ProvideValue(Name);
                if (value is BindingBase binding)
                {
                    if (pvt.TargetProperty is DependencyProperty dp && dp.PropertyType == typeof(BindingBase)
                        || pvt.TargetProperty is PropertyInfo p && p.PropertyType == typeof(BindingBase))
                    {
                        return binding;
                    }

                    var providedValue = binding.ProvideValue(serviceProvider);
                    if (providedValue is BindingExpressionBase expression)
                    {
                        field.BindingCreated(expression, Name);
                    }

                    return providedValue;
                }

                return value;
            }

            if (pvt.TargetObject is TriggerBase || pvt.TargetObject is SetterBase)
            {
                if (pvt.TargetProperty is DependencyProperty dp2 && dp2.PropertyType == typeof(BindingBase)
                    || pvt.TargetProperty is PropertyInfo p2 && p2.PropertyType == typeof(BindingBase))
                {
                    return new Binding($"[{Name}].Value")
                    {
                        Converter = GetConverter()
                    };
                }

                return new Binding($"[{Name}].Value")
                {
                    Converter = GetConverter()
                }.ProvideValue(serviceProvider);
            }

            return this;
        }

        private IValueConverter GetConverter()
        {
            if (Converter == null)
            {
                return null;
            }

            return Resource.GetValueConverter(null, Converter);
        }
    }
}
