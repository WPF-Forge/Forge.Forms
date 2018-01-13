using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Forge.Forms.Components.Fields
{
    public class StringTypeConverter : IValueConverter
    {
        private readonly Func<string, CultureInfo, object> deserializer;

        public StringTypeConverter(Func<string, CultureInfo, object> deserializer)
        {
            this.deserializer = deserializer;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return deserializer(value as string, culture);
            }
            catch
            {
                return DependencyProperty.UnsetValue;
            }
        }
    }
}
