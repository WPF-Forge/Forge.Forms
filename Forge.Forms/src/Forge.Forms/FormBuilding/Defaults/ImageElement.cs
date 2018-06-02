using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Forge.Forms.Controls;
using Forge.Forms.DynamicExpressions;
using Forge.Forms.DynamicExpressions.ValueConverters;

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

        protected internal override void Freeze()
        {
            base.Freeze();
            Resources.Add(nameof(Source), GetSource(Source));
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
