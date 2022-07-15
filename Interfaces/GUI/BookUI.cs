using Entrogic.Common.Globals.Players;
using Entrogic.Common.Hooks.Items;
using Entrogic.Content.Items.BaseTypes;
using Entrogic.Interfaces.UIElements;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using Terraria.UI.Chat;

namespace Entrogic.Interfaces.GUI
{
    public class BookUI : UIState
    {
        public UIPanel Book;//新建UI
        internal static List<BookContent> Contents = new();
        internal static Asset<Texture2D> Texture = TextureManager.BookPanel;
        internal static int MaxPages = 1;

        private UIImage bookPanel;
        private Vector2 bookSize = new(600f, 480f);
        private string _lastBookName = "";

        private Rectangle MouseRect => new(Main.mouseX, Main.mouseY, 1, 1);

        public override void OnInitialize() {
            Vector2 bookPos = new((Main.screenWidth - bookSize.X) / 2, (Main.screenHeight - bookSize.Y) / 2);

            if (!Main.dedServ)
                bookPanel = new UIImage(ModContent.Request<Texture2D>($"{TextureManager.BookAssets}Panel"));
            bookPanel.Left.Set(bookPos.X, 0f); // UI距离左边
            bookPanel.Top.Set(bookPos.Y, 0f); // UI距离上面
            bookPanel.OnClick += new MouseEvent(Clicked); // 尝试换页
            bookPanel.OnRightClick += new MouseEvent((evt, listeningElement) => CloseBook(Main.LocalPlayer.GetModPlayer<BookInfoPlayer>())); // 右键关闭书籍
            bookPanel.Width.Set(bookSize.X, 0f); // UI的宽
            bookPanel.Height.Set(bookSize.Y, 0f); // UI的高
            Append(bookPanel); // 添加UI
        }

        internal void Unload() {
            Texture = null;
            bookPanel = null;
            Contents.Clear();
        }

        private void Clicked(UIMouseEvent evt, UIElement listeningElement) {
            Player player = Main.LocalPlayer;
            BookInfoPlayer book = player.GetModPlayer<BookInfoPlayer>();

            Vector2 bookPos = new((Main.screenWidth - bookSize.X) / 2, (Main.screenHeight - bookSize.Y) / 2);
            Rectangle rectLeft = new((int)bookPos.X, (int)(bookPos.Y + bookSize.Y - 117), 72, 117);
            Rectangle rectRight = new((int)(bookPos.X + bookSize.X - 72), (int)(bookPos.Y + bookSize.Y - 117), 72, 117);
            if (MouseRect.Intersects(rectLeft) && book.CurrentPage > 1) {
                SoundEngine.PlaySound(SoundID.MenuOpen);
                book.CurrentPage -= 1;
                SendInfo(Main.LocalPlayer);
            }
            int MaxPage = 1;
            if (player.HeldItem?.ModItem is ItemBook)
                MaxPage = (player.HeldItem?.ModItem as ItemBook).PageMax;
            if (MouseRect.Intersects(rectRight) && book.CurrentPage < MaxPage) {
                SoundEngine.PlaySound(SoundID.MenuOpen);
                book.CurrentPage += 1;
                SendInfo(Main.LocalPlayer);
            }
        }

        private void UpdatePageInfos() {
            Player player = Main.LocalPlayer;
            BookInfoPlayer book = player.GetModPlayer<BookInfoPlayer>();
            ItemBook item = player.HeldItem.ModItem as ItemBook;
            // 若并非书籍则不会执行本方法，故无需判断
            book.BookName = item.Name;
            Texture = TextureManager.BookPanel;
            IModifyBookContent.Invoke(item.Item, player, ref Contents);
            IModifyBookPanel.Invoke(item.Item, player, ref Texture);
            bookPanel.SetImage(Texture);
            MaxPages = (player.HeldItem?.ModItem as ItemBook).PageMax;
            if (book.CurrentPage > MaxPages)
                book.CurrentPage = MaxPages;
        }

        private void CloseBook(BookInfoPlayer book) {
            UIHandler.Instance.HideUI(UIHandler.UI.Book);
            book.IsReading = false;
            SendInfo(Main.LocalPlayer);
        }

        private void SendInfo(Player plr) {
            BookInfoPlayer book = plr.GetModPlayer<BookInfoPlayer>();
            ModNetHandler.BookInfo.SendPage(-1, plr.whoAmI, (byte)plr.whoAmI, book.CurrentPage, book.IsReading);
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);

            if (new Rectangle((int)bookPanel.Left.Pixels, (int)bookPanel.Top.Pixels, (int)bookSize.X, (int)bookSize.Y).Intersects(MouseRect)) {
                Main.LocalPlayer.mouseInterface = true;
            }

            var player = Main.LocalPlayer;
            var book = player.GetModPlayer<BookInfoPlayer>();
            if (Main.playerInventory || player.HeldItem?.IsAir == true || !(player.HeldItem?.ModItem is ItemBook)) {
                CloseBook(book);
                return;
            }
            if (_lastBookName != "" && player.HeldItem.Name != _lastBookName) { // 在阅读一本书的途中换书，重新加载书籍信息
                BookInfoPlayer.ReloadBook(player);
            }
            _lastBookName = player.HeldItem.Name;
            Vector2 bookPos = new((Main.screenWidth - bookSize.X) / 2, (Main.screenHeight - bookSize.Y) / 2);
            bookPanel.Left.Pixels = bookPos.X;//UI距离左边
            bookPanel.Top.Pixels = bookPos.Y;//UI距离上面

            UpdatePageInfos();
        }

        public override void Draw(SpriteBatch spriteBatch) {
            Player player = Main.LocalPlayer;
            BookInfoPlayer book = player.GetModPlayer<BookInfoPlayer>();
            base.Draw(spriteBatch);

            Vector2 bookPos = new((Main.screenWidth - bookSize.X) / 2, (Main.screenHeight - bookSize.Y) / 2);
            Rectangle rectLeft = new((int)bookPos.X, (int)(bookPos.Y + bookSize.Y - 117), 72, 117);
            Rectangle rectRight = new((int)(bookPos.X + bookSize.X - 72), (int)(bookPos.Y + bookSize.Y - 117), 72, 117);
            if (IDrawReading.Invoke(spriteBatch, player.HeldItem, player)) {
                if (MouseRect.Intersects(rectLeft) && book.CurrentPage > 1) {
                    spriteBatch.Draw(TextureManager.BookBack.Value, new Vector2(rectLeft.X, rectLeft.Y), null, Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
                }
                else {
                    string page = (book.CurrentPage * 2 - 1).ToString();
                    Vector2 size = FontAssets.MouseText.Value.MeasureString(page) * 0.5f;
                    ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.ItemStack.Value, page, new Vector2(bookPos.X + 34 - size.X, bookPos.Y + bookSize.Y - 86 - size.Y), Color.White, 0f, Vector2.Zero, new Vector2(1f));
                }
                if (MouseRect.Intersects(rectRight) && book.CurrentPage < MaxPages) {
                    spriteBatch.Draw(TextureManager.BookNext.Value, new Vector2(rectRight.X, rectRight.Y), null, Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
                }
                else {
                    string page = (book.CurrentPage * 2).ToString();
                    Vector2 size = FontAssets.MouseText.Value.MeasureString(page) * 0.5f;
                    ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.ItemStack.Value, page, new Vector2(bookPos.X + bookSize.X - 34 - size.X, bookPos.Y + bookSize.Y - 86 - size.Y), Color.White, 0f, Vector2.Zero, new Vector2(1f));
                }
            }

            #region Draw Text
            //Main.spriteBatch.Draw(Main.magicPixel, space.ToRectangle(), Color.Yellow * .7f);
            //Main.spriteBatch.Draw(Main.magicPixel, GetOuterDimensions().ToRectangle(), Color.Red * .7f);

            foreach (var content in Contents) {
                var font = content.Font == null ? FontAssets.MouseText.Value : content.Font.Value;
                var color = content.Color == default ? Color.Black : content.Color;
                float baseLineDistance = 30f * content.Scale;
                List<List<TextSnippet>> drawTextSnippets = WordwrapStringSmart(content.Text, content.Scale, color, font, content.TextWarpWidth, -1);
                //ChatManager.DrawColorCodedString(spriteBatch, font, content.Text, new Vector2(200, 200), color, 0f, Vector2.Zero, Vector2.One * content.Scale, -1f);
                float position = 0f;
                TextSnippet[] texts;
                foreach (var snippetList in drawTextSnippets) {
                    texts = snippetList.ToArray();
                    //float snippetListHeight = ChatManager.GetStringSize(font, texts, new Vector2(scale)).Y + lineDistance;
                    //Main.NewText($"Y: {ChatManager.GetStringSize(font, texts, Vector2.One).X}");
                    if (position > -baseLineDistance + content.ExtraLineDistance/*snippetListHeight*/) {
                        //foreach (var snippet in snippetList)
                        //{
                        int hoveredSnippet = -1;
                        var pos = new Vector2(bookPanel.Left.Pixels, bookPanel.Top.Pixels + position /*+ offset*/) + content.Position;
                        ChatManager.ConvertNormalSnippets(texts);
                        ChatManager.DrawColorCodedString(spriteBatch, font, texts, pos, color, 0f, Vector2.Zero, Vector2.One * content.Scale, out hoveredSnippet, -1f);
                        //offset += 20;
                        //offset += texts.Max(t => (int)ChatManager.GetStringSize(FontAssets.MouseText, texts, Vector2.One).X);
                        //if (hoveredSnippet > -1 && IsMouseHovering)
                        //{
                        //    BossChecklist.instance.BossLog.hoveredTextSnippet = texts[hoveredSnippet];
                        //    // BossChecklist change: Use hoveredTextSnippet to bypass clippingRectangle and draw order issues.
                        //    //texts[hoveredSnippet].OnHover();
                        //    //if (Main.mouseLeft && Main.mouseLeftRelease/* && Terraria.GameInput.PlayerInput.Triggers.JustReleased.MouseLeft*/) {
                        //    //	texts[hoveredSnippet].OnClick();
                        //    //}
                        //}
                        position += baseLineDistance + content.ExtraLineDistance/*snippetListHeight*/;
                    }
                }
            }
            #endregion
        }
        public static List<List<TextSnippet>> WordwrapStringSmart(string text, float scale, Color c, DynamicSpriteFont font, int maxWidth, int maxLines) {
            TextSnippet[] array = ChatManager.ParseMessage(text, c).ToArray();
            List<List<TextSnippet>> finalList = new();
            List<TextSnippet> list2 = new();
            for (int i = 0; i < array.Length; i++) {
                TextSnippet textSnippet = array[i];
                string[] array2 = textSnippet.Text.Split(new char[]
                    {
                        '\n'
                    });
                for (int j = 0; j < array2.Length - 1; j++) {
                    list2.Add(textSnippet.CopyMorph(array2[j]));
                    finalList.Add(list2);
                    list2 = new List<TextSnippet>();
                }
                list2.Add(textSnippet.CopyMorph(array2[array2.Length - 1]));
            }
            finalList.Add(list2);
            if (maxWidth != -1) {
                for (int k = 0; k < finalList.Count; k++) {
                    List<TextSnippet> currentLine = finalList[k];
                    float usedWidth = 0f;
                    for (int l = 0; l < currentLine.Count; l++) {
                        //float stringLength = list3[l].GetStringLength(font); // GetStringLength doesn't match UniqueDraw
                        float stringLength = ChatManager.GetStringSize(font, new TextSnippet[] { currentLine[l] }, new Vector2(scale)).X;
                        //float stringLength2 = ChatManager.GetStringSize(font, " ", Vector2.One).X;
                        //float stringLength3 = ChatManager.GetStringSize(font, "1", Vector2.One).X;

                        if (stringLength + usedWidth > maxWidth) {
                            int num2 = maxWidth - (int)usedWidth;
                            if (usedWidth > 0f) {
                                num2 -= 16;
                            }
                            float toFill = num2;
                            bool filled = false;
                            int successfulIndex = -1;
                            int index = 0;
                            while (index < currentLine[l].Text.Length && !filled) {
                                if (currentLine[l].Text[index] == ' ' || IsChinese(currentLine[l].Text[index])) // 这里写换行条件
                                {
                                    if (ChatManager.GetStringSize(font, currentLine[l].Text.Substring(0, index), new Vector2(scale)).X < toFill)
                                        successfulIndex = index;
                                    else {
                                        filled = true;
                                        //if (successfulIndex == 0)
                                        //	successfulIndex = index;
                                    }
                                }
                                index++;
                            }
                            if (currentLine[l].Text.Length == 0) {
                                filled = true;
                            }
                            int num4 = successfulIndex;

                            if (successfulIndex == -1) // last item is too big
                            {
                                if (l == 0) // 1st item in list, keep it and move on
                                {
                                    //list2 = new List<TextSnippet>{currentLine[l]};
                                    list2 = new List<TextSnippet>();
                                    for (int m = l + 1; m < currentLine.Count; m++) {
                                        list2.Add(currentLine[m]);
                                    }
                                    finalList[k] = finalList[k].Take(/*l + */ 1).ToList(); // take 1
                                    finalList.Insert(k + 1, list2);
                                }
                                else // midway through list, keep previous and move this to next
                                {
                                    list2 = new List<TextSnippet>();
                                    for (int m = l; m < currentLine.Count; m++) {
                                        list2.Add(currentLine[m]);
                                    }
                                    finalList[k] = finalList[k].Take(l).ToList(); // take previous ones
                                    finalList.Insert(k + 1, list2);
                                }
                            }
                            else {
                                string newText = currentLine[l].Text.Substring(0, num4);
                                string newText2 = currentLine[l].Text.Substring(num4).TrimStart();
                                list2 = new List<TextSnippet>
                                {
                                    currentLine[l].CopyMorph(newText2)
                                };
                                for (int m = l + 1; m < currentLine.Count; m++) {
                                    list2.Add(currentLine[m]);
                                }
                                currentLine[l] = currentLine[l].CopyMorph(newText);
                                finalList[k] = finalList[k].Take(l + 1).ToList();
                                finalList.Insert(k + 1, list2);
                            }
                            break;
                        }
                        usedWidth += stringLength;
                    }
                }
            }
            if (maxLines != -1) {
                while (finalList.Count > 10) {
                    finalList.RemoveAt(10);
                }
            }
            return finalList;
        }

        public static readonly List<char> cnPuncs = new() {
            '–', '—', '‘', '’', '“', '”',
            '…', '、', '。', '〈', '〉', '《',
            '》', '「', '」', '『', '』', '【',
            '】', '〔', '〕', '！', '（', '）',
            '，', '．', '：', '；', '？'
        };
        public static bool IsChinese(char a) {
            return a >= 0x4E00 && a <= 0x9FA5 || cnPuncs.Contains(a);
        }
    }
}
