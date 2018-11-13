using System.Collections.Generic;
using System.Windows;

namespace Forge.Forms.FormBuilding.Defaults
{
    public class ErrorTextElement : ContentElement
    {
        protected internal override IBindingProvider CreateBindingProvider(IResourceContext context,
            IDictionary<string, IValueProvider> formResources)
        {
            return new ErrorTextPresenter(context, Resources, formResources);
        }
    }

    public class ErrorTextPresenter : BindingProvider
    {
        static ErrorTextPresenter()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ErrorTextPresenter),
                new FrameworkPropertyMetadata(typeof(ErrorTextPresenter)));
        }

        public ErrorTextPresenter(IResourceContext context, IDictionary<string, IValueProvider> fieldResources,
            IDictionary<string, IValueProvider> formResources)
            : base(context, fieldResources, formResources, true)
        {
        }
    }
}
