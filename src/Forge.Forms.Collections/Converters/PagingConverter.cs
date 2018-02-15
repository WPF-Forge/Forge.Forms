using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Forge.Forms.Collections.Converters
{
    public class PagingConverter : IMultiValueConverter
    {
        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.Any(i => i == null))
            {
                return value[0];
            }

            if (value[1] is DynamicDataGrid dataGrid && dataGrid.ItemsSource != null)
            {
                var view = CollectionViewSource.GetDefaultView(dataGrid.ItemsSource);
                return view.Cast<object>().Skip((dataGrid.CurrentPage - 1) * dataGrid.ItemsPerPage)
                           .Take(dataGrid.ItemsPerPage) ?? DefaultReturn(value, dataGrid);
            }

            return value[0];
        }

        /// <inheritdoc />
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private static IEnumerable<object> DefaultReturn(object[] value, DynamicDataGrid dynamicDataGrid)
        {
            return (value[0] as IEnumerable)?.Cast<object>()
                .Skip((dynamicDataGrid.CurrentPage - 1) * dynamicDataGrid.ItemsPerPage)
                .Take(dynamicDataGrid.ItemsPerPage);
        }
    }
}
