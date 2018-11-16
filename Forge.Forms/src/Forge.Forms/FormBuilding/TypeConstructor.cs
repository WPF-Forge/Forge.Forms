using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Forge.Forms.FormBuilding
{
    public abstract class TypeConstructionContext
    {
    }

    public class XmlConstructionContext : TypeConstructionContext
    {
        public XmlConstructionContext(XElement element)
        {
            Element = element;
        }

        public XElement Element { get; }
    }

    public class TypeConstructor
    {
        public TypeConstructor(
            Type propertyType)
            : this(propertyType, null)
        {
        }

        public TypeConstructor(
            Type propertyType,
            params Attribute[] attributes)
            : this(propertyType, (IEnumerable<Attribute>)attributes)
        {
        }

        public TypeConstructor(
            Type propertyType,
            IEnumerable<Attribute> customAttributes)
        {
            PropertyType = propertyType;
            CustomAttributes = customAttributes?.ToArray() ?? new Attribute[0];
        }

        public Type PropertyType { get; }

        public IEnumerable<Attribute> CustomAttributes { get; }

        public static implicit operator TypeConstructor(Type type)
        {
            return new TypeConstructor(type);
        }
    }
}