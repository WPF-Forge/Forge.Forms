using System;
using System.Windows;
using Forge.Forms.Interfaces;

namespace Forge.Forms.Utils
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

        private static void PropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            ((BoolProxy)dependencyObject).ValueChanged?.Invoke();
        }

        public bool Value
        {
            get => (bool)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        object IProxy.Value => Value;

        public Action ValueChanged { get; set; }

        protected override Freezable CreateInstanceCore()
        {
            return new BoolProxy();
        }
    }
}