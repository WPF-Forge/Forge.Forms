using System;
using System.Reflection;
using System.Windows.Controls;

namespace Forge.Forms.Collections.Interfaces
{
    public interface IColumnCreationInterceptorContext
    {
        PropertyInfo Property { get; }
        DynamicDataGrid Parent { get; }
        Type ObjectType { get; }
        DataGridColumn Column { get; }
    }
}