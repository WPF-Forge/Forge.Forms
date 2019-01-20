using System;
using System.Collections.Generic;
using System.Windows;

namespace Forge.Forms.FormBuilding.Defaults
{
    public class DirectContentField : DataFormField
    {
        public DirectContentField(string key, Type propertyType)
            : base(key, propertyType)
        {
        }

        protected internal override IBindingProvider CreateBindingProvider(IResourceContext context, IDictionary<string, IValueProvider> formResources)
        {
            return new DirectContentPresenter(context, Resources, formResources);
        }
    }

    public class DirectContentPresenter : ValueBindingProvider
    {
        static DirectContentPresenter()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DirectContentPresenter),
                new FrameworkPropertyMetadata(typeof(DirectContentPresenter)));
        }

        public DirectContentPresenter(IResourceContext context,
            IDictionary<string, IValueProvider> fieldResources,
            IDictionary<string, IValueProvider> formResources)
            : base(context, fieldResources, formResources, true)
        {
        }
    }
}