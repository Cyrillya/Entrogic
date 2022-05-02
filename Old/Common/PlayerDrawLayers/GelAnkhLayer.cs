using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Entrogic.Common.PlayerDrawLayers
{
    public class GelAnkhLayer : PlayerDrawLayer
	{
		public override bool IsHeadLayer => false;

		//This sets the layer's parent. Layers don't get drawn if their parent layer is not visible, so smart use of this could help you improve compatibility with other mods.
		public override DrawLayer<PlayerDrawSet> Parent => FaceAcc;

		//GetDefaults is called before the layer is queued for drawing, and lets us control the layer's default depth and visibility. Note that other modders may call this method on your layer too.
		public override void GetDefaults(PlayerDrawSet drawInfo, out bool visible, out LayerConstraint constraint)
		{
			visible = drawInfo.drawPlayer.GetModPlayer<EntrogicPlayer>().RebornEffectTime > 0;
			constraint = new LayerConstraint(FaceAcc, before: true);
		}
        public override void Draw(ref PlayerDrawSet drawInfo)
		{
			if (drawInfo.shadow != 0f)
			{
				return;
			}
			Player drawPlayer = drawInfo.drawPlayer;
			EntrogicPlayer entrogicPlayer = drawPlayer.GetModPlayer<EntrogicPlayer>();
			if (entrogicPlayer.RebornEffectTime > 0)
			{
				entrogicPlayer.RebornEffectTime--;
				if (entrogicPlayer.RebornEffectTime > 80)
					entrogicPlayer.AnkhScale += 0.08f;
				if (entrogicPlayer.RebornEffectTime <= 25)
					entrogicPlayer.AnkhAlpha += 0.05f;
				if (entrogicPlayer.AnkhAlpha > 1f)
					entrogicPlayer.AnkhAlpha = 1f;
				Texture2D texture = Entrogic.ModTexturesTable["凝胶安卡"].Value;
				int drawX = (int)(drawInfo.Center.X - Main.screenPosition.X);
				int drawY = (int)(drawInfo.Center.Y - Main.screenPosition.Y);

				//Queues a drawing of a sprite. Do not use SpriteBatch in drawlayers!
				drawInfo.DrawDataCache.Add(new DrawData(
					texture, //The texture to render.
					new Vector2(drawX, drawY), //Position to render at.
					null, //Source rectangle.
					Color.White * entrogicPlayer.AnkhAlpha, //Color.
					0f, //Rotation.
					texture.Size() * 0.5f, //Origin. Uses the texture's center.
					entrogicPlayer.AnkhScale, //Scale.
					SpriteEffects.None, //SpriteEffects.
					0 //'Layer'. This is always 0 in Terraria.
				));
			}
		}
	}
}

