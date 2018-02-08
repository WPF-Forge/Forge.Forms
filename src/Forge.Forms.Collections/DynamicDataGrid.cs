using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using Forge.Forms.Annotations;
using Forge.Forms.Collections.Interfaces;
using Forge.Forms.DynamicExpressions;
using Forge.Forms.FormBuilding;
using Forge.Forms.FormBuilding.Defaults;
using Humanizer;
using MaterialDesignThemes.Wpf;
using Expression = System.Linq.Expressions.Expression;

namespace Forge.Forms.Collections
{
    [TemplatePart(Name = "PART_DataGrid", Type = typeof(DataGrid))]
    public class DynamicDataGrid : Control
    {
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
                new FrameworkPropertyMetadata());

        public static readonly DependencyProperty DialogOptionsProperty =
            DependencyProperty.Register(
                nameof(DialogOptions),
                typeof(DialogOptions),
                typeof(DynamicDataGrid),
                new FrameworkPropertyMetadata(DialogOptions.Default, ItemsSourceChanged));

        public static readonly RoutedCommand CreateItemCommand = new RoutedCommand();
        public static readonly RoutedCommand UpdateItemCommand = new RoutedCommand();
        public static readonly RoutedCommand RemoveItemCommand = new RoutedCommand();

        public static readonly List<IAddActionInterceptor> AddInterceptorChain = new List<IAddActionInterceptor>();

        public static readonly List<IUpdateActionInterceptor> UpdateInterceptorChain =
            new List<IUpdateActionInterceptor>();

        public static readonly List<IRemoveActionInterceptor> RemoveInterceptorChain =
            new List<IRemoveActionInterceptor>();

        private bool canMutate;

        private DataGrid dataGrid;
        private Type itemType;

        private CheckBox HeaderButton { get; } = new CheckBox
        {
            Margin = new Thickness(8, 0, 0, 0)
        };

        private bool IsSelectAll { get; set; }

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

                if (dataGrid == null || itemType == null)
                {
                    return;
                }

                foreach (var dataGridColumn in dataGrid.Columns.Except(ProtectedColumns).ToList())
                {
                    dataGrid.Columns.Remove(dataGridColumn);
                }

                foreach (var propertyInfo in ItemType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Reverse())
                {
                    CreateColumn(propertyInfo);
                }

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
                        dataGrid.SelectAll();
                    }
                    else
                    {
                        dataGrid.UnselectAll();
                    }
                });

                dataGrid.Columns.Insert(0, new DataGridTemplateColumn
                {
                    CellTemplate = new DataTemplate { VisualTree = rowCheckBox },
                    Header = HeaderButton
                });
            }
        }

        private List<DataGridColumn> ProtectedColumns { get; set; }

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
            Loaded += (s, e) => OnItemsSource(ItemsSource);
        }

        private void CreateColumn(PropertyInfo propertyInfo)
        {
            if (propertyInfo.GetCustomAttribute<FieldIgnoreAttribute>() != null)
            {
                return;
            }

            var dataGridTextColumn = new MaterialDataGridTextColumn
            {
                Header = propertyInfo.Name.Humanize(),
                Binding = CreateBinding(propertyInfo),
                EditingElementStyle = TryFindResource("MaterialDesignDataGridTextColumnPopupEditingStyle") as Style,
                MaxLength = propertyInfo.GetCustomAttribute<StringLengthAttribute>()?.MaximumLength ?? 0
            };

            dataGrid.Columns.Insert(0, dataGridTextColumn);
        }

        private static Binding CreateBinding(PropertyInfo propertyInfo)
        {
            return new Binding(propertyInfo.Name)
            {
                Mode = propertyInfo.CanRead && propertyInfo.CanWrite
                    ? BindingMode.TwoWay
                    : BindingMode.Default,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };
        }

        public override void OnApplyTemplate()
        {
            dataGrid = Template.FindName("PART_DataGrid", this) as DataGrid;
            if (dataGrid != null)
            {
                dataGrid.MouseDoubleClick += DataGridOnMouseDoubleClick;
                dataGrid.PreviewMouseDown += PreviewMouseDownHandler;
                dataGrid.MouseEnter += MouseEnterHandler;
                dataGrid.SelectionChanged += DataGridOnSelectionChanged;
                ProtectedColumns = dataGrid?.Columns.ToList();
            }
        }

        private void DataGridOnSelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            if (dataGrid.SelectedItems.Count == dataGrid.Items.Count)
            {
                HeaderButton.IsChecked = true;
            }
            else if (dataGrid.SelectedItems.Count == 0)
            {
                HeaderButton.IsChecked = false;
            }
            else
            {
                HeaderButton.IsChecked = null;
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
                Equals(cell?.Column, dataGrid.Columns.First()))
            {
                row.IsSelected = !row.IsSelected;
            }
            else if (button != null)
            {
                return;
            }

            if (e.ClickCount > 1)
            {
                UpdateItemCommand.Execute(row.Item, dataGrid);
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

            if (e.MouseDevice.DirectlyOver is FrameworkElement frameworkElement &&
                (frameworkElement.Parent is DataGridCell || frameworkElement is DataGridCell ||
                 FindVisualChild<DataGridCell>(frameworkElement) != null) && sender is DataGrid grid &&
                grid.SelectedItems.Count == 1)
            {
                UpdateItemCommand.Execute(dataGrid.SelectedItem, dataGrid);
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
                        return;
                    }
                }

                if (!(collection is INotifyCollectionChanged) && dataGrid != null)
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

        private async void ExecuteRemoveItem(object sender, ExecutedRoutedEventArgs e)
        {
            var model = e.Parameter;
            if (!canMutate || model == null || !ItemType.IsInstanceOfType(model))
            {
                return;
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

                    IRemoveActionContext context = new RemoveActionContext(result.Model);

                    foreach (var globalInterceptor in RemoveInterceptorChain)
                    {
                        globalInterceptor.Intercept(context);
                    }

                    if (!(collection is INotifyCollectionChanged) && dataGrid != null)
                    {
                        ItemsSource = null;
                        RemoveItemFromCollection(ItemType, collection, model);
                        ItemsSource = collection;
                    }
                    else
                    {
                        RemoveItemFromCollection(ItemType, collection, model);
                    }
                }
            }
            catch
            {
                // ignored
            }
        }

        private void CanExecuteRemoveItem(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = canMutate && e.Parameter != null && ItemType.IsInstanceOfType(e.Parameter);
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

        private static void RemoveItemFromCollection(Type itemType, object collection, object item)
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

        public IActionContext InterceptAction(IActionContext actionContext)
        {
            return onAction(actionContext);
        }

        public ActionInterceptor(Func<IActionContext, IActionContext> onAction)
        {
            this.onAction = onAction ?? throw new ArgumentNullException(nameof(onAction));
        }
    }

    internal class FormDefinitionWrapper : IFormDefinition
    {
        private readonly IFormDefinition inner;

        public object CreateInstance(IResourceContext context)
        {
            return inner.CreateInstance(context);
        }

        public IReadOnlyList<FormRow> FormRows { get; }

        public double[] Grid => inner.Grid;

        public Type ModelType => inner.ModelType;

        public IDictionary<string, IValueProvider> Resources => inner.Resources;

        public FormDefinitionWrapper(IFormDefinition inner, IReadOnlyList<FormRow> formRows)
        {
            this.inner = inner;
            FormRows = formRows;
        }
    }

    internal class UpdateFormDefinition : IFormDefinition
    {
        private readonly IFormDefinition inner;

        public object CreateInstance(IResourceContext context)
        {
            return Model;
        }

        public IReadOnlyList<FormRow> FormRows { get; }

        public double[] Grid => inner.Grid;

        public Type ModelType => inner.ModelType;

        public IDictionary<string, IValueProvider> Resources => inner.Resources;

        public object Model { get; }

        public Snapshot Snapshot { get; }

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
    }
}
