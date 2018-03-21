using System;
using System.Reflection;

namespace Forge.Forms.Collections.Interfaces
{
    public interface IColumnCreationInterceptorContext
    {
        PropertyInfo Property { get; }
        DynamicDataGrid Parent { get; }
        Type ObjectType { get; }
    }
}