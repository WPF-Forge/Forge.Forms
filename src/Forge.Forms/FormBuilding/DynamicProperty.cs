using System;
using System.Collections.Generic;
using System.Linq;
using Forge.Forms.Interfaces;

namespace Forge.Forms.FormBuilding
{
    public class DynamicProperty : IFormProperty
    {
        private readonly Attribute[] attributes;

        public DynamicProperty(string name, Type propertyType, Attribute[] attributes)
        {
            this.attributes = attributes;
            Name = name;
            PropertyType = propertyType;
        }

        public string Name { get; }

        public Type PropertyType { get; }

        public Type DeclaringType => null;

        public bool CanWrite => true;

        public T GetCustomAttribute<T>() where T : Attribute
        {
            return attributes.OfType<T>().FirstOrDefault();
        }

        public IEnumerable<T> GetCustomAttributes<T>() where T : Attribute
        {
            return attributes.OfType<T>();
        }
    }
}
