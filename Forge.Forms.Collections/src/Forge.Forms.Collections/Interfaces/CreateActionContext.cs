namespace Forge.Forms.Collections.Interfaces
{
    public class CreateActionContext : ICreateActionContext
    {
        /// <inheritdoc />
        public CreateActionContext(object newModel)
        {
            NewModel = newModel;
        }

        /// <inheritdoc />
        public object NewModel { get; }
    }
}
