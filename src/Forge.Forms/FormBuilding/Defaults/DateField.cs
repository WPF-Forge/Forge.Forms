using System;
using System.Collections.Generic;
using System.Windows;

namespace Forge.Forms.FormBuilding.Defaults
{
    public class DateField : DataFormField
    {
        /// <summary>
        /// </summary>
        /// <param name="key"></param>
        public DateField(string key) : base(key, typeof(DateTime))
        {
        }

        protected internal override IBindingProvider CreateBindingProvider(IResourceContext context,
            IDictionary<string, IValueProvider> formResources)
        {
            var datePresenter = new DatePresenter(context, Resources, formResources);
            return datePresenter;
        }
    }

    public class DatePresenter : ValueBindingProvider
    {
        static DatePresenter()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DatePresenter),
                new FrameworkPropertyMetadata(typeof(DatePresenter)));
        }

        public DatePresenter(IResourceContext context,
            IDictionary<string, IValueProvider> fieldResources,
            IDictionary<string, IValueProvider> formResources)
            : base(context, fieldResources, formResources, true)
        {
        }
    }
}
