namespace Forge.Forms.Collections.Interfaces
{
    public class UpdateActionContext : IUpdateActionContext
    {
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
    }
}
