using System.Collections;

namespace Forge.Forms.Collections.Interfaces
{
    public class AddActionContext : IAddActionContext
    {
        /// <inheritdoc />
        public AddActionContext(object newModel)
        {
            NewModel = newModel;
        }

        
        internal AddActionContext(IEnumerable source, DynamicDataGrid dataGrid, object newModel) : this(newModel)
        {
            Source = source;
            DataGrid = dataGrid;
        }

        /// <inheritdoc />
        public object NewModel { get; }

        /// <inheritdoc />
        public IEnumerable Source { get; }

        /// <inheritdoc />
        public DynamicDataGrid DataGrid { get; }
    }
}
