using System;
using Forge.Forms.Collections.Annotations;
using Forge.Forms.FormBuilding;

namespace Forge.Forms.Collections.Fields
{
    class CrudFieldBuilder : IFieldBuilder
    {
        public FormElement TryBuild(IFormProperty property, Func<string, object> deserializer)
        {
            if (property.GetCustomAttribute<CrudAttribute>() != null)
            {
                return new CrudField(property.Name, property.PropertyType);
            }

            return null;
        }
    }
}
