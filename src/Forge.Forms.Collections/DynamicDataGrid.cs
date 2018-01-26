using System;
using System.Windows;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Forge.Forms.DynamicExpressions;
using Forge.Forms.FormBuilding;
using Forge.Forms.FormBuilding.Defaults;
using MaterialDesignThemes.Wpf;
using Expression = System.Linq.Expressions.Expression;

namespace Forge.Forms.Collections
{
    public class DynamicDataGrid : Control
    {
        public static readonly DependencyProperty CreateDialogPositiveContentProperty =
            DependencyProperty.Register(
                nameof(CreateDialogPositiveContent),
                typeof(string),
                typeof(DynamicDataGrid),
                new FrameworkPropertyMetadata("ADD"));

        public string CreateDialogPositiveContent
        {
            get => (string)GetValue(CreateDialogPositiveContentProperty);
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
            get => (PackIconKind)GetValue(CreateDialogPositiveIconProperty);
            set => SetValue(CreateDialogPositiveIconProperty, value);
        }

        public static readonly DependencyProperty CreateDialogNegativeContentProperty = DependencyProperty.Register(
            nameof(CreateDialogNegativeContent),
            typeof(string),
            typeof(DynamicDataGrid),
            new FrameworkPropertyMetadata("CANCEL"));

        public string CreateDialogNegativeContent
        {
            get => (string)GetValue(CreateDialogNegativeContentProperty);
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
            get => (PackIconKind)GetValue(CreateDialogNegativeIconProperty);
            set => SetValue(CreateDialogNegativeIconProperty, value);
        }

        public static readonly DependencyProperty EditDialogPositiveContentProperty = DependencyProperty.Register(
            nameof(EditDialogPositiveContent),
            typeof(string),
            typeof(DynamicDataGrid),
            new FrameworkPropertyMetadata("SAVE"));

        public string EditDialogPositiveContent
        {
            get => (string)GetValue(EditDialogPositiveContentProperty);
            set => SetValue(EditDialogPositiveContentProperty, value);
        }

        public static readonly DependencyProperty EditDialogPositiveIconProperty =
            DependencyProperty.Register(
                nameof(EditDialogPositiveIcon),
                typeof(PackIconKind?),
                typeof(DynamicDataGrid),
                new FrameworkPropertyMetadata(PackIconKind.Check));

        public PackIconKind? EditDialogPositiveIcon
        {
            get => (PackIconKind)GetValue(EditDialogPositiveIconProperty);
            set => SetValue(EditDialogPositiveIconProperty, value);
        }

        public static readonly DependencyProperty EditDialogNegativeContentProperty = DependencyProperty.Register(
            nameof(EditDialogNegativeContent),
            typeof(string),
            typeof(DynamicDataGrid),
            new FrameworkPropertyMetadata("CANCEL"));

        public string EditDialogNegativeContent
        {
            get => (string)GetValue(EditDialogNegativeContentProperty);
            set => SetValue(EditDialogNegativeContentProperty, value);
        }

        public static readonly DependencyProperty EditDialogNegativeIconProperty =
            DependencyProperty.Register(
                nameof(EditDialogNegativeIcon),
                typeof(PackIconKind?),
                typeof(DynamicDataGrid),
                new FrameworkPropertyMetadata(PackIconKind.Close));

        public PackIconKind? EditDialogNegativeIcon
        {
            get => (PackIconKind)GetValue(EditDialogNegativeIconProperty);
            set => SetValue(EditDialogNegativeIconProperty, value);
        }

        public static readonly DependencyProperty RemoveDialogPositiveContentProperty = DependencyProperty.Register(
            nameof(RemoveDialogPositiveContent),
            typeof(string),
            typeof(DynamicDataGrid),
            new FrameworkPropertyMetadata("REMOVE"));

        public string RemoveDialogPositiveContent
        {
            get => (string)GetValue(RemoveDialogPositiveContentProperty);
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
            get => (PackIconKind)GetValue(RemoveDialogPositiveIconProperty);
            set => SetValue(RemoveDialogPositiveIconProperty, value);
        }

        public static readonly DependencyProperty RemoveDialogNegativeContentProperty = DependencyProperty.Register(
            nameof(RemoveDialogNegativeContent),
            typeof(string),
            typeof(DynamicDataGrid),
            new FrameworkPropertyMetadata("CANCEL"));

        public string RemoveDialogNegativeContent
        {
            get => (string)GetValue(RemoveDialogNegativeContentProperty);
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
            get => (PackIconKind)GetValue(RemoveDialogNegativeIconProperty);
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
            get => (IEnumerable)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public static readonly DependencyProperty DialogOptionsProperty =
            DependencyProperty.Register(
                nameof(DialogOptions),
                typeof(DialogOptions),
                typeof(DynamicDataGrid),
                new FrameworkPropertyMetadata(DialogOptions.Default));

        public DialogOptions DialogOptions
        {
            get => (DialogOptions)GetValue(DialogOptionsProperty);
            set => SetValue(DialogOptionsProperty, value);
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

        private static readonly Dictionary<Type, Action<object, object>> AddItemCache =
            new Dictionary<Type, Action<object, object>>();

        public static RoutedCommand AddItemCommand = new RoutedCommand();
        public static RoutedCommand EditItemCommand = new RoutedCommand();
        public static RoutedCommand RemoveItemCommand = new RoutedCommand();

        public DynamicDataGrid()
        {
            CommandBindings.Add(new CommandBinding(AddItemCommand, ExecuteAddItem, CanExecuteAddItem));
            CommandBindings.Add(new CommandBinding(EditItemCommand, ExecuteEditItem, CanExecuteEditItem));
            CommandBindings.Add(new CommandBinding(AddItemCommand, ExecuteDeleteItem, CanExecuteDeleteItem));
        }

        private void ExecuteAddItem(object sender, ExecutedRoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void CanExecuteAddItem(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ItemsSource != null;
        }

        private void ExecuteEditItem(object sender, ExecutedRoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void CanExecuteEditItem(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ItemsSource != null;
        }

        private void ExecuteDeleteItem(object sender, ExecutedRoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void CanExecuteDeleteItem(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ItemsSource != null;
        }

        private void RefreshItemsView()
        {
            throw new NotImplementedException();
        }

        private static void AddItem(Type itemType, object collection, object item)
        {
            if (!AddItemCache.TryGetValue(itemType, out var action))
            {
                var collectionType = typeof(ICollection<>).MakeGenericType(itemType);
                var addMethod = collectionType.GetMethod("Add");
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

        private async Task AddNewItem(object collection, IModelInputProvider input, Type subType)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            var interfaces = collection
                .GetType()
                .GetInterfaces()
                .Where(t =>
                    t.IsGenericType &&
                    t.GetGenericTypeDefinition() == typeof(ICollection<>))
                .ToList();

            if (interfaces.Count > 1)
            {
                throw new InvalidOperationException("Multiple implementations of ICollection<T> found.");
            }

            if (interfaces.Count == 0)
            {
                throw new InvalidOperationException("No implementation of ICollection<T> found.");
            }

            var collectionType = interfaces[0];
            var itemType = collectionType.GetGenericArguments()[0];
            object item;
            if (subType != null)
            {
                if (!itemType.IsAssignableFrom(subType))
                {
                    throw new InvalidOperationException($"Type {subType} cannot be assigned to {itemType}");
                }

                item = Activator.CreateInstance(subType);
            }
            else
            {
                item = Activator.CreateInstance(itemType);
            }

            var positive = await input.ShowDialog(item);
            if (!positive)
            {
                return;
            }

            AddItem(itemType, collection, item);
            if (!(collection is INotifyCollectionChanged))
            {
                RefreshItemsView();
            }
        }

        private FormRow GetCreateActions(IFormDefinition formDefinition)
        {
            return new FormRow(true, 1)
            {
                Elements =
                {
                    new FormElementContainer(0, formDefinition.Grid.Length, new ActionElement
                    {
                        Action = new LiteralValue("DynamicDataGrid_Create"),
                        Content = new LiteralValue("CREATE" /* TODO Customize from grid */),
                        ClosesDialog = LiteralValue.True,
                        ActionInterceptor = new LiteralValue(new ActionInterceptor(ctx =>
                        {
                            // TODO: handle create
                            return ctx;
                        })),
                        ActionParameter = new LiteralValue(null /* TODO */)
                    }),
                    new FormElementContainer(0, formDefinition.Grid.Length, new ActionElement
                    {
                        Action = new LiteralValue("DynamicDataGrid_Create"),
                        Content = new LiteralValue("CANCEL" /* TODO Customize from grid */),
                        ClosesDialog = LiteralValue.True,
                        ActionInterceptor = new LiteralValue(new ActionInterceptor(ctx =>
                        {
                            // TODO: handle cancel
                            return ctx;
                        })),
                        ActionParameter = new LiteralValue(null /* TODO */)
                    })
                }
            };
        }

        private IFormDefinition AddRows(
            IFormDefinition formDefinition,
            params FormRow[] rows)
        {
            return new FormDefinitionWrapper(
                formDefinition.FormRows.Concat(rows ?? new FormRow[0]).ToList().AsReadOnly(),
                formDefinition.Grid,
                formDefinition.ModelType,
                formDefinition.Resources);
        }
    }

    public interface IModelInputProvider
    {
        Task<bool> ShowDialog(object model);
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
        public FormDefinitionWrapper(IReadOnlyList<FormRow> formRows, double[] grid, Type modelType,
            IDictionary<string, IValueProvider> resources)
        {
            FormRows = formRows;
            Grid = grid;
            ModelType = modelType;
            Resources = resources;
        }

        public IReadOnlyList<FormRow> FormRows { get; }

        public double[] Grid { get; set; }

        public Type ModelType { get; }

        public IDictionary<string, IValueProvider> Resources { get; }

        public object CreateInstance(IResourceContext context)
        {
            throw new NotImplementedException();
        }
    }
}
