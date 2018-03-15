using System.Collections.Generic;
using Forge.Forms.Annotations;
using Forge.Forms.DynamicExpressions;

namespace Forge.Forms.FormBuilding
{
    /// <summary>
    /// Represents a form element, which is not necessarily an input field.
    /// </summary>
    public abstract class FormElement
    {
        protected FormElement()
        {
            Resources = new Dictionary<string, IValueProvider>();
        }

        protected internal Position LinePosition { get; set; }

        public IDictionary<string, IValueProvider> Resources { get; set; }

        /// <summary>
        /// Gets or sets the bool resource that determines whether this element will be visible.
        /// </summary>
        public IValueProvider IsVisible { get; set; }

        /// <summary>
        /// Gets or sets the bool resource that determines whether this element will receive initial focus.
        /// </summary>
        public IValueProvider InitialFocus { get; set; }

        protected internal virtual void Freeze()
        {
            Resources.Add(nameof(IsVisible), IsVisible ?? LiteralValue.True);
            Resources.Add(nameof(InitialFocus), InitialFocus ?? LiteralValue.False);
        }

        public FormElement FreezeResources()
        {
            Freeze();
            return this;
        }

        protected internal abstract IBindingProvider CreateBindingProvider(
            IResourceContext context,
            IDictionary<string, IValueProvider> formResources);
    }
}
