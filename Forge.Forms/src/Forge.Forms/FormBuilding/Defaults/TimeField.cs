using System;
using System.Collections.Generic;
using System.Windows;

namespace Forge.Forms.FormBuilding.Defaults
{
    public class TimeField : DataFormField
    {
        /// <summary>
        /// </summary>
        /// <param name="key"></param>
        public TimeField(string key) : base(key, typeof(DateTime))
        {
        }

        protected internal override IBindingProvider CreateBindingProvider(IResourceContext context,
            IDictionary<string, IValueProvider> formResources)
        {
            var datePresenter = new TimePresenter(context, Resources, formResources);
            return datePresenter;
        }
    }

    public class TimePresenter : ValueBindingProvider
    {
        static TimePresenter()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TimePresenter),
                new FrameworkPropertyMetadata(typeof(TimePresenter)));
        }

        public TimePresenter(IResourceContext context,
            IDictionary<string, IValueProvider> fieldResources,
            IDictionary<string, IValueProvider> formResources)
            : base(context, fieldResources, formResources, true)
        {
        }
    }
}
