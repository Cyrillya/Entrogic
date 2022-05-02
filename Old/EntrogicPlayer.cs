﻿using System;
using System.IO;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameInput;
using Terraria.ModLoader.IO;
using Terraria.Audio;
using Entrogic.Items.Weapons.Melee.Sword;
using Entrogic.NPCs.Boss.PollutElement;
using Entrogic.UI.Cards;
using Entrogic.Items.Books;
using Entrogic.Items.Weapons.Card;
using System.Linq;
using System.Reflection;
using Entrogic.UI.Books;
using static Entrogic.Entrogic;
using Entrogic.NPCs.Boss.AntaGolem;
using System.Text;
using System.Diagnostics;
using Terraria.Graphics.Effects;
using Entrogic.NPCs;
using Entrogic.Buffs.Enemies;
using Entrogic.Projectiles.Miscellaneous;
using Entrogic.NPCs.Enemies;
using Microsoft.Xna.Framework.Input;
using Entrogic.NPCs.CardFightable.CardBullet;
using Entrogic.NPCs.CardMerchantSystem;
<<<<<<< HEAD
using Entrogic.NPCs.CardFightable.Particles;
using Terraria.IO;
using Terraria.Social;
using Entrogic.Items.Weapons.Summon.Whip;
using Entrogic.Common;
=======
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96
//using Entrogic.UI;

namespace Entrogic
{
    public class EntrogicPlayer : ModPlayer
    {
        //internal string PlayerFolder => ModHelper.GetPlayerPathFromName(player.name, SocialAPI.Cloud == null, out string name) + "/sysfile.ent/";

        private KeyboardState _currentKey;
        private KeyboardState _previousKey;

        private KeyboardState _currentKey;
        private KeyboardState _previousKey;

        internal int PickPowerHand = 0;
        internal bool ProjectieHasArmorPenetration = false;
        internal float CritDamagePoint = 1f;
        public Vector2[] oldPosition = new Vector2[241];

        internal bool HasDeadlySphere = false;
        internal bool HasStoneSlime = false;
        internal bool HasMovementWaterElemental = false;

        public bool CanLifeSteal20 = false;
        public bool IsConfused = false;
        public bool HasAntibody = false;
        public int PlagueNPCAmount = 0;
        public int MaxAmount = 0;

        public int SinsMax = 2000;
        public int SinsLowerCD;
        public int Sins;

        public bool CanSwimTile = false;
        public bool HasReborned = false;
        public bool CanReborn = false;
        public int RebornEffectTime = 0;
        public static bool CanAmmoCost85 = false;
        public static bool CanAmmoCost90 = false;
        public bool CanExplode = false;//福波斯盔甲相关
        public bool CanPollutionRing = false;//污染套装相关

        public bool IsZoneLife = false;
        public bool IsGrayScreen = false;
        public bool IsRainyDaysScreen = false;
        public bool IsMagicStormScreen = false;

        public int PolluHeadTimer = 0;
        public bool IsPolluHeadActive = false;
        public int WingFrameCounter = 0;
        public int WingFrame = 0;

        public int[] CardType = new int[9];
        public int[] CardReadyType = new int[250];
        public int[] CardReadyCost = new int[250];
        public int[] CardHandType = new int[6];
        public int[] CardHandCost = new int[6];
        public int[] CardGraveType = new int[150];
        public int ManaLeft = 3;
        public int ManaMax = 3;
        public static int ManaTrueMax = 10;
        public int CardHandMax = 3;
        public int CardPassDelay = 0;
        public int CardPassStatDelay = 600;
        public int CardWashDelay = 0;
        public int CardWashStatDelay = 1150;
        public string CardRecentEvent = "No recent event";
        public string CardRecentEventAdd = "";
        public int CardRecentEventAlpha = 255;
        public int CardUseCount;

        internal int[] CardGameType = new int[3];
        internal bool CardGameActive;
        internal bool CardGaming;
        internal float CardGameHealthAlpha;
        internal float CardGameNPCHealthAlpha;
        internal int CardGameNPCIndex = -1;
        internal int CardGamePlayerMaxHealth = 1000;
        internal int CardGamePlayerHealth;
        internal int CardGamePlayerLastHealth;
        internal int CardGameNPCMaxHealth;
        internal int CardGameNPCHealth;
        internal int CardGameNPCLastHealth;
        internal bool CardGamePlayerTurn;
<<<<<<< HEAD
        internal int CardGameLeftCard;
        internal Vector2 CardGamePlayerCenter;
        internal List<CardFightBullet> _bullets = new List<CardFightBullet>(); // 用List存储
        internal List<Particle> _particles = new List<Particle>(); // 用List存储
=======
        internal List<CardFightBullet> _bullets = new List<CardFightBullet>(); // 用List存储
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96

        internal bool IsBookActive;
        internal bool IsClosingBook;
        internal bool UseBookBubble;
        internal int BookBubbleFrameCounter;
        /// <summary>
        /// 这个值我决定范围为1-19，0-18的话就很反人类
        /// </summary>
        internal int BookBubbleFrame;
        // 一个效果用这么多变量，我是不是要被制裁了（
        internal byte PageNum = 1;

        internal bool IsDestroyNextCard_InnerRage = false;
        internal int MoreCard_EnergyRecovery = 0;
        internal bool IsDelayCycle_StaticWatch = false;
        internal bool IsMoreMana_CryptTreasure = false;
        internal bool IsTwoCards_CryptTreasure = false;
        internal int TwoCardsCount_CryptTreasure = 0;
        internal bool IsMoreManaOrCard_LuckyCoin = false;
        public int LibNum
        {
            get
            {
                int num = 0;
                for (int i = 0; i < CardReadyType.Length; i++)
                {
                    if (CardReadyType[i] != 0)
                        num++;
                }
                return num;
            }
        }
        public static EntrogicPlayer ModPlayer(Player player)
        {
            return player.GetModPlayer<EntrogicPlayer>();
        }
        /// <summary>
        /// This is where you reset any fields you add to your ModPlayer subclass to their default states. This is necessary in order to reset your fields if they are conditionally set by a tick update but the condition is no longer satisfied.
        /// </summary>
        public override void ResetEffects()
        {
            PickPowerHand = 0;

            CanAmmoCost85 = false;
            CanAmmoCost90 = false;
            CanExplode = false;
            CanPollutionRing = false;
            ProjectieHasArmorPenetration = false;
            CritDamagePoint = 1f;

            HasDeadlySphere = false;
            HasStoneSlime = false;
            HasMovementWaterElemental = false;

            HasAntibody = false;

            CanReborn = false;
            CanSwimTile = false;

            IsGrayScreen = false;
            IsRainyDaysScreen = false;
            IsMagicStormScreen = false;

            CanLifeSteal20 = false;
            IsConfused = false;
            player.autoJump = true;

            IsPolluHeadActive = false;

            CardHandMax = 3;
            ManaMax = 3;
            CardWashStatDelay = 1150;
            CardPassStatDelay = 600;
            if (Main.netMode == NetmodeID.SinglePlayer)
            {
                CardWashStatDelay = DEntrogicDebugClient.Instance.CardWashDelayTime;
                CardPassStatDelay = DEntrogicDebugClient.Instance.CardPassDelayTime;
            }
            IsDelayCycle_StaticWatch = false;
            IsMoreManaOrCard_LuckyCoin = false;

            //Main.NewText("CardFake: " + cardFakeType[0] + ", " + cardFakeType[1] + ", " + cardFakeType[2] + ", " + cardFakeType[3] + ", " + cardFakeType[4] + ", " + cardFakeType[5] + ", " + cardFakeType[6] + ", " + cardFakeType[7] + ", " + cardFakeType[8]);
            //Main.NewText("Card: " + cardType[0] + ", " + cardType[1] + ", " + cardType[2] + ", " + cardType[3] + ", " + cardType[4] + ", " + cardType[5] + ", " + cardType[6] + ", " + cardType[7] + ", " + cardType[8]);
        }

        /// <summary>
        /// Similar to UpdateDead, except this is only called when the player is dead. If this is called, then ResetEffects will not be called.
        /// </summary>
        public override void UpdateDead()
        {
            PickPowerHand = 0;

            CanAmmoCost85 = false;
            CanAmmoCost90 = false;

            CanExplode = false;
            CanPollutionRing = false;

            ProjectieHasArmorPenetration = false;

            CanReborn = false;

            IsGrayScreen = false;
            IsRainyDaysScreen = false;
            IsMagicStormScreen = false;

            CanLifeSteal20 = false;
            IsConfused = false;

            CardHandMax = 3;
            ManaMax = 3;
            CardWashStatDelay = 1150;
            CardPassStatDelay = 600;
            if (Main.netMode == NetmodeID.SinglePlayer)
            {
                CardWashStatDelay = DEntrogicDebugClient.Instance.CardWashDelayTime;
                CardPassStatDelay = DEntrogicDebugClient.Instance.CardPassDelayTime;
            }
            IsDelayCycle_StaticWatch = false;
            IsMoreManaOrCard_LuckyCoin = false;
        }

        /// <summary>
        /// Allows you to save custom data for this player. Returns null by default.
        /// </summary>
        /// <returns></returns>
        public override TagCompound Save()
        {
<<<<<<< HEAD
            //if (Main.netMode == NetmodeID.SinglePlayer)
            //{
            //    string path = string.Format(PlayerFolder + "CardData.entini");
            //    SaveCardData(path);
            //}
            //else if (BEntrogicConfigServer.Instance.ClearNewPlayersCard)
            //{
            //    ModHelper.GetWorldPathFromName(Main.worldName, SocialAPI.Cloud != null, out string worldName);
            //    string path = string.Format(PlayerFolder + "CardData" + worldName + ".entini");
            //    SaveCardData(path);
            //}
            string text = ModHelper.GetCardSlotInfo(player);
=======
            if (Main.netMode == NetmodeID.SinglePlayer)
            {
                string path = string.Format(PlayerFolder + "CardData.entini");
                SaveCardData(path);
            }
            else if (BEntrogicConfigServer.Instance.ClearNewPlayersCard)
            {
                string path = string.Format(ServerPlayerFolder + "CardData" + Main.worldName + ".entini");
                SaveCardData(path);
            }
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96
            return new TagCompound
            {
                { "Sins", Sins },
                { "LifeLastTime", LifeLastTime },
                { nameof(CardUseCount), CardUseCount },
                { nameof(CardType), text }
            };
        }

        /// <summary>
        /// Allows you to load custom data you have saved for this player.
        /// </summary>
        /// <param name="tag"></param>
        public override void Load(TagCompound tag)
        {
            Sins = tag.GetInt("Sins");
            LifeLastTime = tag.GetInt("LifeLastTime");
            CardUseCount = tag.GetInt(nameof(CardUseCount));

            string cardInfos = tag.GetString(nameof(CardType));
            List<int> list = new List<int>();
            if (cardInfos == null || cardInfos == "") list = new List<int>() { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            else
            {
                string[] cardInfo = cardInfos.Split('\n');
                foreach (string card in cardInfo)
                {
                    if (list.Count >= 9)
                        break;
                    if (card == "0")
                    {
                        list.Add(0);
                        continue;
                    }
                    string[] read = card.Split(':');
                    try
                    {
                        ModItem modItem = ModItems.Find(s => s.Name.Equals(read[1]));
                        if (modItem != null)
                        {
                            list.Add(modItem.item.type);
                        }
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
            }
            try
            {

                while (list.Count < 9) list.Add(0);
                CardType = list.ToArray();
            }
            catch { }
        }

        // AddStartingItems is a method you can use to add items to the player's starting inventory.
        // It is also called when the player dies a mediumcore death
        // Return an enumerable with the items you want to add to the inventory.
        // If you know what 'yield return' is, you can also use that here, if you prefer so.
        public override IEnumerable<Item> AddStartingItems(bool mediumCoreDeath)
        {
<<<<<<< HEAD
            return new[] {
                new Item(ItemType<TheGuide>())
            };
        }
        // ModifyStartingItems is a more elaborate version of AddStartingItems, which lets you remove items
        // that either vanilla or other mods add. You can technically use it to add items as well, but it's recommended
        // to only do that in AddStartingItems.
        // (If you want to stop another mod from adding an item, its entry is the mod's internal name, e.g itemsByMod["SomeMod"]
        // Terraria's entry is always named just "Terraria"
        public override void ModifyStartingInventory(IReadOnlyDictionary<string, List<Item>> itemsByMod, bool mediumCoreDeath)
        {
            for (int i = 0; i < itemsByMod["Terraria"].Count; i++)
            {
                if (itemsByMod["Terraria"][i].type == ItemID.CopperShortsword) itemsByMod["Terraria"][i].type = ItemType<CopperSwordWhip>();
            }
=======
            Item item = new Item();
            item.SetDefaults(ItemType<TheGuide>());//开局获得物品
            items.Add(item);
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96
        }

        public int AthanasyTimer = 0;
        private float CrazyProgress = 1.0f, CrazyDire;
        public override void ModifyScreenPosition()
        {
            if (Main.gameMenu)
            {
                return;
            }
            Vector2 size = new Vector2((float)Main.screenWidth, (float)Main.screenHeight);
            // 枪抖屏幕
            if (player.itemAnimation > 0 && ItemSafe(player.HeldItem) && !Main.gamePaused && AEntrogicConfigClient.Instance.GunEffect)
            {
                if (player.HeldItem.useAmmo == AmmoID.Bullet && player.HeldItem.useTime <= 12)
                {
                    float useTimes = 60 / player.HeldItem.useTime;
                    useTimes = (useTimes / 24f) + 0.3f;
                    useTimes = MathHelper.Min(1.6f, useTimes);
                    Main.screenPosition += Main.rand.NextVector2Square(-2f, 2f) * useTimes;
                }
            }
            if (AEntrogicConfigClient.Instance.ThatsCrazy && !Main.dedServ)
            {
                Main.screenPosition += Main.rand.NextVector2Square(-8f, 8f);
                Filters.Scene.Activate("Entrogic:GooddShader", Vector2.Zero);
                float gr = -1f, gg = -1f, gb = -1f;
                switch (ModTimer % 9)
                {
                    case 0:
                    case 1:
                    case 2:
                        gr = 0.9f;
                        break;
                    case 3:
                    case 4:
                    case 5:
                        gg = 0.9f;
                        break;
                    case 6:
                    case 7:
                    case 8:
                        gb = 0.9f;
                        break;
                }
                Filters.Scene["Entrogic:GooddShader"].GetShader().UseColor(gr, gg, gb);
                Filters.Scene["Entrogic:GooddShader"].GetShader().UseOpacity(1f);
                if (CrazyDire <= 0)
                {
                    CrazyProgress += 0.15f;
                    if (CrazyProgress >= 2.0f)
                    {
                        CrazyDire = 1;
                    }
                }
                else
                {
                    CrazyProgress -= 0.15f;
                    if (CrazyProgress <= 0f)
                    {
                        CrazyDire = 0;
                    }
                }
                Filters.Scene["Entrogic:GooddShader"].GetShader().UseProgress((int)CrazyProgress);
            }
            else if (Filters.Scene["Entrogic:GooddShader"].IsActive())
            {
                Filters.Scene["Entrogic:GooddShader"].Deactivate();
            }
            if (AthanasyTimer > 150 || NPC.FindFirstNPC(NPCType<Antanasy>()) < 0)
                AthanasyTimer = 0;
            if (NPC.FindFirstNPC(NPCType<Antanasy>()) >= 0)
            {
                NPC npc = Main.npc[NPC.FindFirstNPC(NPCType<Antanasy>())];
                if (AthanasyTimer > 0 && AthanasyTimer <= 150)
                {
                    Vector2 playerPosition = player.Center - size / 2f;
                    Vector2 targetPosition = npc.Center - size / 2f;
                    Vector2 dis = targetPosition - playerPosition;
                    Vector2 disOne = dis / 10f;

                    float screenMove = Math.Min(10f, AthanasyTimer);
                    if (AthanasyTimer >= 140)
                        screenMove = 10 - (AthanasyTimer - 140);
                    Main.screenPosition = playerPosition + disOne * screenMove;
                    player.statLife = LifeLastTime;
                    if (!Main.gamePaused)
                    {
                        AthanasyTimer++;
                    }
                }
            }
            if (AthanasyTimer == 70)
<<<<<<< HEAD
                SoundEngine.PlaySound(SoundID.Zombie, (int)(Main.screenPosition.X + size.X / 2f), (int)(Main.screenPosition.Y + size.Y / 2f), 92, 0.7f, 0.4f);
=======
                Main.PlaySound(SoundID.Zombie, (int)(Main.screenPosition.X + size.X / 2f), (int)(Main.screenPosition.Y + size.Y / 2f), 92, 0.7f, 0.4f);
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96
        }

        public override void UpdateBiomeVisuals()
        {
            bool useRainyDaysScreen = NPC.AnyNPCs(NPCType<PollutionElemental>());
            player.ManageSpecialBiomeVisuals("Entrogic:RainyDaysScreen", useRainyDaysScreen);

            bool antanasyPhase2 = false;
            foreach (NPC npc in Main.npc)
                if (npc.active && npc.type == NPCType<Antanasy>() && npc.ai[2] != 0)
                    antanasyPhase2 = true;
            bool useGrayScreen = antanasyPhase2 && !useRainyDaysScreen;
            player.ManageSpecialBiomeVisuals("Entrogic:GrayScreen", useGrayScreen);

            bool magicStorm = EntrogicWorld.magicStorm;
            bool useMagicScreen = magicStorm && !antanasyPhase2 && !useRainyDaysScreen;
            player.ManageSpecialBiomeVisuals("Entrogic:MagicStormScreen", useMagicScreen);
        }

        /// <summary>
        /// Allows you to set biome variables in your ModPlayer class based on tile counts.
        /// </summary>
        public override void UpdateBiomes()
        {
            IsZoneLife = EntrogicWorld.Check(player.position.X, player.position.Y);
        }

        /// <summary>
        /// Whether or not this player and the other player parameter have the same custom biome variables. This hook is used to help with client/server syncing. Returns true by default.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public override bool CustomBiomesMatch(Player other)
        {
            EntrogicPlayer modOther = other.GetModPlayer<EntrogicPlayer>();
            return IsZoneLife == modOther.IsZoneLife;
        }

        /// <summary>
        /// In this hook, you should copy the custom biome variables from this player to the other player parameter. This hook is used to help with client/server syncing.
        /// </summary>
        /// <param name="other"></param>
        public override void CopyCustomBiomesTo(Player other)
        {
            EntrogicPlayer modOther = other.GetModPlayer<EntrogicPlayer>();
            modOther.IsZoneLife = IsZoneLife;
        }

        /// <summary>
        /// Allows you to send custom biome information between client and server.
        /// </summary>
        /// <param name="writer"></param>
        public override void SendCustomBiomes(BinaryWriter writer)
        {
            BitsByte flags = new BitsByte();
            flags[0] = IsZoneLife;
            writer.Write(flags);
        }

        /// <summary>
        /// Allows you to do things with the custom biome information you send between client and server.
        /// </summary>
        /// <param name="reader"></param>
        public override void ReceiveCustomBiomes(BinaryReader reader)
        {
            BitsByte flags = reader.ReadByte();
            IsZoneLife = flags[0];
        }

        /// <summary>
        /// Allows you to increase the player's life regeneration based on its state. This can be done by incrementing player.lifeRegen by a certain number. The player will recover life at a rate of half the number you add per second. You can also increment player.lifeRegenTime to increase the speed at which the player reaches its maximum natural life regeneration.
        /// </summary>
        public override void UpdateLifeRegen()
        {
            if (HasReborned)
            {
                player.lifeRegen = Math.Min(0, player.lifeRegen);
                player.lifeRegenCount = Math.Min(0, player.lifeRegenCount);
                player.lifeRegenTime = Math.Min(0, player.lifeRegenTime);
            }
        }

        /// <summary>
        /// Allows you to modify the power of the player's natural life regeneration. This can be done by multiplying the regen parameter by any number. For example, campfires multiply it by 1.1, while walking multiplies it by 0.5.
        /// </summary>
        /// <param name="regen"></param>
        public override void NaturalLifeRegen(ref float regen)
        {
            if (HasReborned)
                regen = 0f;
        }

        public int colorDecR, colorDecG, colorDecB, colorTimer;
        public float zoomX, zoomY;
        /// <summary>
        /// 在检查玩家是否存在之后，在该玩家的每次刻度更新开始时调用此方法（自动暂停不调用）
        /// </summary>
        public override void PreUpdate()
        {
            /*zoomX += (Main.mouseX - Main.screenWidth / 2) / 20;
            zoomY += (Main.mouseY - Main.screenHeight / 2) / 20;
            Main.zoomX = zoomX;
            Main.zoomY = zoomY;*/
            if (Sins >= SinsMax) Sins = SinsMax;
            if (Sins < 0) Sins = 0;

            CardPassDelay--;
            CardPassDelay = Math.Max(0, CardPassDelay);
            CardPassDelay = Math.Min(CardPassStatDelay, CardPassDelay);
            CardWashDelay--;
            CardWashDelay = Math.Max(0, CardWashDelay);
            CardWashDelay = Math.Min(CardWashStatDelay, CardWashDelay);

            _previousKey = _currentKey;
            _currentKey = Keyboard.GetState();
            if (_currentKey.IsKeyUp(Keys.Z) && _previousKey.IsKeyDown(Keys.Z))
            {
<<<<<<< HEAD
                CardGameType[0] = ItemType<Items.Weapons.Card.Elements.AstralImpact>();
                CardGameType[1] = ItemType<Items.Weapons.Card.Elements.Fireball>();
                CardGameType[2] = ItemType<Items.Weapons.Card.Elements.BetrayerofDarkFlame>();
=======
                for (int i = 0; i < 3; i++)
                {
                    CardGameType[i] = ItemType<Items.Weapons.Card.Elements.ArcaneMissle>();
                }
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96
            }
            if (_currentKey.IsKeyUp(Keys.N) && _previousKey.IsKeyDown(Keys.N))
            {
                CardGaming = false;
            }
            if (_currentKey.IsKeyDown(Keys.X))
            {
<<<<<<< HEAD
                CardGamePlayerHealth += 3;
            }
            if (_currentKey.IsKeyDown(Keys.C))
            {
                CardGamePlayerHealth -= 3;
=======
                CardGamePlayerHealth+=3;
            }
            if (_currentKey.IsKeyDown(Keys.C))
            {
                CardGamePlayerHealth-=3;
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96
            }

            if (player.ZoneUnderworldHeight && !EntrogicWorld.beArrivedAtUnderworld)
            {
                EntrogicWorld.beArrivedAtUnderworld = true;
                if (Main.netMode == NetmodeID.Server)
                    NetMessage.SendData(MessageID.WorldData);
            }
            DebugModeNewText("Inner Rage: " + IsDestroyNextCard_InnerRage, false);
            #region 又臭又长的代码
            int random = Main.rand.Next(5, 16);
            if (colorTimer == 0)
            {
                colorDecR += random;
                if (colorDecR >= 255)
                {
                    colorDecR = 255;
                    colorTimer++;
                }
            }
            else if (colorTimer == 1)
            {
                colorDecG -= random;
                if (colorDecG <= 0)
                {
                    colorDecG = 0;
                    colorTimer++;
                }
            }
            else if (colorTimer == 2)
            {
                colorDecB += random;
                if (colorDecB >= 255)
                {
                    colorDecB = 255;
                    colorTimer++;
                }
            }
            else if (colorTimer == 3)
            {
                colorDecR -= random;
                if (colorDecR <= 0)
                {
                    colorDecR = 0;
                    colorTimer++;
                }
            }
            else if (colorTimer == 4)
            {
                colorDecG += random;
                if (colorDecG >= 255)
                {
                    colorDecG = 255;
                    colorTimer++;
                }
            }
            else if (colorTimer == 5)
            {
                colorDecB -= random;
                if (colorDecB <= 0)
                {
                    colorDecB = 0;
                    colorTimer = 0;
                }
            }
            #endregion
        }
        public override void PreUpdateMovement()
        {
            Point posPoint = new Point((int)(player.position.X / 16f), (int)(player.position.Y / 16f));
            if (CanSwimTile && WorldGen.InWorld(posPoint.X, posPoint.Y))
            {
                Tile posTile = Main.tile[posPoint.X, posPoint.Y];
                int leftSafeAmount = 0;
                int rightSafeAmount = 0;
                int downSafeAmount = 0;
                int upSafeAmount = 0;
                #region Count Amount
                for (int i = 0; i < 3; i++)
                {
                    if (WorldGen.InWorld(posPoint.X - 1, posPoint.Y + i))
                    {
                        Tile t = Main.tile[posPoint.X - 1, posPoint.Y + i];
                        if (t.active() && TileDirt(t))
                        {
                            leftSafeAmount++;
                        }
                        if (t.active() && !Main.tileSolidTop[t.type] && Main.tileSolid[t.type] && !TileDirt(t))
                        {
                            leftSafeAmount = 0;
                            break;
                        }
                    }
                }
                for (int i = 0; i < 3; i++)
                {
                    if (WorldGen.InWorld(posPoint.X + 2, posPoint.Y + i))
                    {
                        Tile t = Main.tile[posPoint.X + 2, posPoint.Y + i];
                        if (t.active() && TileDirt(t))
                        {
                            rightSafeAmount++;
                        }
                        if (t.active() && !Main.tileSolidTop[t.type] && Main.tileSolid[t.type] && !TileDirt(t))
                        {
                            rightSafeAmount = 0;
                            break;
                        }
                    }
                }
                for (int i = 0; i < 2; i++)
                {
                    if (WorldGen.InWorld(posPoint.X + i, posPoint.Y + 3))
                    {
                        Tile t = Main.tile[posPoint.X + i, posPoint.Y + 3];
                        if (t.active() && TileDirt(t))
                        {
                            downSafeAmount++;
                        }
                        if (t.active() && !Main.tileSolidTop[t.type] && Main.tileSolid[t.type] && !TileDirt(t))
                        {
                            downSafeAmount = 0;
                            break;
                        }
                    }
                }
                for (int i = 0; i < 2; i++)
                {
                    for (int j = -1; j < 2; j++)
                    {
                        if (WorldGen.InWorld(posPoint.X + i, posPoint.Y - j))
                        {
                            Tile t = Main.tile[posPoint.X + i, posPoint.Y - j];
                            if (t.active() && TileDirt(t))
                            {
                                upSafeAmount++;
                            }
                            if (t.active() && !Main.tileSolidTop[t.type] && Main.tileSolid[t.type] && !TileDirt(t))
                            {
                                upSafeAmount = 0;
                                break;
                            }
                        }
                    }
                }
                #endregion
                if (leftSafeAmount >= 1 && (player.controlLeft || player.velocity.X < 0))
                {
                    player.position.X -= 0.6f;
                    player.slowFall = true;
                }
                if (rightSafeAmount >= 1 && (player.controlRight || player.velocity.X > 0))
                {
                    player.position.X += 0.6f;
                    player.slowFall = true;
                }
                if (downSafeAmount >= 1 && player.controlDown)
                {
                    player.position.Y += 0.6f;
                    player.slowFall = true;
                }
                if (upSafeAmount >= 1 && (player.controlJump || player.controlUp))
                {
                    player.position.Y -= 0.6f;
                    player.velocity.Y = -4f; // 我推荐用"="
                    player.slowFall = true;
                }
            }
        }
        /// <summary>
        /// This is called sometime after SetControls is called, and right before all the buffs update on this player. This hook can be used to add buffs to the player based on the player's state (for example, the Campfire buff is added if the player is near a Campfire).
        /// </summary>
        public override void PreUpdateBuffs()
        {
            base.PreUpdateBuffs();
            if (player == Main.LocalPlayer)
            {
<<<<<<< HEAD
                if (EntrogicWorld.Check(player.Center.X, player.Center.Y) && player.wet)
                {
                    player.AddBuff(BuffType<Dissolve>(), 90);
                }
=======
                player.AddBuff(BuffType<Dissolve>(), 90);
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96
            }
        }

        /// <summary>
        /// Called after Update Accessories. 
        /// </summary>
        public override void UpdateEquips()
        {
            if (player.armor[0].type == ItemType<Items.PollutElement.PollutionElementalMask>() && player.armor[10].headSlot < 0)
                IsPolluHeadActive = true;
            else if (player.armor[10].type == ItemType<Items.PollutElement.PollutionElementalMask>())
                IsPolluHeadActive = true;
        }

        public int LifeLastTime = 0;
        public int ticks = 0;
        public bool savedSinceMenuOpen = false;
        /// <summary>
        /// This is called at the very end of the Player.Update method. Final general update tasks can be placed here.
        /// </summary>
        public override void PostUpdate()
        {
            if (player.whoAmI != Main.myPlayer)
            {
                return;
            }
            if (HasReborned)
            {
                if (LifeLastTime < player.statLife)
                    player.statLife = LifeLastTime;
            }
            LifeLastTime = player.statLife;
            CardRecentEventAlpha -= 2;
            CardRecentEventAlpha = Math.Max(80, CardRecentEventAlpha);
            // 卡牌存储
<<<<<<< HEAD
            //if (!Main.gameMenu && !Main.dedServ)
            //{
            //    ticks++;
            //    if ((!Main.ingameOptionsWindow && ticks > 600) || (Main.ingameOptionsWindow && !savedSinceMenuOpen))
            //    {
            //        if (BEntrogicConfigServer.Instance.ClearNewPlayersCard && Main.netMode == NetmodeID.MultiplayerClient)
            //        {
            //            ModHelper.GetWorldPathFromName(Main.worldName, SocialAPI.Cloud != null, out string worldName);
            //            string path = string.Format(PlayerFolder + "CardData" + worldName + ".entini");
            //            SaveCardData(path);
            //        }
            //        else
            //        {
            //            string path = string.Format(PlayerFolder + "CardData.entini");
            //            SaveCardData(path);
            //        }
            //        ticks = 0;
            //    }
            //    if (Main.ingameOptionsWindow)
            //    {
            //        savedSinceMenuOpen = true;
            //        goto GetOffCardSaving;
            //    }
            //    savedSinceMenuOpen = false;
            //}
=======
            if (!Main.gameMenu && !Main.dedServ)
            {
                ticks++;
                if ((!Main.ingameOptionsWindow && ticks > 600) || (Main.ingameOptionsWindow && !savedSinceMenuOpen))
                {
                    if (BEntrogicConfigServer.Instance.ClearNewPlayersCard && Main.netMode == NetmodeID.MultiplayerClient)
                    {
                        string path = string.Format(ServerPlayerFolder + "CardData" + Main.worldName + ".entini");
                        SaveCardData(path);
                    }
                    else
                    {
                        string path = string.Format(PlayerFolder + "CardData.entini");
                        SaveCardData(path);
                    }
                    ticks = 0;
                }
                if (Main.ingameOptionsWindow)
                {
                    savedSinceMenuOpen = true;
                    goto GetOffCardSaving;
                }
                savedSinceMenuOpen = false;
            }
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96
            GetOffCardSaving:
            oldPosition[0] = player.position;
            for (int i = oldPosition.Length - 1; i >= 1; i--)
            {
                oldPosition[i] = oldPosition[i - 1];
            }
            DebugModeNewText(MaxAmount.ToString(), false);
            DebugModeNewText(PlagueNPCAmount.ToString(), false);
            MaxAmount = Math.Max(1, MaxAmount);
            if (PlagueNPCAmount >= 15 * MaxAmount)
            {
                MaxAmount++;
                if (LibNum > 0)
                {
                    NewRecentCardMessage(Language.GetTextValue("Mods.Entrogic.PlagueDrawCard", 15 * MaxAmount));
                    List<int> canChooseCards = new List<int>();
                    for (int i = 0; i < CardReadyType.Length; i++)
                    {
                        if (CardReadyType[i] == 0)
                            continue;
                        canChooseCards.Add(i);
                    }
                    if (canChooseCards.Count > 0)
                    {
                        for (int i = 0; i < CardHandType.Length; i++)
                            if (CardHandType[i] == 0)
                            {
                                int chooseCard = Main.rand.Next(0, canChooseCards.Count);
                                CardHandType[i] = CardReadyType[canChooseCards[chooseCard]];
                                CardHandCost[i] = CardReadyCost[canChooseCards[chooseCard]];
                                CardReadyType[canChooseCards[chooseCard]] = 0;
                                CardReadyCost[canChooseCards[chooseCard]] = 0;
                                break;
                            }
                    }
                }
            }
            if (PlagueNPCAmount <= 0)
                MaxAmount = 1;
            PlagueNPCAmount = 0;
        }

        /// <summary>
        /// Allows you to modify the armor and accessories that visually appear on the player. In addition, you can create special effects around this character, such as creating dust.
        /// </summary>
        public override void FrameEffects()
        {
            if (IsPolluHeadActive)
            {
                int HeadFrameDelay = 5;
                PolluHeadTimer++;
                if (PolluHeadTimer <= HeadFrameDelay)
<<<<<<< HEAD
                    player.head = Instance.GetEquipSlot("PollutionElementalMask1", EquipType.Head);
                else if (PolluHeadTimer <= HeadFrameDelay * 2)
                    player.head = Instance.GetEquipSlot("PollutionElementalMask2", EquipType.Head);
                else if (PolluHeadTimer <= HeadFrameDelay * 3)
                    player.head = Instance.GetEquipSlot("PollutionElementalMask3", EquipType.Head);
                else if (PolluHeadTimer <= HeadFrameDelay * 4)
                    player.head = Instance.GetEquipSlot("PollutionElementalMask4", EquipType.Head);
=======
                    player.head = mod.GetEquipSlot("PollutionElementalMask1", EquipType.Head);
                else if (PolluHeadTimer <= HeadFrameDelay * 2)
                    player.head = mod.GetEquipSlot("PollutionElementalMask2", EquipType.Head);
                else if (PolluHeadTimer <= HeadFrameDelay * 3)
                    player.head = mod.GetEquipSlot("PollutionElementalMask3", EquipType.Head);
                else if (PolluHeadTimer <= HeadFrameDelay * 4)
                    player.head = mod.GetEquipSlot("PollutionElementalMask4", EquipType.Head);
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96
                if (PolluHeadTimer >= HeadFrameDelay * 4)
                    PolluHeadTimer = 0;
            }
            Item item = new Item();
            item.SetDefaults(ItemType<Items.PollutElement.BottleofStorm>());
<<<<<<< HEAD
            if (player.wings == item.wingSlot || player.wings == Instance.GetEquipSlot("PolluWings1", EquipType.Wings) || player.wings == Instance.GetEquipSlot("PolluWings2", EquipType.Wings) || player.wings == Instance.GetEquipSlot("PolluWings3", EquipType.Wings) || player.wings == Instance.GetEquipSlot("PolluWings4", EquipType.Wings) || player.wings == Instance.GetEquipSlot("PolluWings5", EquipType.Wings) || player.wings == Instance.GetEquipSlot("PolluWings6", EquipType.Wings) || player.wings == Instance.GetEquipSlot("PolluWings7", EquipType.Wings))
=======
            if (player.wings == item.wingSlot || player.wings == mod.GetEquipSlot("PolluWings1", EquipType.Wings) || player.wings == mod.GetEquipSlot("PolluWings2", EquipType.Wings) || player.wings == mod.GetEquipSlot("PolluWings3", EquipType.Wings) || player.wings == mod.GetEquipSlot("PolluWings4", EquipType.Wings) || player.wings == mod.GetEquipSlot("PolluWings5", EquipType.Wings) || player.wings == mod.GetEquipSlot("PolluWings6", EquipType.Wings) || player.wings == mod.GetEquipSlot("PolluWings7", EquipType.Wings))
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96
            {
                bool flag18 = false;
                if (player.wingsLogic > 0 && player.controlJump && player.wingTime > 0f && !player.canJumpAgain_Cloud && player.jump == 0 && player.velocity.Y != 0f)
                {
                    flag18 = true;
                }
                if (flag18 || player.jump > 0)
                {
                    WingFrameCounter++;
                    int num79 = 3;
                    if (WingFrameCounter >= num79 * 7)
                    {
                        WingFrameCounter = 0;
                    }
                    WingFrame = WingFrameCounter / num79;
                }
                else if (player.velocity.Y != 0f)
                {
                    if (player.controlJump)
                    {
                        WingFrameCounter++;
                        int num80 = 7;
                        if (player.wingFrameCounter >= num80 * 7)
                        {
                            WingFrameCounter = 0;
                        }
                        WingFrame = WingFrameCounter / num80;
                    }
                    else
                    {
                        WingFrameCounter++;
                        int num81 = 5;
                        if (WingFrameCounter >= num81 * 7)
                        {
                            WingFrameCounter = 0;
                        }
                        WingFrame = WingFrameCounter / num81;
                    }
                }
                else
                {
                    WingFrame = -1;
                }
                if (!flag18)
                    if (player.wingsLogic > 0 && player.controlJump && player.velocity.Y > 0f)
                    {
                        if (player.velocity.Y > 0f)
                        {
                            WingFrameCounter++;
                            int num117 = 5;
                            if (WingFrameCounter >= num117 * 7)
                            {
                                WingFrameCounter = 0;
                            }
                            WingFrame = WingFrameCounter / num117;
                        }
                    }
                if (WingFrame >= 7)
                {
                    WingFrame = 0;
                    WingFrameCounter = 0;
                }
                if (!player.controlJump) // 只有在控制跳跃时显示翅膀，防止不动还显示的情况
                    WingFrame = -1;
                if (WingFrame == 0)
                    player.wings = Instance.GetEquipSlot("PolluWings1", EquipType.Wings);
                if (WingFrame == 1)
                    player.wings = Instance.GetEquipSlot("PolluWings2", EquipType.Wings);
                if (WingFrame == 2)
                    player.wings = Instance.GetEquipSlot("PolluWings3", EquipType.Wings);
                if (WingFrame == 3)
                    player.wings = Instance.GetEquipSlot("PolluWings4", EquipType.Wings);
                if (WingFrame == 4)
                    player.wings = Instance.GetEquipSlot("PolluWings5", EquipType.Wings);
                if (WingFrame == 5)
                    player.wings = Instance.GetEquipSlot("PolluWings6", EquipType.Wings);
                if (WingFrame == 6)
                    player.wings = Instance.GetEquipSlot("PolluWings7", EquipType.Wings);
                if (WingFrame == -1)
                    player.wings = Instance.GetEquipSlot("PolluWings8", EquipType.Wings);
            }
        }

        public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
<<<<<<< HEAD
        {
=======
        {           
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96
            if (NPC.AnyNPCs(NPCType<PollutionElemental>()))
            {
                NPC elemental = Main.npc[NPC.FindFirstNPC(NPCType<PollutionElemental>())];
                if (elemental.active && player.Distance(elemental.Center) <= Main.screenWidth * 8f && elemental.ai[0] >= 6f && elemental.ai[0] <= 9f)
                {
                    // 污染之灵第二阶段的时候玩家不会受到击退
                    player.noKnockback = true;
                }
            }
            return base.PreHurt(pvp, quiet, ref damage, ref hitDirection, ref crit, ref customDamage, ref playSound, ref genGore, ref damageSource);
        }

        /// <summary>
        /// Allows you to make anything happen right before damage is subtracted from the player's health.
        /// </summary>
        /// <param name="pvp"></param>
        /// <param name="quiet"></param>
        /// <param name="damage"></param>
        /// <param name="hitDirection"></param>
        /// <param name="crit"></param>
        public override void Hurt(bool pvp, bool quiet, double damage, int hitDirection, bool crit)
        {
            if (CanExplode && damage >= 0 && Main.rand.Next(2) == 0)
            {
<<<<<<< HEAD
                SoundEngine.PlaySound(SoundID.Item14, player.position);
=======
                Main.PlaySound(SoundID.Item14, player.position);
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96
                Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, 0f, ProjectileType<Explode>(), 50, 30, player.whoAmI);
            }
        }

        /// <summary>
        /// This hook is called whenever the player is about to be killed after reaching 0 health. Set the playSound parameter to false to stop the death sound from playing. Set the genGore parameter to false to stop the gore and dust from being created. (These are useful for creating your own sound or gore.) Return false to stop the player from being killed. Only return false if you know what you are doing! Returns true by default.
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="hitDirection"></param>
        /// <param name="pvp"></param>
        /// <param name="playSound"></param>
        /// <param name="genGore"></param>
        /// <param name="damageSource"></param>
        /// <returns></returns>
        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore,
            ref PlayerDeathReason damageSource)
        {
            if (CanReborn && !HasReborned)
            {
                player.statLife = player.statLifeMax2;
                LifeLastTime = player.statLife;
                player.immune = true;
                player.immuneTime = 180;
                if (player.longInvince)
                {
                    player.immuneTime += 40;
                }
                for (int m = 0; m < player.hurtCooldowns.Length; m++)
                {
                    player.hurtCooldowns[m] = player.immuneTime;
                }
                if (player.whoAmI == Main.myPlayer)
                {
                    NetMessage.SendData(MessageID.Dodge, -1, -1, null, player.whoAmI, 1f, 0f, 0f, 0, 0, 0);
                }
                HasReborned = true;
                RebornEffectTime = 60 + 30;
                SoundEngine.PlaySound(SoundID.Item103, player.position);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Allows you to modify the damage, knockback, etc., that this player does to an NPC by swinging a melee weapon.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="target"></param>
        /// <param name="damage"></param>
        /// <param name="knockback"></param>
        /// <param name="crit"></param>
        public override void ModifyHitNPC(Item item, NPC target, ref int damage, ref float knockback, ref bool crit)
        {
            if (crit)
            {
                damage = (int)(damage * CritDamagePoint);
            }
            if (item.pick > 0)
            {
                if (target.type == NPCType<StoneSlime>())
                {
                    damage += item.pick * (int)10;
                }
            }
        }

        /// <summary>
        /// Allows you to create special effects when this player hits an NPC by swinging a melee weapon (for example how the Pumpkin Sword creates pumpkin heads).
        /// </summary>
        /// <param name="item"></param>
        /// <param name="target"></param>
        /// <param name="damage"></param>
        /// <param name="knockback"></param>
        /// <param name="crit"></param>
        public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
        {
            if (target.life == 0)
            {
                if (target.boss)
                {
                    if (!Main.hardMode) Sins += 1500;
                    else if (!NPC.downedPlantBoss) Sins += 3000;
                    else Sins += 4500;
                }
                else if (Main.hardMode)
                {
                    if (!NPC.downedPlantBoss) Sins += 15;
                    else Sins += 50;
                }
            }
            if (Main.rand.Next(3) == 0)
            {
                if (IsConfused)
                {
                    player.AddBuff(BuffID.Confused, 30);
                }
            }

            if (CanLifeSteal20)
            {
                if (target.type != NPCID.TargetDummy)
                {
                    int num = Main.rand.Next(20);
                    int num2 = Main.rand.Next(1, num + 1);
                    player.statLife += num2;
                    player.HealEffect(num2, true);
                }
            }
        }

        /// <summary>
        /// Allows you to modify the damage, knockback, etc., that a projectile created by this player does to an NPC.
        /// </summary>
        /// <param name="proj"></param>
        /// <param name="target"></param>
        /// <param name="damage"></param>
        /// <param name="knockback"></param>
        /// <param name="crit"></param>
        /// <param name="hitDirection"></param>
        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (proj.friendly && proj.DamageType == DamageClass.Ranged && !proj.arrow && ProjectieHasArmorPenetration)
            {
                player.armorPenetration += 35;
            }
            if (crit)
            {
                damage = (int)(damage * CritDamagePoint);
            }
            if (PickPowerHand > 0 && proj.minionSlots == 0)
            {
                if (target.type == NPCType<StoneSlime>())
                {
                    damage += PickPowerHand * (int)10;
                }
            }
        }

        /// <summary>
        /// Allows you to create special effects when a projectile created by this player hits an NPC (for example, inflicting debuffs).
        /// </summary>
        /// <param name="proj"></param>
        /// <param name="target"></param>
        /// <param name="damage"></param>
        /// <param name="knockback"></param>
        /// <param name="crit"></param>
        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            if (CanLifeSteal20)
            {
                if (target.type != NPCID.TargetDummy)
                {
                    int num = Main.rand.Next(20);
                    int num2 = Main.rand.Next(1, num + 1);
                    player.statLife += num2;
                    player.HealEffect(num2, true);
                }
            }
            if (Main.rand.Next(3) == 0)
            {
                if (IsConfused)
                {
                    player.AddBuff(BuffID.Confused, 30);
                }
            }
        }
<<<<<<< HEAD
        public float AnkhAlpha = 1f;
        public float AnkhScale = 0f;
=======
        int AnkhAlpha = 255;
        float AnkhScale = 0f;
        public static readonly PlayerLayer GelAnkhEffect = new PlayerLayer("Entrogic", "Gel Ankh", PlayerLayer.MiscEffectsFront, delegate (PlayerDrawInfo drawInfo)
        {
            if (drawInfo.shadow != 0f)
            {
                return;
            }
            Player drawPlayer = drawInfo.drawPlayer;
            EntrogicPlayer modPlayer = drawPlayer.GetModPlayer<EntrogicPlayer>();
            if (modPlayer.RebornEffectTime > 0)
            {
                modPlayer.RebornEffectTime--;
                if (modPlayer.RebornEffectTime > 80)
                    modPlayer.AnkhScale += 0.08f;
                if (modPlayer.RebornEffectTime <= 25)
                    modPlayer.AnkhAlpha -= 11;
                if (modPlayer.AnkhAlpha > 255)
                    modPlayer.AnkhAlpha = 255;
                Texture2D texture = ModTexturesTable["凝胶安卡"];
                int drawX = (int)(drawInfo.position.X + drawPlayer.width / 2f - Main.screenPosition.X);
                int drawY = (int)(drawInfo.position.Y + drawPlayer.height / 2f - Main.screenPosition.Y);

                DrawData data = new DrawData(texture, new Vector2(drawX, drawY), null, new Color(modPlayer.AnkhAlpha, modPlayer.AnkhAlpha, modPlayer.AnkhAlpha, modPlayer.AnkhAlpha), 0f, texture.Size() * 0.5f, modPlayer.AnkhScale, SpriteEffects.None, 0);
                Main.playerDrawData.Add(data);
            }
            else
            {
                modPlayer.AnkhScale = 0f;
                modPlayer.AnkhAlpha = 255;
            }
        });
        public static readonly PlayerLayer BookBubbleEffect = new PlayerLayer("Entrogic", "Book Bubble", PlayerLayer.MiscEffectsFront, delegate (PlayerDrawInfo drawInfo)
        {
            if (drawInfo.shadow != 0f)
            {
                return;
            }
            Player drawPlayer = drawInfo.drawPlayer;
            Texture2D t = ModTexturesTable["ReadingBubble"];
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
                DrawData data = new DrawData(t,
                    drawPosition.NoShake(),
                    (Rectangle?)_frame,
                    Color.White,
                    0f,
                    origin,
                    1f,
                    SpriteEffects.None,
                    0);
                Main.playerDrawData.Add(data);

                if (ModHelper.MouseInRectangle(ModHelper.CreateFromVector2(drawPosition - origin, t.Width, frameHeight)) && drawPlayer.whoAmI != Main.myPlayer)
                {
                    Main.instance.MouseText($"{Language.GetTextValue("Mods.Entrogic.Common.Reading")}: {drawPlayer.HeldItem.Name}\n" +
                    $"{Language.GetTextValue("Mods.Entrogic.Common.Page")}: {entrogicPlayer.PageNum * 2 - 1} ~ {entrogicPlayer.PageNum * 2}");
                }
            }
        });
        /// <summary>
        /// Allows you to modify the drawing of the player. This is done by removing from, adding to, or rearranging the list, by setting some of the layers' visible field to false, etc.
        /// </summary>
        /// <param name="layers"></param>
        public override void ModifyDrawLayers(List<PlayerLayer> layers)
        {
            GelAnkhEffect.visible = true;
            layers.Add(GelAnkhEffect);
            BookBubbleEffect.visible = true;
            layers.Add(BookBubbleEffect);
        }
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96

        /// <summary>
        /// Called on the LocalPlayer when that player enters the world. SP and Client. Only called on the player who is entering. A possible use is ensuring that UI elements are reset to the configuration specified in data saved to the ModPlayer. Can also be used for informational messages.
        /// </summary>
        /// <param name="player">The player that entered the world.</param>
        public override void OnEnterWorld(Player player)
        {
            //foreach (ModItem i in Entrogic.modItems())
            //    if (i.mod == ModLoader.GetMod("Entrogic"))
            //        Main.NewText(Entrogic.modItems().IndexOf(i));
            //Main.NewText(Entrogic.FirstModItem());
            if (player == null || player.whoAmI != Main.myPlayer)
            {
                return;
            }
            if (Main.netMode == NetmodeID.MultiplayerClient)
<<<<<<< HEAD
=======
            {
                // 创建一个属于这个Mod的ModPacket
                ModPacket packet = mod.GetPacket();
                // 往里面写入一个封包ID，类型为byte
                packet.Write((byte)EntrogicModMessageType.ReceiveMagicStormMPC);
                // 发送出去
                packet.Send(-1, -1);
            }
            if (IsDev) { Main.NewText(PlayerFolder); }
            if (BEntrogicConfigServer.Instance.ClearNewPlayersCard && Main.netMode == NetmodeID.MultiplayerClient)
            {
                if (!Directory.Exists(ServerPlayerFolder))
                {
                    Directory.CreateDirectory(ServerPlayerFolder);
                }
                string savePath = string.Format(ServerPlayerFolder + "CardData" + Main.worldName + ".entini");
                if (!File.Exists(savePath))
                {
                    for (int i = 0; i < CardType.Length; i++)
                    {
                        CardType[i] = 0;
                    }
                    SaveCardData(savePath);
                    LoadCardData(savePath);
                }
                else
                {
                    LoadCardData(savePath);
                }
            }
            else if (!Main.dedServ)
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96
            {
                // 创建一个属于这个Mod的ModPacket
                ModPacket packet = Instance.GetPacket();
                // 往里面写入一个封包ID，类型为byte
                packet.Write((byte)EntrogicModMessageType.ReceiveMagicStormMPC);
                // 发送出去
                packet.Send(-1, -1);
            }

            for (int i = 0; i < CardReadyType.Length; i++)
            {
                CardReadyType[i] = 0;
                CardReadyCost[i] = 0;
            }
            for (int i = 0; i < CardHandType.Length; i++)
            {
                CardHandType[i] = 0;
                CardHandCost[i] = 0;
            }
            for (int i = 0; i < CardGraveType.Length; i++)
            {
                CardGraveType[i] = 0;
            }
            EntrogicModSystem.Instance.CardInventoryUI.UpdateSlots();
        }

        public override void PostBuyItem(NPC vendor, Item[] shopInventory, Item item)
        {
            if (vendor.type == NPCType<CardMerchant>() && item.type == ItemType<RandomCard>())
            {
                CardMerchant.CardWillingness += 0.06f;
                foreach (var shopItem in shopInventory)
                {
                    if (!shopItem.IsAir && shopItem.type == item.type)
                    {
                        shopItem.shopCustomPrice = 15 - (int)(CardMerchant.CardWillingness * 10.0);
                    }
                }
            }
            else if (vendor.type == NPCType<CardMerchant>()) CardMerchant.CardWillingness += 0.024f;
            base.PostBuyItem(vendor, shopInventory, item);
        }

        /// <summary>
        /// Called when a player respawns in the world.
        /// </summary>
        /// <param name="player">The player that respawns</param>
        public override void OnRespawn(Player player)
        {
            HasReborned = false;
        }

<<<<<<< HEAD
        public override bool ShiftClickSlot(Item[] inventory, int context, int slot)
        {
            int slotCard = ModHelper.FindFirst(CardType, 0);
            if (inventory[slot].type != ItemID.None && !inventory[slot].IsAir && inventory[slot].GetGlobalItem<EntrogicItem>().card && slotCard != -1 && EntrogicModSystem.Instance.CardInventoryUI.slotActive)
=======
        public override void DrawEffects(PlayerDrawInfo drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
        {
            if (!Main.dedServ)
            {
                if (EntrogicWorld.SnowZoneTiles > 80)
                {
                    Filters.Scene.Activate("Entrogic:IceScreen", Vector2.Zero);
                    Filters.Scene["Entrogic:IceScreen"].GetShader().UseProgress(MathHelper.Min((float)(EntrogicWorld.SnowZoneTiles - 80) / 520f, 1.1f));
                }
                else if (Filters.Scene["Entrogic:IceScreen"].IsActive())
                {
                    Filters.Scene["Entrogic:IceScreen"].Deactivate();
                }
            }

            base.DrawEffects(drawInfo, ref r, ref g, ref b, ref a, ref fullBright);
        }

        public override bool ShiftClickSlot(Item[] inventory, int context, int slot)
        {
            int slotCard = ModHelper.FindFirst(CardType, 0);
            if (inventory[slot].type != ItemID.None && !inventory[slot].IsAir && inventory[slot].GetGlobalItem<EntrogicItem>().card && slotCard != -1 && Instance.CardInventoryUI.slotActive)
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96
            {
                if (!CardInventoryGridSlot.AllowPutin(EntrogicModSystem.Instance.CardInventoryUI.Grid[slotCard].inventoryItem, inventory[slot], slotCard))
                    return false;
                CardType[slotCard] = inventory[slot].type;
                EntrogicModSystem.Instance.CardInventoryUI.UpdateSlots(slotCard);
                inventory[slot].stack--;
                if (inventory[slot].stack <= 0)
                {
                    inventory[slot] = new Item();
                }
                SoundEngine.PlaySound(SoundID.Grab);
                return true;
            }
            return base.ShiftClickSlot(inventory, context, slot);
        }
        public virtual void CardSetToGrave(int type, int number = -1, bool special = false, int packType = 1, bool drawCard = false, bool gravedFindHand = true, bool gravedFindReady = true)
        {
            if (type != 0)
            {
                Item item = new Item();
                item.SetDefaults(type);
                ModCard card = (ModCard)item.modItem;
                if (card.special)
                    card.SpecialEffects(player, player.position, item.damage, item.knockBack, number, packType, special, drawCard);
                if (gravedFindHand)
                    for (int i = 0; i < CardHandType.Length; i++)
                    {
                        if (CardHandType[i] == 0 || i == number && packType == 1)
                        {
                            continue;
                        }
                        Item handItem = new Item();
                        handItem.SetDefaults(CardHandType[i]);
                        ModCard handCard = (ModCard)handItem.modItem;
                        handCard.CardGraved(player, number, i, special, packType, drawCard);
                    }
                if (gravedFindReady)
                    for (int i = 0; i < CardReadyType.Length; i++)
                    {
                        if (CardReadyType[i] == 0 || i == number && packType == 2)
                        {
                            continue;
                        }
                        Item readyItem = new Item();
                        readyItem.SetDefaults(CardReadyType[i]);
                        ModCard readyCard = (ModCard)readyItem.modItem;
                        readyCard.CardGravedWhileMeReady(player, number, i, special, packType, drawCard);
                    }
                for (int i = 0; i < CardGraveType.Length; i++)
                    if (CardGraveType[i] == 0)
                    {
                        CardGraveType[i] = type;
                        break;
                    }
            }
        }
        public Item GetHoldItem()
        {
            if (Main.mouseItem != null)
                if (Main.mouseItem.type != ItemID.None)
                    return Main.mouseItem;
            if (player.inventory[player.selectedItem] != null)
                if (player.inventory[player.selectedItem].type != ItemID.None)
                    return player.inventory[player.selectedItem];
            return null;
        }
        public void NewRecentCardMessage(string text, bool useAddText = false, int alphaBase = 380)
        {
            CardRecentEvent = text;
            if (!useAddText)
                CardRecentEventAdd = "";
            CardRecentEventAlpha = alphaBase;
        }
        public void NewRecentCardMessageAdd(string text)
        {
            CardRecentEventAdd = text;
        }
    }
}