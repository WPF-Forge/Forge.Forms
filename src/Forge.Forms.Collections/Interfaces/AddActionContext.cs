namespace Forge.Forms.Collections.Interfaces
{
    internal class AddActionContext : IAddActionContext
    {
        /// <inheritdoc />
        public AddActionContext(object newModel)
        {
            NewModel = newModel;
        }

        /// <inheritdoc />
        public object NewModel { get; }
    }
}