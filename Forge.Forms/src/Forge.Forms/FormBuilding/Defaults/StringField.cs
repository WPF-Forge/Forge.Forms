using System.Collections.Generic;
using System.Windows;

namespace Forge.Forms.FormBuilding.Defaults
{
    public class StringField : DataFormField
    {
        public StringField(string key)
            : base(key, typeof(string))
        {
        }

        public bool IsPassword { get; set; }

        public bool IsMultiline { get; set; }

        protected internal override IBindingProvider CreateBindingProvider(IResourceContext context,
            IDictionary<string, IValueProvider> formResources)
        {
            if (IsMultiline)
            {
                return new MultiLineStringPresenter(context, Resources, formResources);
            }

            if (IsPassword)
            {
                return new PasswordPresenter(context, Resources, formResources);
            }

            return new StringPresenter(context, Resources, formResources);
        }
    }

    public class StringPresenter : ValueBindingProvider
    {
        static StringPresenter()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(StringPresenter),
                new FrameworkPropertyMetadata(typeof(StringPresenter)));
        }

        public StringPresenter(IResourceContext context,
            IDictionary<string, IValueProvider> fieldResources,
            IDictionary<string, IValueProvider> formResources)
            : base(context, fieldResources, formResources, true)
        {
        }
    }

    public class PasswordPresenter : ValueBindingProvider
    {
        static PasswordPresenter()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PasswordPresenter),
                new FrameworkPropertyMetadata(typeof(PasswordPresenter)));
        }

        public PasswordPresenter(IResourceContext context,
            IDictionary<string, IValueProvider> fieldResources,
            IDictionary<string, IValueProvider> formResources)
            : base(context, fieldResources, formResources, true)
        {
        }
    }

    public class MultiLineStringPresenter : ValueBindingProvider
    {
        static MultiLineStringPresenter()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MultiLineStringPresenter),
                new FrameworkPropertyMetadata(typeof(MultiLineStringPresenter)));
        }

        public MultiLineStringPresenter(IResourceContext context,
            IDictionary<string, IValueProvider> fieldResources,
            IDictionary<string, IValueProvider> formResources)
            : base(context, fieldResources, formResources, true)
        {
        }
    }
}
