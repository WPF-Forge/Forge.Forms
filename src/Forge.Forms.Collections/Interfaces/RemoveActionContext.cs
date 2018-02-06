namespace Forge.Forms.Collections.Interfaces
{
    internal class RemoveActionContext : IRemoveActionContext
    {
        /// <inheritdoc />
        public RemoveActionContext(object oldModel)
        {
            OldModel = oldModel;
        }

        /// <inheritdoc />
        public object OldModel { get; }
    }
}