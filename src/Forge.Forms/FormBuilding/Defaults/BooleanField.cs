using System.Collections.Generic;
using System.Windows;

namespace Forge.Forms.FormBuilding.Defaults
{
    public class BooleanField : DataFormField
    {
        public BooleanField(string key) : base(key, typeof(bool))
        {
        }

        public bool IsSwitch { get; set; }

        protected internal override IBindingProvider CreateBindingProvider(IResourceContext context,
            IDictionary<string, IValueProvider> formResources)
        {
            if (IsSwitch)
            {
                return new SwitchPresenter(context, Resources, formResources);
            }

            return new CheckBoxPresenter(context, Resources, formResources);
        }
    }

    public class CheckBoxPresenter : ValueBindingProvider
    {
        static CheckBoxPresenter()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CheckBoxPresenter),
                new FrameworkPropertyMetadata(typeof(CheckBoxPresenter)));
        }

        public CheckBoxPresenter(IResourceContext context,
            IDictionary<string, IValueProvider> fieldResources,
            IDictionary<string, IValueProvider> formResources)
            : base(context, fieldResources, formResources, true)
        {
        }
    }

    public class SwitchPresenter : ValueBindingProvider
    {
        static SwitchPresenter()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SwitchPresenter),
                new FrameworkPropertyMetadata(typeof(SwitchPresenter)));
        }

        public SwitchPresenter(IResourceContext context,
            IDictionary<string, IValueProvider> fieldResources,
            IDictionary<string, IValueProvider> formResources)
            : base(context, fieldResources, formResources, true)
        {
        }
    }
}
