using System.Collections.Generic;
using System.Windows;
using Forge.Forms.Interfaces;

namespace Forge.Forms.Components.Fields.Defaults
{
    public class CardElement : FormElement
    {
        protected internal override IBindingProvider CreateBindingProvider(IResourceContext context, IDictionary<string, IValueProvider> formResources)
        {
            return new CardPresenter(context, Resources, formResources);
        }
    }

    public class CardPresenter : BindingProvider
    {
        static CardPresenter()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CardPresenter), new FrameworkPropertyMetadata(typeof(CardPresenter)));
        }

        public CardPresenter(IResourceContext context, IDictionary<string, IValueProvider> fieldResources, IDictionary<string, IValueProvider> formResources)
            : base(context, fieldResources, formResources, true)
        {
        }
    }
}
