namespace Forge.Forms.Collections.Interfaces
{
    public interface ICreateActionInterceptor
    {
        ICreateActionContext Intercept(ICreateActionContext modelContext);
    }
}
