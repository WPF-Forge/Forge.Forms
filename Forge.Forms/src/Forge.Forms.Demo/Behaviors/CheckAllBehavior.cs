using System;
using System.Collections.Generic;
using Forge.Forms.Behaviors;
using Forge.Forms.FormBuilding;

namespace Forge.Forms.Demo.Behaviors
{
    public class CheckAllBehavior : IModelChangedBehavior, IPropertyChangedBehavior
    {
        private Dictionary<string, string[]> parentChildRelationships = new Dictionary<string, string[]>();
        private Dictionary<string, string> childParentRelationships = new Dictionary<string, string>();
        private bool locked;

        public void PropertyChanged(IPropertyChangedContext context)
        {
            if (locked)
            {
                return;
            }

            if (!(context.Model is IDictionary<string, object> model))
            {
                return;
            }

            locked = true;

            var propertyName = context.PropertyName;
            var value = model[propertyName];
            if (parentChildRelationships.TryGetValue(propertyName, out var children))
            {
                if (value is bool b)
                {
                    foreach (var child in children)
                    {
                        model[child] = b;
                    }
                }
            }
            else if (childParentRelationships.TryGetValue(propertyName, out var parent))
            {
                children = parentChildRelationships[parent];
                var areEqual = true;
                foreach (var child in children)
                {
                    if (!Equals(model[child], value))
                    {
                        areEqual = false;
                        break;
                    }
                }

                model[parent] = areEqual ? value : null;
            }

            locked = false;
        }

        public void ModelChanged(IEventContext context)
        {
            childParentRelationships = new Dictionary<string, string>();
            parentChildRelationships = new Dictionary<string, string[]>();
            locked = false;
            if (context.FormDefinition != null)
            {
                foreach (var element in context.FormDefinition.GetElements())
                {
                    if (!(element is DataFormField field) || field.Key == null)
                    {
                        continue;
                    }

                    if (field.Metadata.TryGetValue("sets", out var sets))
                    {
                        var children = sets?.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
                        if (children != null && children.Length != 0)
                        {
                            parentChildRelationships[field.Key] = children;
                            foreach (var child in children)
                            {
                                childParentRelationships[child] = field.Key;
                            }
                        }
                    }
                }
            }
        }
    }
}
