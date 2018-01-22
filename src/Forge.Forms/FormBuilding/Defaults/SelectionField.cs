using System;
using System.Collections.Generic;
using System.Windows;
using Forge.Forms.DynamicExpressions;

namespace Forge.Forms.FormBuilding.Defaults
{
    public class SelectionField : DataFormField
    {
        public SelectionField(string key, Type propertyType) : base(key, propertyType)
        {
        }

        public IValueProvider ItemsSource { get; set; }

        public IValueProvider ItemStringFormat { get; set; }

        public IValueProvider DisplayPath { get; set; }

        public IValueProvider ValuePath { get; set; }

        public IValueProvider SelectionType { get; set; }

        protected internal override void Freeze()
        {
            base.Freeze();
            Resources.Add(nameof(ItemsSource), ItemsSource ?? LiteralValue.Null);
            Resources.Add(nameof(ItemStringFormat), ItemStringFormat ?? LiteralValue.Null);
            Resources.Add(nameof(DisplayPath), DisplayPath ?? LiteralValue.Null);
            Resources.Add(nameof(ValuePath), ValuePath ?? LiteralValue.Null);
            Resources.Add(nameof(SelectionType), SelectionType ?? new LiteralValue(Annotations.SelectionType.ComboBox));
        }

        protected internal override IBindingProvider CreateBindingProvider(IResourceContext context,
            IDictionary<string, IValueProvider> formResources)
        {
            return new SelectionPresenter(context, Resources, formResources);
        }
    }

    public class SelectionPresenter : ValueBindingProvider
    {
        static SelectionPresenter()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SelectionPresenter),
                new FrameworkPropertyMetadata(typeof(SelectionPresenter)));
        }

        public SelectionPresenter(IResourceContext context,
            IDictionary<string, IValueProvider> fieldResources,
            IDictionary<string, IValueProvider> formResources)
            : base(context, fieldResources, formResources, true)
        {
        }
    }
}
