using System.Windows.Data;
using Forge.Forms.FormBuilding;

namespace Forge.Forms.DynamicExpressions
{
    public class CoercedValueProvider<T> : IValueProvider
    {
        private readonly object defaultValue;
        private readonly IValueProvider innerProvider;

        public CoercedValueProvider(IValueProvider innerProvider, object defaultValue)
        {
            this.innerProvider = innerProvider;
            this.defaultValue = defaultValue;
        }

        public BindingBase ProvideBinding(IResourceContext context)
        {
            var binding = innerProvider.ProvideBinding(context);
            binding.FallbackValue = defaultValue;
            return binding;
        }

        public object ProvideValue(IResourceContext context)
        {
            var value = innerProvider.ProvideValue(context);
            if (value is BindingBase binding)
            {
                binding.FallbackValue = defaultValue;
                return binding;
            }

            if (value is T)
            {
                return value;
            }

            return defaultValue;
        }
    }
}
