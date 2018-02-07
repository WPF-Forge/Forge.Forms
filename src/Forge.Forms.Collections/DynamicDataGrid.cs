using System;
using System.Windows;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
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
        #region Dependency properties

        public static readonly DependencyProperty CreateDialogPositiveContentProperty =
            DependencyProperty.Register(
                nameof(CreateDialogPositiveContent),
                typeof(string),
                typeof(DynamicDataGrid),
                new FrameworkPropertyMetadata("OK"));

        public string CreateDialogPositiveContent
        {
            get => (string) GetValue(CreateDialogPositiveContentProperty);
            set => SetValue(CreateDialogPositiveContentProperty, value);
        }

        public static readonly DependencyProperty CreateDialogPositiveIconProperty =
            DependencyProperty.Register(
                nameof(CreateDialogPositiveIcon),
                typeof(PackIconKind?),
                typeof(DynamicDataGrid),
                new FrameworkPropertyMetadata(PackIconKind.Check));

        public PackIconKind? CreateDialogPositiveIcon
        {
            get => (PackIconKind) GetValue(CreateDialogPositiveIconProperty);
            set => SetValue(CreateDialogPositiveIconProperty, value);
        }

        public static readonly DependencyProperty CreateDialogNegativeContentProperty = DependencyProperty.Register(
            nameof(CreateDialogNegativeContent),
            typeof(string),
            typeof(DynamicDataGrid),
            new FrameworkPropertyMetadata("CANCEL"));

        public string CreateDialogNegativeContent
        {
            get => (string) GetValue(CreateDialogNegativeContentProperty);
            set => SetValue(CreateDialogNegativeContentProperty, value);
        }

        public static readonly DependencyProperty CreateDialogNegativeIconProperty =
            DependencyProperty.Register(
                nameof(CreateDialogNegativeIcon),
                typeof(PackIconKind?),
                typeof(DynamicDataGrid),
                new FrameworkPropertyMetadata(PackIconKind.Close));

        public PackIconKind? CreateDialogNegativeIcon
        {
            get => (PackIconKind) GetValue(CreateDialogNegativeIconProperty);
            set => SetValue(CreateDialogNegativeIconProperty, value);
        }

        public static readonly DependencyProperty UpdateDialogPositiveContentProperty = DependencyProperty.Register(
            nameof(UpdateDialogPositiveContent),
            typeof(string),
            typeof(DynamicDataGrid),
            new FrameworkPropertyMetadata("OK"));

        public string UpdateDialogPositiveContent
        {
            get => (string) GetValue(UpdateDialogPositiveContentProperty);
            set => SetValue(UpdateDialogPositiveContentProperty, value);
        }

        public static readonly DependencyProperty UpdateDialogPositiveIconProperty =
            DependencyProperty.Register(
                nameof(UpdateDialogPositiveIcon),
                typeof(PackIconKind?),
                typeof(DynamicDataGrid),
                new FrameworkPropertyMetadata(PackIconKind.Check));

        public PackIconKind? UpdateDialogPositiveIcon
        {
            get => (PackIconKind) GetValue(UpdateDialogPositiveIconProperty);
            set => SetValue(UpdateDialogPositiveIconProperty, value);
        }

        public static readonly DependencyProperty UpdateDialogNegativeContentProperty = DependencyProperty.Register(
            nameof(UpdateDialogNegativeContent),
            typeof(string),
            typeof(DynamicDataGrid),
            new FrameworkPropertyMetadata("CANCEL"));

        public string UpdateDialogNegativeContent
        {
            get => (string) GetValue(UpdateDialogNegativeContentProperty);
            set => SetValue(UpdateDialogNegativeContentProperty, value);
        }

        public static readonly DependencyProperty UpdateDialogNegativeIconProperty =
            DependencyProperty.Register(
                nameof(UpdateDialogNegativeIcon),
                typeof(PackIconKind?),
                typeof(DynamicDataGrid),
                new FrameworkPropertyMetadata(PackIconKind.Close));

        public PackIconKind? UpdateDialogNegativeIcon
        {
            get => (PackIconKind) GetValue(UpdateDialogNegativeIconProperty);
            set => SetValue(UpdateDialogNegativeIconProperty, value);
        }

        public static readonly DependencyProperty RemoveDialogTitleContentProperty = DependencyProperty.Register(
            nameof(RemoveDialogTitleContent),
            typeof(string),
            typeof(DynamicDataGrid),
            new FrameworkPropertyMetadata());

        public string RemoveDialogTitleContent
        {
            get => (string) GetValue(RemoveDialogTitleContentProperty);
            set => SetValue(RemoveDialogTitleContentProperty, value);
        }

        public static readonly DependencyProperty RemoveDialogTextContentProperty = DependencyProperty.Register(
            nameof(RemoveDialogTextContent),
            typeof(string),
            typeof(DynamicDataGrid),
            new FrameworkPropertyMetadata("Remove item?"));

        public string RemoveDialogTextContent
        {
            get => (string) GetValue(RemoveDialogTextContentProperty);
            set => SetValue(RemoveDialogTextContentProperty, value);
        }

        public static readonly DependencyProperty RemoveDialogPositiveContentProperty = DependencyProperty.Register(
            nameof(RemoveDialogPositiveContent),
            typeof(string),
            typeof(DynamicDataGrid),
            new FrameworkPropertyMetadata("REMOVE"));

        public string RemoveDialogPositiveContent
        {
            get => (string) GetValue(RemoveDialogPositiveContentProperty);
            set => SetValue(RemoveDialogPositiveContentProperty, value);
        }

        public static readonly DependencyProperty RemoveDialogPositiveIconProperty =
            DependencyProperty.Register(
                nameof(RemoveDialogPositiveIcon),
                typeof(PackIconKind?),
                typeof(DynamicDataGrid),
                new FrameworkPropertyMetadata(PackIconKind.Delete));

        public PackIconKind? RemoveDialogPositiveIcon
        {
            get => (PackIconKind) GetValue(RemoveDialogPositiveIconProperty);
            set => SetValue(RemoveDialogPositiveIconProperty, value);
        }

        public static readonly DependencyProperty RemoveDialogNegativeContentProperty = DependencyProperty.Register(
            nameof(RemoveDialogNegativeContent),
            typeof(string),
            typeof(DynamicDataGrid),
            new FrameworkPropertyMetadata("CANCEL"));

        public string RemoveDialogNegativeContent
        {
            get => (string) GetValue(RemoveDialogNegativeContentProperty);
            set => SetValue(RemoveDialogNegativeContentProperty, value);
        }

        public static readonly DependencyProperty RemoveDialogNegativeIconProperty =
            DependencyProperty.Register(
                nameof(RemoveDialogNegativeIcon),
                typeof(PackIconKind?),
                typeof(DynamicDataGrid),
                new FrameworkPropertyMetadata(PackIconKind.Close));

        public PackIconKind? RemoveDialogNegativeIcon
        {
            get => (PackIconKind) GetValue(RemoveDialogNegativeIconProperty);
            set => SetValue(RemoveDialogNegativeIconProperty, value);
        }

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(
                nameof(ItemsSource),
                typeof(IEnumerable),
                typeof(DynamicDataGrid),
                new FrameworkPropertyMetadata());

        public IEnumerable ItemsSource
        {
            get => (IEnumerable) GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public static readonly DependencyProperty DialogOptionsProperty =
            DependencyProperty.Register(
                nameof(DialogOptions),
                typeof(DialogOptions),
                typeof(DynamicDataGrid),
                new FrameworkPropertyMetadata(DialogOptions.Default, ItemsSourceChanged));

        private static void ItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((DynamicDataGrid) d).OnItemsSource(e.NewValue);
        }

        public DialogOptions DialogOptions
        {
            get => (DialogOptions) GetValue(DialogOptionsProperty);
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
            get => (IFormBuilder) GetValue(FormBuilderProperty);
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
            var collectionType = collection.GetType();
            var removeFromCollection = collectionType.GetMethod("Remove");

            if (removeFromCollection != null)
            {
                removeFromCollection.Invoke(collection, new[] {item});
            }
        }

        #endregion

        public static readonly RoutedCommand CreateItemCommand = new RoutedCommand();
        public static readonly RoutedCommand UpdateItemCommand = new RoutedCommand();
        public static readonly RoutedCommand RemoveItemCommand = new RoutedCommand();

        public static List<IAddActionInterceptor> AddInterceptorChain = new List<IAddActionInterceptor>();
        public static List<IUpdateActionInterceptor> UpdateInterceptorChain = new List<IUpdateActionInterceptor>();
        public static List<IRemoveActionInterceptor> RemoveInterceptorChain = new List<IRemoveActionInterceptor>();

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

        private DataGrid dataGrid;


        private List<DataGridColumn> ProtectedColumns { get; set; }

        private Type ItemType
        {
            get => itemType;
            set
            {
                if (itemType == value)
                    return;

                itemType = value;

                if (dataGrid == null || itemType == null) return;

                foreach (var dataGridColumn in dataGrid.Columns.Except(ProtectedColumns))
                {
                    dataGrid.Columns.Remove(dataGridColumn);
                }

                foreach (var propertyInfo in ItemType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Reverse())
                {
                    CreateColumn(propertyInfo);
                }
            }
        }

        private void CreateColumn(PropertyInfo propertyInfo)
        {
            if (propertyInfo.GetCustomAttribute<FieldIgnoreAttribute>() != null)
                return;

            if (FormBuilder is FormBuilder builder)
            {
                var formDefinition = builder.GetDefinition(itemType);
                var formElementContainers = formDefinition.FormRows.SelectMany(i => i.Elements)
                    .SelectMany(i => i.Elements).OfType<DataFormField>().Where(i => i.Key == propertyInfo.Name)
                    .ToList();
                var validations = formElementContainers.SelectMany(i => i.Validators)
                    .Select(i => i.GetValidator(null, new ValidationPipe())).OfType<ValidationRule>().ToList();

                var dataGridTextColumn = new MaterialDataGridTextColumn
                {
                    Header = propertyInfo.Name.Humanize(),
                    Binding = CreateBinding(propertyInfo, validations),
                    EditingElementStyle = TryFindResource("MaterialDesignDataGridTextColumnPopupEditingStyle") as Style,
                    MaxLength = propertyInfo.GetCustomAttribute<StringLengthAttribute>()?.MaximumLength ?? 0
                };

                dataGrid.Columns.Insert(0, dataGridTextColumn);
            }
        }

        private static Binding CreateBinding(PropertyInfo propertyInfo, IEnumerable<ValidationRule> validations)
        {
            var bindingBase = new Binding(propertyInfo.Name)
            {
                Mode = propertyInfo.CanRead && propertyInfo.CanWrite
                    ? BindingMode.TwoWay
                    : BindingMode.Default,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                ValidatesOnDataErrors = true,
                NotifyOnValidationError = true,
                NotifyOnSourceUpdated = true,
                NotifyOnTargetUpdated = true,
                ValidatesOnNotifyDataErrors = true
            };

            foreach (var fieldValidator in validations)
            {
                bindingBase.ValidationRules.Add(fieldValidator);
            }

            return bindingBase;
        }

        private bool canMutate;
        private Type itemType;

        public override void OnApplyTemplate()
        {
            dataGrid = Template.FindName("PART_DataGrid", this) as DataGrid;
            dataGrid.MouseDoubleClick += DataGridOnMouseDoubleClick;
            ProtectedColumns = dataGrid?.Columns.ToList();
        }

        private void DataGridOnMouseDoubleClick(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            if (dataGrid.SelectedItems.Count == 1)
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
                    .For((IFormDefinition) definition);
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
                    throw new InvalidOperationException($"{globalInterceptor.GetType().Name} are not allowed to return null.");
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
                return;
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

        public IReadOnlyList<FormRow> FormRows { get; }

        public double[] Grid => inner.Grid;

        public Type ModelType => inner.ModelType;

        public IDictionary<string, IValueProvider> Resources => inner.Resources;

        public object CreateInstance(IResourceContext context)
        {
            return inner.CreateInstance(context);
        }
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
                .Select(f => ((DataFormField) f).Key)));
        }

        public object Model { get; }

        public Snapshot Snapshot { get; }

        public IReadOnlyList<FormRow> FormRows { get; }

        public double[] Grid => inner.Grid;

        public Type ModelType => inner.ModelType;

        public IDictionary<string, IValueProvider> Resources => inner.Resources;

        public object CreateInstance(IResourceContext context)
        {
            return Model;
        }
    }
}