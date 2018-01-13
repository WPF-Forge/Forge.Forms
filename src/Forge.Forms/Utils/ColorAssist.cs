using System.Windows;
using System.Windows.Media;

namespace Forge.Forms.Utils
{
    public class ColorAssist
    {
        public static readonly DependencyProperty ForegroundProperty =
            DependencyProperty.RegisterAttached(
                "Foreground",
                typeof(Brush),
                typeof(ColorAssist),
                new FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.Inherits));

        public static readonly DependencyProperty OpacityProperty =
            DependencyProperty.RegisterAttached(
                "Opacity",
                typeof(double),
                typeof(ColorAssist),
                new FrameworkPropertyMetadata(0.54d, FrameworkPropertyMetadataOptions.Inherits));

        public static readonly DependencyProperty DisabledOpacityProperty =
            DependencyProperty.RegisterAttached(
                "DisabledOpacity",
                typeof(double),
                typeof(ColorAssist),
                new FrameworkPropertyMetadata(0.38d, FrameworkPropertyMetadataOptions.Inherits));
    }
}
