using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Forge.Forms.Utils.ValueConverters
{
    internal class BoolOrVisibilityConverter : IValueConverter
    {
        private readonly IValueConverter innerConverter;

        public BoolOrVisibilityConverter(IValueConverter innerConverter)
        {
            this.innerConverter = innerConverter;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (innerConverter != null)
            {
                value = innerConverter.Convert(value, targetType, parameter, culture);
            }

            switch (value)
            {
                case bool b:
                    return b ? Visibility.Visible : Visibility.Collapsed;
                case Visibility v:
                    return v;
                default:
                    return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
