using System.Collections.Generic;
using System.Windows.Data;
using Forge.Forms.Interfaces;

namespace Forge.Forms.Components.Fields
{
    /// <summary>
    /// Single source data binding provider that captures resources with name "Value".
    /// </summary>
    public class ValueBindingProvider : BindingProvider, IDataBindingProvider
    {
        public ValueBindingProvider(IResourceContext context, IDictionary<string, IValueProvider> fieldResources,
            IDictionary<string, IValueProvider> formResources, bool throwOnNotFound)
            : base(context, fieldResources, formResources, throwOnNotFound)
        {
        }

        public BindingExpressionBase CurrentBindingExpression { get; private set; }

        public override void BindingCreated(BindingExpressionBase expression, string resource)
        {
            if (resource == "Value")
            {
                CurrentBindingExpression = expression;
            }
        }

        public IEnumerable<BindingExpressionBase> GetBindings()
        {
            return CurrentBindingExpression != null
                ? new[] { CurrentBindingExpression }
                : new BindingExpressionBase[0];
        }

        public void ClearBindings()
        {
            CurrentBindingExpression = null;
        }
    }
}
