using System;
using System.Collections.Generic;
using System.Windows;
using Forge.Forms.DynamicExpressions;
using Forge.Forms.FormBuilding;

namespace Forge.Forms.Collections
{
    public class GridField : DataFormField
    {
        public IValueProvider AddItemCommand { get; set; }

        public IValueProvider EditItemCommand { get; set; }

        public IValueProvider RemoveItemCommand { get; set; }

        public GridField(string key, Type propertyType)
            : base(key, propertyType)
        {
        }

        // Other props will go here

        protected override void Freeze()
        {
            base.Freeze();
            Resources.Add(nameof(AddItemCommand), AddItemCommand ?? LiteralValue.Null);
        }

        protected override IBindingProvider CreateBindingProvider(IResourceContext context,
            IDictionary<string, IValueProvider> formResources)
        {
            return new GridPresenter(context, Resources, formResources);
        }
    }

    public class GridPresenter : ValueBindingProvider
    {
        static GridPresenter()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GridPresenter),
                new FrameworkPropertyMetadata(typeof(GridPresenter)));
        }

        public GridPresenter(IResourceContext context,
            IDictionary<string, IValueProvider> fieldResources,
            IDictionary<string, IValueProvider> formResources)
            : base(context, fieldResources, formResources, true)
        {
        }
    }
}
