using System;
using System.Globalization;
using System.Windows.Data;

namespace Forge.Forms.Utils.ValueConverters
{
    internal class ConverterWrapper : IValueConverter
    {
        private readonly IValueConverter outerConverter;

        private readonly IValueConverter innerConverter;

        public ConverterWrapper(IValueConverter outerConverter, IValueConverter innerConverter)
        {
            this.outerConverter = outerConverter;
            this.innerConverter = innerConverter;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var inner = innerConverter.Convert(value, targetType, parameter, culture);
            return outerConverter.Convert(inner, targetType, parameter, culture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
