using System.Windows.Data;
using Forge.Forms.FormBuilding;

namespace Forge.Forms.DynamicExpressions
{
    public sealed class MultiPropertyBinding : Resource
    {
        public MultiPropertyBinding(string[] propertyPaths, bool oneTimeBinding)
            : this(propertyPaths, oneTimeBinding, null)
        {
        }

        public MultiPropertyBinding(string[] propertyPaths, bool oneTimeBinding, string valueConverter)
            : base(valueConverter)
        {
            PropertyPaths = propertyPaths;
            OneTimeBinding = oneTimeBinding;
        }

        public string[] PropertyPaths { get; }

        public bool OneTimeBinding { get; }

        public override bool IsDynamic => !OneTimeBinding;

        public override BindingBase ProvideBinding(IResourceContext context)
        {
            var multiBinding = new MultiBinding();
            multiBinding.Converter = GetMultiValueConverter(context);
            foreach (var propertyPath in PropertyPaths)
            {
                multiBinding.Bindings.Add(context.CreateModelBinding(propertyPath));
            }

            multiBinding.Mode = OneTimeBinding ? BindingMode.OneTime : BindingMode.OneWay;
            return multiBinding;
        }

        public override bool Equals(Resource other)
        {
            if (other is MultiPropertyBinding resource)
            {
                return PropertyPaths == resource.PropertyPaths
                       && OneTimeBinding == resource.OneTimeBinding
                       && ValueConverter == resource.ValueConverter;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return PropertyPaths.GetHashCode() ^ (OneTimeBinding ? 123456789 : 741852963);
        }
    }
}
