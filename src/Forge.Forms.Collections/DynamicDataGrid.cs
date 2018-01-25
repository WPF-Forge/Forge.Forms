using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using Forge.Forms.DynamicExpressions;
using Forge.Forms.FormBuilding;
using Forge.Forms.FormBuilding.Defaults;

namespace Forge.Forms.Collections
{
    public class DynamicDataGrid : Control
    {
        public static readonly DependencyProperty SaveActionContentProperty = DependencyProperty.Register(
            nameof(SaveActionContent),
            typeof(string),
            typeof(DynamicDataGrid),
            new FrameworkPropertyMetadata("Save"));

        public string SaveActionContent { get; set; }

        public static readonly DependencyProperty SaveAcitionIconProperty = DependencyProperty.Register(
            nameof(SaveAcitionIcon),
            typeof(PackIconKind),
            typeof(DynamicDataGrid),
            new FrameworkPropertyMetadata(PackIconKind.ContentSave));

        public PackIconKind SaveAcitionIcon { get; set; }

        public static readonly DependencyProperty CancelActionContentProperty = DependencyProperty.Register(
            nameof(CancelActionContent),
            typeof(string),
            typeof(DynamicDataGrid),
            new FrameworkPropertyMetadata("Cancel"));

        public string CancelActionContent { get; set; }

        public static readonly DependencyProperty CancelActionIconProperty = DependencyProperty.Register(
            nameof(CancelActionIcon),
            typeof(PackIconKind),
            typeof(DynamicDataGrid),
            new FrameworkPropertyMetadata(PackIconKind.Close));

        public PackIconKind CancelActionIcon { get; set; }

        public static readonly DependencyProperty UpdateActionContentProperty = DependencyProperty.Register(
            nameof(UpdateActionContent),
            typeof(string),
            typeof(DynamicDataGrid),
            new FrameworkPropertyMetadata("Edit"));

        public string UpdateActionContent { get; set; }

        public static readonly DependencyProperty UpdateActionIconProperty = DependencyProperty.Register(
            nameof(UpdateActionIcon),
            typeof(PackIconKind),
            typeof(DynamicDataGrid),
            new FrameworkPropertyMetadata(PackIconKind.Pencil));

        public PackIcon UpdateActionIcon { get; set; }

        private static readonly Dictionary<Type, Action<object, object>> AddItemCache =
            new Dictionary<Type, Action<object, object>>();

        // TODO: Dependency properties
        /// <summary>
        /// The items source.
        /// </summary>
        public IEnumerable ItemsSource { get; set; }

        /// <summary>
        /// The dialog identifier in which the input dialogs should be displayed.
        /// </summary>
        public object TargetDialogIdentifier { get; set; }

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
        public FormDefinitionWrapper(IReadOnlyList<FormRow> formRows, double[] grid, Type modelType, IDictionary<string, IValueProvider> resources)
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
