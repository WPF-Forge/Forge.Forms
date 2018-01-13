using System;
using Forge.Forms.Annotations;
using Forge.Forms.Components.Fields;
using Forge.Forms.Components.Fields.Defaults;
using Forge.Forms.Interfaces;

namespace Forge.Forms.FormBuilding.Defaults.Initializers
{
    internal class BindingInitializer : IFieldInitializer
    {
        public void Initialize(FormElement element, IFormProperty property, Func<string, object> deserializer)
        {
            if (!(element is DataFormField field))
            {
                return;
            }

            var attr = property.GetCustomAttribute<BindingAttribute>();
            if (attr == null)
            {
                return;
            }

            attr.Apply(field.BindingOptions);
            if (attr.ConversionErrorMessage != null && element is ConvertedField convertedField)
            {
                var errorProvider = Utilities.GetErrorProvider(attr.ConversionErrorMessage, property.Name);
                convertedField.ConversionErrorMessage = errorProvider;
            }
        }
    }
}
