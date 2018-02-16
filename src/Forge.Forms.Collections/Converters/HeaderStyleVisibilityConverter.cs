using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Forge.Forms.Collections.Converters
{
    public class HeaderStyleVisibilityConverter : IValueConverter, IMultiValueConverter
    {
        /// <inheritdoc />
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var convert = false;

            if (values[3] is DynamicDataGridHeaderState dynamicDataGridHeaderState)
            {
                switch (dynamicDataGridHeaderState)
                {
                    case DynamicDataGridHeaderState.Selected:
                    {
                        convert = values[0] is int i && i > 0 && Equals(values[1], values[2]);
                        break;
                    }
                    case DynamicDataGridHeaderState.Normal:
                    {
                        convert = values[0] is int i && i == 0 && Equals(values[1], values[2]);
                        break;
                    }
                    case DynamicDataGridHeaderState.All:
                        convert = Equals(values[1], values[2]);
                        break;
                }
            }

            return convert ? Visibility.Visible : Visibility.Collapsed;
        }

        /// <inheritdoc />
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Equals(value, parameter) ? Visibility.Visible : Visibility.Collapsed;
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is Visibility.Visible;
        }
    }
}
