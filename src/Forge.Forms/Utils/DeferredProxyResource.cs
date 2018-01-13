using System;
using System.Windows.Data;
using Forge.Forms.Interfaces;

namespace Forge.Forms.Utils
{
    public sealed class DeferredProxyResource : Resource
    {
        public DeferredProxyResource(Func<IResourceContext, IProxy> proxyProvider, string propertyPath,
            bool oneTimeBinding, string valueConverter)
            : base(valueConverter)
        {
            ProxyProvider = proxyProvider ?? throw new ArgumentNullException(nameof(proxyProvider));
            PropertyPath = propertyPath;
            OneTimeBinding = oneTimeBinding;
        }

        public Func<IResourceContext, IProxy> ProxyProvider { get; }

        public string PropertyPath { get; }

        public bool OneTimeBinding { get; }

        public override bool IsDynamic => !OneTimeBinding;

        public override BindingBase ProvideBinding(IResourceContext context)
        {
            var path = FormatPath(PropertyPath);
            return new Binding(nameof(IProxy.Value) + path)
            {
                Source = ProxyProvider(context) ??
                         throw new InvalidOperationException("A binding proxy could not be resolved."),
                Converter = GetValueConverter(context),
                Mode = OneTimeBinding ? BindingMode.OneTime : BindingMode.OneWay
            };
        }

        public override bool Equals(Resource other)
        {
            if (other is DeferredProxyResource resource)
            {
                return ReferenceEquals(ProxyProvider, resource.ProxyProvider)
                       && PropertyPath == resource.PropertyPath
                       && OneTimeBinding == resource.OneTimeBinding
                       && ValueConverter == resource.ValueConverter;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return ProxyProvider.GetHashCode();
        }
    }
}
