namespace Forge.Forms.Collections.Interfaces
{
    public interface IUpdateActionContext
    {
        object OldModel { get; }
        object NewModel { get; }
    }
}