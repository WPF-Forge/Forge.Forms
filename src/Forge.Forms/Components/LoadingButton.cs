using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Bindables;
using MahApps.Metro.Controls;

namespace Forge.Forms.Components
{
    public class LoadingButton : Button
    {
        private object _oldContent;

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
            var button = (LoadingButton) dependencyObject;
            button.Invoke(() =>
            {
                if (button.IsLoading)
                {
                    button._oldContent = button.Content;
                    button.Content = button.CircularProgressBar;
                    button.IsEnabled = false;
                }
                else if (button._oldContent != null)
                {
                    button.Content = button._oldContent;
                    button.IsEnabled = true;
                }
                else
                {
                    button._oldContent = button.Content;
                }
            });
        }

        public LoadingButton()
        {
            Width = double.NaN;
            CircularProgressBar.Foreground = Foreground;
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.Property == ForegroundProperty)
                CircularProgressBar.Foreground = (Brush) e.NewValue;
        }
    }
}