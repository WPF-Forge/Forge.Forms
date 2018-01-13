using System.Globalization;

namespace Material.Application.Localization
{
    public class Language
    {
        public Language(string name, string dictionaryUri, CultureInfo cultureInfo)
        {
            Name = name;
            DictionaryUri = dictionaryUri;
            CultureInfo = cultureInfo;
        }

        public string Name { get; }

        public string DictionaryUri { get; }

        public CultureInfo CultureInfo { get; }
    }
}
