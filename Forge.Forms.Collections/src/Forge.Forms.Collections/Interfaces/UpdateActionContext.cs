namespace Forge.Forms.Collections.Interfaces
{
    public class UpdateActionContext : IUpdateActionContext
    {
        /// <inheritdoc />
        public UpdateActionContext(object oldModel, object newModel, DynamicDataGrid sender)
        {
            OldModel = oldModel;
            NewModel = newModel;
            Sender = sender;
        }

        /// <inheritdoc />
        public object NewModel { get; }

        /// <inheritdoc />
        public object OldModel { get; }

        /// <inheritdoc />
        public DynamicDataGrid Sender { get; }
    }
}