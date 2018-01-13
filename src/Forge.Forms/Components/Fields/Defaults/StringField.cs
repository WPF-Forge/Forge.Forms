using System.Collections.Generic;
using System.Windows;
using Forge.Forms.Interfaces;
using Forge.Forms.Utils;

namespace Forge.Forms.Components.Fields.Defaults
{
    public class StringField : DataFormField
    {
        public StringField(string key)
            : base(key, typeof(string))
        {
        }

        public bool IsPassword { get; set; }
        public IValueProvider IsMultiline { get; set; }

        protected internal override void Freeze()
        {
            base.Freeze();
            Resources.Add(nameof(IsMultiline), IsMultiline ?? LiteralValue.False);
        }

        protected internal override IBindingProvider CreateBindingProvider(IResourceContext context,
            IDictionary<string, IValueProvider> formResources)
        {
            return !IsPassword
                ? (IBindingProvider)new StringPresenter(context, Resources, formResources)
                : new PasswordPresenter(context, Resources, formResources);
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
}
