using System;
using System.Globalization;
using System.Windows.Data;

namespace Material.Application.ValueConverters
{
    public class VariableExpanderConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Environment.ExpandEnvironmentVariables(value?.ToString() ?? string.Empty);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
