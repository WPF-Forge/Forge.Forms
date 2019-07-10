using System;
using System.Collections.Generic;
using System.Linq;

namespace Forge.Forms.FormBuilding
{
    public class DynamicProperty : IFormProperty
    {
        private readonly Attribute[] attributes;
        private readonly IFormDefinition formDefinition;
        public DynamicProperty(string name, Type propertyType, Attribute[] attributes, IFormDefinition formDefinition)
        {
            this.attributes = attributes;
            Name = name;
            PropertyType = propertyType;
            this.formDefinition = formDefinition;
        }

        public string Name { get; }

        public Type PropertyType { get; }

        public Type DeclaringType => null;

        public IReadOnlyFormDefinition DeclaringForm => formDefinition;

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
