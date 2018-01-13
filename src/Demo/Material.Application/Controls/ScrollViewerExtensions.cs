using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Material.Application.Controls
{
    public static class ScrollViewerExtensions
    {
        public static void AnimateScrollIntoView(this ScrollViewer scrollViewer, UIElement item, double delta = 0d,
            double duration = 1000d)
        {
            var relativePoint = item.TranslatePoint(new Point(0d, 0d), scrollViewer);
            AnimateScrollToOffset(scrollViewer, scrollViewer.VerticalOffset + relativePoint.Y + delta);
        }

        public static void ScrollIntoView(this ScrollViewer scrollViewer, UIElement item, double delta = 0d)
        {
            var relativePoint = item.TranslatePoint(new Point(0d, 0d), scrollViewer);
            scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset + relativePoint.Y + delta);
        }

        public static void AnimateScrollToOffset(this ScrollViewer scrollViewer, double offset, double duration = 1000d)
        {
            var verticalAnimation = new DoubleAnimation
            {
                From = scrollViewer.VerticalOffset,
                To = offset,
                EasingFunction = new QuarticEase { EasingMode = EasingMode.EaseInOut },
                Duration = new Duration(TimeSpan.FromMilliseconds(duration))
            };

            var storyboard = new Storyboard();
            storyboard.Children.Add(verticalAnimation);
            Storyboard.SetTarget(verticalAnimation, scrollViewer);
            Storyboard.SetTargetProperty(verticalAnimation,
                new PropertyPath(ScrollViewerBehavior.VerticalOffsetProperty));
            storyboard.Begin();
        }

        public class ScrollViewerBehavior
        {
            public static DependencyProperty VerticalOffsetProperty =
                DependencyProperty.RegisterAttached("VerticalOffset",
                    typeof(double),
                    typeof(ScrollViewerBehavior),
                    new UIPropertyMetadata(0d, OnVerticalOffsetChanged));

            public static void SetVerticalOffset(FrameworkElement target, double value)
            {
                target.SetValue(VerticalOffsetProperty, value);
            }

            public static double GetVerticalOffset(FrameworkElement target)
            {
                return (double)target.GetValue(VerticalOffsetProperty);
            }

            private static void OnVerticalOffsetChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
            {
                var scrollViewer = target as ScrollViewer;
                scrollViewer?.ScrollToVerticalOffset((double)e.NewValue);
            }
        }
    }
}
