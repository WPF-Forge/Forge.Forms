using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using MaterialDesignThemes.Wpf;

namespace Forge.Forms.DynamicExpressions.ValueConverters
{
    internal class VisibilityConverter : IValueConverter
    {
        private readonly Visibility hidden;

        public VisibilityConverter()
        {
            hidden = Visibility.Collapsed;
        }

        public VisibilityConverter(bool hiddenOnFalse)
        {
            hidden = hiddenOnFalse ? Visibility.Hidden : Visibility.Collapsed;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case bool b:
                    return b ? Visibility.Visible : hidden;
                case Visibility v:
                    return v;
                case PackIconKind i:
                    return i == (PackIconKind)(-2)
                        ? hidden
                        : Visibility.Visible;
                default:
                    return value == null
                        ? hidden
                        : Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
