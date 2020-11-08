using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Entrogic.Common.PlayerDrawLayers
{
    public class BookBubbleLayer : PlayerDrawLayer
    {
        public override bool IsHeadLayer => false;

        //This sets the layer's parent. Layers don't get drawn if their parent layer is not visible, so smart use of this could help you improve compatibility with other mods.
        public override DrawLayer<PlayerDrawSet> Parent => HeadBack;

        //GetDefaults is called before the layer is queued for drawing, and lets us control the layer's default depth and visibility. Note that other modders may call this method on your layer too.
        public override void GetDefaults(PlayerDrawSet drawInfo, out bool visible, out LayerConstraint constraint)
        {
            visible = drawInfo.drawPlayer.GetModPlayer<EntrogicPlayer>().IsBookActive;
            constraint = new LayerConstraint(HeadBack, before: true);
        }
        public override void Draw(ref PlayerDrawSet drawInfo)
        {
            if (drawInfo.shadow != 0f)
            {
                return;
            }
            Player drawPlayer = drawInfo.drawPlayer;
            Texture2D t = Entrogic.ModTexturesTable["ReadingBubble"].Value;
            EntrogicPlayer entrogicPlayer = drawPlayer.GetModPlayer<EntrogicPlayer>();
            if (entrogicPlayer.IsBookActive) entrogicPlayer.UseBookBubble = true;
            if (entrogicPlayer.UseBookBubble)
            {
                if (!entrogicPlayer.IsClosingBook)
                {
                    entrogicPlayer.BookBubbleFrameCounter++;
                    if (entrogicPlayer.BookBubbleFrameCounter >= 6)
                    {
                        entrogicPlayer.BookBubbleFrameCounter = 0;
                        entrogicPlayer.BookBubbleFrame++;
                    }
                    if (entrogicPlayer.BookBubbleFrame >= 19) // 共19帧
                    {
                        entrogicPlayer.BookBubbleFrame = 4;
                    }
                }
                else
                {
                    entrogicPlayer.BookBubbleFrameCounter++;
                    if (entrogicPlayer.BookBubbleFrameCounter >= 6)
                    {
                        entrogicPlayer.BookBubbleFrameCounter = 0;
                        entrogicPlayer.BookBubbleFrame--;
                    }
                    if (entrogicPlayer.BookBubbleFrame == 0)
                    {
                        entrogicPlayer.BookBubbleFrame = 1;
                        entrogicPlayer.IsClosingBook = false;
                        entrogicPlayer.UseBookBubble = false;
                    }
                }
                // 帧切换结束
                if (!entrogicPlayer.IsBookActive) // 停止后并非立刻结束，而是有一个过渡动画
                {
                    int frame = entrogicPlayer.BookBubbleFrame;
                    bool IsOpeningBook = frame >= 1 && frame <= 3; // 前三帧是开书动画
                    bool IsTurningPage = frame >= 8 && frame <= 11 || frame >= 16 && frame <= 19;
                    bool IsStopping = !IsOpeningBook && !IsTurningPage;

                    if (IsStopping)
                    {
                        entrogicPlayer.IsClosingBook = true;
                    }
                }
                int totalFrames = 19;
                int frameHeight = t.Height / totalFrames; // 除出来应为64
                Rectangle _frame = new Rectangle(0, (entrogicPlayer.BookBubbleFrame - 1) * frameHeight, frameHeight, t.Width);
                Vector2 drawPosition = drawPlayer.Center - Main.screenPosition;
                drawPosition.Y -= 20f + frameHeight * 0.5f;
                Vector2 origin = new Vector2(t.Width / 2, frameHeight / 2);
                //Queues a drawing of a sprite. Do not use SpriteBatch in drawlayers!
                drawInfo.DrawDataCache.Add(new DrawData(
                    t, //The texture to render.
                    drawPosition.NoShake(), //Position to render at.
                    (Rectangle?)_frame, //Source rectangle.
                    Color.White,
                    0f,
                    origin,
                    1f,
                    SpriteEffects.None,
                    0
                ));
                if (ModHelper.MouseInRectangle(ModHelper.CreateFromVector2(drawPosition - origin, t.Width, frameHeight)) && drawPlayer.whoAmI != Main.myPlayer)
                {
                    Main.instance.MouseText($"{Language.GetTextValue("Mods.Entrogic.Common.Reading")}: {drawPlayer.HeldItem.Name}\n" +
                    $"{Language.GetTextValue("Mods.Entrogic.Common.Page")}: {entrogicPlayer.PageNum * 2 - 1} ~ {entrogicPlayer.PageNum * 2}");
                }
            }
        }
    }
}
