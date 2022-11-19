using Terraria.GameInput;

namespace Entrogic
{
    public static partial class ModHelper
    {
        #region 空间

        public static Rectangle GetWorldCoordinates(Rectangle rect) => new(rect.X << 4, rect.Y << 4, rect.Width << 4, rect.Height << 4);

        public static Rectangle ToWorldCoordinates(this Rectangle rect) => new(rect.X << 4, rect.Y << 4, rect.Width << 4, rect.Height << 4);

        public static Rectangle GetTileCoordinates(Rectangle rect) => new(rect.X >> 4, rect.Y >> 4, rect.Width >> 4, rect.Height >> 4);

        public static Rectangle ToTileCoordinates(this Rectangle rect) => new(rect.X >> 4, rect.Y >> 4, rect.Width >> 4, rect.Height >> 4);

        public static Rectangle CreateFromVector2(Vector2 vec, float width, float height) => new((int)vec.X, (int)vec.Y, (int)width, (int)height);

        public static Rectangle CreateFromVector2(Vector2 vec, Vector2 size) => CreateFromVector2(vec, size.X, size.Y);

        public static Vector2 GetFromToVector(Vector2 v1, Vector2 v2) => v2 - v1;

        public static Vector2 GetFromToVectorNormalized(Vector2 v1, Vector2 v2) => Vector2.Normalize(GetFromToVector(v1, v2));

        public static float GetFromToRadians(Vector2 v1, Vector2 v2) => GetFromToVector(v1, v2).ToRotation();

        public static bool InRange(this int value, int min, int max) => value >= min && value <= max;

        public static bool InRange(this float value, float min, float max) => value >= min && value <= max;
        
        public static Vector2 ClosestPointInHitbox(Rectangle hitboxOfTarget, Vector2 desiredLocation) {
            Vector2 offset = desiredLocation - hitboxOfTarget.Center.ToVector2();
            offset.X = Math.Min(Math.Abs(offset.X), hitboxOfTarget.Width / 2) * Math.Sign(offset.X);
            offset.Y = Math.Min(Math.Abs(offset.Y), hitboxOfTarget.Height / 2) * Math.Sign(offset.Y);
            return hitboxOfTarget.Center.ToVector2() + offset;
        }

        public static Vector2 ClosestPointInHitbox(Entity entity, Vector2 desiredLocation) {
            return ClosestPointInHitbox(entity.Hitbox, desiredLocation);
        }

        public static float Length(this Rectangle rectangle) => rectangle.Size().Length();

        #endregion

        #region  绘制

        public static void BeginGameSpriteBatch(this SpriteBatch spriteBatch, bool deferred = true, bool alphaBlend = true) =>
            spriteBatch.Begin(deferred ? SpriteSortMode.Deferred : SpriteSortMode.Immediate, alphaBlend ? BlendState.AlphaBlend : BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

        public static void DrawBorderedRect(SpriteBatch spriteBatch, Color color, Color borderColor, Vector2 position, Vector2 size, int borderWidth) {
            spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y), color);
            spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle((int)position.X - borderWidth, (int)position.Y - borderWidth, (int)size.X + borderWidth * 2, borderWidth), borderColor);
            spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle((int)position.X - borderWidth, (int)position.Y + (int)size.Y, (int)size.X + borderWidth * 2, borderWidth), borderColor);
            spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle((int)position.X - borderWidth, (int)position.Y, (int)borderWidth, (int)size.Y), borderColor);
            spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle((int)position.X + (int)size.X, (int)position.Y, (int)borderWidth, (int)size.Y), borderColor);
        }

        #endregion

        #region 文字
        
        /// <summary>
        /// 获取 HJson 文字
        /// </summary>
        public static string GetText(string str, params object[] arg)
        {
            string text = Language.GetTextValue($"Mods.Entrogic.{str}", arg);
            return ConvertLeftRight(text);
        }

        public static string GetTextWith(string str, object arg)
        {
            string text = Language.GetTextValueWith($"Mods.Entrogic.{str}", arg);
            return ConvertLeftRight(text);
        }

        public static string ConvertLeftRight(string text)
        {
            // 支持输入<left>和<right>，就和ItemTooltip一样（原版只有Tooltip支持）
            if (text.Contains("<right>"))
            {
                InputMode inputMode = InputMode.XBoxGamepad;
                if (PlayerInput.UsingGamepad)
                    inputMode = InputMode.XBoxGamepadUI;

                if (inputMode == InputMode.XBoxGamepadUI)
                {
                    KeyConfiguration keyConfiguration = PlayerInput.CurrentProfile.InputModes[inputMode];
                    string input = PlayerInput.BuildCommand("", true, keyConfiguration.KeyStatus["MouseRight"]);
                    input = input.Replace(": ", "");
                    text = text.Replace("<right>", input);
                }
                else
                {
                    text = text.Replace("<right>", Language.GetTextValue("Controls.RightClick"));
                }
            }
            if (text.Contains("<left>"))
            {
                InputMode inputMode2 = InputMode.XBoxGamepad;
                if (PlayerInput.UsingGamepad)
                    inputMode2 = InputMode.XBoxGamepadUI;

                if (inputMode2 == InputMode.XBoxGamepadUI)
                {
                    KeyConfiguration keyConfiguration2 = PlayerInput.CurrentProfile.InputModes[inputMode2];
                    string input = PlayerInput.BuildCommand("", true, keyConfiguration2.KeyStatus["MouseLeft"]);
                    input = input.Replace(": ", "");
                    text = text.Replace("<left>", input);
                }
                else
                {
                    text = text.Replace("<left>", Language.GetTextValue("Controls.LeftClick"));
                }
            }
            return text;
        }

        #endregion

        #region 光标

        /// <summary>
        /// 判断鼠标是否在某个矩形上。
        /// </summary>
        /// <param name="rectangle1">矩形</param>
        /// <returns></returns>
        public static bool MouseInRectangle(Rectangle rectangle1) => rectangle1.Intersects(new Rectangle(Main.mouseX, Main.mouseY, 1, 1));

        /// <summary>
        /// 判断鼠标是否在某个矩形上。
        /// </summary>
        /// <param name="X">矩形横坐标</param>
        /// <param name="Y">矩形纵坐标</param>
        /// <param name="width">矩形宽度</param>
        /// <param name="height">矩形高度</param>
        /// <returns></returns>
        public static bool MouseInRectangle(int X, int Y, int width, int height) => new Rectangle(X, Y, width, height).Intersects(new Rectangle(Main.mouseX, Main.mouseY, 1, 1));
        /// <summary>
        /// 判断鼠标是否在某个矩形上。
        /// </summary>
        /// <param name="X">矩形横坐标</param>
        /// <param name="Y">矩形纵坐标</param>
        /// <param name="width">矩形宽度</param>
        /// <param name="height">矩形高度</param>
        /// <param name="offxLeft">向左偏移长度。</param>
        /// <param name="offyTop">向上偏移长度。</param>
        /// <returns></returns>
        public static bool MouseInRectangle(int X, int Y, int width, int height, int offxLeft = 0, int offyTop = 0) {
            Vector2 mountedCenter = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
            return new Rectangle((int)mountedCenter.X, (int)mountedCenter.Y, 0, 0).Intersects(new Rectangle((int)(X + Main.screenPosition.X - offxLeft), (int)(Y + Main.screenPosition.Y - offyTop), width, height));
        }

        /// <summary>
        /// 鼠标与某矩形重合后绘制鼠标旁的悬浮字
        /// </summary>
        /// <param name="font">字体</param>
        /// <param name="text">文本</param>
        /// <param name="X">矩形横坐标</param>
        /// <param name="Y">矩形纵坐标</param>
        /// <param name="Width">矩形宽</param>
        /// <param name="Hegith">矩形高</param>
        public static void DrawMouseTextOnRectangle(DynamicSpriteFont font, string text, int X, int Y, int Width, int Hegith) {
            Vector2 mountedCenter = Main.MouseScreen;
            if (new Rectangle((int)mountedCenter.X, (int)mountedCenter.Y, 0, 0).Intersects(new Rectangle((int)X, (int)Y, Width, Hegith))) {
                string name = text;
                Vector2 worldPos = new(mountedCenter.X + 15, mountedCenter.Y + 15);
                Vector2 size = font.MeasureString(name);
                Vector2 texPos = worldPos + new Vector2(-size.X * 0.5f, name.Length);
                Main.spriteBatch.DrawString(font, name, new Vector2(texPos.X, texPos.Y), Color.White);
            }
        }
        
        public static Asset<Texture2D> GetTexture(string path) =>
            ModContent.Request<Texture2D>($"{TextureManager.ImageBase}{path}", AssetRequestMode.ImmediateLoad);

        public static Asset<Effect> GetEffect(string path) =>
            ModContent.Request<Effect>($"{ShaderManager.EffectBase}{path}", AssetRequestMode.ImmediateLoad);

        /// <summary>
        /// 获取 HJson 文字
        /// </summary>
        public static string GetText(string str, params object[] arg)
        {
            string text = Language.GetTextValue($"Mods.Entrogic.{str}", arg);
            return ConvertLeftRight(text);
        }

        public static string GetTextWith(string str, object arg)
        {
            string text = Language.GetTextValueWith($"Mods.Entrogic.{str}", arg);
            return ConvertLeftRight(text);
        }

        public static string ConvertLeftRight(string text)
        {
            // 支持输入<left>和<right>，就和ItemTooltip一样（原版只有Tooltip支持）
            if (text.Contains("<right>"))
            {
                InputMode inputMode = InputMode.XBoxGamepad;
                if (PlayerInput.UsingGamepad)
                    inputMode = InputMode.XBoxGamepadUI;

                if (inputMode == InputMode.XBoxGamepadUI)
                {
                    KeyConfiguration keyConfiguration = PlayerInput.CurrentProfile.InputModes[inputMode];
                    string input = PlayerInput.BuildCommand("", true, keyConfiguration.KeyStatus["MouseRight"]);
                    input = input.Replace(": ", "");
                    text = text.Replace("<right>", input);
                }
                else
                {
                    text = text.Replace("<right>", Language.GetTextValue("Controls.RightClick"));
                }
            }
            if (text.Contains("<left>"))
            {
                InputMode inputMode2 = InputMode.XBoxGamepad;
                if (PlayerInput.UsingGamepad)
                    inputMode2 = InputMode.XBoxGamepadUI;

                if (inputMode2 == InputMode.XBoxGamepadUI)
                {
                    KeyConfiguration keyConfiguration2 = PlayerInput.CurrentProfile.InputModes[inputMode2];
                    string input = PlayerInput.BuildCommand("", true, keyConfiguration2.KeyStatus["MouseLeft"]);
                    input = input.Replace(": ", "");
                    text = text.Replace("<left>", input);
                }
                else
                {
                    text = text.Replace("<left>", Language.GetTextValue("Controls.LeftClick"));
                }
            }
            return text;
        }
        
        // 针对textSnippet特殊文本的换行
        public static TextSnippet[] WordwrapString(string text, float scale, DynamicSpriteFont font, int maxWidth, Color baseColor = default) {
            float workingLineLength = 0f; // 当前行长度
            TextSnippet[] originalSnippets = ChatManager.ParseMessage(text, baseColor).ToArray();
            ChatManager.ConvertNormalSnippets(originalSnippets);
            List<TextSnippet> finalSnippets = new() { new TextSnippet() };

            foreach (var snippet in originalSnippets) {
                if (snippet is PlainTagHandler.PlainSnippet) {
                    string cacheString = ""; // 缓存字符串 - 准备输入的字符
                    for (int i = 0; i < snippet.Text.Length; i++) {
                        GlyphMetrics characterMetrics = font.GetCharacterMetrics(snippet.Text[i]);
                        workingLineLength += (font.CharacterSpacing + characterMetrics.KernedWidth) * scale;

                        if (workingLineLength > maxWidth && !char.IsWhiteSpace(snippet.Text[i])) {
                            // 如果第一个字符是空格，单词长度小于19（实际上是18因为第一个字符为空格），可以空格换行
                            bool canWrapWord = cacheString.Length is > 1 and < 19;

                            // 找不到空格，或者拆腻子，则强制换行
                            if (!canWrapWord || (i > 0 && CanBreakBetween(snippet.Text[i - 1], snippet.Text[i]))) {
                                finalSnippets.Add(new TextSnippet(cacheString, snippet.Color));
                                finalSnippets.Add(new TextSnippet("\n"));
                                workingLineLength = characterMetrics.KernedWidthOnNewLine;
                                cacheString = "";
                            }
                            // 空格换行
                            else {
                                finalSnippets.Add(new TextSnippet("\n"));
                                finalSnippets.Add(new TextSnippet(cacheString[1..], snippet.Color));
                                workingLineLength = font.MeasureString(cacheString).X * scale;
                                cacheString = "";
                            }
                        }

                        // 这么做可以分割单词，并且使自然分割单词（即不因换行过长强制分割的单词）第一个字符总是空格
                        // 或者是将CJK字符与非CJK字符分割
                        if (cacheString != string.Empty && (char.IsWhiteSpace(snippet.Text[i]) || IsCJK(cacheString[^1]) != IsCJK(snippet.Text[i]))) {
                            finalSnippets.Add(new TextSnippet(cacheString, snippet.Color));
                            cacheString = "";
                        }

                        // 原有换行则将当前行长度重置
                        if (snippet.Text[i] is '\n') {
                            workingLineLength = 0;
                        }

                        cacheString += snippet.Text[i];
                    }
                    finalSnippets.Add(new TextSnippet(cacheString, snippet.Color));
                }
                else {
                    float length = snippet.GetStringLength(font) * scale;
                    workingLineLength += length;
                    // 超了 - 换行再添加，注意起始长度
                    if (workingLineLength > maxWidth) {
                        workingLineLength = length;
                        finalSnippets.Add(new TextSnippet("\n"));
                    }
                    finalSnippets.Add(snippet);
                }
            }

            return finalSnippets.ToArray();
        }

        // https://unicode-table.com/cn/blocks/cjk-unified-ideographs/ 中日韩统一表意文字
        // https://unicode-table.com/cn/blocks/cjk-symbols-and-punctuation/ 中日韩符号和标点
        public static bool IsCJK(char a) {
            return (a >= 0x4E00 && a <= 0x9FFF) || (a >= 0x3000 && a <= 0x303F);
        }

        internal static bool CanBreakBetween(char previousChar, char nextChar) {
            if (IsCJK(previousChar) || IsCJK(nextChar))
                return true;

            return false;
        }
        
        #endregion
    }
}
