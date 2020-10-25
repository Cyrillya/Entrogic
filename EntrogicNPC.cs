using Entrogic.Buffs.Weapons;
using Entrogic.Items.AntaGolem;
using Entrogic.Items.Equipables.Accessories;
using Entrogic.Items.Materials;
using Entrogic.Items.Weapons.Card;
using Entrogic.Items.Weapons.Card.Elements;
using Entrogic.Items.Weapons.Card.Nones;
using Entrogic.Items.Weapons.Card.Organisms;
using Entrogic.Items.Weapons.Card.Undeads;
using Entrogic.Items.Weapons.Melee.Sword;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.NetModules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;


namespace Entrogic
{
    public class EntrogicNPC : GlobalNPC
    {
        public bool bossLootMsg = false;
        public float specialAI = 0;

        public int buffOwner = -1;
        public bool frozen = false;
        public bool chilled = false;
        public bool antibody = false;
        public bool plague = false;

        public int aiTimer = 0;
        public override void ResetEffects(NPC npc)
        {
            frozen = false;
            chilled = false;
            antibody = false;
            plague = false;
        }
        public override void SetDefaults(NPC npc)
        {
            Main.npcCatchable[NPCID.BlueSlime] = true;
        }
        public static bool FightingWithBoss()
        {
            foreach (NPC npc in Main.npc)
            {
                if (npc.active && (npc.boss || npc.type == NPCID.EaterofWorldsHead))
                {
                    return true;
                }
            }
            return false;
        }
        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            Vector2 Spawn = new Vector2(spawnInfo.spawnTileX, spawnInfo.spawnTileY) * 16f;
            if (EntrogicWorld.Check(Spawn.X, Spawn.Y))
            {
                pool.Remove(0);
            }
        }
        public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
        {
            EntrogicPlayer modPlayer = player.GetModPlayer<EntrogicPlayer>();
            int num = (int)(1 - 0.0001 * modPlayer.Sins);
            spawnRate = (int)((double)spawnRate * num);
            if (EntrogicWorld.magicStorm)
            {
                maxSpawns += 70;
                int reduce = 60;
                spawnRate -= reduce;
                if (spawnRate <= 12 && spawnRate + reduce > 12)
                {
                    spawnRate = 12;
                }
            }
        }
        public int disapperer = 0;
        public override bool PreAI(NPC npc)
        {
            if (EntrogicWorld.Check(npc.position.X, npc.position.Y) && npc.wet && IsPreHardmodeSlime(npc) && Main.netMode != NetmodeID.MultiplayerClient)
            {
                NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, NPCType<NPCs.Boss.凝胶Java盾.Embryo>());
                npc.StrikeNPC(9999, 0, 0);
                if (Main.netMode == NetmodeID.Server)
                {
                    NetMessage.SendData(MessageID.DamageNPC, -1, -1, null, npc.whoAmI, -1f, 0f, 0f, 0, 0, 0);
                }
                Terraria.Audio.SoundEngine.PlaySound(SoundID.NPCHit1, npc.position);
            }
            return !frozen;
        }
        public override void FindFrame(NPC npc, int frameHeight)
        {
            if (frozen) return;
        }
        public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Color drawColor)
        {
            if (frozen)
            {
                // 加载图片
                Texture2D tex = (Texture2D)Entrogic.ModTexturesTable["Frozen"];
                Vector2 worldPos = npc.Center;
                float scale = (float)((npc.width + npc.height) / 2) / 29f;
                spriteBatch.Draw(tex, worldPos - Main.screenPosition, null, drawColor, 0f, tex.Size() * 0.5f, scale, SpriteEffects.None, 0f);
            }
        }
        public override void PostAI(NPC npc)
        {
            if (frozen)
            {
                npc.velocity *= 0f;
                npc.position = npc.oldPosition;
                npc.netUpdate = true;

            }
            if (chilled)
            {
                npc.position -= npc.velocity * 0.5f;
                npc.netUpdate = true;
            }
        }
        public override Color? GetAlpha(NPC npc, Color drawColor)
        {
            return chilled ? new Color(153 + drawColor.R, 217 + drawColor.G, 234 + drawColor.B, drawColor.A) : drawColor;
        }
        public override void NPCLoot(NPC npc)
        {
            if (npc.lifeMax <= 10 * Main.GameModeInfo.EnemyMaxLifeMultiplier && !Main.GameModeInfo.IsJourneyMode || npc.SpawnedFromStatue)
                return;

            Player lootPlayer = Main.player[Player.FindClosest(npc.position, npc.width, npc.height)];

            int num = EntrogicWorld.downedAthanasy ? 240 : 120;
            if (Main.rand.NextBool(num) && (IsPostPlanteraDungeonEnemies(npc) || IsPrePlanteraDungeonEnemies(npc)))
                Item.NewItem(npc.getRect(), ItemType<TitansOrder>());

            if (EntrogicWorld.downedAthanasy && Main.rand.NextBool(30) && ((npc.type == NPCID.Antlion) || (npc.type == NPCID.FlyingAntlion) || (npc.type == NPCID.WalkingAntlion)
                 || (npc.type == NPCID.Vulture) || (npc.type == NPCID.SandSlime) || (npc.type == NPCID.Mummy) || (npc.type == NPCID.LightMummy)
                  || (npc.type == NPCID.DarkMummy) || (npc.type == NPCID.TombCrawlerHead) || (npc.type == NPCID.DuneSplicerHead) || (npc.type == NPCID.DesertLamiaDark)
                   || (npc.type == NPCID.DesertScorpionWalk) || (npc.type == NPCID.DesertScorpionWall) || (npc.type == NPCID.DesertBeast) || (npc.type == NPCID.DesertLamiaLight)
                    || (npc.type == NPCID.DesertGhoulHallow) || (npc.type == NPCID.DesertGhoulCrimson) || (npc.type == NPCID.DesertGhoulCorruption) || (npc.type == NPCID.DesertGhoul) || (npc.type == NPCID.DesertDjinn)))
                Item.NewItem(npc.getRect(), ItemType<GoldenHarvest>());
            //if (Main.eclipse && Main.rand.NextBool(100)) Item.NewItem(npc.getRect(), ModContent.ItemType<衰变立场>());

            if (Main.rand.NextBool(8) && IsSeaEnemies(npc))
                if (!EntrogicWorld.IsDownedPollutionElemental)
                    Item.NewItem(npc.getRect(), ItemType<SoulOfPure>(), Main.rand.Next(1, 2 + 1));
                else
                    Item.NewItem(npc.getRect(), ItemType<SoulofContamination>(), Main.rand.Next(1, 2 + 1));
            if (Main.rand.NextBool(6) && IsPreHardmodeSlime(npc))
                Item.NewItem(npc.getRect(), ItemType<SoulOfPure>(), Main.rand.Next(1, 2 + 1));

            if (lootPlayer.ZoneUnderworldHeight && Main.rand.Next(1, 1001) <= 6) // 0.6 %
                Item.NewItem(npc.getRect(), ItemType<BetrayerofDarkFlame>());
            if (lootPlayer.ZoneUnderworldHeight && Main.rand.Next(1, 101) <= 4 && NPC.downedPlantBoss) // 4 %
                Item.NewItem(npc.getRect(), ItemType<BetrayerofDarkFlame_Strong>());

            if ((npc.type == NPCID.Harpy && Main.rand.NextBool(20)) || (npc.type == NPCID.WyvernHead && Main.rand.NextBool(5))) // 鸟妖5 %、小白龙20 %
                Item.NewItem(npc.getRect(), ItemType<Items.Weapons.Card.Elements.Dragoul>());

            if ((Main.rand.NextBool(100) && IsPrePlanteraDungeonEnemies(npc)) || npc.type == NPCID.SkeletronHead) // 1 %
                Item.NewItem(npc.getRect(), ItemType<UndeadGrudge>());
            if (Main.rand.NextBool(200) && IsPrePlanteraDungeonEnemies(npc)) // 0.5 %
                Item.NewItem(npc.getRect(), ItemType<UndeadGrudge_Electromagnetism>());
            if ((Main.rand.Next(1, 1001) <= 15 && IsPostPlanteraDungeonEnemies(npc)) || npc.type == NPCID.DungeonGuardian) // 1.5 %
                Item.NewItem(npc.getRect(), ItemType<UndeadGrudge_Strong>());

            if (Main.rand.NextFloat() < .003 && IsZombie(npc)) // 0.3 %
                Item.NewItem(npc.getRect(), ItemType<TheWalkingDead>());
            if (Main.rand.NextFloat() < .001 && IsZombie(npc)) // 0.1 %
                Item.NewItem(npc.getRect(), ItemType<TheWalkingDead_Strong>());

            if (Main.rand.NextBool(4) && IsMechanicalBoss(npc) && !Main.expertMode) // 25 %
                Item.NewItem(npc.getRect(), ItemType<PerpetuumMobileoftheFifthKind>());

            if (Main.rand.NextBool(4) && (npc.type == NPCID.WyvernHead || (npc.type == NPCID.WallofFlesh && !Main.expertMode))) // 25 %
                Item.NewItem(npc.getRect(), ItemType<Worldflipper>());

            if (Main.rand.Next(1, 101) <= 35 && npc.type == NPCID.KingSlime && !Main.expertMode) // 35 %
                Item.NewItem(npc.getRect(), ItemType<GelofMimicry>());

            if (Main.rand.Next(1, 101) <= 35 && npc.type == NPCID.EyeofCthulhu && !Main.expertMode) // 35 %
                Item.NewItem(npc.getRect(), ItemType<Foresight>());

            if (Main.rand.Next(1, 1001) <= 5 && lootPlayer.ZoneSnow) // 0.5 %
                Item.NewItem(npc.getRect(), ItemType<OverallFreeze>());

            if (Main.rand.Next(1, 101) <= 1 && (npc.type == NPCID.Demon || npc.type == NPCID.VoodooDemon || npc.type == NPCID.RedDevil)) // 1 %
                Item.NewItem(npc.getRect(), ItemType<DemonCovenant>());

            float dropMimicry = 0.02f; // 2%
            if (EntrogicWorld.magicStorm)
                dropMimicry = 0.045f; // 4.5%
            if (Main.rand.NextFloat() < dropMimicry)
                Projectile.NewProjectile(npc.Center, Vector2.Zero, ProjectileType<Projectiles.Miscellaneous.Arcana>(), 0, 0f, lootPlayer.whoAmI, Main.rand.Next(2));
            if (Main.rand.Next(1, 101) <= 2 && (lootPlayer.ZoneRockLayerHeight || lootPlayer.ZoneDirtLayerHeight) && EntrogicWorld.magicStorm) // 2 %
                Item.NewItem(npc.getRect(), ItemType<CryptTreasure>());

            if (bossLootMsg)
            {
                string typeName = npc.TypeName;
                if (Main.netMode == NetmodeID.SinglePlayer)
                {
                    Main.NewText(Language.GetTextValue("Announcement.HasBeenDefeated_Single", typeName), 175, 75, byte.MaxValue);
                }
                else if (Main.netMode == NetmodeID.Server)
                {
                    NetTextModule.SerializeServerMessage(NetworkText.FromKey("Announcement.HasBeenDefeated_Single", new object[]
                    {
                        npc.GetTypeNetName()
                    }), new Color(175, 75, 255));
                }
            }
        }
        public static bool IsPrePlanteraDungeonEnemies(NPC npc)
        {
            return (npc.type == NPCID.SkeletronHead) || (npc.type == NPCID.AngryBones) || (npc.type == NPCID.AngryBonesBig) || (npc.type == NPCID.AngryBonesBigHelmet)
                   || (npc.type == NPCID.DarkCaster) || (npc.type == NPCID.CursedSkull) || (npc.type == NPCID.DungeonSlime) || (npc.type == NPCID.DungeonGuardian);
        }
        public static bool IsPostPlanteraDungeonEnemies(NPC npc)
        {
            return (npc.type == NPCID.GiantCursedSkull) || (npc.type == NPCID.DungeonSpirit) || (npc.type == NPCID.DiabolistRed) || (npc.type == NPCID.DiabolistWhite)
                   || (npc.type == NPCID.RaggedCaster) || (npc.type == NPCID.RaggedCasterOpenCoat) || (npc.type == NPCID.Necromancer) || (npc.type == NPCID.NecromancerArmored);
        }
        public static bool IsSeaEnemies(NPC npc)
        {
            return npc.type == NPCID.PinkJellyfish || npc.type == NPCID.Crab || npc.type == NPCID.SeaSnail || npc.type == NPCID.Squid || npc.type == NPCID.Shark || npc.type == NPCID.DukeFishron;
        }
        public static bool IsPreHardmodeSlime(NPC npc)
        {
            return npc.type == NPCID.BlueSlime || npc.type == NPCID.BunnySlimed || npc.type == NPCID.IceSlime || npc.type == NPCID.SandSlime || npc.type == NPCID.SlimeRibbonGreen || npc.type == NPCID.SlimeRibbonRed || npc.type == NPCID.SlimeRibbonWhite || npc.type == NPCID.SlimeRibbonYellow || npc.type == NPCID.SlimeSpiked || npc.type == NPCID.SpikedIceSlime || npc.type == NPCID.SpikedJungleSlime || npc.type == NPCID.UmbrellaSlime || npc.type == NPCID.KingSlime;
        }
        public static bool IsZombie(NPC npc)
        {
            return npc.type == NPCID.Zombie || npc.type == NPCID.ZombieDoctor || npc.type == NPCID.ZombieEskimo || npc.type == NPCID.ZombieMushroom || npc.type == NPCID.ZombieMushroomHat || npc.type == NPCID.ZombiePixie || npc.type == NPCID.ZombieRaincoat || npc.type == NPCID.ZombieSuperman || npc.type == NPCID.ZombieSweater || npc.type == NPCID.ZombieXmas || npc.type == NPCID.TwiggyZombie || npc.type == NPCID.SwampZombie || npc.type == NPCID.SlimedZombie || npc.type == NPCID.PincushionZombie || npc.type == NPCID.FemaleZombie || npc.type == NPCID.BloodZombie || npc.type == NPCID.BaldZombie || (npc.type >= NPCID.ArmedZombie && npc.type <= NPCID.ArmedZombieCenx);
        }
        public static bool IsMechanicalBoss(NPC npc)
        {
            return npc.type == NPCID.SkeletronPrime || npc.type == NPCID.TheDestroyer || npc.type == NPCID.Spazmatism || npc.type == NPCID.Retinazer;
        }
        public override void SetupShop(int type, Chest shop, ref int nextSlot)
        {
            if (type == NPCID.Merchant)
            {
                if (DateTime.Now.Month == 2 || DateTime.Now.Month == 1)
                {
                    shop.item[nextSlot].SetDefaults(ItemType<Items.Weapons.Melee.Sword.FuSword>());
                    nextSlot++;
                }
                if (EntrogicWorld.magicStorm)
                {
                    shop.item[nextSlot].SetDefaults(ItemType<LuckyCoin>());
                    nextSlot++;
                }
            }
            if (type == NPCID.Mechanic)
            {
                //肉山后
                if (Main.hardMode)
                {
                    shop.item[nextSlot].SetDefaults(ItemType<CuteWidget>());
                    shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 1, 0, 0);
                    nextSlot++;
                }
            }
            if (type == NPCID.Demolitionist)
            {
                if (NPC.downedMoonlord)
                {
                    shop.item[nextSlot].SetDefaults(ItemID.DrillContainmentUnit);
                    shop.item[nextSlot].shopCustomPrice = Item.buyPrice(platinum: 30);
                    nextSlot++;
                }
            }
            if (type == NPCID.GoblinTinkerer)
            {
                if (NPC.downedMoonlord)
                {
                    shop.item[nextSlot].SetDefaults(ItemID.CellPhone);
                    shop.item[nextSlot].shopCustomPrice = Item.buyPrice(platinum: 10);
                    nextSlot++;
                }
            }
        }
        public override bool InstancePerEntity => true;
    }
}