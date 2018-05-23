namespace Forge.Forms.Collections.Interfaces
{
    public interface IUpdateActionContext: IBasicActionContext
    {
        object NewModel { get; }
        object OldModel { get; }
    }
}
