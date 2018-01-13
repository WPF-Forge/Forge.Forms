using System.Windows;
using System.Windows.Controls;

namespace Material.Application.Controls
{
    internal class NullAsSeparatorTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var element = container as FrameworkElement;
            if (element == null)
            {
                return null;
            }

            if (item == null)
            {
                return element.FindResource("SeparatorDataTemplate") as DataTemplate;
            }

            return element.FindResource("ItemDataTemplate") as DataTemplate;
        }
    }
}
