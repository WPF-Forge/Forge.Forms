using System.Collections;

namespace Forge.Forms.Collections.Interfaces
{
    public class UpdateActionContext : IUpdateActionContext
    {
        internal UpdateActionContext(IEnumerable source, DynamicDataGrid dataGrid, object oldModel, object newModel) :
            this(oldModel, newModel)
        {
            Source = source;
            DataGrid = dataGrid;
        }

        /// <inheritdoc />
        public UpdateActionContext(object oldModel, object newModel)
        {
            OldModel = oldModel;
            NewModel = newModel;
        }

        /// <inheritdoc />
        public object NewModel { get; }

        /// <inheritdoc />
        public object OldModel { get; }

        /// <inheritdoc />
        public IEnumerable Source { get; }

        /// <inheritdoc />
        public DynamicDataGrid DataGrid { get; }
    }
}