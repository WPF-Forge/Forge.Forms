using System.Reflection;
using System.Windows.Data;

namespace Forge.Forms.Collections.Extensions
{
    internal static class PropertyExtensions
    {
        internal static Binding CreateBinding(this PropertyInfo propertyInfo, string path = null)
        {
            return new Binding(string.IsNullOrEmpty(path) ? propertyInfo.Name : path)
            {
                Mode = propertyInfo.CanRead && propertyInfo.CanWrite
                    ? BindingMode.TwoWay
                    : BindingMode.Default,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };
        }
    }
}