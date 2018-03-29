using System;
using System.Reflection;
using System.Windows.Controls;
using Forge.Forms.Collections.Interfaces;

namespace Forge.Forms.Collections
{
    public class ColumnCreationInterceptorContext : IColumnCreationInterceptorContext
    {
        public PropertyInfo Property { get; }
        public DynamicDataGrid Parent { get; }
        public Type ObjectType { get; }

        /// <inheritdoc />
        public DataGridColumn Column { get; }

        /// <inheritdoc />
        public DynamicDataGrid Sender { get; }

        public ColumnCreationInterceptorContext(PropertyInfo property, DynamicDataGrid parent, Type objectType, DataGridColumn column, DynamicDataGrid sender)
        {
            Property = property ?? throw new ArgumentNullException(nameof(property));
            Parent = parent ?? throw new ArgumentNullException(nameof(parent));
            ObjectType = objectType ?? throw new ArgumentNullException(nameof(objectType));
            Column = column;
            Sender = sender;
        }
    }
}