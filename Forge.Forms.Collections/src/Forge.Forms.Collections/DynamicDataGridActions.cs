using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Forge.Forms.Collections
{
    public partial class DynamicDataGrid
    {
        public static readonly DependencyProperty RowStyleProperty = DependencyProperty.Register(
            "RowStyle", typeof(Style), typeof(DynamicDataGrid), new PropertyMetadata(default(Style)));

        public Style RowStyle
        {
            get => (Style) GetValue(RowStyleProperty);
            set => SetValue(RowStyleProperty, value);
        }

        public static readonly DependencyProperty CellStyleProperty = DependencyProperty.Register(
            "CellStyle", typeof(Style), typeof(DynamicDataGrid), new PropertyMetadata(default(Style)));

        public Style CellStyle
        {
            get => (Style) GetValue(CellStyleProperty);
            set => SetValue(CellStyleProperty, value);
        }

        public static readonly DependencyProperty ColumnsProperty = DependencyProperty.Register(
            "Columns", typeof(ObservableCollection<DataGridColumn>), typeof(DynamicDataGrid),
            new PropertyMetadata(new ObservableCollection<DataGridColumn>(), OnColumnsPropertyChanged));

        private static void OnColumnsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DynamicDataGrid dynamicDataGrid)
            {
                dynamicDataGrid.ReloadColumns();

                dynamicDataGrid.Columns.CollectionChanged += (sender, args) =>
                {
                    dynamicDataGrid.ReloadColumns();
                };
            }
        }

        public ObservableCollection<DataGridColumn> Columns
        {
            get => (ObservableCollection<DataGridColumn>) GetValue(ColumnsProperty);
            set => SetValue(ColumnsProperty, value);
        }

        public static readonly DependencyProperty AutoGenerateColumnsProperty = DependencyProperty.Register(
            "AutoGenerateColumns", typeof(bool), typeof(DynamicDataGrid), new PropertyMetadata(true, OnAutoGenerateColumnsChanged));

        private static void OnAutoGenerateColumnsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DynamicDataGrid dynamicDataGrid)
            {
                dynamicDataGrid.ReloadColumns();
            }
        }

        public bool AutoGenerateColumns
        {
            get => (bool) GetValue(AutoGenerateColumnsProperty);
            set => SetValue(AutoGenerateColumnsProperty, value);
        }

        public void InitializeCellStyleAndColumns()
        {
            if (CellStyle == null)
                CellStyle = TryFindResource("CustomDataGridCell") as Style;

            Columns.CollectionChanged += (sender, args) =>
            {
                ReloadColumns();
            };
        }
    }

    public partial class DynamicDataGrid
    {
        public static readonly DependencyProperty DeleteActionTextProperty = DependencyProperty.Register(
            nameof(DeleteActionText), typeof(string), typeof(DynamicDataGrid), new PropertyMetadata("Delete"));

        public string DeleteActionText
        {
            get => (string) GetValue(DeleteActionTextProperty);
            set => SetValue(DeleteActionTextProperty, value);
        }

        public static readonly DependencyProperty CreateActionTextProperty = DependencyProperty.Register(
            nameof(CreateActionText), typeof(string), typeof(DynamicDataGrid), new PropertyMetadata("Add"));

        public string CreateActionText
        {
            get => (string) GetValue(CreateActionTextProperty);
            set => SetValue(CreateActionTextProperty, value);
        }

        public static readonly DependencyProperty EditActionTextProperty = DependencyProperty.Register(
            nameof(EditActionText), typeof(string), typeof(DynamicDataGrid), new PropertyMetadata("Edit"));

        public string EditActionText
        {
            get => (string) GetValue(EditActionTextProperty);
            set => SetValue(EditActionTextProperty, value);
        }
    }

    public partial class DynamicDataGrid
    {
        public static readonly DependencyProperty CanCreateActionProperty = DependencyProperty.Register(
            nameof(CanCreateAction), typeof(Func<object, CanExecuteRoutedEventArgs, bool>), typeof(DynamicDataGrid),
            new PropertyMetadata(new Func<object, CanExecuteRoutedEventArgs, bool>((o, args) =>
            {
                if (o is DynamicDataGrid d) d.CanExecuteCreateItem(o, args);

                return args.CanExecute;
            })));

        public static readonly DependencyProperty CanRemoveActionProperty = DependencyProperty.Register(
            nameof(CanRemoveAction), typeof(Func<object, CanExecuteRoutedEventArgs, bool>), typeof(DynamicDataGrid),
            new PropertyMetadata(new Func<object, CanExecuteRoutedEventArgs, bool>((o, args) =>
            {
                if (o is DynamicDataGrid d) d.CanExecuteRemoveItem(o, args);

                return args.CanExecute;
            })));

        public static readonly DependencyProperty CanUpdateActionProperty = DependencyProperty.Register(
            nameof(CanUpdateAction), typeof(Func<object, CanExecuteRoutedEventArgs, bool>), typeof(DynamicDataGrid),
            new PropertyMetadata(new Func<object, CanExecuteRoutedEventArgs, bool>((o, args) =>
            {
                if (o is DynamicDataGrid d) d.CanExecuteUpdateItem(o, args);

                return args.CanExecute;
            })));

        public static readonly DependencyProperty CreateActionProperty = DependencyProperty.Register(
            nameof(CreateAction), typeof(Action<object, ExecutedRoutedEventArgs>), typeof(DynamicDataGrid),
            new PropertyMetadata(new Action<object, ExecutedRoutedEventArgs>(
                (o, args) =>
                {
                    if (o is DynamicDataGrid d) d.ExecuteCreateItem(o, args);
                })));

        public static readonly DependencyProperty RemoveActionProperty = DependencyProperty.Register(
            nameof(RemoveAction), typeof(Action<object, ExecutedRoutedEventArgs>), typeof(DynamicDataGrid),
            new FrameworkPropertyMetadata(new Action<object, ExecutedRoutedEventArgs>(
                (o, args) =>
                {
                    if (o is DynamicDataGrid d) d.ExecuteRemoveItem(o, args);
                })));

        public static readonly DependencyProperty EditActionProperty = DependencyProperty.Register(
            nameof(EditAction), typeof(Action<object, ExecutedRoutedEventArgs>), typeof(DynamicDataGrid),
            new PropertyMetadata(new Action<object, ExecutedRoutedEventArgs>(
                (o, args) =>
                {
                    if (o is DynamicDataGrid d) d.ExecuteUpdateItem(o, args);
                })));

        public Func<object, CanExecuteRoutedEventArgs, bool> CanCreateAction
        {
            get => (Func<object, CanExecuteRoutedEventArgs, bool>) GetValue(CanCreateActionProperty);
            set => SetValue(CanCreateActionProperty, value);
        }

        public Func<object, CanExecuteRoutedEventArgs, bool> CanRemoveAction
        {
            get => (Func<object, CanExecuteRoutedEventArgs, bool>) GetValue(CanRemoveActionProperty);
            set => SetValue(CanRemoveActionProperty, value);
        }

        public Func<object, CanExecuteRoutedEventArgs, bool> CanUpdateAction
        {
            get => (Func<object, CanExecuteRoutedEventArgs, bool>) GetValue(CanUpdateActionProperty);
            set => SetValue(CanUpdateActionProperty, value);
        }

        public Action<object, ExecutedRoutedEventArgs> CreateAction
        {
            get => (Action<object, ExecutedRoutedEventArgs>) GetValue(CreateActionProperty);
            set => SetValue(CreateActionProperty, value);
        }

        public Action<object, ExecutedRoutedEventArgs> RemoveAction
        {
            get => (Action<object, ExecutedRoutedEventArgs>) GetValue(RemoveActionProperty);
            set => SetValue(RemoveActionProperty, value);
        }

        public Action<object, ExecutedRoutedEventArgs> EditAction
        {
            get => (Action<object, ExecutedRoutedEventArgs>) GetValue(EditActionProperty);
            set => SetValue(EditActionProperty, value);
        }
    }
}