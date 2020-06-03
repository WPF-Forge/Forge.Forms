using System.Windows;

namespace FancyGrid.Converters
{
    public class DynamicDataGridExtensions : DependencyObject
    {
        public static readonly DependencyProperty
            CanUserFilterProperty =
                DependencyProperty.RegisterAttached(
                    "CanUserFilter", typeof(bool), typeof(DynamicDataGridExtensions),
                    new PropertyMetadata(true));
        public static bool GetCanUserFilter(
            DependencyObject d)
        {
            return (bool)d.GetValue(CanUserFilterProperty);
        }
        public static void SetCanUserFilter(
            DependencyObject d, bool value)
        {
            d.SetValue(CanUserFilterProperty, value);
        }
    }
}