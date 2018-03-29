using System;
using System.Reflection;
using System.Windows.Controls;

namespace Forge.Forms.Collections.Interfaces
{
    public interface IColumnCreationInterceptorContext
    {
        DataGridColumn Column { get; }

        Type ObjectType { get; }

        DynamicDataGrid Parent { get; }

        PropertyInfo Property { get; }

        DynamicDataGrid Sender { get; }
    }
}