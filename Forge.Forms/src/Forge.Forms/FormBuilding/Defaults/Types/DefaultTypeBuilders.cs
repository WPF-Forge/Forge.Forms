using System;
using System.Globalization;
using System.Linq;
using Forge.Forms.Annotations;

namespace Forge.Forms.FormBuilding.Defaults.Types
{
    internal class StringFieldBuilder : TypeBuilder<string>
    {
        protected override FormElement Build(IFormProperty property, Func<string, object> deserializer)
        {
            return new StringField(property.Name)
            {
                IsPassword = property.GetCustomAttribute<PasswordAttribute>() != null,
                IsMultiline = property.GetCustomAttribute<MultiLineAttribute>() != null
            };
        }
    }

    internal class BooleanFieldBuilder : IFieldBuilder
    {
        public FormElement TryBuild(IFormProperty property, Func<string, object> deserializer)
        {
            var isSwitch = property.GetCustomAttribute<ToggleAttribute>() != null;
            return new BooleanField(property.Name)
            {
                IsSwitch = isSwitch
            };
        }
    }

    internal class DateTimeFieldBuilder : IFieldBuilder
    {
        public FormElement TryBuild(IFormProperty property, Func<string, object> deserializer)
        {
            return new DateField(property.Name);
        }
    }

    internal class NumericFieldBuilder : IFieldBuilder
    {
        public NumericFieldBuilder(Func<string, CultureInfo, NumberStyles?, object> deserializer)
        {
            Deserializer = deserializer ?? throw new ArgumentNullException(nameof(deserializer));
        }

        public Func<string, CultureInfo, NumberStyles?, object> Deserializer { get; }

        public FormElement TryBuild(IFormProperty property, Func<string, object> deserializer)
        {
            var bindingAttribute = property.GetCustomAttribute<BindingAttribute>();
            IValueProvider numberStyles = null;
            if (bindingAttribute != null)
            {
                numberStyles = Utilities.GetResource<NumberStyles>(
                    bindingAttribute.NumberStyles,
                    null,
                    Deserializers.Enum<NumberStyles>());
            }

            var replacements = property
                .GetCustomAttributes<ReplaceAttribute>()
                .OrderBy(attr => attr.Position)
                .Select(attr => attr.GetReplacement());
            return new NumericField(
                property.Name,
                property.PropertyType,
                new NumericReplacementPipe(Deserializer, replacements))
            {
                NumberStyles = numberStyles
            };
        }
    }

    internal class ConvertedFieldBuilder : IFieldBuilder
    {
        public ConvertedFieldBuilder(Func<string, CultureInfo, object> deserializer)
        {
            Deserializer = deserializer ?? throw new ArgumentNullException(nameof(deserializer));
        }

        public Func<string, CultureInfo, object> Deserializer { get; }

        public FormElement TryBuild(IFormProperty property, Func<string, object> deserializer)
        {
            var replacements = property
                .GetCustomAttributes<ReplaceAttribute>()
                .OrderBy(attr => attr.Position)
                .Select(attr => attr.GetReplacement());
            return new ConvertedField(
                property.Name,
                property.PropertyType,
                new ReplacementPipe(Deserializer, replacements));
        }
    }
}
