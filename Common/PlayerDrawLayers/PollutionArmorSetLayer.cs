using Entrogic.Items.PollutElement.Armor;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Entrogic.Common.PlayerDrawLayers
{
    public class PollutionArmorSetLayer : PlayerDrawLayer
	{
		public override bool IsHeadLayer => false;

		//This sets the layer's parent. Layers don't get drawn if their parent layer is not visible, so smart use of this could help you improve compatibility with other mods.
		public override DrawLayer<PlayerDrawSet> Parent => Leggings;

		//GetDefaults is called before the layer is queued for drawing, and lets us control the layer's default depth and visibility. Note that other modders may call this method on your layer too.
		public override void GetDefaults(PlayerDrawSet drawInfo, out bool visible, out LayerConstraint constraint)
        {
            PollutionArmorSet modPlayer = drawInfo.drawPlayer.GetModPlayer<PollutionArmorSet>();
            visible = modPlayer.pEffect && modPlayer.EffectEnable;
			constraint = new LayerConstraint(Leggings, before: true);
		}
		public override void Draw(ref PlayerDrawSet drawInfo)
        {
            if (drawInfo.shadow != 0f)
            {
                return;
            }
            Player drawPlayer = drawInfo.drawPlayer;
            PollutionArmorSet modPlayer = drawPlayer.GetModPlayer<PollutionArmorSet>();
            Texture2D texture = Entrogic.ModTexturesTable["PollutionArmorSetEffect"].Value;
            int frameSize = texture.Height / 3;
            int drawX = (int)(drawInfo.Center.X - Main.screenPosition.X);
            int drawY = (int)(drawInfo.Center.Y - Main.screenPosition.Y);

            float num = 0.1f;
            float num2 = 0.9f;
            modPlayer.fRingScale += 0.004f;
            float flameRingScale;
            if (modPlayer.fRingScale < 1f)
            {
                flameRingScale = modPlayer.fRingScale;
            }
            else
            {
                modPlayer.fRingScale = 0.8f;
                flameRingScale = modPlayer.fRingScale;
            }
            modPlayer.flameRingRot += 0.05f;
            if (modPlayer.flameRingRot > 6.28318548f)
            {
                modPlayer.flameRingRot -= 6.28318548f;
            }
            if (modPlayer.flameRingRot < -6.28318548f)
            {
                modPlayer.flameRingRot += 6.28318548f;
            }
            float num3 = flameRingScale + num;
            if (num3 > 1f)
            {
                num3 -= num * 2f;
            }
            float num4 = MathHelper.Lerp(0.8f, 0f, Math.Abs(num3 - num2) * 10f);
            //Queues a drawing of a sprite. Do not use SpriteBatch in drawlayers!
            drawInfo.DrawDataCache.Add(new DrawData(
                texture, //The texture to render.
                new Vector2(drawX, drawY), //Position to render at.
                new Rectangle(0, 0, texture.Width, frameSize), //Source rectangle.
                new Color(num4, num4, num4, num4 / 2f), //Color.
                modPlayer.flameRingRot, //Rotation.
                new Vector2(texture.Width / 2f, frameSize / 2f), //Origin. Uses the texture's center.
                num3 * 0.6f, //Scale.
                SpriteEffects.None, //SpriteEffects.
                0 //'Layer'. This is always 0 in Terraria.
            ));
            modPlayer.fRingScale = 1f;
        }
	}
}
