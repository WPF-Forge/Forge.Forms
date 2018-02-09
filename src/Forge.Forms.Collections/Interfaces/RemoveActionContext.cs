namespace Forge.Forms.Collections.Interfaces
{
    public class RemoveActionContext : IRemoveActionContext
    {
        /// <inheritdoc />
        public object OldModel { get; }

        /// <inheritdoc />
        public RemoveActionContext(object oldModel)
        {
            OldModel = oldModel;
        }
    }
}
