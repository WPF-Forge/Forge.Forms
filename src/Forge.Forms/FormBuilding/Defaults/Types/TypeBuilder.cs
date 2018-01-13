using System;
using Forge.Forms.Components.Fields;
using Forge.Forms.Interfaces;

namespace Forge.Forms.FormBuilding.Defaults.Types
{
    public abstract class TypeBuilder<T> : IFieldBuilder
    {
        public FormElement TryBuild(IFormProperty property, Func<string, object> deserializer)
        {
            if (property.PropertyType != typeof(T))
            {
                return null;
            }

            return Build(property, deserializer);
        }

        protected abstract FormElement Build(IFormProperty property, Func<string, object> deserializer);
    }
}
