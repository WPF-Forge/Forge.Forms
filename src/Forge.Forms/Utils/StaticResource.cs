using System;
using System.Windows.Data;
using Forge.Forms.Interfaces;

namespace Forge.Forms.Utils
{
    public sealed class StaticResource : Resource
    {
        public StaticResource(string resourceKey)
            : this(resourceKey, null)
        {
        }

        public StaticResource(string resourceKey, string valueConverter)
            : base(valueConverter)
        {
            ResourceKey = resourceKey ?? throw new ArgumentNullException(nameof(resourceKey));
        }

        public string ResourceKey { get; }

        public override bool IsDynamic => false;

        public override BindingBase ProvideBinding(IResourceContext context)
        {
            return new Binding
            {
                Source = context.FindResource(ResourceKey),
                Converter = GetValueConverter(context),
                Mode = BindingMode.OneTime
            };
        }

        public override bool Equals(Resource other)
        {
            if (other is StaticResource resource)
            {
                return ResourceKey == resource.ResourceKey
                       && ValueConverter == resource.ValueConverter;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return ResourceKey.GetHashCode();
        }
    }
}
