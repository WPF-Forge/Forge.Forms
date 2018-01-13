using System;
using System.Windows;
using System.Windows.Controls;
using MaterialDesignThemes.Wpf;

namespace Material.Application.MaterialDesign
{
    public static partial class MaterialDesignHelper
    {
        private sealed class CustomPasswordBoxHintProxy : IHintProxy
        {
            private readonly PasswordBox passwordBox;

            public bool IsEmpty() => passwordBox.SecurePassword?.Length == 0
                && !passwordBox.IsKeyboardFocused;

            public object Content
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public bool IsLoaded => passwordBox.IsLoaded;

            public bool IsVisible => passwordBox.IsVisible;

            public event EventHandler ContentChanged;
            public event EventHandler IsVisibleChanged;
            public event EventHandler Loaded;

            public CustomPasswordBoxHintProxy(PasswordBox passwordBox)
            {
                if (passwordBox == null) throw new ArgumentNullException(nameof(passwordBox));

                this.passwordBox = passwordBox;
                this.passwordBox.PasswordChanged += PasswordBoxPasswordChanged;
                this.passwordBox.IsKeyboardFocusedChanged += PasswordBoxFocusChanged;
                this.passwordBox.Loaded += PasswordBoxLoaded;
                this.passwordBox.IsVisibleChanged += PasswordBoxIsVisibleChanged;
            }

            private void PasswordBoxIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
            {
                IsVisibleChanged?.Invoke(this, EventArgs.Empty);
            }

            private void PasswordBoxLoaded(object sender, RoutedEventArgs e)
            {
                Loaded?.Invoke(this, EventArgs.Empty);
            }

            private void PasswordBoxFocusChanged(object sender, DependencyPropertyChangedEventArgs e)
            {
                ContentChanged?.Invoke(this, EventArgs.Empty);
            }

            private void PasswordBoxPasswordChanged(object sender, RoutedEventArgs e)
            {
                ContentChanged?.Invoke(this, EventArgs.Empty);
            }

            public void Dispose()
            {
                passwordBox.PasswordChanged -= PasswordBoxPasswordChanged;
                passwordBox.IsKeyboardFocusedChanged -= PasswordBoxFocusChanged;
                passwordBox.Loaded -= PasswordBoxLoaded;
                passwordBox.IsVisibleChanged -= PasswordBoxIsVisibleChanged;
            }
        }
    }
}