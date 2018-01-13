using System.Collections.Generic;
using System.Windows.Data;

namespace Forge.Forms.Interfaces
{
    public interface IDataBindingProvider : IBindingProvider
    {
        IEnumerable<BindingExpressionBase> GetBindings();

        void ClearBindings();
    }
}
