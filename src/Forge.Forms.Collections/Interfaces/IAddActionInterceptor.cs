namespace Forge.Forms.Collections.Interfaces
{
    public interface IAddActionInterceptor
    {
        IAddActionContext Intercept(IAddActionContext modelContext);
    }
}
