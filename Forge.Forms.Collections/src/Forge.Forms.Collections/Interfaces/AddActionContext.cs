namespace Forge.Forms.Collections.Interfaces
{
    public class AddActionContext : IAddActionContext
    {
        /// <inheritdoc />
        public AddActionContext(object newModel, DynamicDataGrid sender)
        {
            NewModel = newModel;
            Sender = sender;
        }

        /// <inheritdoc />
        public object NewModel { get; }

        /// <inheritdoc />
        public DynamicDataGrid Sender { get; }
    }
}