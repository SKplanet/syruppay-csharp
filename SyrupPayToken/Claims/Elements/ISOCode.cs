using System;
using System.Collections.Generic;
using System.Globalization;

namespace SyrupPayToken.Claims
{
    public class ISOCode
    {
        private static HashSet<string> ISO_LANGUAGES;
        private static HashSet<string> ISO_COUNTRIES;

        static ISOCode()
        {
            if (Object.ReferenceEquals(null, ISO_LANGUAGES) || ISO_LANGUAGES.Count == 0)
                LoadLanguageByIso639();
            if (Object.ReferenceEquals(null, ISO_COUNTRIES) || ISO_COUNTRIES.Count == 0)
                LoadCountriesByIso3166();
        }

        private static void LoadLanguageByIso639()
        {
            ISO_LANGUAGES = new HashSet<string>();
            CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
            foreach (var culture in cultures)
            {
                ISO_LANGUAGES.Add(culture.TwoLetterISOLanguageName);
            }
        }

        private static void LoadCountriesByIso3166()
        {
            ISO_COUNTRIES = new HashSet<String>();
            foreach (CultureInfo culture in CultureInfo.GetCultures(CultureTypes.SpecificCultures))
            {
                RegionInfo country = new RegionInfo(culture.LCID);
                if (!ISO_COUNTRIES.Contains(country.TwoLetterISORegionName))
                {
                    ISO_COUNTRIES.Add(country.TwoLetterISORegionName);
                }
            }
        }

        public static bool IsValidCountryAlpha2Code(string code)
        {
            return ISO_COUNTRIES.Contains(code.Contains(":") ? code.Substring(code.IndexOf(":") + 1).ToUpper() : code.ToUpper());
        }

        public static bool IsValidLanuageCode(string code)
        {
            return ISO_LANGUAGES.Contains(code);
        }
    }
}
