using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace FancyGrid.Converters
{
    public class ValidHeaderConverter : IValueConverter
    {
        private static List<DataGridColumnHeader> PropertyDescriptors { get; set; } = new List<DataGridColumnHeader>();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DataGridColumnHeader header)
            {
                var dGrid = FilteringDataGrid.TryFindParent<FilteringDataGrid>(header);

                if (dGrid != null)
                {
                    if (!(header.Column?.Header is string))
                    {
                        return Visibility.Collapsed;
                    }

                    if (!PropertyDescriptors.Contains(header))
                    {
                        DependencyPropertyDescriptor
                            .FromProperty(FilteringDataGrid.CanFilterProperty, typeof(FilteringDataGrid))
                            .AddValueChanged(dGrid, (s, e) =>
                            {
                                if (header.Template.FindName("filterTextBox", header) is TextBox filterTextBox)
                                {
                                    filterTextBox.Text = "";
                                    filterTextBox.Visibility =
                                        dGrid.CanFilter ? Visibility.Visible : Visibility.Collapsed;
                                }
                            });
                        PropertyDescriptors.Add(header);
                    }

                    return dGrid.CanFilter ? Visibility.Visible : Visibility.Collapsed;
                }
            }

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
