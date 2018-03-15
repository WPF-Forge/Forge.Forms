using Proxier.Extensions;

namespace Forge.Forms.Mapper.Interceptors
{
    class MapperInterceptor : IModelInterceptor
    {
        public IModelContext Intercept(IModelContext modelContext)
        {
            return modelContext.NewModel == null
                ? modelContext
                : new ModelContext(modelContext.NewModel.GetInjectedObject(), modelContext.ResourceContext);
        }
    }
}