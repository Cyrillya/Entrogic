using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Entrogic.Common;

namespace Entrogic.NPCs.Boss.凝胶Java盾
{
    /// <summary>
    /// IREUL细胞 的摘要说明
    /// 创建时间：2019/8/10 15:29:37
    /// </summary>
    public class IREUL : ModNPC
    {
        public override void SetDefaults()
        {
            npc.width = 10;
            npc.height = 10;
            npc.aiStyle = -1;
            npc.damage = 24;
            npc.lifeMax = 1;
            if (CrossModHandler.ModLoaded("CalamityMod"))
            {
                if (CrossModHandler.IsCalamityModRevengenceMode)
                {
                    npc.damage *= 3;
                }
                else if (CrossModHandler.IsCalamityModDeathMode)
                {
                    npc.lifeMax = 10;
                    npc.damage *= 5;
                }
            }
            npc.knockBackResist = 0f;
            npc.noTileCollide = true;
            npc.noGravity = true;
            npc.alpha = 30;
            npc.value = 0f;
        }
        Vector2 MyPosition = new Vector2(-1);
        Tile MyTile = null;
        public override void AI()
        {
            Lighting.AddLight(npc.Center, 175f / 255f, 73f / 255f, 73f / 255f);
            if (npc.ai[3] == 0)
            {
                npc.rotation = Main.rand.Next(100, (int)(MathHelper.TwoPi * 10000)) * 0.0001f;
                npc.ai[3] = 1;
            }
            npc.position.Y +=4.4f;
            npc.TargetClosest(false);
            Point tileMe = new Point((int)(npc.position.X / 16), (int)(npc.position.Y / 16));
            Tile tile = Main.tile[tileMe.X, tileMe.Y];
            if (MyTile != null)
            {
                npc.position = MyPosition + new Vector2(3, -1);
                int count = 0;
                foreach (NPC n in Main.npc)
                {
                    if (n.active && n.Center.X >= npc.Center.X - 25 && n.Center.X <= npc.Center.X + 25 && n.Center.Y >= npc.Center.Y - 25 && n.Center.Y <= npc.Center.Y + 25 && n.type == NPCType<IREUL2>())
                    {
                        count++;
                    }
                }
                npc.ai[0]++;
                if (npc.ai[0] >= (Main.expertMode ? 40 : 60))
                {
                    if (Main.rand.NextBool(2) && Main.tile[tileMe.X + 1, tileMe.Y].active() && (Main.tileSolid[Main.tile[tileMe.X + 1, tileMe.Y].type] || Main.tileSolidTop[Main.tile[tileMe.X + 1, tileMe.Y].type]))
                        NPC.NewNPC(0, 0, NPCType<IREUL2>(), 0, MyPosition.X + 16, MyPosition.Y);
                    if (Main.rand.NextBool(2) && Main.tile[tileMe.X - 1, tileMe.Y].active() && (Main.tileSolid[Main.tile[tileMe.X - 1, tileMe.Y].type] || Main.tileSolidTop[Main.tile[tileMe.X - 1, tileMe.Y].type]))
                        NPC.NewNPC(0, 0, NPCType<IREUL2>(), 0, MyPosition.X - 16, MyPosition.Y);
                    if (Main.rand.NextBool(2) && Main.tile[tileMe.X, tileMe.Y + 1].active() && (Main.tileSolid[Main.tile[tileMe.X, tileMe.Y + 1].type] || Main.tileSolidTop[Main.tile[tileMe.X, tileMe.Y + 1].type]))
                        NPC.NewNPC(0, 0, NPCType<IREUL2>(), 0, MyPosition.X, MyPosition.Y + 16);
                    if (Main.rand.NextBool(2) && Main.tile[tileMe.X, tileMe.Y - 1].active() && (Main.tileSolid[Main.tile[tileMe.X, tileMe.Y - 1].type] || Main.tileSolidTop[Main.tile[tileMe.X, tileMe.Y - 1].type]))
                        NPC.NewNPC(0, 0, NPCType<IREUL2>(), 0, MyPosition.X, MyPosition.Y - 16);
                    /*if (Main.rand.NextBool(4) && Main.tile[tileMe.X + 1, tileMe.Y + 1].active() && (Main.tileSolid[Main.tile[tileMe.X + 1, tileMe.Y + 1].type] || Main.tileSolidTop[Main.tile[tileMe.X + 1, tileMe.Y + 1].type]))
                        NPC.NewNPC(0, 0, NPCType<IREUL细胞2>(), 0, MyPosition.X + 16, MyPosition.Y + 16);
                    if (Main.rand.NextBool(4) && Main.tile[tileMe.X - 1, tileMe.Y - 1].active() && (Main.tileSolid[Main.tile[tileMe.X - 1, tileMe.Y - 1].type] || Main.tileSolidTop[Main.tile[tileMe.X - 1, tileMe.Y - 1].type]))
                        NPC.NewNPC(0, 0, NPCType<IREUL细胞2>(), 0, MyPosition.X - 16, MyPosition.Y - 16);
                    if (Main.rand.NextBool(4) && Main.tile[tileMe.X - 1, tileMe.Y + 1].active() && (Main.tileSolid[Main.tile[tileMe.X - 1, tileMe.Y + 1].type] || Main.tileSolidTop[Main.tile[tileMe.X - 1, tileMe.Y + 1].type]))
                        NPC.NewNPC(0, 0, NPCType<IREUL细胞2>(), 0, MyPosition.X - 16, MyPosition.Y + 16);
                    if (Main.rand.NextBool(4) && Main.tile[tileMe.X + 1, tileMe.Y - 1].active() && (Main.tileSolid[Main.tile[tileMe.X + 1, tileMe.Y - 1].type] || Main.tileSolidTop[Main.tile[tileMe.X + 1, tileMe.Y - 1].type]))
                        NPC.NewNPC(0, 0, NPCType<IREUL细胞2>(), 0, MyPosition.X + 16, MyPosition.Y - 16);*/
                    npc.ai[0] = 0;
                }
                if (count >= 3 && Main.rand.NextBool(2))
                {
                    npc.StrikeNPCNoInteraction(9999, 0, 0);
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        NetMessage.SendData(MessageID.DamageNPC, -1, -1, null, npc.whoAmI, -1f, 0f, 0f, 0, 0, 0);
                    }
                }
            }
            else if (tile.active() && (Main.tileSolid[tile.type] || Main.tileSolidTop[tile.type]) && npc.Center.Y >= Main.player[npc.target].Center.Y)
            {
                MyPosition = new Vector2((int)(npc.position.X / 16), (int)(npc.position.Y / 16)) * 16;
                MyTile = tile;
            }
            int Java = 0;
            foreach (NPC n in Main.npc)
            {
                if (n.active && n.type == NPCType<Volutio>())
                {
                    Java++;
                }
            }
            if (Java <= 0 && npc.ai[3] >= 300)
            {
                npc.StrikeNPCNoInteraction(9999, 0, 0);
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    NetMessage.SendData(MessageID.DamageNPC, -1, -1, null, npc.whoAmI, -1f, 0f, 0f, 0, 0, 0);
                }
            }
            npc.ai[3]++;
        }
        public override void HitEffect(int hitDirection, double damage)
        {
            for (int i = 0; i < 3; i++)
                Dust.NewDust(npc.position, npc.width, npc.height, 4, (float)(2 * hitDirection), -2f, npc.alpha, new Color(0, 70, 118, 137), 1f);
        }
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            if (Main.rand.NextBool(2))
                target.AddBuff(BuffType<Buffs.Enemies.Dissolve>(), (int)(Main.rand.Next(90, 151) * Main.GameModeInfo.DebuffTimeMultiplier));
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            spriteBatch.Draw(((Texture2D)Terraria.GameContent.TextureAssets.Npc[npc.type]), npc.position - Main.screenPosition + new Vector2(0, 5), null, drawColor, npc.rotation, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            return false;
        }
    }
    public class IREUL2 : ModNPC
    {
        public override string Texture { get { return "Entrogic/NPCs/Boss/凝胶Java盾/IREUL"; } }
        public override void SetDefaults()
        {
            npc.width = 10;
            npc.height = 10;
            npc.aiStyle = -1;
            npc.damage = 24;
            npc.lifeMax = 1;
            if (CrossModHandler.ModLoaded("CalamityMod"))
            {
                if (CrossModHandler.IsCalamityModRevengenceMode)
                {
                    npc.damage *= 3;
                }
                else if (CrossModHandler.IsCalamityModDeathMode)
                {
                    npc.lifeMax = 10;
                    npc.damage *= 5;
                }
            }
            npc.knockBackResist = 0f;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.alpha = 30;
            npc.value = 0f;
        }
        Vector2 MyPosition = new Vector2(-1);
        public override void AI()
        {
            Lighting.AddLight(npc.Center, 175f / 255f, 73f / 255f, 73f / 255f);
            if (npc.ai[3] == 0)
            {
                npc.rotation = Main.rand.Next(1000, (int)(MathHelper.TwoPi * 10000)) * 0.0001f;
                npc.ai[3] = 1;
            }
            MyPosition = new Vector2(npc.ai[0], npc.ai[1]);
            npc.position = MyPosition + new Vector2(3, -1);
            Point tileMe = new Point((int)(npc.position.X / 16), (int)((npc.position.Y + 4) / 16));
            if (!Main.tile[tileMe.X, tileMe.Y].active())
            {
                npc.StrikeNPCNoInteraction(9999, 0, 0);
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    NetMessage.SendData(MessageID.DamageNPC, -1, -1, null, npc.whoAmI, -1f, 0f, 0f, 0, 0, 0);
                }
            }
            int Java = 0;
            int count = 0;
            int count2 = 0;
            foreach (NPC n in Main.npc)
            {
                if (n.active && n.Center.X >= npc.Center.X - 25 && n.Center.X <= npc.Center.X + 25 && n.Center.Y >= npc.Center.Y - 25 && n.Center.Y <= npc.Center.Y + 25 && n.type == NPCType<IREUL2>())
                {
                    count++;
                }
                if (n.active && (n.type == NPCType<IREUL2>() || n.type == NPCType<IREUL>()))
                {
                    count2++;
                }
                if (n.active && n.type == NPCType<Volutio>())
                {
                    Java++;
                }
            }
            npc.ai[2]++;
            if (npc.ai[2] >= (Main.expertMode ? 40 : 60) && count2 < 100)
            {
                if (Main.rand.NextBool(2) && Main.tile[tileMe.X + 1, tileMe.Y].active() && (Main.tileSolid[Main.tile[tileMe.X + 1, tileMe.Y].type] || Main.tileSolidTop[Main.tile[tileMe.X + 1, tileMe.Y].type]))
                    NPC.NewNPC(0, 0, NPCType<IREUL2>(), 0, MyPosition.X + 16, MyPosition.Y);
                if (Main.rand.NextBool(2) && Main.tile[tileMe.X - 1, tileMe.Y].active() && (Main.tileSolid[Main.tile[tileMe.X - 1, tileMe.Y].type] || Main.tileSolidTop[Main.tile[tileMe.X - 1, tileMe.Y].type]))
                    NPC.NewNPC(0, 0, NPCType<IREUL2>(), 0, MyPosition.X - 16, MyPosition.Y);
                if (Main.rand.NextBool(2) && Main.tile[tileMe.X, tileMe.Y + 1].active() && (Main.tileSolid[Main.tile[tileMe.X, tileMe.Y + 1].type] || Main.tileSolidTop[Main.tile[tileMe.X, tileMe.Y + 1].type]))
                    NPC.NewNPC(0, 0, NPCType<IREUL2>(), 0, MyPosition.X, MyPosition.Y + 16);
                if (Main.rand.NextBool(2) && Main.tile[tileMe.X, tileMe.Y - 1].active() && (Main.tileSolid[Main.tile[tileMe.X, tileMe.Y - 1].type] || Main.tileSolidTop[Main.tile[tileMe.X, tileMe.Y - 1].type]))
                    NPC.NewNPC(0, 0, NPCType<IREUL2>(), 0, MyPosition.X, MyPosition.Y - 16);
                npc.ai[2] = 0;
            }
            if (count >= 3 && Main.rand.NextBool(2))
            {
                npc.StrikeNPCNoInteraction(9999, 0, 0);
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    NetMessage.SendData(MessageID.DamageNPC, -1, -1, null, npc.whoAmI, -1f, 0f, 0f, 0, 0, 0);
                }
            }
            if (Java <= 0 || npc.ai[3] >= 300)
            {
                npc.StrikeNPCNoInteraction(9999, 0, 0);
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    NetMessage.SendData(MessageID.DamageNPC, -1, -1, null, npc.whoAmI, -1f, 0f, 0f, 0, 0, 0);
                }
            }
            npc.ai[3]++;
        }
        public override void HitEffect(int hitDirection, double damage)
        {
            for (int i = 0; i < 3; i++)
                Dust.NewDust(npc.position, npc.width, npc.height, 4, (float)(2 * hitDirection), -2f, npc.alpha, new Color(0, 70, 118, 137), 1f);
        }
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            if (Main.rand.NextBool(2))
                target.AddBuff(BuffType<Buffs.Enemies.Dissolve>(), (int)(Main.rand.Next(90, 151) * Main.GameModeInfo.DebuffTimeMultiplier));
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            spriteBatch.Draw(((Texture2D)Terraria.GameContent.TextureAssets.Npc[npc.type]), npc.position - Main.screenPosition + new Vector2(0, 5), null, drawColor, npc.rotation, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            return false;
        }
    }
}
