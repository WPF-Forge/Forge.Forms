using System;
using System.Collections.Generic;
using System.Reflection;

namespace Forge.Forms.FormBuilding
{
    internal class PropertyInfoWrapper : IFormProperty
    {
        private readonly PropertyInfo property;
        private readonly IFormDefinition formDefinition;
        public PropertyInfoWrapper(PropertyInfo property,IFormDefinition formDefinition)
        {
            this.property = property;
            this.formDefinition = formDefinition;
        }

        public string Name => property.Name;

        public Type PropertyType => property.PropertyType;

        public Type DeclaringType => property.DeclaringType;
        public IReadOnlyFormDefinition DeclaringForm => formDefinition;

        public bool CanWrite => property.CanWrite && property.GetSetMethod(true).IsPublic;

        public T GetCustomAttribute<T>() where T : Attribute
        {
            return property.GetCustomAttribute<T>();
        }

        public IEnumerable<T> GetCustomAttributes<T>() where T : Attribute
        {
            return property.GetCustomAttributes<T>();
        }
    }
}
