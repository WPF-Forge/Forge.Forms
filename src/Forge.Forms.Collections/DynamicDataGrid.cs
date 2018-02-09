using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using FancyGrid;
using Forge.Forms.Annotations;
using Forge.Forms.Collections.Interfaces;
using Forge.Forms.DynamicExpressions;
using Forge.Forms.FormBuilding;
using Forge.Forms.FormBuilding.Defaults;
using Humanizer;
using MaterialDesignThemes.Wpf;
using PropertyChanged;
using Expression = System.Linq.Expressions.Expression;

namespace Forge.Forms.Collections
{
    [TemplatePart(Name = "PART_DataGrid", Type = typeof(DataGrid))]
    public class DynamicDataGrid : Control, INotifyPropertyChanged
    {
        public static readonly DependencyProperty ToggleFilterCommandProperty =
            DependencyProperty.Register("ToggleFilterCommand", typeof(ICommand), typeof(DynamicDataGrid),
                new PropertyMetadata());

        public static readonly DependencyProperty IsFilteringEnabledProperty =
            DependencyProperty.Register("IsFilteringEnabled", typeof(bool), typeof(DynamicDataGrid),
                new PropertyMetadata(false));


        public static readonly DependencyProperty CreateDialogPositiveContentProperty =
            DependencyProperty.Register(
                nameof(CreateDialogPositiveContent),
                typeof(string),
                typeof(DynamicDataGrid),
                new FrameworkPropertyMetadata("OK"));

        public static readonly DependencyProperty CreateDialogPositiveIconProperty =
            DependencyProperty.Register(
                nameof(CreateDialogPositiveIcon),
                typeof(PackIconKind?),
                typeof(DynamicDataGrid),
                new FrameworkPropertyMetadata(PackIconKind.Check));

        public static readonly DependencyProperty CreateDialogNegativeContentProperty = DependencyProperty.Register(
            nameof(CreateDialogNegativeContent),
            typeof(string),
            typeof(DynamicDataGrid),
            new FrameworkPropertyMetadata("CANCEL"));

        public static readonly DependencyProperty CreateDialogNegativeIconProperty =
            DependencyProperty.Register(
                nameof(CreateDialogNegativeIcon),
                typeof(PackIconKind?),
                typeof(DynamicDataGrid),
                new FrameworkPropertyMetadata(PackIconKind.Close));

        public static readonly DependencyProperty UpdateDialogPositiveContentProperty = DependencyProperty.Register(
            nameof(UpdateDialogPositiveContent),
            typeof(string),
            typeof(DynamicDataGrid),
            new FrameworkPropertyMetadata("OK"));

        public static readonly DependencyProperty UpdateDialogPositiveIconProperty =
            DependencyProperty.Register(
                nameof(UpdateDialogPositiveIcon),
                typeof(PackIconKind?),
                typeof(DynamicDataGrid),
                new FrameworkPropertyMetadata(PackIconKind.Check));

        public static readonly DependencyProperty UpdateDialogNegativeContentProperty = DependencyProperty.Register(
            nameof(UpdateDialogNegativeContent),
            typeof(string),
            typeof(DynamicDataGrid),
            new FrameworkPropertyMetadata("CANCEL"));

        public static readonly DependencyProperty UpdateDialogNegativeIconProperty =
            DependencyProperty.Register(
                nameof(UpdateDialogNegativeIcon),
                typeof(PackIconKind?),
                typeof(DynamicDataGrid),
                new FrameworkPropertyMetadata(PackIconKind.Close));

        public static readonly DependencyProperty RemoveDialogTitleContentProperty = DependencyProperty.Register(
            nameof(RemoveDialogTitleContent),
            typeof(string),
            typeof(DynamicDataGrid),
            new FrameworkPropertyMetadata());

        public static readonly DependencyProperty RemoveDialogTextContentProperty = DependencyProperty.Register(
            nameof(RemoveDialogTextContent),
            typeof(string),
            typeof(DynamicDataGrid),
            new FrameworkPropertyMetadata("Remove item?"));

        public static readonly DependencyProperty RemoveDialogPositiveContentProperty = DependencyProperty.Register(
            nameof(RemoveDialogPositiveContent),
            typeof(string),
            typeof(DynamicDataGrid),
            new FrameworkPropertyMetadata("REMOVE"));

        public static readonly DependencyProperty RemoveDialogPositiveIconProperty =
            DependencyProperty.Register(
                nameof(RemoveDialogPositiveIcon),
                typeof(PackIconKind?),
                typeof(DynamicDataGrid),
                new FrameworkPropertyMetadata(PackIconKind.Delete));

        public static readonly DependencyProperty RemoveDialogNegativeContentProperty = DependencyProperty.Register(
            nameof(RemoveDialogNegativeContent),
            typeof(string),
            typeof(DynamicDataGrid),
            new FrameworkPropertyMetadata("CANCEL"));

        public static readonly DependencyProperty RemoveDialogNegativeIconProperty =
            DependencyProperty.Register(
                nameof(RemoveDialogNegativeIcon),
                typeof(PackIconKind?),
                typeof(DynamicDataGrid),
                new FrameworkPropertyMetadata(PackIconKind.Close));

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(
                nameof(ItemsSource),
                typeof(IEnumerable),
                typeof(DynamicDataGrid),
                new FrameworkPropertyMetadata
                {
                    PropertyChangedCallback = PropertyChangedCallback
                });

        /// <summary>
        /// Identifies the NextPage dependency property.
        /// </summary>
        public static DependencyProperty MoveNextCommandProperty =
            DependencyProperty.Register("MoveNextCommand", typeof(ICommand), typeof(DynamicDataGrid),
                new PropertyMetadata());

        /// <summary>
        /// Identifies the ClearFiltersMessage dependency property.
        /// </summary>
        public static DependencyProperty ClearFiltersMessageProperty =
            DependencyProperty.Register("ClearFiltersMessage", typeof(string), typeof(FilteringDataGrid),
                new PropertyMetadata("Clear all filters"));

        /// <summary>
        /// Identifies the IncludeItemsMessage dependency property.
        /// </summary>
        public static DependencyProperty IncludeItemsMessageProperty =
            DependencyProperty.Register("IncludeItemsMessage", typeof(string), typeof(FilteringDataGrid),
                new PropertyMetadata("Include items like this"));

        /// <summary>
        /// Identifies the ExcludeItemsMessage dependency property.
        /// </summary>
        public static DependencyProperty ExcludeItemsMessageProperty =
            DependencyProperty.Register("ExcludeItemsMessage", typeof(string), typeof(FilteringDataGrid),
                new PropertyMetadata("Exclude items like this"));

        /// <summary>
        /// Identifies the MovePrevious dependency property.
        /// </summary>
        public static DependencyProperty MoveBackCommandProperty =
            DependencyProperty.Register("MoveBackCommand", typeof(ICommand), typeof(DynamicDataGrid),
                new PropertyMetadata());

        public static readonly DependencyProperty DialogOptionsProperty =
            DependencyProperty.Register(
                nameof(DialogOptions),
                typeof(DialogOptions),
                typeof(DynamicDataGrid),
                new FrameworkPropertyMetadata(DialogOptions.Default, ItemsSourceChanged));

        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(DynamicDataGrid),
                new PropertyMetadata(""));

        // Using a DependencyProperty as the backing store for HasCheckboxes.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HasCheckboxesProperty =
            DependencyProperty.Register("HasCheckboxes", typeof(bool), typeof(DynamicDataGrid),
                new PropertyMetadata(true));

        public static readonly RoutedCommand CreateItemCommand = new RoutedCommand();
        public static readonly RoutedCommand UpdateItemCommand = new RoutedCommand();
        public static readonly RoutedCommand RemoveItemCommand = new RoutedCommand();

        public static readonly List<IAddActionInterceptor> AddInterceptorChain = new List<IAddActionInterceptor>();

        public static readonly List<IUpdateActionInterceptor> UpdateInterceptorChain =
            new List<IUpdateActionInterceptor>();

        public static readonly List<IRemoveActionInterceptor> RemoveInterceptorChain =
            new List<IRemoveActionInterceptor>();

        public static readonly DependencyProperty RowsPerPageTextProperty =
            DependencyProperty.Register("RowsPerPageText", typeof(string), typeof(DynamicDataGrid),
                new PropertyMetadata("Rows per page"));

        /// <summary>
        /// Identifies the CurrentPage dependency property.
        /// </summary>
        public static DependencyProperty CurrentPageProperty =
            DependencyProperty.Register("CurrentPage", typeof(int), typeof(DynamicDataGrid), new PropertyMetadata(1)
            {
                PropertyChangedCallback = PropertyChangedCallback
            });

        public static readonly DependencyProperty IsDeleteButtonVisibleProperty =
            DependencyProperty.Register("IsDeleteButtonVisible", typeof(bool), typeof(DynamicDataGrid),
                new PropertyMetadata(false));

        public static readonly DependencyProperty IsFilterButtonVisibleProperty =
            DependencyProperty.Register("IsFilterButtonVisible", typeof(bool), typeof(DynamicDataGrid),
                new PropertyMetadata(true));

        private bool canMutate;
        private Type itemType;

        static DynamicDataGrid()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DynamicDataGrid),
                new FrameworkPropertyMetadata(typeof(DynamicDataGrid)));
        }

        public DynamicDataGrid()
        {
            CommandBindings.Add(new CommandBinding(CreateItemCommand, ExecuteCreateItem, CanExecuteCreateItem));
            CommandBindings.Add(new CommandBinding(UpdateItemCommand, ExecuteUpdateItem, CanExecuteUpdateItem));
            CommandBindings.Add(new CommandBinding(RemoveItemCommand, ExecuteRemoveItem, CanExecuteRemoveItem));

            MoveNextCommand = new RelayCommand(x => CurrentPage++, o => CurrentPage < MaxPages);
            MoveBackCommand = new RelayCommand(x => CurrentPage--, o => CurrentPage > 1);
            ToggleFilterCommand = new RelayCommand(x => IsFilteringEnabled = !IsFilteringEnabled);

            Loaded += (s, e) => OnItemsSource(ItemsSource);
        }

        public string ClearFiltersMessage
        {
            get => (string)GetValue(ClearFiltersMessageProperty);
            set => SetValue(ClearFiltersMessageProperty, value);
        }

        public string IncludeItemsMessage
        {
            get => (string)GetValue(IncludeItemsMessageProperty);
            set => SetValue(IncludeItemsMessageProperty, value);
        }

        public string ExcludeItemsMessage
        {
            get => (string)GetValue(ExcludeItemsMessageProperty);
            set => SetValue(ExcludeItemsMessageProperty, value);
        }

        public int CurrentPage
        {
            get => (int)GetValue(CurrentPageProperty);
            set => SetValue(CurrentPageProperty, value);
        }

        private FilteringDataGrid DataGrid { get; set; }

        public bool HasCheckboxes
        {
            get => (bool)GetValue(HasCheckboxesProperty);
            set => SetValue(HasCheckboxesProperty, value);
        }

        private CheckBox HeaderButton { get; } = new CheckBox
        {
            Margin = new Thickness(8, 0, 0, 0),
            VerticalAlignment = VerticalAlignment.Top
        };


        public bool IsDeleteButtonVisible
        {
            get => (bool)GetValue(IsDeleteButtonVisibleProperty);
            private set => SetValue(IsDeleteButtonVisibleProperty, value);
        }


        public bool IsFilterButtonVisible
        {
            get => (bool)GetValue(IsFilterButtonVisibleProperty);
            private set => SetValue(IsFilterButtonVisibleProperty, value);
        }


        public bool IsFilteringEnabled
        {
            get => (bool)GetValue(IsFilteringEnabledProperty);
            set => SetValue(IsFilteringEnabledProperty, value);
        }


        private bool IsSelectAll { get; set; }

        [AlsoNotifyFor(nameof(MaxPages))]
        public int ItemsPerPage { get; set; } = 10;

        private Type ItemType
        {
            get => itemType;
            set
            {
                if (itemType == value)
                {
                    return;
                }

                itemType = value;

                if (DataGrid == null || itemType == null)
                {
                    return;
                }

                foreach (var dataGridColumn in DataGrid.Columns.Except(ProtectedColumns).ToList())
                {
                    DataGrid.Columns.Remove(dataGridColumn);
                }

                foreach (var propertyInfo in ItemType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Reverse())
                {
                    CreateColumn(propertyInfo);
                }

                CreateCheckboxColumn();
            }
        }

        public int MaxPages => (int)Math.Ceiling((double)TotalItems / ItemsPerPage);

        public ICommand MoveBackCommand
        {
            get => (ICommand)GetValue(MoveBackCommandProperty);
            set => SetValue(MoveBackCommandProperty, value);
        }

        public ICommand MoveNextCommand
        {
            get => (ICommand)GetValue(MoveNextCommandProperty);
            set => SetValue(MoveNextCommandProperty, value);
        }

        private ComboBox PerPageComboBox { get; set; }

        private List<DataGridColumn> ProtectedColumns { get; set; }

        public string RowsPerPageText
        {
            get => (string)GetValue(RowsPerPageTextProperty);
            set => SetValue(RowsPerPageTextProperty, value);
        }


        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public ICommand ToggleFilterCommand
        {
            get => (ICommand)GetValue(ToggleFilterCommandProperty);
            set => SetValue(ToggleFilterCommandProperty, value);
        }

        public int TotalItems => GetIEnumerableCount(ItemsSource) ?? 0;

        public event PropertyChangedEventHandler PropertyChanged;

        private void CreateCheckboxColumn()
        {
            var rowCheckBox = new FrameworkElementFactory(typeof(CheckBox));
            rowCheckBox.SetValue(MaxWidthProperty, 18.0);
            rowCheckBox.SetValue(HorizontalAlignmentProperty, HorizontalAlignment.Left);
            rowCheckBox.SetBinding(ToggleButton.IsCheckedProperty, new Binding
            {
                Path = new PropertyPath("IsSelected"),
                RelativeSource =
                    new RelativeSource(RelativeSourceMode.FindAncestor) { AncestorType = typeof(DataGridRow) },
                Mode = BindingMode.TwoWay
            });

            HeaderButton.Command = new RelayCommand(_ =>
            {
                IsSelectAll = !IsSelectAll;

                if (IsSelectAll)
                {
                    DataGrid.SelectAll();
                }
                else
                {
                    DataGrid.UnselectAll();
                }
            });


            if (HasCheckboxes)
            {
                DataGrid.Columns.Insert(0, new DataGridTemplateColumn
                {
                    CellTemplate = new DataTemplate { VisualTree = rowCheckBox },
                    Header = HeaderButton,
                    MaxWidth = 48,
                    CanUserResize = false,
                    CanUserReorder = false
                });
            }
        }

        private static void PropertyChangedCallback(DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            if (dependencyObject is DynamicDataGrid grid)
            {
                grid.OnPropertyChanged(nameof(grid.PaginatedItemsSource));
            }
        }

        public static void DynamicUsing(object resource, Action action)
        {
            try
            {
                action();
            }
            finally
            {
                if (resource is IDisposable d)
                {
                    d.Dispose();
                }
            }
        }

        public int? GetIEnumerableCount(IEnumerable enumerable)
        {
            switch (enumerable)
            {
                case null:
                    return null;
                case ICollection col:
                    return col.Count;
            }

            var c = 0;
            var e = enumerable.GetEnumerator();
            DynamicUsing(e, () =>
            {
                while (e.MoveNext())
                {
                    c++;
                }
            });

            return c;
        }

        private void CreateColumn(PropertyInfo propertyInfo)
        {
            if (propertyInfo.GetCustomAttribute<FieldIgnoreAttribute>() != null)
            {
                return;
            }

            try
            {
                var path = propertyInfo.Name;

                if (propertyInfo.PropertyType.GetConstructor(Type.EmptyTypes) != null)
                {
                    var newItem = Activator.CreateInstance(propertyInfo.PropertyType);
                    if (newItem.ToString() == propertyInfo.PropertyType.ToString())
                    {
                        if (propertyInfo.GetCustomAttribute<SelectFromAttribute>() is SelectFromAttribute
                                selectFromAttribute && !string.IsNullOrEmpty(selectFromAttribute.DisplayPath))
                        {
                            path =
                                $"{propertyInfo.Name}.{propertyInfo.PropertyType.GetProperty(selectFromAttribute.DisplayPath)?.Name}";
                        }
                        else if (propertyInfo.GetCustomAttribute<DisplayNameAttribute>() is DisplayNameAttribute
                                     displayNameAttribute && !string.IsNullOrEmpty(displayNameAttribute.DisplayName))
                        {
                            path =
                                $"{propertyInfo.Name}.{propertyInfo.PropertyType.GetProperty(displayNameAttribute.DisplayName)?.Name}";
                        }
                        else if (propertyInfo.PropertyType.GetCustomAttribute<DisplayNameAttribute>() is
                                     DisplayNameAttribute
                                     displayNameAttribute1 && !string.IsNullOrEmpty(displayNameAttribute1.DisplayName))
                        {
                            path =
                                $"{propertyInfo.Name}.{propertyInfo.PropertyType.GetProperty(displayNameAttribute1.DisplayName)?.Name}";
                        }
                        else
                        {
                            return;
                        }
                    }
                }
                else if (propertyInfo.PropertyType.Namespace != null &&
                         !propertyInfo.PropertyType.Namespace.StartsWith("System"))
                {
                    return;
                }

                var dataGridTextColumn = new MaterialDataGridTextColumn
                {
                    Header = propertyInfo.GetCustomAttribute<FieldAttribute>() is FieldAttribute attr ? attr.Name : propertyInfo.Name.Humanize(),
                    Binding = CreateBinding(propertyInfo, path),
                    EditingElementStyle = TryFindResource("MaterialDesignDataGridTextColumnPopupEditingStyle") as Style,
                    MaxLength = propertyInfo.GetCustomAttribute<StringLengthAttribute>()?.MaximumLength ?? 0
                };

                DataGrid.Columns.Insert(0, dataGridTextColumn);
            }
            catch (Exception e)
            {
                // ignored
            }
        }

        private static Binding CreateBinding(PropertyInfo propertyInfo, string path = null)
        {
            return new Binding(string.IsNullOrEmpty(path) ? propertyInfo.Name : path)
            {
                Mode = propertyInfo.CanRead && propertyInfo.CanWrite
                    ? BindingMode.TwoWay
                    : BindingMode.Default,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };
        }

        public override void OnApplyTemplate()
        {
            PerPageComboBox = Template.FindName("PART_PerPage", this) as ComboBox;
            DataGrid = Template.FindName("PART_DataGrid", this) as FilteringDataGrid;

            if (DataGrid != null)
            {
                ((INotifyCollectionChanged)DataGrid.Items).CollectionChanged += OnCollectionChanged;
            }

            SetupPerPageCombobox();
            SetupDataGrid();
        }

        private void OnCollectionChanged(object o, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            OnPropertyChanged(nameof(MaxPages));
            OnPropertyChanged(nameof(TotalItems));
        }

        private void SetupDataGrid()
        {
            if (DataGrid != null)
            {
                ProtectedColumns = DataGrid.Columns.ToList();
                DataGrid.MouseDoubleClick += DataGridOnMouseDoubleClick;

                if (!HasCheckboxes)
                {
                    return;
                }

                DataGrid.PreviewMouseDown += PreviewMouseDownHandler;
                DataGrid.MouseEnter += MouseEnterHandler;
                DataGrid.SelectionChanged += DataGridOnSelectionChanged;
            }
        }

        private void SetupPerPageCombobox()
        {
            for (var i = 10; i < 100; i += 5)
            {
                PerPageComboBox?.Items.Add(i);
            }

            if (PerPageComboBox != null)
            {
                PerPageComboBox.SelectedIndex = 0;
            }

            if (PerPageComboBox != null)
            {
                PerPageComboBox.SelectionChanged += (sender, args) =>
                    ItemsPerPage = PerPageComboBox?.SelectedItem is int i ? i : 0;

                ItemsPerPage = PerPageComboBox?.SelectedItem is int i2 ? i2 : 0;
            }
        }

        private void DataGridOnSelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            if (DataGrid.SelectedItems.Count == DataGrid.Items.Count)
            {
                HeaderButton.IsChecked = true;
            }
            else if (DataGrid.SelectedItems.Count == 0)
            {
                HeaderButton.IsChecked = false;
            }
            else
            {
                HeaderButton.IsChecked = null;
            }

            IsDeleteButtonVisible = DataGrid.SelectedItems.Count > 0;
            IsFilterButtonVisible = !(DataGrid.SelectedItems.Count > 0);
        }

        private static DependencyObject GetVisualParentByType(DependencyObject startObject, Type type)
        {
            var parent = startObject;
            while (parent != null)
            {
                if (type.IsInstanceOfType(parent))
                {
                    break;
                }

                parent = VisualTreeHelper.GetParent(parent);
            }

            return parent;
        }

        private void PreviewMouseDownHandler(object sender, MouseButtonEventArgs e)
        {
            //TODO: Find a better way to do this, since some buttons might get caught in e.Handled=true and then not be executed.

            if (e.RightButton == MouseButtonState.Pressed)
            {
                return;
            }

            var cell = GetVisualParentByType(
                (FrameworkElement)e.OriginalSource, typeof(DataGridCell)) as DataGridCell;

            var button = GetVisualParentByType(
                (FrameworkElement)e.OriginalSource, typeof(ButtonBase)) as ButtonBase;

            if (!(GetVisualParentByType(
                (FrameworkElement)e.OriginalSource, typeof(DataGridRow)) is DataGridRow row))
            {
                return;
            }

            if (button != null && button.GetType() == typeof(CheckBox) && button.IsMouseOver &&
                Equals(cell?.Column, DataGrid.Columns.First()))
            {
                row.IsSelected = !row.IsSelected;
            }
            else if (button != null)
            {
                return;
            }
            else if (e.ClickCount > 1)
            {
                UpdateItemCommand.Execute(row.Item, DataGrid);
                e.Handled = true;
                return;
            }


            e.Handled = true;
        }

        private static void MouseEnterHandler(object sender, MouseEventArgs e)
        {
            if (!(e.OriginalSource is DataGridRow row) || e.RightButton == MouseButtonState.Pressed)
            {
                return;
            }

            row.IsSelected = !row.IsSelected;
            e.Handled = true;
        }

        private static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj)
            where T : DependencyObject
        {
            if (depObj == null)
            {
                yield break;
            }

            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                var child = VisualTreeHelper.GetChild(depObj, i);
                if (child is T variable)
                {
                    yield return variable;
                }

                foreach (var childOfChild in FindVisualChildren<T>(child))
                {
                    yield return childOfChild;
                }
            }
        }

        private static TChildItem FindVisualChild<TChildItem>(DependencyObject obj)
            where TChildItem : DependencyObject
        {
            return FindVisualChildren<TChildItem>(obj).FirstOrDefault();
        }

        private void DataGridOnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
            {
                return;
            }

            var cell = (DataGridCell)GetVisualParentByType(
                (FrameworkElement)e.OriginalSource, typeof(DataGridCell));

            if (cell != null && sender is DataGrid grid &&
                grid.SelectedItems.Count == 1)
            {
                UpdateItemCommand.Execute(DataGrid.SelectedItem, DataGrid);
            }
        }

        private void OnItemsSource(object collection)
        {
            ItemType = null;
            if (collection == null)
            {
                canMutate = false;
                return;
            }

            var interfaces = collection
                .GetType()
                .GetInterfaces()
                .Where(t =>
                    t.IsGenericType &&
                    t.GetGenericTypeDefinition() == typeof(ICollection<>))
                .ToList();

            if (interfaces.Count > 1 || interfaces.Count == 0)
            {
                canMutate = false;
                return;
            }

            var collectionType = interfaces[0];
            ItemType = collectionType.GetGenericArguments()[0];
            canMutate = ItemType.GetConstructor(Type.EmptyTypes) != null;
        }

        private async void ExecuteCreateItem(object sender, ExecutedRoutedEventArgs e)
        {
            if (!canMutate)
            {
                return;
            }

            DialogResult result;
            var definition = GetCreateDefinition();
            try
            {
                result = await Show.Dialog(TargetDialogIdentifier, DataContext, DialogOptions).For(definition);
            }
            catch
            {
                return;
            }

            if (result.Action is "DynamicDataGrid_CreateDialogPositive")
            {
                var collection = ItemsSource;

                IAddActionContext context = new AddActionContext(result.Model);
                foreach (var globalInterceptor in AddInterceptorChain)
                {
                    context = globalInterceptor.Intercept(context);
                    if (context == null)
                    {
                        OnPropertyChanged(nameof(PaginatedItemsSource));
                        return;
                    }
                }

                if (!(collection is INotifyCollectionChanged) && DataGrid != null)
                {
                    ItemsSource = null;
                    AddItemToCollection(ItemType, collection, context.NewModel);
                    ItemsSource = collection;
                }
                else
                {
                    AddItemToCollection(ItemType, collection, context.NewModel);
                }

                OnPropertyChanged(nameof(PaginatedItemsSource));
            }
        }

        private void CanExecuteCreateItem(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = canMutate;
        }

        private async void ExecuteUpdateItem(object sender, ExecutedRoutedEventArgs e)
        {
            var model = e.Parameter;
            if (!canMutate || model == null || !ItemType.IsInstanceOfType(model))
            {
                return;
            }

            DialogResult result;

            var definition = GetUpdateDefinition(model);
            try
            {
                result = await Show
                    .Dialog(TargetDialogIdentifier, DataContext, DialogOptions)
                    .For((IFormDefinition)definition);
            }
            catch
            {
                return;
            }

            if (result.Action is "DynamicDataGrid_UpdateDialogNegative")
            {
                definition.Snapshot.Apply(model);
                return;
            }

            var oldModel = GetOldModel(definition);
            IUpdateActionContext context = new UpdateActionContext(oldModel, definition.Model);

            foreach (var globalInterceptor in UpdateInterceptorChain)
            {
                context = globalInterceptor.Intercept(context);
                if (context == null)
                {
                    throw new InvalidOperationException(
                        $"{globalInterceptor.GetType().Name} are not allowed to return null.");
                }
            }

            var contextDefinition = GetUpdateDefinition(context.NewModel);
            contextDefinition.Snapshot.Apply(model);
        }

        private static object GetOldModel(UpdateFormDefinition definition)
        {
            try
            {
                var oldModel = Activator.CreateInstance(definition.ModelType);
                definition.Snapshot.Apply(oldModel);
                return oldModel;
            }
            catch
            {
                // ignored
            }

            return null;
        }

        private void CanExecuteUpdateItem(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = canMutate && e.Parameter != null && ItemType.IsInstanceOfType(e.Parameter);
        }

        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        private async void ExecuteRemoveItem(object sender, ExecutedRoutedEventArgs e)
        {
            void DoInterceptions(IRemoveActionContext context)
            {
                try
                {
                    foreach (var globalInterceptor in RemoveInterceptorChain)
                    {
                        globalInterceptor.Intercept(context);
                    }
                }
                catch
                {
                    //supress
                }
            }

            var model = e.Parameter;
            if (!canMutate || model == null || !ItemType.IsInstanceOfType(e.Parameter))
            {
                if (!(e.Parameter is IEnumerable enumerable &&
                      enumerable.Cast<object>().First().GetType() == ItemType))
                {
                    return;
                }
            }

            try
            {
                var result = await Show
                    .Dialog(TargetDialogIdentifier, DataContext, DialogOptions)
                    .For(new Confirmation(
                        RemoveDialogTextContent,
                        RemoveDialogTitleContent,
                        RemoveDialogPositiveContent,
                        RemoveDialogNegativeContent
                    )
                    {
                        PositiveActionIcon = RemoveDialogPositiveIcon,
                        NegativeActionIcon = RemoveDialogNegativeIcon
                    });

                if (result.Action is "positive")
                {
                    var collection = ItemsSource;

                    if (model is IEnumerable modelEnum)
                    {
                        foreach (var item in modelEnum.Cast<object>().ToList())
                        {
                            IRemoveActionContext context = new RemoveActionContext(item);
                            DoInterceptions(context);
                        }
                    }
                    else
                    {
                        IRemoveActionContext context = new RemoveActionContext(model);
                        DoInterceptions(context);
                    }

                    if (!(collection is INotifyCollectionChanged) && DataGrid != null)
                    {
                        ItemsSource = null;
                        RemoveItems(model, collection);
                        ItemsSource = collection;
                    }
                    else
                    {
                        RemoveItems(model, collection);
                    }

                    OnPropertyChanged(nameof(PaginatedItemsSource));
                }
            }
            catch
            {
                // ignored
            }
        }

        private void RemoveItems(object model, IEnumerable collection)
        {
            if (model is IEnumerable modelEnum)
            {
                foreach (var item in modelEnum.Cast<object>().ToList())
                {
                    RemoveItemFromCollection(ItemType, collection, item);
                }
            }
            else
            {
                RemoveItemFromCollection(ItemType, collection, model);
            }
        }

        private void CanExecuteRemoveItem(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = canMutate && e.Parameter != null &&
                           (ItemType.IsInstanceOfType(e.Parameter) || e.Parameter is IEnumerable enumerable &&
                            enumerable.Cast<object>().Any() &&
                            enumerable.Cast<object>().First().GetType() == ItemType);
        }

        private IFormDefinition GetCreateDefinition()
        {
            var formDefinition = FormBuilder.GetDefinition(ItemType);
            return AddRows(formDefinition, new FormRow(true, 1)
            {
                Elements =
                {
                    new FormElementContainer(0, formDefinition.Grid.Length, new List<FormElement>
                    {
                        GetCreateNegativeAction().FreezeResources(),
                        GetCreatePositiveAction().FreezeResources()
                    })
                }
            });
        }

        private UpdateFormDefinition GetUpdateDefinition(object model)
        {
            var formDefinition = FormBuilder.GetDefinition(ItemType);
            return new UpdateFormDefinition(
                formDefinition,
                model,
                formDefinition.FormRows.Concat(
                    new[]
                    {
                        new FormRow(true, 1)
                        {
                            Elements =
                            {
                                new FormElementContainer(0, formDefinition.Grid.Length, new List<FormElement>
                                {
                                    GetUpdateNegativeAction().FreezeResources(),
                                    GetUpdatePositiveAction().FreezeResources()
                                })
                            }
                        }
                    }
                ).ToList().AsReadOnly()
            );
        }

        private ActionElement GetCreatePositiveAction()
        {
            return new ActionElement
            {
                Action = new LiteralValue("DynamicDataGrid_CreateDialogPositive"),
                Content = new LiteralValue(CreateDialogPositiveContent),
                Icon = new LiteralValue(CreateDialogPositiveIcon),
                ClosesDialog = LiteralValue.True,
                IsDefault = LiteralValue.True
            };
        }

        private ActionElement GetCreateNegativeAction()
        {
            return new ActionElement
            {
                Action = new LiteralValue("DynamicDataGrid_CreateDialogNegative"),
                Content = new LiteralValue(CreateDialogNegativeContent),
                Icon = new LiteralValue(CreateDialogNegativeIcon),
                ClosesDialog = LiteralValue.True,
                IsCancel = LiteralValue.True
            };
        }

        private ActionElement GetUpdatePositiveAction()
        {
            return new ActionElement
            {
                Action = new LiteralValue("DynamicDataGrid_UpdateDialogPositive"),
                Content = new LiteralValue(UpdateDialogPositiveContent),
                Icon = new LiteralValue(UpdateDialogPositiveIcon),
                ClosesDialog = LiteralValue.True,
                IsDefault = LiteralValue.True
            };
        }

        private ActionElement GetUpdateNegativeAction()
        {
            return new ActionElement
            {
                Action = new LiteralValue("DynamicDataGrid_UpdateDialogNegative"),
                Content = new LiteralValue(UpdateDialogNegativeContent),
                Icon = new LiteralValue(UpdateDialogNegativeIcon),
                ClosesDialog = LiteralValue.True,
                IsCancel = LiteralValue.True
            };
        }

        private static IFormDefinition AddRows(
            IFormDefinition formDefinition,
            params FormRow[] rows)
        {
            return new FormDefinitionWrapper(
                formDefinition,
                formDefinition.FormRows.Concat(rows ?? new FormRow[0]).ToList().AsReadOnly());
        }

        internal void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Dependency properties

        public string CreateDialogPositiveContent
        {
            get => (string)GetValue(CreateDialogPositiveContentProperty);
            set => SetValue(CreateDialogPositiveContentProperty, value);
        }


        public PackIconKind? CreateDialogPositiveIcon
        {
            get => (PackIconKind)GetValue(CreateDialogPositiveIconProperty);
            set => SetValue(CreateDialogPositiveIconProperty, value);
        }


        public string CreateDialogNegativeContent
        {
            get => (string)GetValue(CreateDialogNegativeContentProperty);
            set => SetValue(CreateDialogNegativeContentProperty, value);
        }


        public PackIconKind? CreateDialogNegativeIcon
        {
            get => (PackIconKind)GetValue(CreateDialogNegativeIconProperty);
            set => SetValue(CreateDialogNegativeIconProperty, value);
        }


        public string UpdateDialogPositiveContent
        {
            get => (string)GetValue(UpdateDialogPositiveContentProperty);
            set => SetValue(UpdateDialogPositiveContentProperty, value);
        }


        public PackIconKind? UpdateDialogPositiveIcon
        {
            get => (PackIconKind)GetValue(UpdateDialogPositiveIconProperty);
            set => SetValue(UpdateDialogPositiveIconProperty, value);
        }

        /// <summary>
        /// Identifies the IsFilteringCaseSensitive dependency property.
        /// </summary>
        public static DependencyProperty IsFilteringCaseSensitiveProperty =
            DependencyProperty.Register("IsFilteringCaseSensitive", typeof(bool), typeof(DynamicDataGrid),
                new PropertyMetadata(true));

        public bool IsFilteringCaseSensitive
        {
            get => (bool)GetValue(IsFilteringCaseSensitiveProperty);
            set => SetValue(IsFilteringCaseSensitiveProperty, value);
        }

        public string UpdateDialogNegativeContent
        {
            get => (string)GetValue(UpdateDialogNegativeContentProperty);
            set => SetValue(UpdateDialogNegativeContentProperty, value);
        }


        public PackIconKind? UpdateDialogNegativeIcon
        {
            get => (PackIconKind)GetValue(UpdateDialogNegativeIconProperty);
            set => SetValue(UpdateDialogNegativeIconProperty, value);
        }


        public string RemoveDialogTitleContent
        {
            get => (string)GetValue(RemoveDialogTitleContentProperty);
            set => SetValue(RemoveDialogTitleContentProperty, value);
        }


        public string RemoveDialogTextContent
        {
            get => (string)GetValue(RemoveDialogTextContentProperty);
            set => SetValue(RemoveDialogTextContentProperty, value);
        }


        public string RemoveDialogPositiveContent
        {
            get => (string)GetValue(RemoveDialogPositiveContentProperty);
            set => SetValue(RemoveDialogPositiveContentProperty, value);
        }


        public PackIconKind? RemoveDialogPositiveIcon
        {
            get => (PackIconKind)GetValue(RemoveDialogPositiveIconProperty);
            set => SetValue(RemoveDialogPositiveIconProperty, value);
        }


        public string RemoveDialogNegativeContent
        {
            get => (string)GetValue(RemoveDialogNegativeContentProperty);
            set => SetValue(RemoveDialogNegativeContentProperty, value);
        }


        public PackIconKind? RemoveDialogNegativeIcon
        {
            get => (PackIconKind)GetValue(RemoveDialogNegativeIconProperty);
            set => SetValue(RemoveDialogNegativeIconProperty, value);
        }


        public IEnumerable ItemsSource
        {
            get => (IEnumerable)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public IEnumerable PaginatedItemsSource =>
            ItemsSource.Cast<object>().Skip((CurrentPage - 1) * ItemsPerPage).Take(ItemsPerPage);


        private static void ItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((DynamicDataGrid)d).OnItemsSource(e.NewValue);
        }

        public DialogOptions DialogOptions
        {
            get => (DialogOptions)GetValue(DialogOptionsProperty);
            set => SetValue(DialogOptionsProperty, value);
        }

        public static readonly DependencyProperty FormBuilderProperty =
            DependencyProperty.Register(
                nameof(FormBuilder),
                typeof(IFormBuilder),
                typeof(DynamicDataGrid),
                new FrameworkPropertyMetadata(FormBuilding.FormBuilder.Default));

        public IFormBuilder FormBuilder
        {
            get => (IFormBuilder)GetValue(FormBuilderProperty);
            set => SetValue(FormBuilderProperty, value);
        }

        public static readonly DependencyProperty TargetDialogIdentifierProperty =
            DependencyProperty.Register(
                nameof(TargetDialogIdentifier),
                typeof(object),
                typeof(DynamicDataGrid),
                new FrameworkPropertyMetadata());

        public object TargetDialogIdentifier
        {
            get => GetValue(TargetDialogIdentifierProperty);
            set => SetValue(TargetDialogIdentifierProperty, value);
        }

        #endregion

        #region Collection helpers

        private static readonly Dictionary<Type, Action<object, object>> AddItemCache =
            new Dictionary<Type, Action<object, object>>();

        private static readonly Dictionary<Type, Action<object, object>> RemoveItemCache =
            new Dictionary<Type, Action<object, object>>();

        private static void AddItemToCollection(Type itemType, object collection, object item)
        {
            if (!AddItemCache.TryGetValue(itemType, out var action))
            {
                var collectionType = typeof(ICollection<>).MakeGenericType(itemType);
                var addMethod = collectionType.GetMethod("Add") ??
                                throw new InvalidOperationException("This should not happen.");
                var collectionParam = Expression.Parameter(typeof(object), "collection");
                var itemParam = Expression.Parameter(typeof(object), "item");
                var lambda = Expression.Lambda<Action<object, object>>(
                    Expression.Call(
                        Expression.Convert(collectionParam, collectionType),
                        addMethod,
                        Expression.Convert(itemParam, itemType)),
                    collectionParam,
                    itemParam
                );

                action = lambda.Compile();
                AddItemCache[itemType] = action;
            }

            action(collection, item);
        }

        private void RemoveItemFromCollection(Type itemType, object collection, object item)
        {
            if (!RemoveItemCache.TryGetValue(itemType, out var action))
            {
                var collectionType = typeof(ICollection<>).MakeGenericType(itemType);
                var removeMethod = collectionType.GetMethod("Remove") ??
                                   throw new InvalidOperationException("This should not happen.");
                var collectionParam = Expression.Parameter(typeof(object), "collection");
                var itemParam = Expression.Parameter(typeof(object), "item");
                var lambda = Expression.Lambda<Action<object, object>>(
                    Expression.Call(
                        Expression.Convert(collectionParam, collectionType),
                        removeMethod,
                        Expression.Convert(itemParam, itemType)),
                    collectionParam,
                    itemParam
                );

                action = lambda.Compile();
                RemoveItemCache[itemType] = action;
            }

            action(collection, item);
        }

        #endregion
    }

    internal class ActionInterceptor : IActionInterceptor
    {
        private readonly Func<IActionContext, IActionContext> onAction;

        public ActionInterceptor(Func<IActionContext, IActionContext> onAction)
        {
            this.onAction = onAction ?? throw new ArgumentNullException(nameof(onAction));
        }

        public IActionContext InterceptAction(IActionContext actionContext)
        {
            return onAction(actionContext);
        }
    }

    internal class FormDefinitionWrapper : IFormDefinition
    {
        private readonly IFormDefinition inner;

        public FormDefinitionWrapper(IFormDefinition inner, IReadOnlyList<FormRow> formRows)
        {
            this.inner = inner;
            FormRows = formRows;
        }

        public object CreateInstance(IResourceContext context)
        {
            return inner.CreateInstance(context);
        }

        public IReadOnlyList<FormRow> FormRows { get; }

        public double[] Grid => inner.Grid;

        public Type ModelType => inner.ModelType;

        public IDictionary<string, IValueProvider> Resources => inner.Resources;
    }

    internal class UpdateFormDefinition : IFormDefinition
    {
        private readonly IFormDefinition inner;

        public UpdateFormDefinition(
            IFormDefinition inner,
            object model,
            IReadOnlyList<FormRow> formRows)
        {
            this.inner = inner;
            FormRows = formRows;
            Model = model;
            Snapshot = new Snapshot(model, new HashSet<string>(formRows
                .SelectMany(r => r.Elements.SelectMany(e => e.Elements))
                .Where(e => e is DataFormField)
                .Select(f => ((DataFormField)f).Key)));
        }

        public object Model { get; }

        public Snapshot Snapshot { get; }

        public object CreateInstance(IResourceContext context)
        {
            return Model;
        }

        public IReadOnlyList<FormRow> FormRows { get; }

        public double[] Grid => inner.Grid;

        public Type ModelType => inner.ModelType;

        public IDictionary<string, IValueProvider> Resources => inner.Resources;
    }
}
