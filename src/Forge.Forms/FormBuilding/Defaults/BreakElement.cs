using System.Collections.Generic;
using System.Windows;
using Forge.Forms.DynamicExpressions;

namespace Forge.Forms.FormBuilding.Defaults
{
    public class BreakElement : FormElement
    {
        public IValueProvider Height { get; set; }

        protected internal override void Freeze()
        {
            base.Freeze();
            Resources.Add(nameof(Height), Height ?? new LiteralValue(8d));
        }

        protected internal override IBindingProvider CreateBindingProvider(IResourceContext context,
            IDictionary<string, IValueProvider> formResources)
        {
            return new BreakPresenter(context, Resources, formResources);
        }
    }

    public class BreakPresenter : BindingProvider
    {
        static BreakPresenter()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BreakPresenter),
                new FrameworkPropertyMetadata(typeof(BreakPresenter)));
        }

        public BreakPresenter(IResourceContext context, IDictionary<string, IValueProvider> fieldResources,
            IDictionary<string, IValueProvider> formResources)
            : base(context, fieldResources, formResources, true)
        {
        }
    }
}
