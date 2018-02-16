namespace Forge.Forms.Collections.Interfaces
{
    public class RemoveActionContext : IRemoveActionContext
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
