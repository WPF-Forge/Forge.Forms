using System;
using System.Reflection;
using Forge.Forms.Collections.Interfaces;

namespace Forge.Forms.Collections
{
    public class ColumnCreationInterceptorContext : IColumnCreationInterceptorContext
    {
        public PropertyInfo Property { get; }
        public DynamicDataGrid Parent { get; }
        public Type ObjectType { get; }

        public ColumnCreationInterceptorContext(PropertyInfo property, DynamicDataGrid parent, Type objectType)
        {
            Property = property ?? throw new ArgumentNullException(nameof(property));
            Parent = parent ?? throw new ArgumentNullException(nameof(parent));
            ObjectType = objectType ?? throw new ArgumentNullException(nameof(objectType));
        }
    }
}