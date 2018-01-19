using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using MaterialDesignThemes.Wpf;

namespace Forge.Forms.DynamicExpressions.ValueConverters
{
    internal class VisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case bool b:
                    return b ? Visibility.Visible : Visibility.Collapsed;
                case Visibility v:
                    return v;
                case PackIconKind i:
                    return i == (PackIconKind)(-2)
                        ? Visibility.Collapsed
                        : Visibility.Visible;
                default:
                    return value == null
                        ? Visibility.Collapsed
                        : Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
