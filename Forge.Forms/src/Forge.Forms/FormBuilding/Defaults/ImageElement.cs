using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using Forge.Forms.DynamicExpressions;

namespace Forge.Forms.FormBuilding.Defaults
{
    public class ImageElement : FormElement
    {
        private static readonly ImageSourceConverter Converter = new ImageSourceConverter();

        private static ImageSource FromString(string value)
        {
            return (ImageSource)Converter.ConvertFromString(value);
        }

        private class ImageSourceValueConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                switch (value)
                {
                    case string path:
                        return FromString(path);
                    case ImageSource imageSource:
                        return imageSource;
                    default:
                        return null;
                }
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return Binding.DoNothing;
            }
        }

        private static IValueProvider GetSource(IValueProvider source)
        {
            switch (source)
            {
                case LiteralValue literal:
                    switch (literal.Value)
                    {
                        case string str:
                            return new LiteralValue(FromString(str));
                        case ImageSource src:
                            return new LiteralValue(src);
                        default:
                            return LiteralValue.Null;
                    }
                case null:
                    return LiteralValue.Null;
                default:
                    return source.Wrap(new ImageSourceValueConverter());
            }
        }

        public IValueProvider Source { get; set; }

        public IValueProvider Width { get; set; }

        public IValueProvider Height { get; set; }

        public IValueProvider VerticalAlignment { get; set; }

        public IValueProvider HorizontalAlignment { get; set; }

        public IValueProvider Stretch { get; set; }

        public IValueProvider StretchDirection { get; set; }

        protected internal override void Freeze()
        {
            base.Freeze();
            Resources.Add(nameof(Source), GetSource(Source));
            Resources.Add(nameof(Width), Width ?? new LiteralValue(double.NaN));
            Resources.Add(nameof(Height), Height ?? new LiteralValue(double.NaN));
            Resources.Add(nameof(VerticalAlignment), VerticalAlignment ?? new LiteralValue(System.Windows.VerticalAlignment.Center));
            Resources.Add(nameof(HorizontalAlignment), HorizontalAlignment ?? new LiteralValue(System.Windows.HorizontalAlignment.Stretch));
            Resources.Add(nameof(Stretch), Stretch ?? new LiteralValue(System.Windows.Media.Stretch.Uniform));
            Resources.Add(nameof(StretchDirection), StretchDirection ?? new LiteralValue(System.Windows.Controls.StretchDirection.DownOnly));
        }

        protected internal override IBindingProvider CreateBindingProvider(IResourceContext context,
            IDictionary<string, IValueProvider> formResources)
        {
            return new ImagePresenter(context, Resources, formResources);
        }
    }

    public class ImagePresenter : BindingProvider
    {
        static ImagePresenter()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ImagePresenter),
                new FrameworkPropertyMetadata(typeof(ImagePresenter)));
        }

        public ImagePresenter(IResourceContext context, IDictionary<string, IValueProvider> fieldResources,
            IDictionary<string, IValueProvider> formResources)
            : base(context, fieldResources, formResources, true)
        {
        }
    }
}
