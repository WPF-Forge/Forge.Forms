using System;
using Forge.Forms.Annotations;

namespace Forge.Forms.FormBuilding.Defaults.Properties
{
    internal class SliderBuilder : IFieldBuilder
    {
        public FormElement TryBuild(IFormProperty property, Func<string, object> deserializer)
        {
            var attr = property.GetCustomAttribute<SliderAttribute>();
            if (attr == null)
            {
                return null;
            }

            return new SliderField(property.Name, property.PropertyType)
            {
                // Since WPF slider uses doubles, we have to guess a double stringified value.
                // Defaults to 0d-10d.
                Minimum = Utilities.GetResource<object>(attr.Minimum, 0d, Deserializers.Double),
                Maximum = Utilities.GetResource<object>(attr.Maximum, 10d, Deserializers.Double)
            };
        }
    }
}
