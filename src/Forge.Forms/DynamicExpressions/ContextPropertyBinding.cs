using System.Windows.Data;
using Forge.Forms.FormBuilding;

namespace Forge.Forms.DynamicExpressions
{
    public sealed class ContextPropertyBinding : Resource
    {
        public ContextPropertyBinding(string propertyPath, bool oneTimeBinding)
            : this(propertyPath, oneTimeBinding, null)
        {
        }

        public ContextPropertyBinding(string propertyPath, bool oneTimeBinding, string valueConverter)
            : base(valueConverter)
        {
            PropertyPath = propertyPath;
            OneTimeBinding = oneTimeBinding;
        }

        public string PropertyPath { get; }

        public bool OneTimeBinding { get; }

        public override bool IsDynamic => !OneTimeBinding;

        public override BindingBase ProvideBinding(IResourceContext context)
        {
            var binding = context.CreateContextBinding(PropertyPath);
            binding.Converter = GetValueConverter(context);
            binding.Mode = OneTimeBinding ? BindingMode.OneTime : BindingMode.OneWay;
            return binding;
        }

        public override bool Equals(Resource other)
        {
            if (other is ContextPropertyBinding resource)
            {
                return PropertyPath == resource.PropertyPath && OneTimeBinding == resource.OneTimeBinding &&
                       ValueConverter == resource.ValueConverter;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return PropertyPath.GetHashCode() ^ (OneTimeBinding ? 741852963 : 123456789);
        }
    }
}
