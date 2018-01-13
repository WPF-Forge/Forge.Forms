using System;
using System.Windows;
using System.Windows.Controls;
using MaterialDesignThemes.Wpf;

namespace Material.Application.MaterialDesign
{
    public static partial class MaterialDesignHelper
    {
        private sealed class CustomTextBoxHintProxy : IHintProxy
        {
            private readonly TextBox textBox;

            public object Content
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public bool IsLoaded => textBox.IsLoaded;

            public bool IsVisible => textBox.IsVisible;

            public bool IsEmpty() => string.IsNullOrWhiteSpace(textBox.Text)
                && !textBox.IsKeyboardFocused;

            public event EventHandler ContentChanged;
            public event EventHandler IsVisibleChanged;
            public event EventHandler Loaded;

            public CustomTextBoxHintProxy(TextBox textBox)
            {
                if (textBox == null) throw new ArgumentNullException(nameof(textBox));

                this.textBox = textBox;
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
    }
}