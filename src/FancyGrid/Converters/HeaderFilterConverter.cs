using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using System.Xml.Linq;

namespace FancyGrid.Converters
{
    /// <summary>
    /// This converter will:
    /// - Take the header
    /// - Take the filtered word (if any)
    /// - Add '(Filter: (bold)x(/bold))' to the header
    /// </summary>
    public class HeaderFilterConverter : IMultiValueConverter
    {
        /// <inheritdoc />
        /// <summary>
        /// Create a nice looking header
        /// </summary>
        /// <param name="values"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object[] values, Type targetType, object parameter,
            CultureInfo culture)
        {
            var filter = values[0] as string;
            var headerText = values[1] as string;
            string filtertype;

            if (filter != null && filter.StartsWith("<"))
            {
                filtertype = "Less Than";
            }
            else if (filter != null && filter.StartsWith(">"))
            {
                filtertype = "Greater Than";
            }
            else if (filter != null && filter.StartsWith("="))
            {
                filtertype = "Exactly";
            }
            else if (filter != null && filter.StartsWith("!"))
            {
                filtertype = "Not";
            }
            else if (filter != null && filter.StartsWith("~"))
            {
                filtertype = "Doesn't Contain";
            }
            else if (filter != null && filter.StartsWith(@""""))
            {
                filtertype = "Blank";
            }
            else if (filter != null && filter.Equals("*"))
            {
                filtertype = "Any";
            }
            else
            {
                filtertype = "Contains";
            }


            var text = "{0}{3}" + headerText + " {4}";
            if (!string.IsNullOrEmpty(filter))
            {
                text += "({2}" + filtertype + "{4})";
            }

            text += "{1}";

            text = new XText(text).ToString();

            text = string.Format(text,
                @"<TextBlock xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'>",
                "</TextBlock>", "<Run FontWeight='bold' Text='", "<Run Text='", @"'/>");

            var stream = new MemoryStream(Encoding.UTF8.GetBytes(text));
            return (TextBlock)XamlReader.Load(stream);
        }

        /// <inheritdoc />
        /// <summary>
        /// Not required
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetTypes"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter,
            CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
