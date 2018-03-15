namespace Forge.Forms.Collections.Interfaces
{
    public interface IUpdateActionInterceptor
    {
        IUpdateActionContext Intercept(IUpdateActionContext modelContext);
    }
}
