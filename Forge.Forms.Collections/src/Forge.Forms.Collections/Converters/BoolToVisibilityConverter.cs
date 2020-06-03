using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Forge.Forms.Collections.Converters
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool valueAsBool)
            {
                var invert = parameter is bool inverted && inverted;
                
                if (invert)
                    valueAsBool = !valueAsBool;
                
                return valueAsBool ? Visibility.Visible : Visibility.Collapsed;
            }
            
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}