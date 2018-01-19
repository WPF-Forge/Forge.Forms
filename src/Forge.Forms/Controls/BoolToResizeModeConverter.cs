using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Forge.Forms.Controls
{
    internal class BoolToResizeModeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is bool && (bool)value ? ResizeMode.CanResize : ResizeMode.CanMinimize;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}