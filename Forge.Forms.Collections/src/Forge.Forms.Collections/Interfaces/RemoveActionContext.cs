using System.Collections;

namespace Forge.Forms.Collections.Interfaces
{
    public class RemoveActionContext : IRemoveActionContext
    {
        internal RemoveActionContext(IEnumerable source, DynamicDataGrid dataGrid, object oldModel) : this(oldModel)
        {
            Source = source;
            DataGrid = dataGrid;
        }

        /// <inheritdoc />
        public RemoveActionContext(object oldModel)
        {
            OldModel = oldModel;
        }

        /// <inheritdoc />
        public object OldModel { get; }

        /// <inheritdoc />
        public IEnumerable Source { get; }

        /// <inheritdoc />
        public DynamicDataGrid DataGrid { get; }
    }
}
