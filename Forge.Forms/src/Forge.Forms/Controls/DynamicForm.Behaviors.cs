using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Forge.Forms.Behaviors;

namespace Forge.Forms.Controls
{
    public partial class DynamicForm
    {
        internal static readonly List<object> GlobalBehaviors = new List<object>();

        /// <summary>
        /// Adds a global behavior that applies to all forms.
        /// </summary>
        /// <param name="behavior">Behavior implementation.</param>
        public static void AddBehavior(object behavior)
        {
            GlobalBehaviors.Add(behavior);
        }

        /// <summary>
        /// Removes specified behavior instance.
        /// </summary>
        /// <param name="behavior">Behavior implementation.</param>
        public static void RemoveBehavior(object behavior)
        {
            GlobalBehaviors.Remove(behavior);
        }

        /// <summary>
        /// Remove all behaviors that are of T.
        /// </summary>
        /// <typeparam name="T">Behavior type to remove.</typeparam>
        public static void RemoveBehavior<T>()
        {
            GlobalBehaviors.RemoveAll(obj => obj is T);
        }

        private void HandleModelChanged()
        {
            var handlers = GlobalBehaviors
                .Where(b => b is IModelChangedBehavior)
                .ToList();

            if (handlers.Count == 0)
            {
                return;
            }

            var context = new EventContext(
                Value,
                currentDefinition,
                Model,
                Context,
                ResourceContext);
            foreach (var handler in handlers)
            {
                ((IModelChangedBehavior)handler).ModelChanged(context);
            }
        }

        private void HandlePropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            var handlers = GlobalBehaviors
                .Where(b => b is IPropertyChangedBehavior)
                .ToList();

            if (handlers.Count == 0)
            {
                return;
            }

            var context = new PropertyChangedContext(
                Value,
                currentDefinition,
                Model,
                Context,
                ResourceContext,
                args.PropertyName);
            foreach (var handler in handlers)
            {
                ((IPropertyChangedBehavior)handler).PropertyChanged(context);
            }
        }
    }
}
