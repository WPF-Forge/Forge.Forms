using System;
using Forge.Forms.Annotations;
using Forge.Forms.DynamicExpressions;
using Humanizer;

namespace Forge.Forms.FormBuilding.Defaults.Initializers
{
    internal class FieldInitializer : IFieldInitializer
    {
        public void Initialize(FormElement element, IFormProperty property, Func<string, object> deserializer)
        {
            var attr = property.GetCustomAttribute<FieldAttribute>();
            if (attr == null)
            {
                if (element is FormField field && field.Name == null)
                {
                    field.Name = new LiteralValue(property.Name.Humanize());
                }


                if (element is DataFormField dataField)
                {
                    if (!property.CanWrite)
                    {
                        dataField.IsReadOnly = LiteralValue.True;
                    }
                }
            }
            else
            {
                element.IsVisible = Utilities.GetResource<bool>(attr.IsVisible, true, Deserializers.Boolean);
                element.IsEnabled = Utilities.GetResource<bool>(attr.IsEnabled, true, Deserializers.Boolean);
                element.InitialFocus = Utilities.GetResource<bool>(attr.InitialFocus, false, Deserializers.Boolean);
                if (element is FormField field)
                {
                    field.Name = attr.HasName
                        ? Utilities.GetStringResource(attr.Name)
                        : new LiteralValue(property.Name.Humanize());
                    field.ToolTip = Utilities.GetStringResource(attr.ToolTip);
                    field.Icon = Utilities.GetIconResource(attr.Icon);
                }

                if (element is DataFormField dataField)
                {
                    if (property.CanWrite)
                    {
                        dataField.IsReadOnly = Utilities.GetResource<bool>(attr.IsReadOnly, false, Deserializers.Boolean);
                    }
                    else
                    {
                        dataField.IsReadOnly = LiteralValue.True;
                    }

                    var type = property.PropertyType;
                    if (attr.DefaultValue != null)
                    {
                        dataField.DefaultValue = Utilities.GetResource<object>(attr.DefaultValue, null, deserializer);
                    }
                    else if (!type.IsValueType)
                    {
                        // Null for reference types and nullables.
                        dataField.DefaultValue = LiteralValue.Null;
                    }
                    else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        // Same for nullables.
                        dataField.DefaultValue = LiteralValue.Null;
                    }
                }
            }
        }
    }
}
