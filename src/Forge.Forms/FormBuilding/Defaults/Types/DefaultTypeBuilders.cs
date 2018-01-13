using System;
using System.Globalization;
using System.Linq;
using Forge.Forms.Annotations;
using Forge.Forms.Annotations.Display;
using Forge.Forms.Components.Fields;
using Forge.Forms.Components.Fields.Defaults;
using Forge.Forms.Interfaces;

namespace Forge.Forms.FormBuilding.Defaults.Types
{
    internal class StringFieldBuilder : TypeBuilder<string>
    {
        protected override FormElement Build(IFormProperty property, Func<string, object> deserializer)
        {
            return new StringField(property.Name)
            {
                IsPassword = property.GetCustomAttribute<PasswordAttribute>() != null
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

    //internal class ByteFieldBuilder : TypeBuilder<Byte> {
    //    protected override FormElement Build(PropertyInfo property, Func<string, object> deserializer)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    //internal class SByteFieldBuilder : TypeBuilder<SByte> {
    //    protected override FormElement Build(PropertyInfo property, Func<string, object> deserializer)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    //internal class Int16FieldBuilder : TypeBuilder<Int16> {
    //    protected override FormElement Build(PropertyInfo property, Func<string, object> deserializer)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    //internal class Int32FieldBuilder : TypeBuilder<Int32> {
    //    protected override FormElement Build(PropertyInfo property, Func<string, object> deserializer)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    //internal class Int64FieldBuilder : TypeBuilder<Int64> {
    //    protected override FormElement Build(PropertyInfo property, Func<string, object> deserializer)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    //internal class UInt16FieldBuilder : TypeBuilder<UInt16> {
    //    protected override FormElement Build(PropertyInfo property, Func<string, object> deserializer)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    //internal class UInt32FieldBuilder : TypeBuilder<UInt32> {
    //    protected override FormElement Build(PropertyInfo property, Func<string, object> deserializer)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    //internal class UInt64FieldBuilder : TypeBuilder<UInt64> {
    //    protected override FormElement Build(PropertyInfo property, Func<string, object> deserializer)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    //internal class SingleFieldBuilder : TypeBuilder<Single> {
    //    protected override FormElement Build(PropertyInfo property, Func<string, object> deserializer)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    //internal class DoubleFieldBuilder : TypeBuilder<Double> {
    //    protected override FormElement Build(PropertyInfo property, Func<string, object> deserializer)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    //internal class DecimalFieldBuilder : TypeBuilder<Decimal> {
    //    protected override FormElement Build(PropertyInfo property, Func<string, object> deserializer)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}
