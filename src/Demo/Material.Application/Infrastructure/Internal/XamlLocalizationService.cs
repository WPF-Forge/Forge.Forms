using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using Material.Application.Localization;

namespace Material.Application.Infrastructure
{
    internal class XamlLocalizationService : ILocalizationService
    {
        private readonly Dictionary<string, Language> languages;

        public XamlLocalizationService()
        {
            languages = new Dictionary<string, Language>();
        }

        public Language CurrentLanguage { get; private set; }

        public event EventHandler LanguageChanged;

        public string GetString(string key)
        {
            if (key == null)
            {
                return null;
            }

            return System.Windows.Application.Current?.TryFindResource(key) as string;
        }

        public void SwitchLanguage(string languageKey)
        {
            Language language;
            if (!languages.TryGetValue(languageKey, out language) || language == null)
            {
                return;
            }

            SwitchLanguage(language);
        }

        public void RegisterLanguage(string languageKey, Language language)
        {
            if (languageKey == null || language == null)
            {
                throw new ArgumentNullException();
            }

            languages.Add(languageKey, language);
        }

        public Language GetLanguage(string languageKey) => languages[languageKey];

        protected virtual Language GetInitialLanguage()
        {
            return null;
        }

        private void SwitchLanguage(Language language)
        {
            if (language == null)
            {
                return;
            }

            Thread.CurrentThread.CurrentCulture = language.CultureInfo;
            Thread.CurrentThread.CurrentUICulture = language.CultureInfo;
            SetLanguageResourceDictionary(language);
            CurrentLanguage = language;
            LanguageChanged?.Invoke(this, EventArgs.Empty);
        }

        private static void SetLanguageResourceDictionary(Language language)
        {
            var languageDictionary = new ResourceDictionary
            {
                Source = new Uri(language.DictionaryUri, UriKind.Relative)
            };

            var dictionaries = System.Windows.Application.Current.Resources.MergedDictionaries;
            var dictionary = dictionaries.FirstOrDefault(d =>
                d.Contains("ResourceDictionaryName") &&
                d["ResourceDictionaryName"].ToString().StartsWith("Loc-"));

            if (dictionary == null)
            {
                dictionaries.Add(languageDictionary);
            }
            else
            {
                dictionaries[dictionaries.IndexOf(dictionary)] = languageDictionary;
            }
        }
    }
}
