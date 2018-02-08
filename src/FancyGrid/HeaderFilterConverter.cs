using System;
using System.Windows.Data;
using System.Windows;
using System.ComponentModel;
using System.Windows.Controls;
using System.Text;
using System.IO;

namespace FancyGrid
{
    /// <summary>
    /// This converter will:
    ///  - Take the header
    ///  - Take the filtered word (if any)
    ///  - Add '(Filter: (bold)x(/bold))' to the header
    /// </summary>
    public class HeaderFilterConverter : IMultiValueConverter
    {
        /// <summary>
        /// Create a nice looking header
        /// </summary>
        /// <param name="values"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            // Get values
            string filter = values[0] as string;
            string headerText = values[1] as string;
            string filtertype = "";

            if (filter.StartsWith("<"))
                filtertype = "Less Than";
            else if (filter.StartsWith(">"))
                filtertype = "Greater Than";
            else if (filter.StartsWith("="))
                filtertype = "Exactly";
            else if (filter.StartsWith("!"))
                filtertype = "Not";
            else if (filter.StartsWith("~"))
                filtertype = "Doesn't Contain";
            else if (filter.StartsWith(@""""))
                filtertype = "Blank";
            else if (filter.Equals("*"))
                filtertype = "Any";
            else
                filtertype = "Contains";




            // Generate header text
            string text = "{0}{3}" + headerText + " {4}";
            if (!String.IsNullOrEmpty(filter))
                text += "({2}" + filtertype + "{4})";
            text += "{1}";

            // Escape special XML characters like <>&'
            text = new System.Xml.Linq.XText(text).ToString();

            // Format the text
            text = String.Format(text,
             @"<TextBlock xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'>",
             "</TextBlock>", "<Run FontWeight='bold' Text='", "<Run Text='", @"'/>");

            // Convert to stream
            MemoryStream stream = new MemoryStream(ASCIIEncoding.UTF8.GetBytes(text));

            // Convert to object
            TextBlock block = (TextBlock)System.Windows.Markup.XamlReader.Load(stream);
            return block;
        }

        /// <summary>
        /// Not required
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetTypes"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
