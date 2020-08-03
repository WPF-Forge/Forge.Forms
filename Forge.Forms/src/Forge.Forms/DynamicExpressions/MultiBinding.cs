using System.Collections.Generic;
using System.Linq;
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
                if (Resources.Length == resource.Resources.Length
                    && OneTimeBinding == resource.OneTimeBinding
                    && ValueConverter == resource.ValueConverter)
                {
                    for (int i = 0; i < Resources.Length; i++)
                    {
                        if (Resources[i].Equals(resource.Resources[i]) == false)
                        {
                            return false;
                        }
                    }

                    return true;
                }
            }

            return false;
        }

        public override int GetHashCode()
        {
            var hashCode = (OneTimeBinding ? 123456789 : 741852963);
            foreach (var resource in Resources)
            {
                hashCode = resource.GetHashCode() ^ hashCode;
            }

            return hashCode;
        }
    }
}