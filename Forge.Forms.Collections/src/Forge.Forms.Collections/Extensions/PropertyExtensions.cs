using Forge.Forms.Annotations;
using System.Globalization;
using System.Reflection;
using System.Windows.Data;

namespace Forge.Forms.Collections.Extensions
{
    internal static class PropertyExtensions
    {
        internal static Binding CreateBinding(this PropertyInfo propertyInfo, string path = null)
        {
            Binding binding = new Binding(string.IsNullOrEmpty(path) ? propertyInfo.Name : path)
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

                if (!string.IsNullOrEmpty(bindingAttribute.ConverterCulture))
                    binding.ConverterCulture = new CultureInfo(bindingAttribute.ConverterCulture);

                binding.Delay = bindingAttribute.Delay;
                binding.ValidatesOnDataErrors = bindingAttribute.ValidatesOnDataErrors;
                binding.ValidatesOnNotifyDataErrors = bindingAttribute.ValidatesOnNotifyDataErrors;
                binding.ValidatesOnExceptions = bindingAttribute.ValidatesOnExceptions;
            }

            return binding;
        }
    }
}