using System.Windows;

namespace Forge.Forms.Controls
{
    public static class TextProperties
    {
        public static readonly DependencyProperty TitleFontSizeProperty =
            DependencyProperty.RegisterAttached(
                "TitleFontSize",
                typeof(double),
                typeof(TextProperties),
                new FrameworkPropertyMetadata(20d, FrameworkPropertyMetadataOptions.Inherits));

        public static double GetTitleFontSize(DependencyObject element)
        {
            return (double)element.GetValue(TitleFontSizeProperty);
        }

        public static void SetTitleFontSize(DependencyObject element, double value)
        {
            element.SetValue(TitleFontSizeProperty, value);
        }

        public static readonly DependencyProperty HeadingFontSizeProperty =
            DependencyProperty.RegisterAttached(
                "HeadingFontSize",
                typeof(double),
                typeof(TextProperties),
                new FrameworkPropertyMetadata(15d, FrameworkPropertyMetadataOptions.Inherits));

        public static double GetHeadingFontSize(DependencyObject element)
        {
            return (double)element.GetValue(HeadingFontSizeProperty);
        }

        public static void SetHeadingFontSize(DependencyObject element, double value)
        {
            element.SetValue(HeadingFontSizeProperty, value);
        }

        public static readonly DependencyProperty TextFontSizeProperty =
            DependencyProperty.RegisterAttached(
                "TextFontSize",
                typeof(double),
                typeof(TextProperties),
                new FrameworkPropertyMetadata(13d, FrameworkPropertyMetadataOptions.Inherits));

        public static double GetTextFontSize(DependencyObject element)
        {
            return (double)element.GetValue(TextFontSizeProperty);
        }

        public static void SetTextFontSize(DependencyObject element, double value)
        {
            element.SetValue(TextFontSizeProperty, value);
        }
    }
}