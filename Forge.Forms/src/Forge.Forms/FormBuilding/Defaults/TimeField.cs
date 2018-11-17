using System;
using System.Collections.Generic;
using System.Windows;
using Forge.Forms.DynamicExpressions;

namespace Forge.Forms.FormBuilding.Defaults
{
    public class TimeField : DataFormField
    {
        public TimeField(string key) : base(key, typeof(DateTime))
        {
        }

        public IValueProvider Is24Hours { get; set; }

        protected internal override void Freeze()
        {
            base.Freeze();
            Resources.Add(nameof(Is24Hours), Is24Hours ?? LiteralValue.False);
        }

        protected internal override IBindingProvider CreateBindingProvider(IResourceContext context,
            IDictionary<string, IValueProvider> formResources)
        {
            return new TimePresenter(context, Resources, formResources);
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
