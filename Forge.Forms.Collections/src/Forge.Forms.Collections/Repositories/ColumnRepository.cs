using System.Reflection;
using System.Windows.Controls;
using Forge.Forms.Collections.Interfaces;

namespace Forge.Forms.Collections.Repositories
{
    internal class ColumnRepository
    {
        public DataGridColumn GetColumn(PropertyInfo propertyInfo, DynamicDataGrid dataGrid)
        {
            IColumnCreationInterceptorContext column = null;

            foreach (var columnCreationInterceptor in dataGrid.ColumnCreationInterceptors)
            {
                var interceptorContext = columnCreationInterceptor.Intercept(
                    new ColumnCreationInterceptorContext(propertyInfo, dataGrid, dataGrid.ItemType, null));

                if (interceptorContext == null)
                {
                    return null;
                }

                column = interceptorContext;
            }

            return column?.Column;
        }
    }
}