using System.Windows.Controls;

namespace Forge.Forms.Collections.Interfaces
{
    public interface IColumnCreationInterceptor
    {
        DataGridColumn Intercept(IColumnCreationInterceptorContext context);
    }
}