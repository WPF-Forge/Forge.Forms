using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Forge.Forms.Controls.Internal
{
    // Source: https://stackoverflow.com/questions/817610/wpf-and-initial-focus
    internal static class FocusHelper
    {
        public static readonly DependencyProperty InitialFocusProperty =
            DependencyProperty.RegisterAttached(
                "InitialFocus",
                typeof(bool),
                typeof(FocusHelper),
                new PropertyMetadata(false, OnInitialFocusPropertyChanged));

        public static bool GetInitialFocus(Control control)
        {
            return (bool)control.GetValue(InitialFocusProperty);
        }

        public static void SetInitialFocus(Control control, bool value)
        {
            control.SetValue(InitialFocusProperty, value);
        }

        private static void OnInitialFocusPropertyChanged(
            DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if (!(obj is Control control))
            {
                return;
            }

            if (args.NewValue is true)
            {
                control.Loaded += HandleFocus;
            }
            else
            {
                control.Loaded -= HandleFocus;
            }
        }

        private static void HandleFocus(object sender, EventArgs e)
        {
            ((Control)sender).Focus();
        }
    }
}
