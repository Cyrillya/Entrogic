using Entrogic.Common.Globals.Players;
using Entrogic.Interfaces.UIElements;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using Terraria.UI.Chat;

namespace Entrogic.Interfaces.GUI
{
    public class BookGUI : UIState
    {
        public UIPanel Book;//新建UI
        internal static List<BookContent> Contents = new();
        internal static Asset<Texture2D> Texture = TextureManager.BookPanel;
        internal static int MaxPages = 1;

        private UIImage _bookPanel;
        private readonly Vector2 _bookSize = new(600f, 480f);
        private string _lastBookName = "";

        private static Rectangle MouseRect => new(Main.mouseX, Main.mouseY, 1, 1);

        public override void OnInitialize() {
            Vector2 bookPos = new((Main.screenWidth - _bookSize.X) / 2, (Main.screenHeight - _bookSize.Y) / 2);

            if (!Main.dedServ)
                _bookPanel = new UIImage(ModContent.Request<Texture2D>($"{TextureManager.BookAssets}Panel"));
            _bookPanel.Left.Set(bookPos.X, 0f); // UI距离左边
            _bookPanel.Top.Set(bookPos.Y, 0f); // UI距离上面
            _bookPanel.OnClick += Clicked; // 尝试换页
            _bookPanel.OnRightClick += (_, _) => CloseBook(Main.LocalPlayer.GetModPlayer<BookInfoPlayer>()); // 右键关闭书籍
            _bookPanel.Width.Set(_bookSize.X, 0f); // UI的宽
            _bookPanel.Height.Set(_bookSize.Y, 0f); // UI的高
            Append(_bookPanel); // 添加UI
        }

        internal void Unload() {
            Texture = null;
            _bookPanel = null;
            Contents.Clear();
        }

        private void Clicked(UIMouseEvent evt, UIElement listeningElement) {
            var player = Main.LocalPlayer;
            var bookPlayer = player.GetModPlayer<BookInfoPlayer>();

            Vector2 bookPos = new((Main.screenWidth - _bookSize.X) / 2, (Main.screenHeight - _bookSize.Y) / 2);
            Rectangle rectLeft = new((int)bookPos.X, (int)(bookPos.Y + _bookSize.Y - 117), 72, 117);
            Rectangle rectRight = new((int)(bookPos.X + _bookSize.X - 72), (int)(bookPos.Y + _bookSize.Y - 117), 72, 117);
            if (MouseRect.Intersects(rectLeft) && bookPlayer.CurrentPage > 1) {
                SoundEngine.PlaySound(SoundID.MenuOpen);
                bookPlayer.CurrentPage -= 1;
                SendInfo(Main.LocalPlayer);
            }
            int maxPage = 1;
            if (player.HeldItem?.ModItem is ItemBook itemBook)
                maxPage = itemBook.PageMax;
            if (MouseRect.Intersects(rectRight) && bookPlayer.CurrentPage < maxPage) {
                SoundEngine.PlaySound(SoundID.MenuOpen);
                bookPlayer.CurrentPage += 1;
                SendInfo(Main.LocalPlayer);
            }
        }

        private void UpdatePageInfos() {
            var player = Main.LocalPlayer;
            var book = player.GetModPlayer<BookInfoPlayer>();
            var item = player.HeldItem.ModItem as ItemBook;
            // 若并非书籍则不会执行本方法，故无需判断
            book.BookName = item.Name;
            Texture = TextureManager.BookPanel;
            IModifyBookContent.Invoke(item.Item, player, ref Contents);
            IModifyBookPanel.Invoke(item.Item, player, ref Texture);
            _bookPanel.SetImage(Texture);
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

            if (new Rectangle((int)_bookPanel.Left.Pixels, (int)_bookPanel.Top.Pixels, (int)_bookSize.X, (int)_bookSize.Y).Intersects(MouseRect)) {
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
            Vector2 bookPos = new((Main.screenWidth - _bookSize.X) / 2, (Main.screenHeight - _bookSize.Y) / 2);
            _bookPanel.Left.Pixels = bookPos.X;//UI距离左边
            _bookPanel.Top.Pixels = bookPos.Y;//UI距离上面

            UpdatePageInfos();
        }

        public override void Draw(SpriteBatch spriteBatch) {
            var player = Main.LocalPlayer;
            var book = player.GetModPlayer<BookInfoPlayer>();
            
            base.Draw(spriteBatch);

            Vector2 bookPos = new((Main.screenWidth - _bookSize.X) / 2, (Main.screenHeight - _bookSize.Y) / 2);
            Rectangle rectLeft = new((int)bookPos.X, (int)(bookPos.Y + _bookSize.Y - 117), 72, 117);
            Rectangle rectRight = new((int)(bookPos.X + _bookSize.X - 72), (int)(bookPos.Y + _bookSize.Y - 117), 72, 117);

            #region 文字与图像绘制

            var textContents = from c in Contents where c is TextContent select c as TextContent;
            var imageContents = from c in Contents where c is ImageContent select c as ImageContent;

            //Main.spriteBatch.Draw(Main.magicPixel, space.ToRectangle(), Color.Yellow * .7f);
            //Main.spriteBatch.Draw(Main.magicPixel, GetOuterDimensions().ToRectangle(), Color.Red * .7f);
            
            // 重新开启spriteBatch，以去掉不明觉厉的UI绘制的一层模糊滤镜
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, null, null, null, Main.UIScaleMatrix);
            
            foreach (var content in imageContents) {
                var pos = _bookPanel.GetDimensions().Position() + content.Position;
                Main.spriteBatch.Draw(content.Texture.Value, pos, null, Color.White, 0f, Vector2.Zero, content.Scale,
                    SpriteEffects.None, 0f);
            }

            // 还原
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.SamplerStateForCursor, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.UIScaleMatrix);
            
            foreach (var content in textContents) {
                var font = content.Font == null ? FontAssets.MouseText.Value : content.Font.Value;
                var snippets = ModHelper.WordwrapString(content.Text, content.Scale, font, content.TextWarpWidth, content.Color);
                var pos = _bookPanel.GetDimensions().Position() + content.Position;
                ChatManager.DrawColorCodedString(Main.spriteBatch, font, snippets.ToArray(), pos, content.Color, 0, Vector2.Zero, Vector2.One * content.Scale, out _, -1);
            }
            #endregion

            #region 当前页角标

            if (IDrawReading.Invoke(spriteBatch, player.HeldItem, player)) {
                if (MouseRect.Intersects(rectLeft) && book.CurrentPage > 1) {
                    spriteBatch.Draw(TextureManager.BookBack.Value, new Vector2(rectLeft.X, rectLeft.Y), null, Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
                }
                else {
                    string page = (book.CurrentPage * 2 - 1).ToString();
                    Vector2 size = FontAssets.MouseText.Value.MeasureString(page) * 0.5f;
                    ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.ItemStack.Value, page, new Vector2(bookPos.X + 34 - size.X, bookPos.Y + _bookSize.Y - 86 - size.Y), Color.White, 0f, Vector2.Zero, new Vector2(1f));
                }
                if (MouseRect.Intersects(rectRight) && book.CurrentPage < MaxPages) {
                    spriteBatch.Draw(TextureManager.BookNext.Value, new Vector2(rectRight.X, rectRight.Y), null, Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
                }
                else {
                    string page = (book.CurrentPage * 2).ToString();
                    Vector2 size = FontAssets.MouseText.Value.MeasureString(page) * 0.5f;
                    ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.ItemStack.Value, page, new Vector2(bookPos.X + _bookSize.X - 34 - size.X, bookPos.Y + _bookSize.Y - 86 - size.Y), Color.White, 0f, Vector2.Zero, new Vector2(1f));
                }
            }

            #endregion
        }
    }
}
