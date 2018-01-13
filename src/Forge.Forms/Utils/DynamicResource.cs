using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using Forge.Forms.Interfaces;

namespace Forge.Forms.Utils
{
    public sealed class DynamicResource : Resource
    {
        public DynamicResource(string resourceKey)
            : this(resourceKey, null)
        {
        }

        public DynamicResource(string resourceKey, string valueConverter)
            : base(valueConverter)
        {
            ResourceKey = resourceKey ?? throw new ArgumentNullException(nameof(resourceKey));
        }

        public string ResourceKey { get; }

        public override bool IsDynamic => true;

        public override BindingBase ProvideBinding(IResourceContext context)
        {
            var key = new DynamicResourceKey(ResourceKey);
            if (context.TryFindResource(key) is BindingProxy proxy)
            {
                return CreateBinding(context, proxy);
            }

            proxy = new BindingProxy();
            context.AddResource(key, proxy);
            proxy.Value = new DynamicResourceExtension(ResourceKey).ProvideValue(new Target(context));
            return CreateBinding(context, proxy);
        }

        public override bool Equals(Resource other)
        {
            if (other is DynamicResource resource)
            {
                return ResourceKey == resource.ResourceKey && ValueConverter == resource.ValueConverter;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return ResourceKey.GetHashCode();
        }

        private Binding CreateBinding(IResourceContext context, BindingProxy proxy)
        {
            return new Binding
            {
                Source = proxy,
                Path = new PropertyPath(BindingProxy.ValueProperty),
                Converter = GetValueConverter(context),
                Mode = BindingMode.OneWay
            };
        }

        private struct Target : IServiceProvider, IProvideValueTarget
        {
            public Target(object targetObject)
            {
                TargetObject = targetObject;
            }

            public object GetService(Type serviceType)
            {
                if (serviceType == typeof(IProvideValueTarget))
                {
                    return this;
                }

                return null;
            }

            public object TargetObject { get; }

            public object TargetProperty => null;
        }
    }

    internal struct DynamicResourceKey : IEquatable<DynamicResourceKey>
    {
        public DynamicResourceKey(string key)
        {
            Key = key;
        }

        public string Key { get; }

        public bool Equals(DynamicResourceKey other)
        {
            return Key == other.Key;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is DynamicResourceKey key && Equals(key);
        }

        public override int GetHashCode()
        {
            return Key != null ? Key.GetHashCode() : 0;
        }
    }
}
