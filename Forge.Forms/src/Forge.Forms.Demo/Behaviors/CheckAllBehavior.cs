using System;
using System.Collections.Generic;
using System.Linq;
using Forge.Forms.Behaviors;
using Forge.Forms.FormBuilding;

namespace Forge.Forms.Demo.Behaviors
{
    public class CheckAllBehavior : IPropertyChangedBehavior
    {
        public void PropertyChanged(IPropertyChangedContext context)
        {
            if (!(context.Model is IDictionary<string, object> model))
            {
                return;
            }

            // Select all only on checked.
            if (model[context.PropertyName] is true)
            {
                var meta = context
                    .FormDefinition
                    .GetElements()
                    .FirstOrDefault(elem => elem is DataFormField d && d.Key == context.PropertyName)
                    ?.Metadata;
                if (meta == null || !meta.TryGetValue("sets", out var value))
                {
                    return;
                }

                var props = value?.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (props != null)
                {
                    foreach (var prop in props)
                    {
                        model[prop] = true;
                    }
                }
            }
        }
    }
}
