using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Terraria.Localization;
using Terraria.ModLoader;

namespace Entrogic
{
    internal static class Translation
    {
        // 不会反射，告辞
        //internal static Dictionary<string, ModTranslation> translations => new Dictionary<string, ModTranslation>(typeof(Mod).GetFields(BindingFlags.Static | BindingFlags.NonPublic).Where(field => field.Name == "translations").First().GetValue(null) as IDictionary<string, ModTranslation>);
        internal static readonly IDictionary<string, ModTranslation> translations = new Dictionary<string, ModTranslation>();

        /// <summary>
        /// 注册一个翻译文本，返回使用时的键值
        /// 多次注册同一个键值时合并
        /// </summary>
        /// <param name="original">原文本</param>
        /// <param name="language">翻译文本语言</param>
        /// <param name="translated">译后文本</param>
        /// <param name="key">文本键值</param>
        /// <returns></returns>
<<<<<<< HEAD
        internal static string RegisterTranslation(string key, GameCulture.CultureName language, string translated, string original)
        {
            ModTranslation translate = GetOrCreateTranslation(key);
            translate.AddTranslation((int)language, translated);
=======
        internal static string RegisterTranslation(string key, GameCulture language, string translated, string original)
        {
            ModTranslation translate = GetOrCreateTranslation(key);
            translate.AddTranslation(language, translated);
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96
            translate.SetDefault(original);
            Entrogic.Instance.AddTranslation(translate);
            if (translations.TryGetValue(key, out ModTranslation value))
            {
                translations[key] = translate;
            }
            else
            {
                translations.Add(key, translate);
            }
            return $"Mods.Entrogic.{key}";
        }

        /// <summary>
        /// 获取或创建翻译文本(仅限使用RegisterTranslation添加的)
        /// 如果已经有过这个键值，那么返回原有的ModTranslation，否则返回一个新的基于键值生成的ModTranslation
        /// </summary>
        /// <param name="key">用于查找是否相同的键值</param>
        /// <returns></returns>
        internal static ModTranslation GetOrCreateTranslation(string key)
        {
            key = key.Replace(" ", "_");
            if (!translations.TryGetValue(key, out ModTranslation value))
            {
                return Entrogic.Instance.CreateTranslation(key);
            }

            return value;
        }

        public static string GetGameCultureLanguage(GameCulture gameCulture)
        {
            switch (gameCulture.Name)
            {
                case "en-US":
                    return "English";
                case "de-DE":
                    return "German";
                case "it-IT":
                    return "Italian";
                case "dr-DR":
                    return "French";
                case "es-ES":
                    return "Spanish";
                case "ru-RU":
                    return "Russian";
                case "zh-Hans":
                    return "Chinese";
                case "pt-BR":
                    return "Portuguese";
                case "pl-PL":
                    return "Polish";
                default: // 如果都不是
                    return "English";
            }
        }
    }
}
