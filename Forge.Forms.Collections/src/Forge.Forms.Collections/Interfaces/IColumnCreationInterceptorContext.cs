using System;
using System.Reflection;
using System.Windows.Controls;

namespace Forge.Forms.Collections.Interfaces
{
    public interface IColumnCreationInterceptorContext : IBasicActionContext
    {
        PropertyInfo Property { get; }
        DynamicDataGrid Parent { get; }
        Type ObjectType { get; }
        DataGridColumn Column { get; }
    }
}