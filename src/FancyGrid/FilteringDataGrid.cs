using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;

namespace FancyGrid
{
    /// <inheritdoc />
    /// <summary>
    /// A grid that makes inline filtering possible.
    /// </summary>
    public class FilteringDataGrid : DataGrid
    {
        /// <summary>
        /// The can filter property
        /// </summary>
        public static readonly DependencyProperty CanFilterProperty =
            DependencyProperty.Register("CanFilter", typeof(bool), typeof(FilteringDataGrid),
                new PropertyMetadata(false));


        /// <summary>
        /// Case sensitive filtering
        /// </summary>
        public static DependencyProperty IsFilteringCaseInternalSensitiveProperty =
            DependencyProperty.Register("IsFilteringCaseInternalSensitive", typeof(bool), typeof(FilteringDataGrid),
                new PropertyMetadata(true));

        /// <summary>
        /// Identifies the ClearFiltersMessage dependency property.
        /// </summary>
        public static DependencyProperty ClearFiltersInternalMessageProperty =
            DependencyProperty.Register("ClearFiltersInternalMessage", typeof(string), typeof(FilteringDataGrid),
                new PropertyMetadata("Clear all filters"));

        /// <summary>
        /// Identifies the IncludeItemsMessage dependency property.
        /// </summary>
        public static DependencyProperty IncludeItemsInternalMessageProperty =
            DependencyProperty.Register("IncludeItemsInternalMessage", typeof(string), typeof(FilteringDataGrid),
                new PropertyMetadata("Include items like this"));

        /// <summary>
        /// Identifies the ExcludeItemsMessage dependency property.
        /// </summary>
        public static DependencyProperty ExcludeItemsInternalMessageProperty =
            DependencyProperty.Register("ExcludeItemsInternalMessage", typeof(string), typeof(FilteringDataGrid),
                new PropertyMetadata("Exclude items like this"));

        /// <summary>
        /// This dictionary will map a column to the filter behavior
        /// </summary>
        private readonly Dictionary<string, Func<object, string, bool>> columnFilterModes;

        /// <summary>
        /// This dictionary will have a list of all applied filters
        /// </summary>
        private readonly Dictionary<string, string> columnFilters;

        /// <summary>
        /// Cache with properties for better performance
        /// </summary>
        private readonly Dictionary<string, PropertyInfo> propertyCache;


        /// <inheritdoc />
        /// <summary>
        /// Register for all text changed events
        /// </summary>
        public FilteringDataGrid()
        {
            columnFilters = new Dictionary<string, string>();
            columnFilterModes = new Dictionary<string, Func<object, string, bool>>();
            propertyCache = new Dictionary<string, PropertyInfo>();
            AddHandler(TextBoxBase.TextChangedEvent, new TextChangedEventHandler(OnTextChanged), true);
            AutoGenerateColumns = false;
            DataContextChanged += FilteringDataGrid_DataContextChanged;

            ContextMenuOpening += FilteringDataGrid_ContextMenuOpening;
        }

        public bool CanFilter
        {
            get => (bool)GetValue(CanFilterProperty);
            set => SetValue(CanFilterProperty, value);
        }

        public IEnumerable<MenuItem> ExtraContextMenuItems { get; set; }

        /// <summary>
        /// Case sensitive filtering
        /// </summary>
        public bool IsFilteringCaseInternalSensitive
        {
            get => (bool)GetValue(IsFilteringCaseInternalSensitiveProperty);
            set => SetValue(IsFilteringCaseInternalSensitiveProperty, value);
        }

        /// <summary>
        /// Gets or sets the clear filters internal message.
        /// </summary>
        /// <value>
        /// The clear filters internal message.
        /// </value>
        public string ClearFiltersInternalMessage
        {
            get => (string)GetValue(ClearFiltersInternalMessageProperty);
            set => SetValue(ClearFiltersInternalMessageProperty, value);
        }

        /// <summary>
        /// Gets or sets the include items internal message.
        /// </summary>
        /// <value>
        /// The include items internal message.
        /// </value>
        public string IncludeItemsInternalMessage
        {
            get => (string)GetValue(IncludeItemsInternalMessageProperty);
            set => SetValue(IncludeItemsInternalMessageProperty, value);
        }

        /// <summary>
        /// Gets or sets the exclude items internal message.
        /// </summary>
        /// <value>
        /// The exclude items internal message.
        /// </value>
        public string ExcludeItemsInternalMessage
        {
            get => (string)GetValue(ExcludeItemsInternalMessageProperty);
            set => SetValue(ExcludeItemsInternalMessageProperty, value);
        }

        private void FilteringDataGrid_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            if (!Equals(sender, this))
            {
                return;
            }

            var row = TryFindParent<DataGridRow>((DependencyObject)e.OriginalSource);

            if (row == null)
            {
                return;
            }

            var cell = TryFindParent<DataGridCell>((DependencyObject)e.OriginalSource);

            if (cell == null)
            {
                return;
            }

            var cellInfo = new DataGridCellInfo(cell);

            if (ContextMenu == null)
            {
                ContextMenu = new ContextMenu();
            }

            ContextMenu.Items.Clear();

            ContextMenu.Items.Add(new MenuItem
            {
                Header =
                    $"{IncludeItemsInternalMessage} {GetCellValue(cellInfo, cell)}",
                Command = new FunctionRunnerCommand(FilterToSelected)
            });

            ContextMenu?.Items.Add(new MenuItem
            {
                Header =
                    $"{ExcludeItemsInternalMessage} {GetCellValue(cellInfo, cell)}",
                Command = new FunctionRunnerCommand(FilterToNotSelected)
            });

            ContextMenu?.Items.Add(new MenuItem
            {
                Header = ClearFiltersInternalMessage,
                Command = new FunctionRunnerCommand(ClearFilters)
            });

            if (ExtraContextMenuItems != null)
            {
                ContextMenu?.Items.Add(new Separator());
                foreach (var unused in ExtraContextMenuItems)
                {
                    ContextMenu?.Items.Add(ExtraContextMenuItems);
                }
            }
        }

        private static object GetCellValue(DataGridCellInfo cellInfo, DataGridCell cell)
        {
            return cellInfo.Item?.GetType().GetProperty(cell.Column.SortMemberPath)?.GetValue(cellInfo.Item);
        }

        private void FilterToSelected(object p)
        {
            var tbs = this.AllChildren<TextBox>();
            TextBox tb = null;
            foreach (var item in tbs)
            {
                var header = TryFindParent<DataGridColumnHeader>(item);
                if (header.Column != null && Equals(header.Column, CurrentColumn))
                {
                    tb = item;
                    break;
                }
            }

            if (tb != null)
            {
                tb.Text = (CurrentColumn.GetCellContent(CurrentCell.Item) as TextBlock)?.Text ??
                          throw new InvalidOperationException();
            }
        }

        private void FilterToNotSelected(object p)
        {
            var tbs = this.AllChildren<TextBox>();
            TextBox tb = null;
            foreach (var item in tbs)
            {
                var header = TryFindParent<DataGridColumnHeader>(item);
                if (header.Column != null && Equals(header.Column, CurrentColumn))
                {
                    tb = item;
                    break;
                }
            }

            var text = (CurrentColumn.GetCellContent(CurrentCell.Item) as TextBlock)?.Text;
            if (tb != null)
            {
                tb.Text = "!" + text;
            }
        }

        private void ClearFilters(object p)
        {
            foreach (var item in this.AllChildren<TextBox>())
            {
                item.Clear();
            }

            columnFilters.Clear();
            columnFilterModes.Clear();
            ApplyFilters();
        }

        public event EventHandler AfterSorting;

        private void FilteringDataGrid_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            propertyCache.Clear();
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var filterTextBox = e.OriginalSource as TextBox;

            var header = TryFindParent<DataGridColumnHeader>(filterTextBox);
            if (header != null)
            {
                UpdateFilter(filterTextBox, header);
                ApplyFilters();
            }
        }

        private void UpdateFilter(TextBox textBox, DataGridColumnHeader header)
        {
            var columnBinding = header.Column.SortMemberPath ?? "";

            if (!string.IsNullOrEmpty(columnBinding))
            {
                var filter = textBox.Text;

                if (filter.StartsWith("="))
                {
                    columnFilterModes[columnBinding] = fm_is;
                }
                else if (filter.StartsWith("!"))
                {
                    columnFilterModes[columnBinding] = fm_isNot;
                }
                else if (filter.StartsWith("~"))
                {
                    columnFilterModes[columnBinding] = fm_doesNotContain;
                }
                else if (filter.StartsWith("<"))
                {
                    columnFilterModes[columnBinding] = fm_Lessthan;
                }
                else if (filter.StartsWith(">"))
                {
                    columnFilterModes[columnBinding] = fm_GreaterThanEqual;
                }
                else if (filter == "\"\"")
                {
                    columnFilterModes[columnBinding] = fm_blank;
                }
                else if (filter == @"*")
                {
                    columnFilterModes[columnBinding] = fm_notblank;
                }
                else
                {
                    columnFilterModes[columnBinding] = fm_Contains;
                }

                columnFilters[columnBinding] = filter.TrimStart('<', '>', '~', '=', '!');
            }
        }


        private void ApplyFilters()
        {
            var view = CollectionViewSource.GetDefaultView(ItemsSource);
            view.Filter = Filter;

            var expression = BindingOperations.GetMultiBindingExpression(this, ItemsSourceProperty);
            expression?.UpdateSource();
            expression?.UpdateTarget();
        }

        private bool Filter(object item)
        {
            foreach (var filter in columnFilters)
            {
                var property = GetPropertyValue(item, filter.Key);
                if (property != null && !string.IsNullOrEmpty(filter.Value))
                {
                    if (columnFilterModes.ContainsKey(filter.Key))
                    {
                        if (!columnFilterModes[filter.Key](property, filter.Value))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return fm_Contains(property, filter.Value);
                    }
                }
                else if (!string.IsNullOrEmpty(filter.Value))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Get the value of a property
        /// </summary>
        /// <param name="item"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        private object GetPropertyValue(object item, string property)
        {
            object value = null;

            PropertyInfo pi;
            if (propertyCache.ContainsKey(property))
            {
                pi = propertyCache[property];
            }
            else
            {
                pi = item.GetType().GetProperty(property);
                propertyCache.Add(property, pi);
            }

            if (pi != null)
            {
                value = pi.GetValue(item, null);
            }

            return value;
        }

        /// <summary>
        /// Finds a parent of a given item on the visual tree.
        /// </summary>
        /// <typeparam name="T">The type of the queried item.</typeparam>
        /// <param name="child">A direct or indirect child of the queried item.</param>
        /// <returns>
        /// The first parent item that matches the submitted
        /// type parameter. If not matching item can be found, a null reference is being returned.
        /// </returns>
        public static T TryFindParent<T>(DependencyObject child) where T : DependencyObject
        {
            while (true)
            {
                var parentObject = GetParentObject(child);

                if (parentObject == null)
                {
                    return null;
                }

                if (parentObject is T parent)
                {
                    return parent;
                }

                child = parentObject;
            }
        }

        /// <summary>
        /// This method is an alternative to WPF's
        /// <see cref="VisualTreeHelper.GetParent" /> method, which also
        /// supports content elements. Do note, that for content element,
        /// this method falls back to the logical tree of the element.
        /// </summary>
        /// <param name="child">The item to be processed.</param>
        /// <returns>The submitted item's parent, if available. Otherwise null.</returns>
        public static DependencyObject GetParentObject(DependencyObject child)
        {
            if (child == null)
            {
                return null;
            }

            if (child is ContentElement contentElement)
            {
                var parent = ContentOperations.GetParent(contentElement);
                if (parent != null)
                {
                    return parent;
                }

                return contentElement is FrameworkContentElement fce ? fce.Parent : null;
            }

            return VisualTreeHelper.GetParent(child);
        }

        protected virtual void OnAfterSorting()
        {
            AfterSorting?.Invoke(this, EventArgs.Empty);
        }

        #region FilterMethods

        private bool fm_blank(object item, string filter)
        {
            return string.IsNullOrWhiteSpace(item?.ToString());
        }

        private bool fm_notblank(object item, string filter)
        {
            return !fm_blank(item, filter);
        }

        private bool fm_Contains(object item, string filter)
        {
            if (string.IsNullOrEmpty(item.ToString()))
            {
                return false;
            }

            if (IsFilteringCaseInternalSensitive)
            {
                var fmContains = item.ToString().Contains(filter);
                return fmContains;
            }

            var contains = item.ToString().IndexOf(filter,
                               IsFilteringCaseInternalSensitive
                                   ? StringComparison.OrdinalIgnoreCase
                                   : StringComparison.Ordinal) >= 0;
            return contains;
        }

        private bool fm_Startswith(object item, string filter)
        {
            var compareMode = IsFilteringCaseInternalSensitive
                ? StringComparison.CurrentCulture
                : StringComparison.CurrentCultureIgnoreCase;

            return item.ToString().StartsWith(filter, compareMode);
        }

        private bool fm_Lessthan(object item, string filter)
        {
            if (double.TryParse(filter, out var b) && double.TryParse(item.ToString(), out var a))
            {
                return a < b;
            }

            return false;
        }

        private bool fm_GreaterThanEqual(object item, string filter)
        {
            return !fm_Lessthan(item, filter);
        }

        private bool fm_Endswith(object item, string filter)
        {
            var compareMode = IsFilteringCaseInternalSensitive
                ? StringComparison.CurrentCulture
                : StringComparison.CurrentCultureIgnoreCase;
            return item.ToString().EndsWith(filter, compareMode);
        }

        private bool fm_is(object item, string filter)
        {
            var a = item.ToString();
            if (IsFilteringCaseInternalSensitive)
            {
                a = a.ToLower();
                filter = filter.ToLower();
            }

            return a == filter;
        }

        private bool fm_isNot(object item, string filter)
        {
            return !fm_is(item, filter);
        }

        private bool fm_doesNotContain(object item, string filter)
        {
            return !fm_Contains(item, filter);
        }

        #endregion
    }
}
