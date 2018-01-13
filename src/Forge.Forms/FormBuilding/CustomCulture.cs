using System.Collections.Generic;
using System.Globalization;

namespace Forge.Forms.FormBuilding
{
    public static class ConversionCulture
    {
        /// <summary>
        /// Stores custom cultures which can be used for date and number conversion.
        /// </summary>
        public static readonly Dictionary<string, CultureInfo> CustomCultures = new Dictionary<string, CultureInfo>();

        public static CultureInfo Get(string name)
        {
            if (name == null)
            {
                return null;
            }

            return CustomCultures.TryGetValue(name, out var value) 
                ? value 
                : CultureInfo.GetCultureInfo(name);
        }

        public static void Set(string name, CultureInfo cultureInfo)
        {
            CustomCultures[name] = cultureInfo;
        }
    }
}
