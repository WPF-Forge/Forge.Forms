namespace Forge.Forms.Collections.Interfaces
{
    public interface IRemoveActionContext: IBasicActionContext
    {
        object OldModel { get; }
    }
}
