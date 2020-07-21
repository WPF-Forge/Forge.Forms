using System;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace Forge.Forms.DynamicExpressions.MultiValueConverters
{
    public class AppendMultiConverter : IMultiValueConverter
    {
        public AppendMultiConverter()
        {
        }

        public AppendMultiConverter(object delimeter)
        {
            Delimeter = delimeter;
        }

        public object Delimeter { get; }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (Delimeter != null)
            {
                return string.Join(Delimeter.ToString() ?? string.Empty, values);
            }
            else
            {
                var sb = new StringBuilder();
                foreach (var val in values)
                {
                    sb.Append(val);
                }

                return sb.ToString();
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}