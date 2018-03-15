using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace Forge.Forms.DynamicExpressions.BooleanExpressions
{
    internal class BooleanMultiConverter : IMultiValueConverter
    {
        public BooleanMultiConverter(BooleanExpression expression)
        {
            Expression = expression;
        }

        public BooleanExpression Expression { get; }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var result = Expression.Evaluate(values.Select(c => c is true).ToArray());
            if (targetType == typeof(Visibility))
            {
                return result ? Visibility.Visible : Visibility.Collapsed;
            }

            return result;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return new object[0];
        }
    }
}
