using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace Forge.Forms.Collections.Converters
{
    public class CheckedConverter : IMultiValueConverter
    {
        private static Dictionary<DynamicDataGrid, Dictionary<object, bool>> CheckedItems { get; } =
            new Dictionary<DynamicDataGrid, Dictionary<object, bool>>();

        internal static bool IsChecked(DynamicDataGrid grid, object model)
        {
            var i = GetItems(grid);
            if (!i.ContainsKey(model))
                i.Add(model, false);

            return i[model];
        }

        internal static void Remove(DynamicDataGrid grid, object model)
        {
            var i = GetItems(grid);
            i.Remove(model);
            grid.OnPropertyChanged("SelectedItems");
        }

        internal static void SetChecked(DynamicDataGrid grid, object model, bool state)
        {
            var i = GetItems(grid);
            if (!i.ContainsKey(model))
                i.Add(model, false);

            i[model] = state;
            grid.OnPropertyChanged("SelectedItems");
        }

        internal static Dictionary<object, bool> GetItems(DynamicDataGrid grid)
        {
            if (!CheckedItems.ContainsKey(grid))
                CheckedItems.Add(grid, new Dictionary<object, bool>());

            return CheckedItems[grid];
        }

        /// <inheritdoc />
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is CheckBox && values[1] is DataGridRow row && parameter is DynamicDataGrid dataGrid)
            {
                var items = GetItems(dataGrid);

                if (!items.ContainsKey(row.Item))
                    items.Add(row.Item, false);

                dataGrid.UpdateHeaderButton();

                return items[row.Item];
            }

            return false;
        }

        /// <inheritdoc />
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return new[] { value };
        }
    }
}
