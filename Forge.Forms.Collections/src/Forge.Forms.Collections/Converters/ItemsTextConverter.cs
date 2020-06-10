using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace Forge.Forms.Collections.Converters
{
    public class ItemsTextConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length > 3 && values.Last() is string format)
            {
                return string.Format(format, values.Take(values.Length - 1).ToArray());
            }

            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}