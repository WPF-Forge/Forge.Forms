using System;
using System.Collections.Generic;
using System.Windows;
using Forge.Forms.DynamicExpressions;

namespace Forge.Forms.FormBuilding.Defaults
{
    public class SliderField : DataFormField
    {
        public SliderField(string key, Type propertyType)
            : base(key, propertyType)
        {
        }

        public IValueProvider Minimum { get; set; }

        public IValueProvider Maximum { get; set; }

        public IValueProvider IsDiscrete { get; set; }

        protected internal override void Freeze()
        {
            base.Freeze();
            // Defaults to WPF 0d-10d if not specified otherwise.
            Resources.Add(nameof(Minimum), Minimum ?? new LiteralValue(0d));
            Resources.Add(nameof(Maximum), Maximum ?? new LiteralValue(10d));
            Resources.Add(nameof(IsDiscrete), IsDiscrete ?? LiteralValue.False);
        }

        protected internal override IBindingProvider CreateBindingProvider(IResourceContext context,
            IDictionary<string, IValueProvider> formResources)
        {
            return new SliderPresenter(context, Resources, formResources);
        }
    }

    public class SliderPresenter : ValueBindingProvider
    {
        static SliderPresenter()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SliderPresenter),
                new FrameworkPropertyMetadata(typeof(SliderPresenter)));
        }

        public SliderPresenter(IResourceContext context,
            IDictionary<string, IValueProvider> fieldResources,
            IDictionary<string, IValueProvider> formResources)
            : base(context, fieldResources, formResources, true)
        {
        }
    }
}
