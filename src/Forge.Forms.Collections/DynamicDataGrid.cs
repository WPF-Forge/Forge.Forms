using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Forge.Forms.Collections
{
    public class DynamicDataGrid : Control
    {
        private static readonly Dictionary<Type, Action<object, object>> AddItemCache
            = new Dictionary<Type, Action<object, object>>();

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
                .Where(t => t.GetGenericTypeDefinition() == typeof(ICollection<>))
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
            var item = Activator.CreateInstance(subType ?? itemType);
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
    }

    public interface IModelInputProvider
    {
        Task<bool> ShowDialog(object model);
    }
}
