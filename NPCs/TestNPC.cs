using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.NPCs
{
    [AutoloadBossHead]
    public class TestNPC : ModNPC
    {
        public override void SetDefaults()
        {
            npc.Size = new Vector2(80f);
            npc.noGravity = npc.noTileCollide = true;
            npc.lifeMax = 1000;
            npc.boss = true;
            npc.defense = 10;
            npc.knockBackResist = 0f;
            npc.aiStyle = -1;
            npc.value = Item.buyPrice(0, 7, 0, 0);
            npc.HitSound = SoundID.NPCHit3;
            npc.DeathSound = SoundID.NPCDeath37;
            npc.timeLeft = NPC.activeTime * 30;
            npc.alpha = 128;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Main.NewText(drawColor);
            npc.rotation += 0.1f;
            spriteBatch.Draw(((Texture2D)Terraria.GameContent.TextureAssets.Npc[npc.type]), npc.Center - Main.screenPosition, null, new Color(drawColor.R, drawColor.G, drawColor.B, npc.alpha), npc.rotation, ((Texture2D)Terraria.GameContent.TextureAssets.Npc[npc.type]).Size() * 0.5f, 1f, SpriteEffects.None, 0f);
            return false;
        }
    }
}
