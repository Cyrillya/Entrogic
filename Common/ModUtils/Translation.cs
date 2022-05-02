namespace Entrogic
{
    internal static class Translation
    {
        // 不会反射，告辞
        //internal static Dictionary<string, ModTranslation> translations => new Dictionary<string, ModTranslation>(typeof(Mod).GetFields(BindingFlags.Static | BindingFlags.NonPublic).Where(field => field.Name == "translations").First().GetValue(null) as IDictionary<string, ModTranslation>);
        internal static readonly IDictionary<string, ModTranslation> translations = new Dictionary<string, ModTranslation>();

        /// <summary>
        /// 注册一个翻译文本，返回使用时的键值
        /// 多次注册同一个键值时覆盖
        /// </summary>
        /// <param name="original">原文本</param>
        /// <param name="language">翻译文本语言</param>
        /// <param name="translated">译后文本</param>
        /// <param name="key">文本键值</param>
        /// <returns></returns>
        internal static string RegisterTranslation(string key, GameCulture.CultureName language, string translated, string original) {
            ModTranslation translate = GetOrCreateTranslation(key);
            translate.SetDefault(original);
            translate.AddTranslation((int)language, translated);
            LocalizationLoader.AddTranslation(translate);
            if (translations.TryGetValue(key, out _)) {
                translations[key] = translate;
            }
            else {
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
        internal static ModTranslation GetOrCreateTranslation(string key) {
            key = key.Replace(" ", "_");
            if (!translations.TryGetValue(key, out ModTranslation value)) {
                return LocalizationLoader.CreateTranslation(key);
            }

            return value;
        }

        public static void AddArmorTranslation(this ModTranslation translation, string chinese, bool endline = true) {
            string english = chinese;
            english.Replace("伤害", " damage ");
            english.Replace("暴击率", " critical strike chance ");
            english.Replace("的几率不消耗弹药", " chance not to consume ammo ");
            english.Replace("速度", " speed ");
            english.Replace("范围", " range ");
            english.Replace("容量", " capacity ");
            english.Replace("移动", " movement ");
            english.Replace("魔力", " mana ");
            english.Replace("近战", " melee ");
            english.Replace("远程", " ranged ");
            english.Replace("仆从", " minion ");
            english.Replace("魔法", " magic ");
            english.Replace("鞭子", " whip ");

            // 清除多余空格
            english = english.Trim();
            string[] strArray = english.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            english = string.Join(" ", strArray);

            if (endline) {
                english += '\n';
                chinese += '\n';
            }

            RegisterTranslation(translation.Key, GameCulture.CultureName.Chinese, chinese, english);
            translation.SetDefault(english);
            translation.AddTranslation((int)GameCulture.CultureName.Chinese, chinese);
        }

        public static void AddIntoDefault(this ModTranslation translation, string value, bool after = true) {
            string defaultv = translation.GetDefault();
            translation.SetDefault(after ? defaultv + value : value + defaultv);
        }
        public static void AddIntoTranslation(this ModTranslation translation, int culture, string value, bool after = true) {
            string translationv = translation.GetTranslation(culture);
            translation.AddTranslation(culture, after ? translationv + value : value + translationv);
        }

        public static string GetGameCultureLanguage(GameCulture gameCulture) {
            switch (gameCulture.Name) {
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

        public static void TranslationChinese(this ModTranslation translation, string value) {
            translation.AddTranslation((int)GameCulture.CultureName.Chinese, value);
        }
    }
}
