using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Forge.Forms.Demo.Infrastructure
{
    public class ModelTypeToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value is string || value is ValueType)
            {
                return Visibility.Collapsed;
            }

            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
