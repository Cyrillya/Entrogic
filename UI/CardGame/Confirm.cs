using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Entrogic.UI.CardGame
{
    public class Confirm : SpriteButton
    {
        public Confirm() : base((Texture2D)ModContent.GetTexture(typeof(Confirm).FullName.Replace('.', '/')))
        {
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            int frameHeight = buttonImage.Height / 3;
            Rectangle frame = new Rectangle(0, 0, buttonImage.Width, frameHeight);
            uiHeight = frameHeight;
            Player clientPlayer = Main.LocalPlayer;
            EntrogicPlayer clientModPlayer = EntrogicPlayer.ModPlayer(clientPlayer);
            if (ModHelper.MouseInRectangle(uiRectangle) && clientModPlayer.CardGamePlayerTurn)
            {
                Main.hoverItemName = "确认";
                frame = new Rectangle(0, 1 * frameHeight, buttonImage.Width, frameHeight);
            }
            if (!clientModPlayer.CardGamePlayerTurn)
                frame = new Rectangle(0, 2 * frameHeight, buttonImage.Width, frameHeight);
            spriteBatch.Draw(buttonImage, finalUIPosition, (Rectangle?)frame, uiColor);
        }
        public override void ClickEvent()
        {
            Player clientPlayer = Main.LocalPlayer;
            EntrogicPlayer clientModPlayer = EntrogicPlayer.ModPlayer(clientPlayer);
            if (clientModPlayer.CardGamePlayerTurn)
            {
                Entrogic.Instance.CardGameUI.PlayerTurnOver = true;
                Terraria.Audio.SoundEngine.PlaySound(SoundID.MenuTick);
            }
        }
    }
}
