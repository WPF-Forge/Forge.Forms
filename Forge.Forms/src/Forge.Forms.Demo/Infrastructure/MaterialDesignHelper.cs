using System;
using System.Collections;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using MaterialDesignThemes.Wpf;

namespace Forge.Forms.Demo.Infrastructure
{
    public static class MaterialDesignHelper
    {
        public static void ReplaceDefaultHintProxies()
        {
            var list = (IList)typeof(HintProxyFabric).GetField("Builders", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
            list.RemoveAt(2);
            list.RemoveAt(1);
            HintProxyFabric.RegisterBuilder(c => c is TextBox, c => new CustomTextBoxHintProxy((TextBox)c));
            HintProxyFabric.RegisterBuilder(c => c is PasswordBox, c => new CustomPasswordBoxHintProxy((PasswordBox)c));
        }

        private sealed class CustomTextBoxHintProxy : IHintProxy
        {
            private readonly TextBox textBox;

            public object Content => throw new NotImplementedException();

            public bool IsLoaded => textBox.IsLoaded;

            public bool IsVisible => textBox.IsVisible;

            public bool IsEmpty() => string.IsNullOrWhiteSpace(textBox.Text)
                && !textBox.IsKeyboardFocused;

            public event EventHandler ContentChanged;
            public event EventHandler IsVisibleChanged;
            public event EventHandler Loaded;

            public CustomTextBoxHintProxy(TextBox textBox)
            {
                this.textBox = textBox ?? throw new ArgumentNullException(nameof(textBox));
                this.textBox.TextChanged += TextBoxTextChanged;
                this.textBox.IsKeyboardFocusedChanged += TextBoxFocusChanged;
                this.textBox.Loaded += TextBoxLoaded;
                this.textBox.IsVisibleChanged += TextBoxIsVisibleChanged;
            }

            private void TextBoxIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
            {
                IsVisibleChanged?.Invoke(sender, EventArgs.Empty);
            }

            private void TextBoxLoaded(object sender, RoutedEventArgs e)
            {
                Loaded?.Invoke(sender, EventArgs.Empty);
            }

            private void TextBoxFocusChanged(object sender, DependencyPropertyChangedEventArgs e)
            {
                ContentChanged?.Invoke(sender, EventArgs.Empty);
            }

            private void TextBoxTextChanged(object sender, TextChangedEventArgs e)
            {
                ContentChanged?.Invoke(sender, EventArgs.Empty);
            }

            public void Dispose()
            {
                textBox.TextChanged -= TextBoxTextChanged;
                textBox.IsKeyboardFocusedChanged -= TextBoxFocusChanged;
                textBox.Loaded -= TextBoxLoaded;
                textBox.IsVisibleChanged -= TextBoxIsVisibleChanged;
            }
        }
        private sealed class CustomPasswordBoxHintProxy : IHintProxy
        {
            private readonly PasswordBox passwordBox;

            public bool IsEmpty() => passwordBox.SecurePassword?.Length == 0
                && !passwordBox.IsKeyboardFocused;

            public object Content => throw new NotImplementedException();

            public bool IsLoaded => passwordBox.IsLoaded;

            public bool IsVisible => passwordBox.IsVisible;

            public event EventHandler ContentChanged;
            public event EventHandler IsVisibleChanged;
            public event EventHandler Loaded;

            public CustomPasswordBoxHintProxy(PasswordBox passwordBox)
            {
                this.passwordBox = passwordBox ?? throw new ArgumentNullException(nameof(passwordBox));
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
