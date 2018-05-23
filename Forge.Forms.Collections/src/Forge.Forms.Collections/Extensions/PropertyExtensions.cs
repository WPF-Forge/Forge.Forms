using System.Reflection;
using System.Windows.Data;
using Forge.Forms.Annotations;

namespace Forge.Forms.Collections.Extensions
{
    internal static class PropertyExtensions
    {
        internal static Binding CreateBinding(this PropertyInfo propertyInfo, string path = null)
        {
            var binding = new Binding(string.IsNullOrEmpty(path) ? propertyInfo.Name : path)
            {
                Mode = propertyInfo.CanRead && propertyInfo.CanWrite
                    ? BindingMode.TwoWay
                    : BindingMode.Default,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };

            if (propertyInfo.GetCustomAttribute<BindingAttribute>() is BindingAttribute bindingAttribute)
            {
                binding.UpdateSourceTrigger = bindingAttribute.UpdateSourceTrigger;
                binding.StringFormat = bindingAttribute.StringFormat;
            }

            return binding;
        }
    }
}