using System;
using System.Globalization;
using System.Windows.Data;

namespace Forge.Forms.DynamicExpressions.ValueConverters
{
    public class IsEqualConverter : IValueConverter
    {
        public IsEqualConverter(object value)
        {
            Value = value;
        }

        public object Value { get; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Equals(Value, value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
