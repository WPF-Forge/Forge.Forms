using System;
using System.Globalization;
using System.Windows.Data;
using ICSharpCode.AvalonEdit.Document;

namespace Forge.Forms.Demo.Infrastructure
{
    // Source: https://github.com/Keboo/ShowMeTheXAML/blob/master/ShowMeTheXAML.AvalonEdit/TextDocumentValueConverter.cs
    public class TextDocumentValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string @string)
            {
                return new TextDocument(@string);
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value as TextDocument)?.Text;
        }
    }
}
