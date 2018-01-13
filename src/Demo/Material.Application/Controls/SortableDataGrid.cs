using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Material.Application.Controls
{
    public class SortableDataGrid : DataGrid
    {
        #region Dependency Properties

        // The main collection of the Grid. It should be of type ISortable in order to be sortable.
        public ISortable FullItemsSource
        {
            get { return (ISortable)GetValue(FullItemsSourceProperty); }
            set { SetValue(FullItemsSourceProperty, value); }
        }

        public static readonly DependencyProperty FullItemsSourceProperty = DependencyProperty.Register(nameof(FullItemsSource), typeof(ISortable), typeof(SortableDataGrid));
        #endregion

        #region Private Properties

        private ListSortDirection? currentSortDirection;
        private DataGridColumn currentSortColumn;

        #endregion

        #region Event Handlers

        // Get the current sort column from XAML or sort using the first column of the Grid.
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            // The current sorted column must be specified in XAML.
            currentSortColumn = Columns.FirstOrDefault(c => c.SortDirection.HasValue);

            //// if not, then take the first column of the grid and set the sort direction to ascending
            //if (currentSortColumn == null)
            //{
            //    currentSortColumn = Columns.First();
            //    currentSortColumn.SortDirection = ListSortDirection.Ascending;
            //}

            //currentSortDirection = currentSortColumn.SortDirection;
        }

        // Deactivate the default Grid sorting, call the ISortbleSorting
        protected override void OnSorting(DataGridSortingEventArgs eventArgs)
        {
            eventArgs.Handled = true;

            currentSortColumn = eventArgs.Column;

            var direction = (currentSortColumn.SortDirection != ListSortDirection.Ascending)
                ? ListSortDirection.Ascending
                : ListSortDirection.Descending;

            // Call ISortable Sorting to sort the complete collection
            FullItemsSource?.Sort(currentSortColumn.SortMemberPath, direction.ToString());

            currentSortColumn.SortDirection = direction;

            currentSortDirection = direction;
        }

        // Restores the sorting direction every time the source gets updated e.g. the page is changed
        protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            base.OnItemsSourceChanged(oldValue, newValue);

            if (currentSortColumn != null)
                currentSortColumn.SortDirection = currentSortDirection;
        }

        #endregion
    }
}