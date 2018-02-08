using System.ComponentModel;
using System.Windows.Data;
using System.Collections.Generic;
using System.Windows;
using System;
using System.Windows.Media;

namespace FancyGrid
{
    public static class Helpers
    {
        //public static string GetSortMemberPath(DataGridColumn column)
        //{
        //    // find the sortmemberpath
        //    string sortPropertyName = column.SortMemberPath;
        //    if (string.IsNullOrEmpty(sortPropertyName))
        //    {
        //        DataGridBoundColumn boundColumn = column as DataGridBoundColumn;
        //        if (boundColumn != null)
        //        {
        //            Binding binding = boundColumn.Binding as Binding;
        //            if (binding != null)
        //            {
        //                if (!string.IsNullOrEmpty(binding.XPath))
        //                {
        //                    sortPropertyName = binding.XPath;
        //                }
        //                else if (binding.Path != null)
        //                {
        //                    sortPropertyName = binding.Path.Path;
        //                }
        //            }
        //        }
        //    }

        //    return sortPropertyName;
        //}

        public static List<T> AllChildren<T>(this FrameworkElement ele, Func<DependencyObject, bool> whereFunc = null) where T : class
        {
            if (ele == null)
                return null;
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
                if ((ch is T))
                    output.Add(ch as T);
                if (!(ch is FrameworkElement))
                    continue;

                output.AddRange((ch as FrameworkElement).AllChildren<T>(whereFunc));
            }
            return output;
        }

        public static SortDescription? FindSortDescription(SortDescriptionCollection sortDescriptions, string sortPropertyName)
        {
            foreach (SortDescription sortDesc in sortDescriptions)
                if (string.Compare(sortDesc.PropertyName, sortPropertyName) == 0)
                    return sortDesc;
            return null;
        }
    }
}
