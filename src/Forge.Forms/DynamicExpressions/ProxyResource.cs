using System;
using System.Windows.Data;
using Forge.Forms.FormBuilding;

namespace Forge.Forms.DynamicExpressions
{
    public sealed class ProxyResource : Resource
    {
        public ProxyResource(IProxy proxy, string propertyPath, bool oneTimeBinding, string valueConverter)
            : base(valueConverter)
        {
            Proxy = proxy ?? throw new ArgumentNullException(nameof(proxy));
            PropertyPath = propertyPath;
            OneTimeBinding = oneTimeBinding;
        }

        public IProxy Proxy { get; }

        public string PropertyPath { get; }

        public bool OneTimeBinding { get; }

        public override bool IsDynamic => !OneTimeBinding;

        public override BindingBase ProvideBinding(IResourceContext context)
        {
            var path = FormatPath(PropertyPath);
            return new Binding(nameof(IProxy.Value) + path)
            {
                Source = Proxy,
                Converter = GetValueConverter(context),
                Mode = OneTimeBinding ? BindingMode.OneTime : BindingMode.OneWay
            };
        }

        public override bool Equals(Resource other)
        {
            if (other is ProxyResource resource)
            {
                return ReferenceEquals(Proxy, resource.Proxy)
                       && PropertyPath == resource.PropertyPath
                       && OneTimeBinding == resource.OneTimeBinding
                       && ValueConverter == resource.ValueConverter;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Proxy.GetHashCode();
        }
    }
}
