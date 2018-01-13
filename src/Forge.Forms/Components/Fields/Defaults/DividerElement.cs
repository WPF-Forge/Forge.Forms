using System.Collections.Generic;
using System.Windows;
using Forge.Forms.Interfaces;
using Forge.Forms.Utils;

namespace Forge.Forms.Components.Fields.Defaults
{
    public class DividerElement : FormElement
    {
        public IValueProvider HasMargin { get; set; }

        protected internal override void Freeze()
        {
            base.Freeze();
            Resources.Add(nameof(HasMargin), HasMargin ?? LiteralValue.True);
        }

        protected internal override IBindingProvider CreateBindingProvider(IResourceContext context,
            IDictionary<string, IValueProvider> formResources)
        {
            return new DividerPresenter(context, Resources, formResources);
        }
    }

    public class DividerPresenter : BindingProvider
    {
        static DividerPresenter()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DividerPresenter),
                new FrameworkPropertyMetadata(typeof(DividerPresenter)));
        }

        public DividerPresenter(IResourceContext context, IDictionary<string, IValueProvider> fieldResources,
            IDictionary<string, IValueProvider> formResources)
            : base(context, fieldResources, formResources, true)
        {
        }
    }
}
