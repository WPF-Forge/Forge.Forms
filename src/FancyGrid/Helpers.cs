using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace FancyGrid
{
    public static class Helpers
    {
        public static List<T> AllChildren<T>(this FrameworkElement ele, Func<DependencyObject, bool> whereFunc = null)
            where T : class
        {
            if (ele == null)
            {
                return null;
            }

            var output = new List<T>();
            var c = VisualTreeHelper.GetChildrenCount(ele);
            for (var i = 0; i < c; i++)
            {
                var ch = VisualTreeHelper.GetChild(ele, i);
                if (whereFunc != null)
                {
                    if (!whereFunc(ch))
                    {
                        continue;
                    }
                }

                if (ch is T)
                {
                    output.Add(ch as T);
                }

                if (!(ch is FrameworkElement))
                {
                    continue;
                }

                output.AddRange((ch as FrameworkElement).AllChildren<T>(whereFunc));
            }

            return output;
        }

        public static SortDescription? FindSortDescription(SortDescriptionCollection sortDescriptions,
            string sortPropertyName)
        {
            foreach (var sortDesc in sortDescriptions)
            {
                if (string.Equals(sortDesc.PropertyName, sortPropertyName, StringComparison.Ordinal))
                {
                    return sortDesc;
                }
            }

            return null;
        }
    }
}
