using System;
using System.Windows;

namespace Forge.Forms.DynamicExpressions
{
    /// <summary>
    /// Encapsulates a string bound to a resource.
    /// </summary>
    public class BoolProxy : Freezable, IBoolProxy, IProxy
    {
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                nameof(Value),
                typeof(bool),
                typeof(BoolProxy),
                new UIPropertyMetadata(PropertyChangedCallback));

        public bool Value
        {
            get => (bool)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        object IProxy.Value => Value;

        public Action ValueChanged { get; set; }

        private static void PropertyChangedCallback(DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            ((BoolProxy)dependencyObject).ValueChanged?.Invoke();
        }

        protected override Freezable CreateInstanceCore()
        {
            return new BoolProxy();
        }
    }
}
