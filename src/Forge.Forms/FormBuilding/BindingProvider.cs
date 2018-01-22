using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Data;
using Forge.Forms.DynamicExpressions;

namespace Forge.Forms.FormBuilding
{
    /// <summary>
    /// Default implementation of <see cref="IBindingProvider" />.
    /// </summary>
    public abstract class BindingProvider : Control, IBindingProvider
    {
        private readonly Dictionary<string, BindingProxy> proxyCache;

        protected BindingProvider(IResourceContext context,
            IDictionary<string, IValueProvider> fieldResources,
            IDictionary<string, IValueProvider> formResources,
            bool throwOnNotFound)
        {
            IsTabStop = false;
            DataContext = this;
            Context = context;
            FieldResources = fieldResources;
            FormResources = formResources;
            ThrowOnNotFound = throwOnNotFound;
            proxyCache = new Dictionary<string, BindingProxy>();
        }

        /// <summary>
        /// Gets the context associated with the form control.
        /// </summary>
        public IResourceContext Context { get; }

        /// <summary>
        /// Gets the field resources identified by name.
        /// </summary>
        public IDictionary<string, IValueProvider> FieldResources { get; }

        /// <summary>
        /// Gets the form resources identified by name.
        /// </summary>
        public IDictionary<string, IValueProvider> FormResources { get; }

        /// <summary>
        /// Gets whether this object will throw when a resource is not found.
        /// </summary>
        public bool ThrowOnNotFound { get; }

        /// <summary>
        /// Returns a <see cref="BindingProxy" /> bound to the value returned by <see cref="ProvideValue" />.
        /// </summary>
        /// <param name="name">Resource name. This is not the object property name.</param>
        /// <returns></returns>
        public BindingProxy this[string name]
        {
            get
            {
                if (proxyCache.TryGetValue(name, out var proxy))
                {
                    return proxy;
                }

                proxy = new BindingProxy();
                proxyCache[name] = proxy;
                var value = ProvideValue(name);
                if (value is BindingBase binding)
                {
                    BindingOperations.SetBinding(proxy, BindingProxy.ValueProperty, binding);
                }
                else
                {
                    proxy.Value = value;
                }

                return proxy;
            }
        }

        /// <summary>
        /// Resolves the value for the specified resource.
        /// The result may be a <see cref="BindingBase" /> or a literal value.
        /// </summary>
        /// <param name="name">Resource name. This is not the object property name.</param>
        public virtual object ProvideValue(string name)
        {
            if (FieldResources.TryGetValue(name, out var resource))
            {
                return resource.ProvideValue(Context);
            }

            if (FormResources.TryGetValue(name, out resource))
            {
                return resource.ProvideValue(Context);
            }

            if (ThrowOnNotFound)
            {
                throw new InvalidOperationException($"Resource {name} not found.");
            }

            return null;
        }

        public virtual void BindingCreated(BindingExpressionBase expression, string resource)
        {
        }
    }
}
