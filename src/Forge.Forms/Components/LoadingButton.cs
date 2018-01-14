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

            try
            {
                CircularProgressBar = new ProgressBar
                {
                    IsIndeterminate = true,
                    Style = Application.Current.TryFindResource("MaterialDesignCircularProgressBar") as Style ??
                            Application.Current.TryFindResource("ProgressRing") as Style,
                    Foreground = Foreground
                };
            }
            catch
            {
                // ignored
            }
        }

        private ProgressBar CircularProgressBar { get; }


        [DependencyProperty(OnPropertyChanged = nameof(IsLoadingChanged))]
        public bool IsLoading { get; set; }

        public static void IsLoadingChanged(DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs eventArgs)
        {
            var button = (LoadingButton)dependencyObject;
            button.Invoke(() =>
            {
                if (button.IsLoading && button.CircularProgressBar != null)
                {
                    button.oldContent = button.Content;
                    button.Content = button.CircularProgressBar;
                    button.IsEnabled = false;
                }
                else if (button.IsLoading && button.CircularProgressBar == null)
                {
                    button.IsEnabled = false;
                    button.oldContent = button.Content;
                    button.Content = "Loading...";
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
                if (CircularProgressBar != null)
                    CircularProgressBar.Foreground = (Brush)e.NewValue;
            }
        }
    }
}
