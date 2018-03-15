using System.Collections.Generic;
using System.Windows;

namespace Forge.Forms.FormBuilding.Defaults
{
    public class CardElement : FormElement
    {
        protected internal override IBindingProvider CreateBindingProvider(IResourceContext context,
            IDictionary<string, IValueProvider> formResources)
        {
            return new CardPresenter(context, Resources, formResources);
        }
    }

    public class CardPresenter : BindingProvider
    {
        static CardPresenter()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CardPresenter),
                new FrameworkPropertyMetadata(typeof(CardPresenter)));
        }

        public CardPresenter(IResourceContext context, IDictionary<string, IValueProvider> fieldResources,
            IDictionary<string, IValueProvider> formResources)
            : base(context, fieldResources, formResources, true)
        {
        }
    }
}
