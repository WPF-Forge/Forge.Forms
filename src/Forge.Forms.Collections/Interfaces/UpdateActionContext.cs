namespace Forge.Forms.Collections.Interfaces
{
    internal class UpdateActionContext : IUpdateActionContext
    {
        /// <inheritdoc />
        public UpdateActionContext(object oldModel, object newModel)
        {
            OldModel = oldModel;
            NewModel = newModel;
        }

        /// <inheritdoc />
        public object OldModel { get; }

        /// <inheritdoc />
        public object NewModel { get; }
    }
}