using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace FancyGrid.Converters
{
    public class ValidHeaderConverter : IValueConverter
    {
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

                    DependencyPropertyDescriptor
                        .FromProperty(FilteringDataGrid.CanFilterProperty, typeof(FilteringDataGrid))
                        .AddValueChanged(dGrid, (s, e) =>
                        {
                            if (header.Template.FindName("filterTextBox", header) is TextBox filterTextBox)
                            {
                                filterTextBox.Text = "";
                                filterTextBox.Visibility = dGrid.CanFilter ? Visibility.Visible : Visibility.Collapsed;
                            }
                        });

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
