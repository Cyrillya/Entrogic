
using Entrogic.NPCs.Banners;
using Entrogic.Projectiles.Enemies;

using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.NPCs.Enemies
{
    public class StoneSlime : ModNPC
    {
        public bool _Life = true;
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 2;
        }
        public override void SetDefaults()
        {
            banner = npc.type;
            bannerItem = ItemType<StoneSlimeBanner>();
            Player player = Main.player[Main.myPlayer];
            npc.width = 30;
            npc.height = 24;
            npc.damage = 25;
            npc.defense = 15;
            npc.lifeMax = 1000;
            npc.HitSound = SoundID.NPCHit3;
            npc.DeathSound = SoundID.NPCDeath37;
            npc.value = 10f;
            npc.knockBackResist = 0.5f;
            npc.buffImmune[20] = true;
            npc.buffImmune[24] = true;
            npc.buffImmune[30] = true;
            npc.buffImmune[46] = true;
            npc.buffImmune[47] = true;
            npc.buffImmune[70] = true;
            if (Main.hardMode) { npc.buffImmune[39] = true; npc.buffImmune[69] = true; npc.damage = 135; npc.defense = 60; npc.lifeMax = 600; }
            animationType = NPCID.BlueSlime;
            npc.aiStyle = 1;
            if (npc.confused) npc.confused = false;
        }
        public override void AI()
        {
            Player player = Main.player[Main.myPlayer]; //Get Player
            Mod mod = ModLoader.GetMod("Entrogic"); //Get Mod
            EntrogicPlayer modPlayer = player.GetModPlayer<EntrogicPlayer>();
            if (_Life)
            {
                _Life = false;
                npc.life = 800;
            }
            npc.lifeMax = 800;

            if (npc.localAI[0] > 0f)
            {
                npc.localAI[0] -= 1f;
            }
            if (!npc.wet && !Main.player[npc.target].npcTypeNoAggro[npc.type])
            {
                Vector2 vector3 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
                float num11 = Main.player[npc.target].position.X + (float)Main.player[npc.target].width * 0.5f - vector3.X;
                float num12 = Main.player[npc.target].position.Y - vector3.Y;
                float num13 = (float)Math.Sqrt((double)(num11 * num11 + num12 * num12));
                float n2 = 256f;
                if (num13 < n2 && Collision.CanHit(npc.position, npc.width, npc.height, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height) && npc.velocity.Y == 0f)
                {
                    npc.ai[0] = -40f;
                    if (npc.velocity.Y == 0f)
                    {
                        npc.velocity.X = npc.velocity.X * 0.9f;
                    }
                    if (Main.netMode != NetmodeID.MultiplayerClient && npc.localAI[0] == 0f)
                    {
                        num12 = Main.player[npc.target].position.Y - vector3.Y - (float)Main.rand.Next(0, 180);
                        num13 = (float)Math.Sqrt((double)(num11 * num11 + num12 * num12));
                        num13 = 4.5f / num13;
                        num11 *= num13;
                        num12 *= num13;

                        float HarryPotter = Main.expertMode ? 20f : 0f;
                        float HarryWalker = Main.hardMode ? 70f : 125f;
                        npc.localAI[0] = HarryWalker - HarryPotter;

                        float num14 = Main.rand.Next(0, 60) * 0.1f;
                        float num15 = Main.rand.Next(0, 30) * 0.1f;
                        float num16 = Main.rand.Next(0, 45) * 0.1f;
                        float num17 = Main.rand.Next(0, 15) * 0.1f;
                        Projectile.NewProjectile(vector3.X, vector3.Y, num11, num12, ProjectileType<Gravel>(), 9, 0f, Main.myPlayer, 0f, 0f);
                        npc.life -= Main.hardMode && Main.expertMode ? 1 : Main.expertMode || Main.hardMode ? 3 : 5;
                        if (Main.expertMode || Main.hardMode)
                        {
                            Projectile.NewProjectile(vector3.X, vector3.Y, num11 - num14, num12 + num15, ProjectileType<Gravel>(), 9, 0f, Main.myPlayer, 0f, 0f);
                            npc.life -= Main.hardMode && Main.expertMode ? 1 : 3;
                            if (Main.hardMode && Main.expertMode)
                            {
                                Projectile.NewProjectile(vector3.X, vector3.Y, num11 - num16, num12 + num17, ProjectileType<Gravel>(), 9, 0f, Main.myPlayer, 0f, 0f);
                                npc.life -= 1;
                            }
                        }
                        /*if (ModLoader.GetMod("CalamityMod") != null)
                        {
                            Projectile.NewProjectile(vector3.X, vector3.Y, num11 + num16, num12 - num17, ModContent.ProjectileType<Gravel>(), 9, 0f, Main.myPlayer, 0f, 0f);
                            Projectile.NewProjectile(vector3.X, vector3.Y, num11 + num14, num12 - num15, ModContent.ProjectileType<Gravel>(), 9, 0f, Main.myPlayer, 0f, 0f);
                        }*/
                    }
                }
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.player.ZoneRockLayerHeight)
                return 0.085f;
            return 0f;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            for (int i = 0; i < 5; i++)
            {
                Dust d = Dust.NewDustDirect(npc.position, npc.width, npc.height, MyDustId.GreyStone, 0, 0, 0);
            }
        }

        public override void OnHitPlayer(Player player, int damage, bool crit)
        {
            if (Main.rand.Next(4) == 0)
            {
                player.AddBuff(BuffID.Stoned, 90, true);
            }
        }
        public override void NPCLoot()
        {
            int num = Main.rand.Next(20, 50 + 1); // 20 - 50¸ö
            if (!Main.hardMode)
            {
                int i = Main.rand.Next(39);
                if (i >= 0 && i <= 12)
                {
                    if (Main.rand.Next(1) == 0) Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.CopperOre, num);
                    else Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.TinOre, num);
                }
                else if (i >= 13 && i <= 23)
                {
                    if (Main.rand.Next(1) == 0) Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.IronOre, num);
                    else Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.LeadOre, num);
                }
                else if (i >= 24 && i <= 32)
                {
                    if (Main.rand.Next(1) == 0) Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.TungstenOre, num);
                    else Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.SilverOre, num);
                }
                else
                {
                    if (Main.rand.Next(1) == 0) Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.GoldOre, num);
                    else Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.PalladiumOre, num);
                }
            }
            else
            {
                int i = Main.rand.Next(99);
                if (i >= 0 && i <= 20)
                {
                    if (Main.rand.Next(1) == 0) Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.CopperOre, num);
                    else Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.TinOre, num);
                }
                else if (i >= 21 && i <= 39)
                {
                    if (Main.rand.Next(1) == 0) Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.IronOre, num);
                    else Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.LeadOre, num);
                }
                else if (i >= 40 && i <= 55)
                {
                    if (Main.rand.Next(1) == 0) Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.TungstenOre, num);
                    else Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.SilverOre, num);
                }
                else if (i >= 56 && i <= 69)
                {
                    if (Main.rand.Next(1) == 0) Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.GoldOre, num);
                    else Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.PlatinumOre, num);
                }
                else if (i >= 70 && i <= 82)
                {
                    if (Main.rand.Next(1) == 0) Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.CobaltOre, num);
                    else Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.PalladiumOre, num);
                }
                else if (i >= 83 && i <= 91)
                {
                    if (Main.rand.Next(1) == 0) Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.MythrilOre, num);
                    else Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.OrichalcumOre, num);
                }
                else
                {
                    if (Main.rand.Next(1) == 0) Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.AdamantiteOre, num);
                    else Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.TitaniumOre, num);
                }
            }
        }
    }
}