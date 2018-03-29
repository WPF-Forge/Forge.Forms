namespace Forge.Forms.Collections.Interfaces
{
    public class RemoveActionContext : IRemoveActionContext
    {
        /// <inheritdoc />
        public RemoveActionContext(object oldModel, DynamicDataGrid sender)
        {
            OldModel = oldModel;
            Sender = sender;
        }

        /// <inheritdoc />
        public object OldModel { get; }

        /// <inheritdoc />
        public DynamicDataGrid Sender { get; }
    }
}