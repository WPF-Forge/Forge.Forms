namespace Forge.Forms.Collections.Interfaces
{
    public interface IRemoveActionContext
    {
        object OldModel { get; }

        DynamicDataGrid Sender { get; }
    }
}