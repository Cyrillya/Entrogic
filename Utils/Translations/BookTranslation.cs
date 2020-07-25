using System;

using Terraria.Localization;
using Terraria.ModLoader;

namespace Entrogic.Items.Books.Translations
{
    internal static class BookTranslation
    {
        internal static string CreateTooltipTranslation(GameCulture[] languages, string key, string original, string[] translations)
        {
            for (int i = 0; i < languages.Length; i++)
            {
                GameCulture language = languages[i];
                string translation = translations[i];
                string translationKeyWord = $"BookTranslation.{key}.{language.Name}";

                ModTranslation template = Entrogic.Instance.CreateTranslation(translationKeyWord);
                template.AddTranslation(language, translation);
                template.SetDefault(original);
            }
            return $"BookTranslation.{key}.";
        }
    }
}
