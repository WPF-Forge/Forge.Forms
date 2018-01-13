using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace Material.Application.Helpers
{
    internal static class AttachedProperties
    {
        public static readonly DependencyProperty InputBindingsSourceProperty =
            DependencyProperty.RegisterAttached
                (
                    "InputBindingsSource",
                    typeof(IEnumerable),
                    typeof(AttachedProperties),
                    new UIPropertyMetadata(null, InputBindingsSource_Changed)
                );

        public static IEnumerable GetInputBindingsSource(DependencyObject obj)
        {
            return (IEnumerable)obj.GetValue(InputBindingsSourceProperty);
        }

        public static void SetInputBindingsSource(DependencyObject obj, IEnumerable value)
        {
            obj.SetValue(InputBindingsSourceProperty, value);
        }

        private static void InputBindingsSource_Changed(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var uiElement = obj as UIElement;
            if (uiElement == null)
            {
                throw new Exception($"Object of type '{obj.GetType()}' does not support InputBindings");
            }

            uiElement.InputBindings.Clear();
            if (e.NewValue == null)
            {
                return;
            }
            ;

            var bindings = (IEnumerable<InputBinding>)e.NewValue;
            foreach (var binding in bindings)
            {
                uiElement.InputBindings.Add(binding);
            }
        }
    }
}
