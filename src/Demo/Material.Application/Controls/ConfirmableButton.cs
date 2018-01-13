using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Material.Application.Controls
{
    public class ConfirmableButton : Button
    {
        public static readonly DependencyProperty ConfirmationFunctionProperty =
            DependencyProperty.Register("ConfirmationFunction",
                typeof(Func<ConfirmableButton, Task<bool>>),
                typeof(ConfirmableButton),
                new FrameworkPropertyMetadata(null));

        public Func<ConfirmableButton, Task<bool>> ConfirmationFunction
        {
            get { return (Func<ConfirmableButton, Task<bool>>)GetValue(ConfirmationFunctionProperty); }
            set { SetValue(ConfirmationFunctionProperty, value); }
        }

        protected override async void OnClick()
        {
            var confirmed = true;
            var confirmationFunction = ConfirmationFunction;
            if (confirmationFunction != null)
            {
                try
                {
                    confirmed = await confirmationFunction(this);
                }
                catch
                {
                    // ignored
                }
            }

            if (confirmed)
            {
                base.OnClick();
            }
        }
    }
}
