using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using Terraria.ID;
using ReLogic.Graphics;

using Entrogic.Items.Weapons.Card;
using System;
using System.Collections.Generic;
using static Terraria.ModLoader.ModContent;
using Entrogic.Items.Books;
using Terraria.UI.Chat;
using System.Linq;

namespace Entrogic.UI.Books
{
    public class BookUI : UIState
    {
        public UIPanel Book;//新建UI
        internal static bool IsActive = false;
        BookPanel BookPanel = new BookPanel(GetTexture("Entrogic/UI/Books/书本_01"), 1);
        Vector2 bookSize = new Vector2(600f, 480f);
        public override void OnInitialize()
        {
            Vector2 bookPos = new Vector2((Main.screenWidth - bookSize.X) / 2, (Main.screenHeight - bookSize.Y) / 2);

            BookPanel.Left.Set(bookPos.X, 0f);//UI距离左边
            BookPanel.Top.Set(bookPos.Y, 0f);//UI距离上面
            BookPanel.OnClick += new MouseEvent(Clicked);
            BookPanel.OnRightClick += new MouseEvent(RightClicked);
            BookPanel.Width.Set(bookSize.X, 0f);//UI的宽
            BookPanel.Height.Set(bookSize.Y, 0f);//UI的高
            Append(BookPanel);//添加UI
        }
        private void Clicked(UIMouseEvent evt, UIElement listeningElement)
        {
            Player player = Main.LocalPlayer;
            EntrogicPlayer ePlayer = player.GetModPlayer<EntrogicPlayer>();

            Vector2 bookPos = new Vector2((Main.screenWidth - bookSize.X) / 2, (Main.screenHeight - bookSize.Y) / 2);
            Rectangle rectLeft = new Rectangle((int)bookPos.X, (int)(bookPos.Y + bookSize.Y - 117), 72, 117);
            Rectangle rectRight = new Rectangle((int)(bookPos.X + bookSize.X - 72), (int)(bookPos.Y + bookSize.Y - 117), 72, 117);
            if (mouseRect.Intersects(rectLeft) && ePlayer.PageNum > 1)
            {
                Main.PlaySound(SoundID.MenuOpen);
                ePlayer.PageNum -= 1;
            }
            int MaxPage = 1;
            if (ePlayer.GetHoldItem() != null)
                if (ePlayer.GetHoldItem().GetGlobalItem<EntrogicItem>().book)
                    MaxPage = ((ModBook)ePlayer.GetHoldItem().modItem).MaxPage;
            if (mouseRect.Intersects(rectRight) && ePlayer.PageNum < MaxPage)
            {
                Main.PlaySound(SoundID.MenuOpen);
                ePlayer.PageNum += 1;
            }
        }
        private void RightClicked(UIMouseEvent evt, UIElement listeningElement)
        {
            EntrogicPlayer plr = Main.LocalPlayer.GetModPlayer<EntrogicPlayer>();
            IsActive = false;
        }
        public Rectangle mouseRect
        {
            get
            {
                return new Rectangle(Main.mouseX, Main.mouseY, 1, 1);
            }
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Player player = Main.LocalPlayer;
            EntrogicPlayer ePlayer = player.GetModPlayer<EntrogicPlayer>();
            if (ePlayer.GetHoldItem() == null)
            {
                IsActive = false;
                return;
            }
            if (!ePlayer.GetHoldItem().GetGlobalItem<EntrogicItem>().book)
            {
                IsActive = false;
                return;
            }
            // 开了物品栏就不能让你看书了哦
            if (Main.playerInventory)
            {
                IsActive = false;
                return;
            }
            BookPanel.num = ePlayer.PageNum;
            Vector2 bookPos = new Vector2((Main.screenWidth - bookSize.X) / 2, (Main.screenHeight - bookSize.Y) / 2);
            BookPanel.Left.Pixels = bookPos.X;//UI距离左边
            BookPanel.Top.Pixels = bookPos.Y;//UI距离上面

            int MaxPage = 1;
            if (ePlayer.GetHoldItem() != null)
                if (ePlayer.GetHoldItem().GetGlobalItem<EntrogicItem>().book)
                    MaxPage = ((ModBook)ePlayer.GetHoldItem().modItem).MaxPage;
            if (ePlayer.PageNum > MaxPage)
                ePlayer.PageNum = MaxPage;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Player player = Main.LocalPlayer;
            EntrogicPlayer ePlayer = player.GetModPlayer<EntrogicPlayer>();
            if (IsActive)
            {
                base.Draw(spriteBatch);

                Vector2 bookPos = new Vector2((Main.screenWidth - bookSize.X) / 2, (Main.screenHeight - bookSize.Y) / 2);
                Rectangle rectLeft = new Rectangle((int)bookPos.X, (int)(bookPos.Y + bookSize.Y - 117), 72, 117);
                Rectangle rectRight = new Rectangle((int)(bookPos.X + bookSize.X - 72), (int)(bookPos.Y + bookSize.Y - 117), 72, 117);
                if (mouseRect.Intersects(rectLeft) && ePlayer.PageNum > 1)
                {
                    spriteBatch.Draw(Entrogic.Instance.GetTexture("UI/Books/书本_03"), new Vector2(rectLeft.X, rectLeft.Y), null, Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
                }
                else
                {
                    string page = (ePlayer.PageNum * 2 - 1).ToString();
                    Vector2 size = Main.fontMouseText.MeasureString(page) * 0.5f;
                    ChatManager.DrawColorCodedStringWithShadow(spriteBatch, Main.fontItemStack, page, new Vector2(bookPos.X + 34 - size.X, bookPos.Y + bookSize.Y - 86 - size.Y), Color.White, 0f, Vector2.Zero, new Vector2(1f));
                }
                int MaxPage = 1;
                if (ePlayer.GetHoldItem() != null)
                    if (ePlayer.GetHoldItem().GetGlobalItem<EntrogicItem>().book)
                        MaxPage = ((ModBook)ePlayer.GetHoldItem().modItem).MaxPage;
                if (mouseRect.Intersects(rectRight) && ePlayer.PageNum < MaxPage)
                {
                    spriteBatch.Draw(Entrogic.Instance.GetTexture("UI/Books/书本_02"), new Vector2(rectRight.X, rectRight.Y), null, Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
                }
                else
                {
                    string page = (ePlayer.PageNum * 2).ToString();
                    Vector2 size = Main.fontMouseText.MeasureString(page) * 0.5f;
                    ChatManager.DrawColorCodedStringWithShadow(spriteBatch, Main.fontItemStack, page, new Vector2(bookPos.X + bookSize.X - 34 - size.X, bookPos.Y + bookSize.Y - 86 - size.Y), Color.White, 0f, Vector2.Zero, new Vector2(1f));
                }

                #region Draw Text
                Vector2 space = new Vector2((Main.screenWidth - bookSize.X) / 2 + 38, (Main.screenHeight - bookSize.Y) / 2 + 46);
                //Main.spriteBatch.Draw(Main.magicPixel, space.ToRectangle(), Color.Yellow * .7f);
                //Main.spriteBatch.Draw(Main.magicPixel, GetOuterDimensions().ToRectangle(), Color.Red * .7f);
                DynamicSpriteFont font = Entrogic.PixelFont;
                float scale = 1f;
                string text = " ";
                Color baseColor = Color.White;
                float lineDistance = 0f;
                Item playerHeldItem = player.HeldItem;
                if (Entrogic.ItemSafe(playerHeldItem))
                    if (playerHeldItem.GetGlobalItem<EntrogicItem>().book)
                    {
                        ModBook itemBook = (ModBook)playerHeldItem.modItem;
                        if (itemBook.PageText[ePlayer.PageNum * 2 - 1] != null)
                            text = itemBook.PageText[ePlayer.PageNum * 2 - 1];
                        //if (((ModBook)ePlayer.GetHoldItem().modItem).font != null)
                        //    font = ((ModBook)ePlayer.GetHoldItem().modItem).font;
                        if (itemBook.textBaseColor != null)
                            baseColor = itemBook.textBaseColor;
                        lineDistance = itemBook.lineDistance[ePlayer.PageNum * 2 - 1];
                        scale = itemBook.textScale[ePlayer.PageNum * 2 - 1];
                    }
                float baseLineDistance = 46f * scale;
                { // 左书页文字
                    List<List<TextSnippet>> drawTextSnippets = WordwrapStringSmart(text, scale, baseColor, font, (int)(bookSize.X * 0.5f) - 72, -1);
                    float position = 0f;
                    TextSnippet[] texts;
                    foreach (var snippetList in drawTextSnippets)
                    {
                        texts = snippetList.ToArray();
                        //float snippetListHeight = ChatManager.GetStringSize(font, texts, new Vector2(scale)).Y + lineDistance;
                        //Main.NewText($"Y: {ChatManager.GetStringSize(font, texts, Vector2.One).X}");
                        if (position > -baseLineDistance + lineDistance/*snippetListHeight*/)
                        {
                            //foreach (var snippet in snippetList)
                            //{
                            int hoveredSnippet = -1;
                            ChatManager.ConvertNormalSnippets(texts);
                            ChatManager.DrawColorCodedString(spriteBatch, font, texts, new Vector2(space.X, space.Y + position /*+ offset*/), baseColor, 0f, Vector2.Zero, new Vector2(scale), out hoveredSnippet, -1f);
                            //offset += 20;
                            //offset += texts.Max(t => (int)ChatManager.GetStringSize(Main.fontMouseText, texts, Vector2.One).X);
                            //if (hoveredSnippet > -1 && IsMouseHovering)
                            //{
                            //    BossChecklist.instance.BossLog.hoveredTextSnippet = texts[hoveredSnippet];
                            //    // BossChecklist change: Use hoveredTextSnippet to bypass clippingRectangle and draw order issues.
                            //    //texts[hoveredSnippet].OnHover();
                            //    //if (Main.mouseLeft && Main.mouseLeftRelease/* && Terraria.GameInput.PlayerInput.Triggers.JustReleased.MouseLeft*/) {
                            //    //	texts[hoveredSnippet].OnClick();
                            //    //}
                            //}
                        }
                        position += baseLineDistance + lineDistance/*snippetListHeight*/;
                        if (position > bookSize.Y - 48)
                            break;
                        //}
                    }
                }
                { // 右书页文字
                    if (ePlayer.GetHoldItem() != null)
                        if (ePlayer.GetHoldItem().GetGlobalItem<EntrogicItem>().book)
                        {
                            if (((ModBook)ePlayer.GetHoldItem().modItem).PageText[ePlayer.PageNum * 2] != null)
                                text = ((ModBook)ePlayer.GetHoldItem().modItem).PageText[ePlayer.PageNum * 2];
                            lineDistance = ((ModBook)ePlayer.GetHoldItem().modItem).lineDistance[ePlayer.PageNum * 2];
                            scale = ((ModBook)ePlayer.GetHoldItem().modItem).textScale[ePlayer.PageNum * 2];
                        }

                    List<List<TextSnippet>> drawTextSnippets = WordwrapStringSmart(text, scale, baseColor, font, (int)(bookSize.X * 0.5f) - 78, -1);
                    float position = 0f;
                    TextSnippet[] texts;
                    foreach (var snippetList in drawTextSnippets)
                    {
                        texts = snippetList.ToArray();
                        //float snippetListHeight = ChatManager.GetStringSize(font, texts, new Vector2(scale)).Y + lineDistance;
                        //Main.NewText($"Y: {ChatManager.GetStringSize(font, texts, Vector2.One).X}");
                        if (position > -baseLineDistance + lineDistance/*snippetListHeight*/)
                        {
                            //foreach (var snippet in snippetList)
                            //{
                            int hoveredSnippet = -1;
                            ChatManager.ConvertNormalSnippets(texts);
                            ChatManager.DrawColorCodedString(spriteBatch, font, texts, new Vector2(space.X + bookSize.X * 0.5f, space.Y + position /*+ offset*/), baseColor, 0f, Vector2.Zero, new Vector2(scale), out hoveredSnippet, -1f);
                            //offset += 20;
                            //offset += texts.Max(t => (int)ChatManager.GetStringSize(Main.fontMouseText, texts, Vector2.One).X);
                            //if (hoveredSnippet > -1 && IsMouseHovering)
                            //{
                            //    BossChecklist.instance.BossLog.hoveredTextSnippet = texts[hoveredSnippet];
                            //    // BossChecklist change: Use hoveredTextSnippet to bypass clippingRectangle and draw order issues.
                            //    //texts[hoveredSnippet].OnHover();
                            //    //if (Main.mouseLeft && Main.mouseLeftRelease/* && Terraria.GameInput.PlayerInput.Triggers.JustReleased.MouseLeft*/) {
                            //    //	texts[hoveredSnippet].OnClick();
                            //    //}
                            //}
                        }
                        position += baseLineDistance + lineDistance/*snippetListHeight*/;
                        if (position > bookSize.Y - 48)
                            break;
                        //}
                    }
                }
                #endregion
            }
        }
        public static List<List<TextSnippet>> WordwrapStringSmart(string text, float scale, Color c, DynamicSpriteFont font, int maxWidth, int maxLines)
        {
            TextSnippet[] array = ChatManager.ParseMessage(text, c).ToArray();
            List<List<TextSnippet>> finalList = new List<List<TextSnippet>>();
            List<TextSnippet> list2 = new List<TextSnippet>();
            for (int i = 0; i < array.Length; i++)
            {
                TextSnippet textSnippet = array[i];
                string[] array2 = textSnippet.Text.Split(new char[]
                    {
                        '\n'
                    });
                for (int j = 0; j < array2.Length - 1; j++)
                {
                    list2.Add(textSnippet.CopyMorph(array2[j]));
                    finalList.Add(list2);
                    list2 = new List<TextSnippet>();
                }
                list2.Add(textSnippet.CopyMorph(array2[array2.Length - 1]));
            }
            finalList.Add(list2);
            if (maxWidth != -1)
            {
                for (int k = 0; k < finalList.Count; k++)
                {
                    List<TextSnippet> currentLine = finalList[k];
                    float usedWidth = 0f;
                    for (int l = 0; l < currentLine.Count; l++)
                    {
                        //float stringLength = list3[l].GetStringLength(font); // GetStringLength doesn't match UniqueDraw
                        float stringLength = ChatManager.GetStringSize(font, new TextSnippet[] { currentLine[l] }, new Vector2(scale)).X;
                        //float stringLength2 = ChatManager.GetStringSize(font, " ", Vector2.One).X;
                        //float stringLength3 = ChatManager.GetStringSize(font, "1", Vector2.One).X;

                        if (stringLength + usedWidth > (float)maxWidth)
                        {
                            int num2 = maxWidth - (int)usedWidth;
                            if (usedWidth > 0f)
                            {
                                num2 -= 16;
                            }
                            float toFill = num2;
                            bool filled = false;
                            int successfulIndex = -1;
                            int index = 0;
                            while (index < currentLine[l].Text.Length && !filled)
                            {
                                if (currentLine[l].Text[index] == ' ' || isChinese(currentLine[l].Text[index])) // 这里写换行条件
                                {
                                    if (ChatManager.GetStringSize(font, currentLine[l].Text.Substring(0, index), new Vector2(scale)).X < toFill)
                                        successfulIndex = index;
                                    else
                                    {
                                        filled = true;
                                        //if (successfulIndex == 0)
                                        //	successfulIndex = index;
                                    }
                                }
                                index++;
                            }
                            if (currentLine[l].Text.Length == 0)
                            {
                                filled = true;
                            }
                            int num4 = successfulIndex;

                            if (successfulIndex == -1) // last item is too big
                            {
                                if (l == 0) // 1st item in list, keep it and move on
                                {
                                    //list2 = new List<TextSnippet>{currentLine[l]};
                                    list2 = new List<TextSnippet>();
                                    for (int m = l + 1; m < currentLine.Count; m++)
                                    {
                                        list2.Add(currentLine[m]);
                                    }
                                    finalList[k] = finalList[k].Take(/*l + */ 1).ToList<TextSnippet>(); // take 1
                                    finalList.Insert(k + 1, list2);
                                }
                                else // midway through list, keep previous and move this to next
                                {
                                    list2 = new List<TextSnippet>();
                                    for (int m = l; m < currentLine.Count; m++)
                                    {
                                        list2.Add(currentLine[m]);
                                    }
                                    finalList[k] = finalList[k].Take(l).ToList<TextSnippet>(); // take previous ones
                                    finalList.Insert(k + 1, list2);
                                }
                            }
                            else
                            {
                                string newText = currentLine[l].Text.Substring(0, num4);
                                string newText2 = currentLine[l].Text.Substring(num4).TrimStart();
                                list2 = new List<TextSnippet>
                                {
                                    currentLine[l].CopyMorph(newText2)
                                };
                                for (int m = l + 1; m < currentLine.Count; m++)
                                {
                                    list2.Add(currentLine[m]);
                                }
                                currentLine[l] = currentLine[l].CopyMorph(newText);
                                finalList[k] = finalList[k].Take(l + 1).ToList<TextSnippet>();
                                finalList.Insert(k + 1, list2);
                            }
                            break;
                        }
                        usedWidth += stringLength;
                    }
                }
            }
            if (maxLines != -1)
            {
                while (finalList.Count > 10)
                {
                    finalList.RemoveAt(10);
                }
            }
            return finalList;
        }
        public static bool isChinese(char a)
        {
            return a >= 0x4E00 && a <= 0x9FA5;
        }
    }
    public class BookPanel : UIImage
    {
        public Texture2D tex = GetTexture("Entrogic/UI/Books/书本_01");
        public int num = 1;
        public BookPanel(Texture2D texture, int number) : base(texture)
        {
            tex = texture;
            num = number;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Player player = Main.LocalPlayer;
            EntrogicPlayer ePlayer = player.GetModPlayer<EntrogicPlayer>();
            if (!BookUI.IsActive)
                return;
            if (ePlayer.GetHoldItem() != null)
                if (ePlayer.GetHoldItem().GetGlobalItem<EntrogicItem>().book)
                    if (((ModBook)ePlayer.GetHoldItem().modItem).PageTexture[num] != null)
                        tex = ((ModBook)ePlayer.GetHoldItem().modItem).PageTexture[num];
            float alpha = 1.0f;
            //base.Draw(spriteBatch); // 不使用原绘制方案
            ChatManager.DrawColorCodedString(spriteBatch, Main.fontMouseText, ePlayer.PageNum.ToString(), new Vector2(Left.Pixels, Top.Pixels), Color.White, 0f, Vector2.Zero, Vector2.One);
            spriteBatch.Draw(tex, new Vector2(Left.Pixels, Top.Pixels), null, new Color(alpha, alpha, alpha, alpha), 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
        }
        public override void Update(GameTime gameTime)
        {
            Player player = Main.LocalPlayer;
            EntrogicPlayer ePlayer = player.GetModPlayer<EntrogicPlayer>();
            base.Update(gameTime); // don't remove.
            if (ContainsPoint(Main.MouseScreen) && BookUI.IsActive)
            {
                Main.LocalPlayer.mouseInterface = true;
            }
        }
    }
}
