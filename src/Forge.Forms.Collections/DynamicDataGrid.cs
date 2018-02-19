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
using Forge.Forms.Collections.Annotations;
using Forge.Forms.Collections.Converters;
using Forge.Forms.Collections.Extensions;
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
        /// <summary>
        /// Identifies the HeaderStyle dependency property.
        /// </summary>
        public static DependencyProperty HeaderStyleProperty =
            DependencyProperty.Register("HeaderStyle", typeof(DynamicDataGridHeaderStyle), typeof(DynamicDataGrid),
                new PropertyMetadata());

        /// <summary>
        /// Identifies the CanUserAdd dependency property.
        /// </summary>
        public static DependencyProperty CanUserAddProperty =
            DependencyProperty.Register("CanUserAdd", typeof(bool), typeof(DynamicDataGrid),
                new PropertyMetadata(true));

        /// <summary>
        /// Identifies the CanUserEdit dependency property.
        /// </summary>
        public static DependencyProperty CanUserEditProperty =
            DependencyProperty.Register("CanUserEdit", typeof(bool), typeof(DynamicDataGrid),
                new PropertyMetadata(true));

        /// <summary>
        /// Identifies the CanUserRemove dependency property.
        /// </summary>
        public static DependencyProperty CanUserRemoveProperty =
            DependencyProperty.Register("CanUserRemove", typeof(bool), typeof(DynamicDataGrid),
                new PropertyMetadata(true));

        /// <summary>
        /// The toggle filter command property
        /// </summary>
        public static readonly DependencyProperty ToggleFilterCommandProperty =
            DependencyProperty.Register("ToggleFilterCommand", typeof(ICommand), typeof(DynamicDataGrid),
                new PropertyMetadata());

        /// <summary>
        /// The is filtering enabled property
        /// </summary>
        public static readonly DependencyProperty IsFilteringEnabledProperty =
            DependencyProperty.Register("IsFilteringEnabled", typeof(bool), typeof(DynamicDataGrid),
                new PropertyMetadata(false));


        /// <summary>
        /// The create dialog positive content property
        /// </summary>
        public static readonly DependencyProperty CreateDialogPositiveContentProperty =
            DependencyProperty.Register(
                nameof(CreateDialogPositiveContent),
                typeof(string),
                typeof(DynamicDataGrid),
                new FrameworkPropertyMetadata("ADD"));

        /// <summary>
        /// The create dialog positive icon property
        /// </summary>
        public static readonly DependencyProperty CreateDialogPositiveIconProperty =
            DependencyProperty.Register(
                nameof(CreateDialogPositiveIcon),
                typeof(PackIconKind?),
                typeof(DynamicDataGrid),
                new FrameworkPropertyMetadata(PackIconKind.Check));

        /// <summary>
        /// The create dialog negative content property
        /// </summary>
        public static readonly DependencyProperty CreateDialogNegativeContentProperty = DependencyProperty.Register(
            nameof(CreateDialogNegativeContent),
            typeof(string),
            typeof(DynamicDataGrid),
            new FrameworkPropertyMetadata("CANCEL"));

        /// <summary>
        /// The create dialog negative icon property
        /// </summary>
        public static readonly DependencyProperty CreateDialogNegativeIconProperty =
            DependencyProperty.Register(
                nameof(CreateDialogNegativeIcon),
                typeof(PackIconKind?),
                typeof(DynamicDataGrid),
                new FrameworkPropertyMetadata(PackIconKind.Close));

        /// <summary>
        /// The update dialog positive content property
        /// </summary>
        public static readonly DependencyProperty UpdateDialogPositiveContentProperty = DependencyProperty.Register(
            nameof(UpdateDialogPositiveContent),
            typeof(string),
            typeof(DynamicDataGrid),
            new FrameworkPropertyMetadata("OK"));

        /// <summary>
        /// The update dialog positive icon property
        /// </summary>
        public static readonly DependencyProperty UpdateDialogPositiveIconProperty =
            DependencyProperty.Register(
                nameof(UpdateDialogPositiveIcon),
                typeof(PackIconKind?),
                typeof(DynamicDataGrid),
                new FrameworkPropertyMetadata(PackIconKind.Check));

        /// <summary>
        /// The update dialog negative content property
        /// </summary>
        public static readonly DependencyProperty UpdateDialogNegativeContentProperty = DependencyProperty.Register(
            nameof(UpdateDialogNegativeContent),
            typeof(string),
            typeof(DynamicDataGrid),
            new FrameworkPropertyMetadata("CANCEL"));

        /// <summary>
        /// The update dialog negative icon property
        /// </summary>
        public static readonly DependencyProperty UpdateDialogNegativeIconProperty =
            DependencyProperty.Register(
                nameof(UpdateDialogNegativeIcon),
                typeof(PackIconKind?),
                typeof(DynamicDataGrid),
                new FrameworkPropertyMetadata(PackIconKind.Close));

        /// <summary>
        /// The remove dialog title content property
        /// </summary>
        public static readonly DependencyProperty RemoveDialogTitleContentProperty = DependencyProperty.Register(
            nameof(RemoveDialogTitleContent),
            typeof(string),
            typeof(DynamicDataGrid),
            new FrameworkPropertyMetadata());

        /// <summary>
        /// The remove dialog text content property
        /// </summary>
        public static readonly DependencyProperty RemoveDialogTextContentProperty = DependencyProperty.Register(
            nameof(RemoveDialogTextContent),
            typeof(string),
            typeof(DynamicDataGrid),
            new FrameworkPropertyMetadata("Remove item(s)?"));

        /// <summary>
        /// The remove dialog positive content property
        /// </summary>
        public static readonly DependencyProperty RemoveDialogPositiveContentProperty = DependencyProperty.Register(
            nameof(RemoveDialogPositiveContent),
            typeof(string),
            typeof(DynamicDataGrid),
            new FrameworkPropertyMetadata("REMOVE"));

        /// <summary>
        /// The remove dialog positive icon property
        /// </summary>
        public static readonly DependencyProperty RemoveDialogPositiveIconProperty =
            DependencyProperty.Register(
                nameof(RemoveDialogPositiveIcon),
                typeof(PackIconKind?),
                typeof(DynamicDataGrid),
                new FrameworkPropertyMetadata(PackIconKind.Delete));

        /// <summary>
        /// The remove dialog negative content property
        /// </summary>
        public static readonly DependencyProperty RemoveDialogNegativeContentProperty = DependencyProperty.Register(
            nameof(RemoveDialogNegativeContent),
            typeof(string),
            typeof(DynamicDataGrid),
            new FrameworkPropertyMetadata("CANCEL"));

        /// <summary>
        /// The remove dialog negative icon property
        /// </summary>
        public static readonly DependencyProperty RemoveDialogNegativeIconProperty =
            DependencyProperty.Register(
                nameof(RemoveDialogNegativeIcon),
                typeof(PackIconKind?),
                typeof(DynamicDataGrid),
                new FrameworkPropertyMetadata(PackIconKind.Close));

        /// <summary>
        /// The items source property
        /// </summary>
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(
                nameof(ItemsSource),
                typeof(IEnumerable),
                typeof(DynamicDataGrid));

        /// <summary>
        /// Identifies the FirstPage dependency property.
        /// </summary>
        public static DependencyProperty MoveFirstCommandProperty =
            DependencyProperty.Register("MoveFirstCommand", typeof(ICommand), typeof(DynamicDataGrid),
                new PropertyMetadata());

        /// <summary>
        /// Identifies the LastPage dependency property.
        /// </summary>
        public static DependencyProperty MoveLastCommandProperty =
            DependencyProperty.Register("MoveLastCommand", typeof(ICommand), typeof(DynamicDataGrid),
                new PropertyMetadata());

        /// <summary>
        /// Identifies the NextPage dependency property.
        /// </summary>
        public static DependencyProperty MoveNextCommandProperty =
            DependencyProperty.Register("MoveNextCommand", typeof(ICommand), typeof(DynamicDataGrid),
                new PropertyMetadata());

        /// <summary>
        /// Identifies the NextPage dependency property.
        /// </summary>
        public static DependencyProperty MoveToPageCommandProperty =
            DependencyProperty.Register("MoveToPageCommand", typeof(ICommand), typeof(DynamicDataGrid),
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

        /// <summary>
        /// The dialog options property
        /// </summary>
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
        public static readonly DependencyProperty HasCheckboxesColumnProperty =
            DependencyProperty.Register("HasCheckboxesColumn", typeof(bool), typeof(DynamicDataGrid),
                new PropertyMetadata(true));

        public static readonly RoutedCommand CreateItemCommand = new RoutedCommand();
        public static readonly RoutedCommand UpdateItemCommand = new RoutedCommand();
        public static readonly RoutedCommand RemoveItemCommand = new RoutedCommand();

        /// <summary>
        /// The add interceptor chain
        /// </summary>
        public static readonly List<IAddActionInterceptor> AddInterceptorChain = new List<IAddActionInterceptor>();

        /// <summary>
        /// The update interceptor chain
        /// </summary>
        public static readonly List<IUpdateActionInterceptor> UpdateInterceptorChain =
            new List<IUpdateActionInterceptor>();

        /// <summary>
        /// The remove interceptor chain
        /// </summary>
        public static readonly List<IRemoveActionInterceptor> RemoveInterceptorChain =
            new List<IRemoveActionInterceptor>();

        /// <summary>
        /// The rows per page text property
        /// </summary>
        public static readonly DependencyProperty RowsPerPageTextProperty =
            DependencyProperty.Register("RowsPerPageText", typeof(string), typeof(DynamicDataGrid),
                new PropertyMetadata("Rows per page"));

        /// <summary>
        /// Identifies the CurrentPage dependency property.
        /// </summary>
        public static DependencyProperty CurrentPageProperty =
            DependencyProperty.Register("CurrentPage", typeof(int), typeof(DynamicDataGrid), new PropertyMetadata(1,
                OnCurrentPageChanged));

        /// <summary>
        /// The is delete button visible property
        /// </summary>
        public static readonly DependencyProperty IsDeleteButtonVisibleProperty =
            DependencyProperty.Register("IsDeleteButtonVisible", typeof(bool), typeof(DynamicDataGrid),
                new PropertyMetadata(false));

        /// <summary>
        /// The is filter button visible property
        /// </summary>
        public static readonly DependencyProperty IsFilterButtonVisibleProperty =
            DependencyProperty.Register("IsFilterButtonVisible", typeof(bool), typeof(DynamicDataGrid),
                new PropertyMetadata(true));

        private bool canMutate;
        private Type itemType;

        private List<SortDescription> CachedSortDescriptions =
            new List<SortDescription>();

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

            MoveNextCommand = new RelayCommand(x => CurrentPage++, o => CurrentPage < LastPage);
            MoveBackCommand = new RelayCommand(x => CurrentPage--, o => CurrentPage > 1);
            MoveLastCommand = new RelayCommand(x => CurrentPage = LastPage, o => CurrentPage < LastPage);
            MoveFirstCommand = new RelayCommand(x => CurrentPage = 1, o => CurrentPage > 1);
            MoveToPageCommand = new RelayCommand(
                x => CurrentPage = int.Parse((string)x), o => int.TryParse((string)o, out var val) && val != CurrentPage);
            ToggleFilterCommand = new RelayCommand(x => IsFilteringEnabled = !IsFilteringEnabled);
            CheckboxColumnCommand = new RelayCommand(sender =>
            {
                if (sender is DataGridRow row)
                {
                    CheckedConverter.SetChecked(this, row.Item, !CheckedConverter.IsChecked(this, row.Item));
                    BindingOperations
                        .GetMultiBindingExpression(row.TryFindChild<CheckBox>(), ToggleButton.IsCheckedProperty)
                        ?.UpdateTarget();
                }
            });

            PropertyChanged += OnPropertyChanged;
            Loaded += (s, e) => OnItemsSource(ItemsSource);
        }

        /// <summary>
        /// Gets or sets the header style.
        /// </summary>
        /// <value>
        /// The header style.
        /// </value>
        public DynamicDataGridHeaderStyle HeaderStyle
        {
            get => (DynamicDataGridHeaderStyle)GetValue(HeaderStyleProperty);
            set => SetValue(HeaderStyleProperty, value);
        }

        internal IEnumerable<object> DatagridSelectedItems
        {
            get
            {
                return DataGrid.GetRows()
                    .Where(row => CheckedConverter.IsChecked(this, row.Item))
                    .Select(row => row.Item).ToList();
            }
        }

        /// <summary>
        /// Gets the selected items.
        /// </summary>
        /// <value>
        /// The selected items.
        /// </value>
        public IList<object> SelectedItems
        {
            get { return CheckedConverter.GetItems(this).Where(i => i.Value).Select(i => i.Key).ToList(); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this the user can add new items.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance can add new items; otherwise, <c>false</c>.
        /// </value>
        public bool CanUserAdd
        {
            get => (bool)GetValue(CanUserAddProperty);
            set => SetValue(CanUserAddProperty, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance can add edit items.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance can add edit items; otherwise, <c>false</c>.
        /// </value>
        public bool CanUserEdit
        {
            get => (bool)GetValue(CanUserEditProperty);
            set => SetValue(CanUserEditProperty, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance can remove items.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance can remove items; otherwise, <c>false</c>.
        /// </value>
        public bool CanUserRemove
        {
            get => (bool)GetValue(CanUserRemoveProperty);
            set => SetValue(CanUserRemoveProperty, value);
        }

        /// <summary>
        /// Gets or sets the clear filters message.
        /// </summary>
        /// <value>
        /// The clear filters message.
        /// </value>
        public string ClearFiltersMessage
        {
            get => (string)GetValue(ClearFiltersMessageProperty);
            set => SetValue(ClearFiltersMessageProperty, value);
        }

        /// <summary>
        /// Gets or sets the include items message.
        /// </summary>
        /// <value>
        /// The include items message.
        /// </value>
        public string IncludeItemsMessage
        {
            get => (string)GetValue(IncludeItemsMessageProperty);
            set => SetValue(IncludeItemsMessageProperty, value);
        }

        /// <summary>
        /// Gets or sets the exclude items message.
        /// </summary>
        /// <value>
        /// The exclude items message.
        /// </value>
        public string ExcludeItemsMessage
        {
            get => (string)GetValue(ExcludeItemsMessageProperty);
            set => SetValue(ExcludeItemsMessageProperty, value);
        }

        /// <summary>
        /// Gets the current page.
        /// </summary>
        /// <value>
        /// The current page.
        /// </value>
        [AlsoNotifyFor(nameof(PaginationPageNumbers))]
        public int CurrentPage
        {
            get => (int)GetValue(CurrentPageProperty);
            private set
            {
                IsSelectAll = false;
                SetValue(CurrentPageProperty, value);
            }
        }

        private int CurrentMaxItem => Math.Min(TotalItems, CurrentPage * ItemsPerPage);
        private int CurrentMinItem => Math.Min(TotalItems, CurrentMaxItem - ItemsOnPage + 1);

        internal FilteringDataGrid DataGrid { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has checkboxes column.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has checkboxes column; otherwise, <c>false</c>.
        /// </value>
        private bool HasCheckboxesColumn
        {
            get => (bool)GetValue(HasCheckboxesColumnProperty);
            set => SetValue(HasCheckboxesColumnProperty, value);
        }

        private CheckBox HeaderButton { get; } = new CheckBox
        {
            Margin = new Thickness(8, 0, 0, 0),
            VerticalAlignment = VerticalAlignment.Top
        };


        /// <summary>
        /// Gets a value indicating whether this instance's delete button visible.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has delete button visible; otherwise, <c>false</c>.
        /// </value>
        public bool IsDeleteButtonVisible
        {
            get => (bool)GetValue(IsDeleteButtonVisibleProperty);
            private set => SetValue(IsDeleteButtonVisibleProperty, value);
        }


        /// <summary>
        /// Gets a value indicating whether this instance's filter button visible.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has filter button visible; otherwise, <c>false</c>.
        /// </value>
        public bool IsFilterButtonVisible
        {
            get => (bool)GetValue(IsFilterButtonVisibleProperty);
            private set => SetValue(IsFilterButtonVisibleProperty, value);
        }


        /// <summary>
        /// Gets or sets a value indicating whether this instance has filtering enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has filtering enabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsFilteringEnabled
        {
            get => (bool)GetValue(IsFilteringEnabledProperty);
            set => SetValue(IsFilteringEnabledProperty, value);
        }
        
        private bool IsSelectAll { get; set; }

        private int SelectedItemsCount => DataGrid?.SelectedItems.Count ?? 0;

        /// <summary>
        /// Gets or sets the items per page.
        /// </summary>
        /// <value>
        /// The items per page.
        /// </value>
        [AlsoNotifyFor(nameof(LastPage))]
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

                foreach (var propertyInfo in ItemType
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                    .Reverse())
                {
                    CreateColumn(propertyInfo);
                }

                CreateCheckboxColumn();
            }
        }

        /// <summary>
        /// Gets the last page.
        /// </summary>
        /// <value>
        /// The last page.
        /// </value>
        public int LastPage => Math.Max((int)Math.Ceiling((double)TotalItems / ItemsPerPage), 1);

        /// <summary>
        /// Gets or sets the move back command.
        /// </summary>
        /// <value>
        /// The move back command.
        /// </value>
        public ICommand MoveBackCommand
        {
            get => (ICommand)GetValue(MoveBackCommandProperty);
            set => SetValue(MoveBackCommandProperty, value);
        }

        /// <summary>
        /// Gets or sets the move next command.
        /// </summary>
        /// <value>
        /// The move next command.
        /// </value>
        public ICommand MoveNextCommand
        {
            get => (ICommand)GetValue(MoveNextCommandProperty);
            set => SetValue(MoveNextCommandProperty, value);
        }

        /// <summary>
        /// Gets or sets the move last command.
        /// </summary>
        /// <value>
        /// The move last command.
        /// </value>
        public ICommand MoveLastCommand
        {
            get => (ICommand)GetValue(MoveLastCommandProperty);
            set => SetValue(MoveLastCommandProperty, value);
        }

        /// <summary>
        /// Gets or sets the move first command.
        /// </summary>
        /// <value>
        /// The move first command.
        /// </value>
        public ICommand MoveFirstCommand
        {
            get => (ICommand)GetValue(MoveFirstCommandProperty);
            set => SetValue(MoveFirstCommandProperty, value);
        }

        /// <summary>
        /// Gets or sets the move to page command.
        /// </summary>
        /// <value>
        /// The move to page command.
        /// </value>
        public ICommand MoveToPageCommand
        {
            get => (ICommand)GetValue(MoveToPageCommandProperty);
            set => SetValue(MoveToPageCommandProperty, value);
        }

        private ComboBox PerPageComboBox { get; set; }

        private List<DataGridColumn> ProtectedColumns { get; set; }

        /// <summary>
        /// Gets or sets the rows per page text.
        /// </summary>
        /// <value>
        /// The rows per page text.
        /// </value>
        public string RowsPerPageText
        {
            get => (string)GetValue(RowsPerPageTextProperty);
            set => SetValue(RowsPerPageTextProperty, value);
        }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        /// <summary>
        /// Gets or sets the toggle filter command.
        /// </summary>
        /// <value>
        /// The toggle filter command.
        /// </value>
        public ICommand ToggleFilterCommand
        {
            get => (ICommand)GetValue(ToggleFilterCommandProperty);
            set => SetValue(ToggleFilterCommandProperty, value);
        }

        /// <summary>
        /// Gets the total items.
        /// </summary>
        /// <value>
        /// The total items.
        /// </value>
        public int TotalItems => GetIEnumerableCount(ItemsSource) ?? 0;

        /// <summary>
        /// Gets the items on page.
        /// </summary>
        /// <value>
        /// The items on page.
        /// </value>
        public int ItemsOnPage => Math.Min(ItemsPerPage, TotalItems - ((CurrentPage - 1 )* ItemsPerPage));

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(object sender1, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName == nameof(SelectedItems))
            {
                IsDeleteButtonVisible = SelectedItems.Any() && HeaderStyle != DynamicDataGridHeaderStyle.Alternative;
                IsFilterButtonVisible = !SelectedItems.Any();
            }
        }

        internal void UpdateHeaderButton()
        {
            var items = DatagridSelectedItems.ToList();
            if (items.Count > 0 && items.Count < DataGrid.Items.Count)
            {
                HeaderButton.IsChecked = null;
            }
            else if (items.Count == DataGrid.Items.Count)
            {
                HeaderButton.IsChecked = true;
            }
            else if (items.Count == 0)
            {
                HeaderButton.IsChecked = false;
            }
        }

        private static void OnCurrentPageChanged(DependencyObject x, DependencyPropertyChangedEventArgs y)
        {
            if (x is DynamicDataGrid grid)
            {
                grid.DataGridOnSorting(null, null);
            }
        }

        private void CreateCheckboxColumn()
        {
            var rowCheckBox = new FrameworkElementFactory(typeof(CheckBox));
            rowCheckBox.SetValue(MaxWidthProperty, 18.0);
            rowCheckBox.SetValue(HorizontalAlignmentProperty, HorizontalAlignment.Left);
            rowCheckBox.SetBinding(ToggleButton.IsCheckedProperty, new MultiBinding
            {
                Converter = new CheckedConverter(),
                Bindings =
                {
                    new Binding
                    {
                        Path = new PropertyPath("."),
                        RelativeSource =
                            new RelativeSource(RelativeSourceMode.Self)
                    },
                    new Binding
                    {
                        Path = new PropertyPath("."),
                        RelativeSource =
                            new RelativeSource(RelativeSourceMode.FindAncestor) { AncestorType = typeof(DataGridRow) }
                    }
                },
                ConverterParameter = this
            });
            rowCheckBox.SetBinding(ButtonBase.CommandParameterProperty, new Binding
            {
                RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor)
                {
                    AncestorType = typeof(DataGridRow)
                }
            });

            rowCheckBox.SetValue(ButtonBase.CommandProperty, CheckboxColumnCommand);

            HeaderButton.Command = new RelayCommand(_ =>
            {
                IsSelectAll = !IsSelectAll;

                foreach (var dataGridRow in DataGrid.GetRows())
                {
                    CheckedConverter.SetChecked(this, dataGridRow.Item, !IsSelectAll);
                    CheckboxColumnCommand.Execute(dataGridRow);
                }
            });

            if (HasCheckboxesColumn)
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

        private static void DynamicUsing(object resource, Action action)
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

        private static int? GetIEnumerableCount(IEnumerable enumerable)
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
            if (propertyInfo.GetCustomAttribute<FieldIgnoreAttribute>() != null ||
                propertyInfo.GetCustomAttribute<CrudIgnoreAttribute>() != null)
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
                    Header = propertyInfo.GetCustomAttribute<FieldAttribute>() is FieldAttribute attr
                        ? attr.Name
                        : propertyInfo.Name.Humanize(),
                    Binding = CreateBinding(propertyInfo, path),
                    EditingElementStyle = TryFindResource("MaterialDesignDataGridTextColumnPopupEditingStyle") as Style,
                    MaxLength = propertyInfo.GetCustomAttribute<StringLengthAttribute>()?.MaximumLength ?? 0
                };

                DataGrid.Columns.Insert(0, dataGridTextColumn);
            }
            catch (Exception)
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
                DataGrid.AfterSorting += DataGridOnSorting;
            }

            SetupPerPageCombobox();
            SetupDataGrid();
        }

        private void DataGridOnSorting(object sender, EventArgs eventArgs)
        {
            UpdateSorting();

            var itemsBinding = BindingOperations.GetMultiBindingExpression(DataGrid, ItemsControl.ItemsSourceProperty);
            itemsBinding?.UpdateTarget();

            UpdateView();
        }

        private ICollectionView UpdateView()
        {
            var view = CollectionViewSource.GetDefaultView(ItemsSource);
            view.SortDescriptions.Clear();

            foreach (var sortDescription in CachedSortDescriptions)
            {
                view.SortDescriptions.Add(sortDescription);
                var column = DataGrid.Columns.FirstOrDefault(c => c.SortMemberPath == sortDescription.PropertyName);
                if (column != null)
                {
                    column.SortDirection = sortDescription.Direction;
                }
            }

            CachedSortDescriptions.Clear();
            return view;
        }

        private void UpdateSorting()
        {
            var view = CollectionViewSource.GetDefaultView(DataGrid.ItemsSource);
            CachedSortDescriptions = new List<SortDescription>(view.SortDescriptions);
        }

        private void OnCollectionChanged(object o, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            OnPropertyChanged(nameof(LastPage));
            OnPropertyChanged(nameof(TotalItems));
            OnPropertyChanged(nameof(CurrentMaxItem));
            OnPropertyChanged(nameof(CurrentMinItem));
            OnPropertyChanged(nameof(PaginationPageNumbers));
        }

        private void SetupDataGrid()
        {
            if (DataGrid != null)
            {
                ProtectedColumns = DataGrid.Columns.ToList();
                DataGrid.MouseDoubleClick += DataGridOnMouseDoubleClick;

                if (!HasCheckboxesColumn)
                {
                    return;
                }

                DataGrid.MouseEnter += MouseEnterHandler;
            }
        }

        private void SetupPerPageCombobox()
        {
            if (PerPageComboBox.Items.Count > 0)
            {
                return;
            }

            PerPageComboBox?.Items.Add(1);

            for (var i = 5; i < 30; i += 5)
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
                    {
                        ItemsPerPage = PerPageComboBox?.SelectedItem is int i ? i : 0;
                        IsSelectAll = false;
                        HandleCurrentPageOnMaxPagesChange();
                        BindingOperations.GetMultiBindingExpression(DataGrid, ItemsControl.ItemsSourceProperty)
                            ?.UpdateTarget();
                    };

                ItemsPerPage = PerPageComboBox?.SelectedItem is int i2 ? i2 : 0;
            }
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

            ViewSource.Source = collection;

            if (collection is INotifyCollectionChanged collectionChanged)
            {
                collectionChanged.CollectionChanged += (sender, args) =>
                {
                    ViewSource.Source = null;
                    ViewSource.Source = collection;
                };
            }

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
                DialogOptions.EnvironmentFlags.Add("create");
                result = await Show.Dialog(TargetDialogIdentifier, DataContext, DialogOptions).For(definition);
                DialogOptions.EnvironmentFlags.Remove("create");
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
            }
        }

        private void CanExecuteCreateItem(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = CanUserAdd && canMutate;
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
                DialogOptions.EnvironmentFlags.Add("update");
                result = await Show
                    .Dialog(TargetDialogIdentifier, DataContext, DialogOptions)
                    .For((IFormDefinition)definition);
                DialogOptions.EnvironmentFlags.Remove("update");
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
            e.CanExecute = CanUserEdit && canMutate && e.Parameter != null && ItemType.IsInstanceOfType(e.Parameter);
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
                DialogOptions.EnvironmentFlags.Add("delete");
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
                DialogOptions.EnvironmentFlags.Remove("delete");

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

                    IsSelectAll = IsSelectAll && SelectedItemsCount > 0;
                    HeaderButton.IsChecked = IsSelectAll;
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

            HandleCurrentPageOnMaxPagesChange();
        }

        private void HandleCurrentPageOnMaxPagesChange()
        {
            CurrentPage = Math.Min(LastPage, CurrentPage);
        }

        private void CanExecuteRemoveItem(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = CanUserRemove && canMutate && e.Parameter != null &&
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
                Validates = LiteralValue.True,
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
                Validates = LiteralValue.True,
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

        public CollectionViewSource ViewSource { get; set; } = new CollectionViewSource();

        public IEnumerable<string> PaginationPageNumbers
        {
            get
            {
                const int EitherSide = 1;
                var range = new List<int>();
                var l = 0;

                range.Add(1);

                if (LastPage < 1 + EitherSide)
                {
                    yield return range.First().ToString();
                }
                else if (LastPage <= 5)
                {
                    foreach (var i in Enumerable.Range(1, LastPage))
                    {
                        yield return i.ToString();
                    }
                }
                else
                {
                    for (var i = CurrentPage - EitherSide; i <= CurrentPage + EitherSide; i++)
                    {
                        if (i < LastPage && i > 1)
                        {
                            range.Add(i);
                        }
                    }

                    range.Add(LastPage);

                    foreach (var i in range)
                    {
                        if (l != default(int))
                        {
                            if (i - l == 2)
                            {
                                yield return (l + 1).ToString();
                            }
                            else if (i - l != 1)
                            {
                                yield return "...";
                            }
                        }
                        yield return i.ToString();
                        l = i;
                    }
                }
            }
        }

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

        private RelayCommand CheckboxColumnCommand { get; }

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

            CheckedConverter.Remove(this, item);
            action(collection, item);
        }

        #endregion
    }
}
