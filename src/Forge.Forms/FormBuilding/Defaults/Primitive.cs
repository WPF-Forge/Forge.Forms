using System;
using System.Globalization;

namespace Forge.Forms.FormBuilding.Defaults
{
    internal static class Primitive
    {
        private static FormDefinition BuildWith(DataFormField field)
        {
            var definition = new FormDefinition(field.PropertyType);
            definition.FormRows.Add(new FormRow
            {
                Elements = { new FormElementContainer(0, 1, field) }
            });

            field.Freeze();
            return definition;
        }

        private static FormDefinition BuildConverted(Type type, Func<string, CultureInfo, object> deserializer)
        {
            var field = new ConvertedField(null, type, new ReplacementPipe(deserializer))
            {
                IsDirectBinding = true
            };

            return BuildWith(field);
        }

        public static FormDefinition String()
        {
            var field = new StringField(null)
            {
                IsDirectBinding = true
            };

            return BuildWith(field);
        }

        public static FormDefinition DateTime()
        {
            var field = new DateField(null)
            {
                IsDirectBinding = true
            };

            return BuildWith(field);
        }

        public static FormDefinition Boolean()
        {
            var field = new BooleanField(null)
            {
                IsDirectBinding = true
            };

            return BuildWith(field);
        }

        public static FormDefinition Char()
        {
            return BuildConverted(typeof(char), Deserializers.Char);
        }

        public static FormDefinition Byte()
        {
            return BuildConverted(typeof(byte), Deserializers.Byte);
        }

        public static FormDefinition SByte()
        {
            return BuildConverted(typeof(sbyte), Deserializers.SByte);
        }

        public static FormDefinition Int16()
        {
            return BuildConverted(typeof(short), Deserializers.Int16);
        }

        public static FormDefinition Int32()
        {
            return BuildConverted(typeof(int), Deserializers.Int32);
        }

        public static FormDefinition Int64()
        {
            return BuildConverted(typeof(long), Deserializers.Int64);
        }

        public static FormDefinition UInt16()
        {
            return BuildConverted(typeof(ushort), Deserializers.UInt16);
        }

        public static FormDefinition UInt32()
        {
            return BuildConverted(typeof(uint), Deserializers.UInt32);
        }

        public static FormDefinition UInt64()
        {
            return BuildConverted(typeof(ulong), Deserializers.UInt64);
        }

        public static FormDefinition Single()
        {
            return BuildConverted(typeof(float), Deserializers.Single);
        }

        public static FormDefinition Double()
        {
            return BuildConverted(typeof(double), Deserializers.Double);
        }

        public static FormDefinition Decimal()
        {
            return BuildConverted(typeof(decimal), Deserializers.Decimal);
        }
    }
}
