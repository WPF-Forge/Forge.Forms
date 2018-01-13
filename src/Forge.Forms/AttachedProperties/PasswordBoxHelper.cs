using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Forge.Forms.AttachedProperties
{
    //Source: https://antonymale.co.uk/binding-to-a-passwordbox-password-in-wpf.html
    public static class PasswordBoxHelper
    {
        private static readonly DependencyProperty PasswordInitializedProperty =
            DependencyProperty.RegisterAttached("PasswordInitialized", typeof(bool), typeof(PasswordBoxHelper),
                new PropertyMetadata(false));

        private static readonly DependencyProperty SettingPasswordProperty =
            DependencyProperty.RegisterAttached("SettingPassword", typeof(bool), typeof(PasswordBoxHelper),
                new PropertyMetadata(false));

        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.RegisterAttached("Password", typeof(string), typeof(PasswordBoxHelper),
                new FrameworkPropertyMetadata(Guid.NewGuid().ToString(), HandleBoundPasswordChanged)
                {
                    BindsTwoWayByDefault = true,
                    DefaultUpdateSourceTrigger = UpdateSourceTrigger.LostFocus // Match the default on Binding
                });

        public static string GetPassword(DependencyObject obj)
        {
            return (string)obj.GetValue(PasswordProperty);
        }

        public static void SetPassword(DependencyObject obj, string value)
        {
            obj.SetValue(PasswordProperty, value);
        }

        private static void HandleBoundPasswordChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                if (!(dp is PasswordBox passwordBox))
                {
                    return;
                }

                // If we're being called because we set the value of the property we're bound to (from inside 
                // HandlePasswordChanged, then do nothing - we already have the latest value).
                if ((bool)passwordBox.GetValue(SettingPasswordProperty))
                {
                    return;
                }

                // If this is the initial set (see the comment on PasswordProperty), set ourselves up
                if (!(bool)passwordBox.GetValue(PasswordInitializedProperty))
                {
                    passwordBox.SetValue(PasswordInitializedProperty, true);
                    passwordBox.PasswordChanged += HandlePasswordChanged;
                }

                passwordBox.Password = e.NewValue as string ?? throw new InvalidOperationException();
            }
            catch
            {
                // ignored
            }
        }

        private static void HandlePasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = (PasswordBox)sender;
            passwordBox.SetValue(SettingPasswordProperty, true);
            SetPassword(passwordBox, passwordBox.Password);
            passwordBox.SetValue(SettingPasswordProperty, false);
        }
    }
}
