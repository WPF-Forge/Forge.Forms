using System;
using Forge.Forms.Annotations;

namespace Forge.Forms.FormBuilding.Defaults.Properties
{
    internal class DirectContentBuilder : IFieldBuilder
    {
        public FormElement TryBuild(IFormProperty property, Func<string, object> deserializer)
        {
            var attr = property.GetCustomAttribute<DirectContentAttribute>();
            return attr == null
                ? null
                : new DirectContentField(property.Name, property.PropertyType);
        }
    }
}
