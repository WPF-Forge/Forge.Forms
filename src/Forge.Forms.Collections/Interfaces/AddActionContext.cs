namespace Forge.Forms.Collections.Interfaces
{
    public class AddActionContext : IAddActionContext
    {
        /// <inheritdoc />
        public object NewModel { get; }

        /// <inheritdoc />
        public AddActionContext(object newModel)
        {
            NewModel = newModel;
        }
    }
}
