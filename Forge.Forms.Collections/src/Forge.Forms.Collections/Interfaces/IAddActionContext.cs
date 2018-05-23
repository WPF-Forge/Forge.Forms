using System.Collections;

namespace Forge.Forms.Collections.Interfaces
{
    public interface IBasicActionContext
    {
        IEnumerable Source { get; }
        DynamicDataGrid DataGrid { get; }
    }

    public interface IAddActionContext : IBasicActionContext
    {
        object NewModel { get; }
    }
}
