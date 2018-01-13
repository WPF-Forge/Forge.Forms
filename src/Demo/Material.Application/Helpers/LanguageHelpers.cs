using System.Globalization;
using System.Reflection;
using System.Windows.Markup;

namespace Material.Application.Helpers
{
    public static class LanguageHelpers
    {
        public static XmlLanguage CreateXmlLanguage(string name, CultureInfo cultureInfo)
        {
            var xmlLanguage = XmlLanguage.GetLanguage(name);
            var cultureProperty = xmlLanguage.GetType()
                .GetField("_specificCulture", BindingFlags.NonPublic | BindingFlags.Instance);
            cultureProperty?.SetValue(xmlLanguage, cultureInfo);
            return xmlLanguage;
        }

        public static CultureInfo CreateCulture(string name)
        {
            return CreateCulture(name, "dd/MM/yyyy", "/");
        }

        public static CultureInfo CreateCulture(string name, string datePattern, string dateSeperator)
        {
            var cultureInfo = CultureInfo.CreateSpecificCulture(name);
            cultureInfo.DateTimeFormat.ShortDatePattern = datePattern;
            cultureInfo.DateTimeFormat.DateSeparator = dateSeperator;
            return cultureInfo;
        }
    }
}
