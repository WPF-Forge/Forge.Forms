using System.Collections.Generic;
using System.Windows.Data;
using Forge.Forms.FormBuilding;

namespace Forge.Forms.DynamicExpressions
{
    public sealed class MultiBinding : Resource
    {
        public MultiBinding(Resource[] resources, bool oneTimeBinding)
            : this(resources, oneTimeBinding, null)
        {
        }

        public MultiBinding(Resource[] resources, bool oneTimeBinding, string valueConverter)
            : base(valueConverter)
        {
            Resources = resources;
            OneTimeBinding = oneTimeBinding;
        }

        public Resource[] Resources { get; }

        public bool OneTimeBinding { get; }

        public override bool IsDynamic => !OneTimeBinding;

        public override BindingBase ProvideBinding(IResourceContext context)
        {
            var multiBinding = new System.Windows.Data.MultiBinding();
            multiBinding.Converter = GetMultiValueConverter(context);
            foreach (var resource in Resources)
            {
                multiBinding.Bindings.Add(resource.ProvideBinding(context));
            }

            multiBinding.Mode = OneTimeBinding ? BindingMode.OneTime : BindingMode.OneWay;
            return multiBinding;
        }

        public override bool Equals(Resource other)
        {
            if (other is MultiBinding resource)
            {
                return Resources == resource.Resources
                       && OneTimeBinding == resource.OneTimeBinding
                       && ValueConverter == resource.ValueConverter;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Resources.GetHashCode() ^ (OneTimeBinding ? 123456789 : 741852963);
        }
    }
}
