using System;
using System.Collections.Generic;
using System.Windows;
using Forge.Forms.FormBuilding;

namespace Forge.Forms.Collections.Fields
{
    public class CrudField : DataFormField
    {
        public CrudField(string key, Type propertyType)
            : base(key, propertyType)
        {
        }

        protected override IBindingProvider CreateBindingProvider(IResourceContext context, IDictionary<string, IValueProvider> formResources)
        {
            return new CrudPresenter(context, Resources, formResources);
        }
    }

    public class CrudPresenter : ValueBindingProvider
    {
        static CrudPresenter()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CrudPresenter),
                new FrameworkPropertyMetadata(typeof(CrudPresenter)));
        }

        public CrudPresenter(IResourceContext context,
            IDictionary<string, IValueProvider> fieldResources,
            IDictionary<string, IValueProvider> formResources)
            : base(context, fieldResources, formResources, true)
        {
        }
    }
}
