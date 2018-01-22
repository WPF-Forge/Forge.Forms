using Forge.Forms.FormBuilding;

namespace Forge.Forms
{
    public interface IModelInterceptor
    {
        IModelContext Intercept(IModelContext modelContext);
    }

    public interface IModelContext
    {
        object NewModel { get; }

        IResourceContext ResourceContext { get; }
    }

    internal class ModelContext : IModelContext
    {
        public ModelContext(object newModel, IResourceContext resourceContext)
        {
            NewModel = newModel;
            ResourceContext = resourceContext;
        }

        public object NewModel { get; }

        public IResourceContext ResourceContext { get; }
    }
}
