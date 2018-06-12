using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using Forge.Forms.FormBuilding;

namespace Forge.Forms.DynamicExpressions.BooleanExpressions
{
    internal class BooleanMultiConverter : IMultiValueConverter
    {
        public BooleanMultiConverter(BooleanExpression expression, IValueConverter innerConverter)
        {
            Expression = expression;
            Converter = innerConverter;
        }

        public BooleanExpression Expression { get; }

        public IValueConverter Converter { get; }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var boolResult = Expression.Evaluate(values.Select(c => c is true).ToArray());
            object result = boolResult;
            if (Converter != null)
            {
                result = Converter.Convert(boolResult, targetType, parameter, culture);
            }

            if (targetType == typeof(Visibility))
            {
                switch (result)
                {
                    case bool b:
                        return b ? Visibility.Visible : Visibility.Collapsed;
                    case Visibility v:
                        return v;
                    default:
                        return Visibility.Collapsed;
                }
            }

            return result;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return new object[0];
        }

        private static IValueConverter GetValueConverter(IResourceContext context, string valueConverter)
        {
            if (string.IsNullOrEmpty(valueConverter))
            {
                return null;
            }

            if (Resource.ValueConverters.TryGetValue(valueConverter, out var c))
            {
                return c;
            }

            if (context != null && context.TryFindResource(valueConverter) is IValueConverter converter)
            {
                return converter;
            }

            throw new InvalidOperationException($"Value converter '{valueConverter}' not found.");
        }
    }
}
