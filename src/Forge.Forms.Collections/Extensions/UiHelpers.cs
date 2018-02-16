using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Forge.Forms.Collections.Extensions
{
    /// <summary>
    /// Contains helper methods for UI.
    /// </summary>
    public static class UiHelpers
    {
        /// <summary>
        /// Gets the parent. Which tree the parent is retrieved from depends on the parameters.
        /// </summary>
        /// <param name="child">The child to get parent for.</param>
        /// <param name="searchCompleteVisualTree">
        /// If true the parent in the visual tree is returned, if false the parent may be
        /// retrieved from another tree depending on the child type.
        /// </param>
        /// <returns>
        /// The parent element, and depending on the parameters its retrieved from either visual tree, logical tree or a
        /// tree not strictly speaking either the logical tree or the visual tree.
        /// </returns>
        public static DependencyObject GetParentObject(this DependencyObject child, bool searchCompleteVisualTree)
        {
            if (child == null)
            {
                return null;
            }

            if (!searchCompleteVisualTree)
            {
                if (child is ContentElement contentElement)
                {
                    var parent = ContentOperations.GetParent(contentElement);
                    if (parent != null)
                    {
                        return parent;
                    }

                    return contentElement is FrameworkContentElement fce ? fce.Parent : null;
                }

                if (child is FrameworkElement frameworkElement)
                {
                    var parent = frameworkElement.Parent;
                    if (parent != null)
                    {
                        return parent;
                    }
                }
            }

            return VisualTreeHelper.GetParent(child);
        }

        /// <summary>
        /// Gets first parent element of the specified type. Which tree the parent is retrieved from depends on the parameters.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="child">The child to get parent for.</param>
        /// <param name="searchCompleteVisualTree">
        /// If true the parent in the visual tree is returned, if false the parent may be
        /// retrieved from another tree depending on the child type.
        /// </param>
        /// <returns>
        /// The first parent element of the specified type, and depending on the parameters its retrieved from either visual tree,
        /// logical tree or a tree not strictly speaking either the logical tree or the visual tree.
        /// null is returned if no parent of the specified type is found.
        /// </returns>
        public static T TryFindParent<T>(this DependencyObject child, bool searchCompleteVisualTree = false) where T : DependencyObject
        {
            while (true)
            {
                var parentObject = GetParentObject(child, searchCompleteVisualTree);

                if (parentObject == null)
                {
                    return null;
                }

                if (parentObject is T parent)
                {
                    return parent;
                }

                child = parentObject;
                searchCompleteVisualTree = false;
            }
        }

        /// <summary>
        /// Returns the first child of the specified type found in the visual tree.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parent">The parent element where the search is started.</param>
        /// <returns>
        /// The first child of the specified type found in the visual tree, or null if no parent of the specified type is
        /// found.
        /// </returns>
        public static T TryFindChild<T>(this DependencyObject parent) where T : DependencyObject
        {
            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);

                if (child is T variable)
                {
                    return variable;
                }

                child = TryFindChild<T>(child);
                if (child != null)
                {
                    return (T)child;
                }
            }

            return null;
        }

        /// <summary>
        /// Returns the first child of the specified type and with the specified name found in the visual tree.
        /// </summary>
        /// <param name="parent">The parent element where the search is started.</param>
        /// <param name="childName">The name of the child element to find, or an empty string or null to only look at the type.</param>
        /// <returns>The first child that matches the specified type and child name, or null if no match is found.</returns>
        public static T TryFindChild<T>(this DependencyObject parent, string childName) where T : DependencyObject
        {
            if (parent == null)
            {
                return null;
            }

            T foundChild = null;
            var childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (var i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);

                if (!(child is T))
                {
                    foundChild = TryFindChild<T>(child, childName);
                    if (foundChild != null)
                    {
                        break;
                    }
                }
                else if (!string.IsNullOrEmpty(childName))
                {
                    if (child is FrameworkElement frameworkElement && frameworkElement.Name == childName)
                    {
                        foundChild = (T)child;
                        break;
                    }
                }
                else
                {
                    foundChild = (T)child;
                    break;
                }
            }

            return foundChild;
        }

        /// <summary>
        /// Gets the datagrid rows.
        /// </summary>
        /// <param name="grid">The grid.</param>
        /// <returns></returns>
        public static IEnumerable<DataGridRow> GetRows(this DataGrid grid)
        {
            var itemsSource = grid.ItemsSource;
            if (null == itemsSource)
            {
                yield return null;
            }

            if (itemsSource != null)
            {
                foreach (var item in itemsSource)
                {
                    if (grid.ItemContainerGenerator.ContainerFromItem(item) is DataGridRow row)
                    {
                        yield return row;
                    }
                }
            }
        }

        /// <summary>
        /// Returns the first ancestor of specified type
        /// </summary>
        public static T FindAncestor<T>(DependencyObject current) where T : DependencyObject
        {
            current = GetVisualOrLogicalParent(current);

            while (current != null)
            {
                if (current is T variable)
                {
                    return variable;
                }

                current = GetVisualOrLogicalParent(current);
            }

            return null;
        }

        private static DependencyObject GetVisualOrLogicalParent(DependencyObject obj)
        {
            if (obj is Visual || obj is Visual3D)
            {
                return VisualTreeHelper.GetParent(obj);
            }

            return LogicalTreeHelper.GetParent(obj);
        }
    }
}
