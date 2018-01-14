using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Bindables;
using MahApps.Metro.Controls;

namespace Forge.Forms.Components
{
    internal class LoadingButton : Button
    {
        private object oldContent;

        public LoadingButton()
        {
            Width = double.NaN;
            CircularProgressBar.Foreground = Foreground;
        }

        private ProgressBar CircularProgressBar { get; } = new ProgressBar
        {
            IsIndeterminate = true,
            Style = Application.Current.TryFindResource("MaterialDesignCircularProgressBar") as Style ??
                    Application.Current.TryFindResource("ProgressRing") as Style
        };


        [DependencyProperty(OnPropertyChanged = nameof(IsLoadingChanged))]
        public bool IsLoading { get; set; }

        public static void IsLoadingChanged(DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs eventArgs)
        {
            var button = (LoadingButton)dependencyObject;
            button.Invoke(() =>
            {
                if (button.IsLoading)
                {
                    button.oldContent = button.Content;
                    button.Content = button.CircularProgressBar;
                    button.IsEnabled = false;
                }
                else if (button.oldContent != null)
                {
                    button.Content = button.oldContent;
                    button.IsEnabled = true;
                }
                else
                {
                    button.oldContent = button.Content;
                }
            });
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.Property == ForegroundProperty)
            {
                CircularProgressBar.Foreground = (Brush)e.NewValue;
            }
        }
    }
}
