using Entrogic.Common.Globals.Players;

namespace Entrogic.Common.PlayerLayers
{
    public class BookBubbleLayer : PlayerDrawLayer
    {
        private Asset<Texture2D> Texture => TextureManager.BookBubble;

        public override bool IsHeadLayer => false;
        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) {
            BookInfoPlayer book = drawInfo.drawPlayer.GetModPlayer<BookInfoPlayer>();
            return book.IsReading || book.UseBookBubble;

            // If you'd like to reference another PlayerDrawLayer's visibility,
            // you can do so by getting its instance via ModContent.GetInstance<OtherDrawLayer>(), and calling GetDefaultVisibility on it
        }

        public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.Head);

        protected override void Draw(ref PlayerDrawSet drawInfo) {
            if (drawInfo.shadow != 0f) return;

            Player drawPlayer = drawInfo.drawPlayer;
            BookInfoPlayer book = drawPlayer.GetModPlayer<BookInfoPlayer>();
            if (book.IsReading) book.UseBookBubble = true;
            if (book.UseBookBubble) {
                if (!book.IsClosingBook) {
                    book.BookBubbleFrameCounter++;
                    if (book.BookBubbleFrameCounter >= 6) {
                        book.BookBubbleFrameCounter = 0;
                        book.BookBubbleFrame++;
                    }
                    if (book.BookBubbleFrame >= 19) // 共19帧
                    {
                        book.BookBubbleFrame = 4;
                    }
                }
                else {
                    book.BookBubbleFrameCounter++;
                    if (book.BookBubbleFrameCounter >= 6) {
                        book.BookBubbleFrameCounter = 0;
                        book.BookBubbleFrame--;
                    }
                    if (book.BookBubbleFrame == 0) {
                        book.BookBubbleFrame = 1;
                        book.IsClosingBook = false;
                        book.UseBookBubble = false;
                    }
                }
                // 帧切换结束
                if (!book.IsReading) // 停止后并非立刻结束，而是有一个过渡动画
                {
                    int frame = book.BookBubbleFrame;
                    bool IsOpeningBook = frame >= 1 && frame <= 3; // 前三帧是开书动画
                    bool IsTurningPage = frame >= 8 && frame <= 11 || frame >= 16 && frame <= 19;
                    bool IsStopping = !IsOpeningBook && !IsTurningPage;

                    if (IsStopping) {
                        book.IsClosingBook = true;
                    }
                }

                int totalFrames = 19;
                int frameHeight = Texture.Height() / totalFrames; // 除出来应为64

                Rectangle _frame = new(0, (book.BookBubbleFrame - 1) * frameHeight, frameHeight, Texture.Width());
                Vector2 drawPosition = drawPlayer.Center - Main.screenPosition;
                drawPosition.Y -= 20f + frameHeight * 0.5f;

                Vector2 origin = new(Texture.Width() / 2, frameHeight / 2);
                //Queues a drawing of a sprite. Do not use SpriteBatch in drawlayers!
                drawInfo.DrawDataCache.Add(new DrawData(
                    Texture.Value, //The texture to render.
                    drawPosition.Floor(), //Position to render at.
                    (Rectangle?)_frame, //Source rectangle.
                    Color.White,
                    0f,
                    origin,
                    1f,
                    SpriteEffects.None,
                    0
                ));
                if (ModHelper.MouseInRectangle(ModHelper.CreateFromVector2(drawPosition - origin, Texture.Width(), frameHeight)) && drawPlayer.whoAmI != Main.myPlayer) {
                    Main.instance.MouseText($"{Language.GetTextValue("Mods.Entrogic.Common.Reading")}: {book.BookName}\n" +
                    $"{Language.GetTextValue("Mods.Entrogic.Common.Page")}: {book.CurrentPage * 2 - 1} ~ {book.CurrentPage * 2}");
                }
            }
        }
    }
}
