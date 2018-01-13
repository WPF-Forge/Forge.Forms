using System.Collections.Generic;
using System.Windows;
using Forge.Forms.Interfaces;

namespace Forge.Forms.Components.Fields.Defaults
{
    public class HeadingElement : ContentElement
    {
        protected internal override IBindingProvider CreateBindingProvider(IResourceContext context,
            IDictionary<string, IValueProvider> formResources)
        {
            return new HeadingPresenter(context, Resources, formResources);
        }
    }

    public class HeadingPresenter : BindingProvider
    {
        static HeadingPresenter()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HeadingPresenter),
                new FrameworkPropertyMetadata(typeof(HeadingPresenter)));
        }

        public HeadingPresenter(IResourceContext context, IDictionary<string, IValueProvider> fieldResources,
            IDictionary<string, IValueProvider> formResources)
            : base(context, fieldResources, formResources, true)
        {
        }
    }
}
