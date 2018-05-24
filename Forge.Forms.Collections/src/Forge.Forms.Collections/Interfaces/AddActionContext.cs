namespace Forge.Forms.Collections.Interfaces
{
    public class AddActionContext : IAddActionContext
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
