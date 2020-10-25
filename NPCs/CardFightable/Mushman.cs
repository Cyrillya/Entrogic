using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;
using Entrogic.Items.Consumables.Mushrooms;
using Entrogic.NPCs.CardFightable.CardBullet;
using Entrogic.Items.Weapons.Card.Organisms;

namespace Entrogic.NPCs.CardFightable
{
    public class Mushman : CardFightableNPC
    {
        public override void SetupDefaults()
        {
            Main.npcFrameCount[npc.type] = 16;
        }
        public override bool UsesPartyHat()
        {
            return false;
        }
        public override void SetDefaults()
        {
            npc.CloneDefaults(NPCID.Truffle);
            aiType = NPCID.Truffle;
            npc.width = 32;
            npc.height = 62;
            npc.townNPC = false;
            drawOffsetY = -2;
        }
        public bool STARTCHANGE = false;
        public bool changed = false;
        public int changeID = -1;
        public int changeAmount = 0;
        public override void AI()
        {
            if (!STARTCHANGE)
            {
                STARTCHANGE = true;
                while (changeID == -1)
                {
                    if (Main.rand.NextBool(4))
                    {
                        changeID = ItemID.GoldCoin;
                        changeAmount = Main.hardMode ? Main.rand.Next(3, 10 + 1) : Main.rand.Next(1, 4 + 1);
                    }
                    if (Main.rand.NextBool(4))
                    {
                        changeID = ItemID.FallenStar;
                        changeAmount = Main.hardMode ? Main.rand.Next(3, 9 + 1) : Main.rand.Next(1, 7 + 1);
                    }
                    if (Main.rand.NextBool(4))
                    {
                        changeID = ItemID.GlowingMushroom;
                        changeAmount = Main.hardMode ? Main.rand.Next(27, 42 + 1) : Main.rand.Next(15, 22 + 1);
                    }
                    if (Main.rand.NextBool(20))
                    {
                        changeID = ItemID.Glass;
                        changeAmount = Main.hardMode ? Main.rand.Next(15, 26 + 1) : Main.rand.Next(7, 18 + 1);
                    }
                    if (Main.rand.Next(1, 101) <= 15)
                    {
                        changeID = ItemID.LifeCrystal;
                        changeAmount = 1;
                    }
                    if (Main.rand.NextBool(10))
                    {
                        changeID = ItemID.Blinkroot;
                        changeAmount = Main.hardMode ? Main.rand.Next(4, 6 + 1) : Main.rand.Next(1, 4 + 1);
                    }
                }
                switch (WorldGen.genRand.Next(5))
                {
                    case 0:
                        npc.GivenName = "Needlemushroom";
                        break;
                    case 1:
                        npc.GivenName = "Shiitake";
                        break;
                    case 2:
                        npc.GivenName = "Straw Mushroom";
                        break;
                    case 3:
                        npc.GivenName = "Heicium";
                        break;
                    default:
                        npc.GivenName = "Mushroom";
                        break;
                }
            }
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            Player player = spawnInfo.player;
            if ((NPC.CountNPCS(NPCType<Mushman>()) < 1) && Main.dayTime && ModHelper.NoBiomeNormalSpawn(spawnInfo) && player.ZoneOverworldHeight)
            {
                if (player.ZoneRain)
                    return 0.0078f;
                else
                    return 0.005f;
            }
            return 0f;
        }
        public override void FindFrame(int frameHeight)
        {
            if (npc.velocity.Y == 0f)
            {
                if (npc.velocity.X == 0f)
                {
                    npc.frame.Y = 0;
                    npc.frameCounter = 0;
                }
                else
                {
                    npc.frameCounter++;
                    if (npc.frameCounter >= 3)
                    {
                        npc.frame.Y += frameHeight;
                        npc.frameCounter = 0;
                    }
                    if (npc.frame.Y > frameHeight * 15)
                        npc.frame.Y = frameHeight * 2;
                    npc.spriteDirection = npc.direction;
                }
            }
            else
                npc.frame.Y = frameHeight;
        }
        public override string GetChat()
        {
            int Truffle = NPC.FindFirstNPC(NPCID.Truffle);
            int rand = Main.rand.Next(5);
            if (Main.raining)
                rand = Main.rand.Next(6);
            if (Truffle >= 0)
                rand = Main.rand.Next(8);
            switch (rand)
            {
                case 0:
                    return "你或许会遇见一个蓝色的东西，他的破烂你可以翻翻看有没有用。不要误会！只是身为一个热心的菌的一个客观的建议。";
                case 1:
                    return "真菌有数百种性别，我是哪种？那就是个菌隐私了。";
                case 2:
                    return "我这里有一些路上捡到的蘑菇...一定记住不要吃它们，绝不可以做那么残忍的事情。";
                case 3:
                    return "蘑菇人喜欢一些亮晶晶的东西，我可以用一些可爱的孩子与你交换。";
                case 4://下雨天
                    return "真是个令菌舒爽的雨天啊，荧光蘑菇也在就好了...你什么时候在这的，没...没什么。";
                case 5://真菌蘑菇
                    return "那个蓝色的家伙？不要以为蘑菇人就不要以为真菌就都很聊得来，人类难道会和猩猩有共同语言？";
                case 6://真菌蘑菇
                    return Main.npc[Truffle].GivenName + " 也在这吗？";
                default:
                    return "你上过菌特网吗？哦，我忘记你是个人类了。";
            }
        }
        public override void SetupContents(ref string ImgPath, ref Vector2 ImgPosition)
        {
            ImgPath = "Entrogic/NPCs/CardFightable/Mushman";
            ImgPosition = new Vector2(274f, 12f);
            base.SetupContents(ref ImgPath, ref ImgPosition);
        }

        public override void OnSecondButtonClicked(ref bool shop)
        {
            if (Main.dedServ) return;
            Player clienPlayer = Main.LocalPlayer;
            EntrogicPlayer clientModPlayer = EntrogicPlayer.ModPlayer(clienPlayer);
            clientModPlayer.CardGameNPCIndex = npc.whoAmI;
            StartAttacking();
        }

        public override void StartAttacking()
        {
            CardGameHealthMax = 1000;
            base.StartAttacking();
        }

        public override void Initialize()
        {
            RegisterState(new RainingSpore());
            RegisterState(new FungusCannon());
        }

        public override void PreStartRound(bool playerTurn)
        {
            if (!playerTurn)
            {
                Entrogic.Instance.CardGameUI.IsUseBiggerTexture = false;
                int AttackMode = Main.rand.Next(2);
                switch (AttackMode)
                {
                    case 0:
                        SetState<RainingSpore>();
                        break;
                    case 1:
                        SetState<FungusCannon>();
                        break;
                }
            }
        }

        public override void OnFirstButtonClicked(ref bool shop)
        {
            Player player = Main.player[Main.myPlayer];
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item35);
            if (!changed)
            {
                int LifeC = 0;
                int GoldC = 0;
                int FallenS = 0;
                int LightningB = 0;
                int GlowingM = 0;
                int Glass = 0;
                for (int i = 0; i < 59; i++)
                {
                    Item item = player.inventory[i];
                    if (item != null)
                    {
                        if (item.type == ItemID.LifeCrystal)
                            LifeC += item.stack;
                        if (item.type == ItemID.GoldCoin)
                            GoldC += item.stack;
                        if (item.type == ItemID.FallenStar)
                            FallenS += item.stack;
                        if (item.type == ItemID.Blinkroot)
                            LightningB += item.stack;
                        if (item.type == ItemID.GlowingMushroom)
                            GlowingM += item.stack;
                        if (item.type == ItemID.Glass)
                            Glass += item.stack;
                    }
                }

                if (changeID == ItemID.LifeCrystal)
                    LetsChange(ItemID.LifeCrystal, LifeC, player);
                if (changeID == ItemID.GoldCoin)
                    LetsChange(ItemID.GoldCoin, GoldC, player);
                if (changeID == ItemID.FallenStar)
                    LetsChange(ItemID.FallenStar, FallenS, player);
                if (changeID == ItemID.Blinkroot)
                    LetsChange(ItemID.Blinkroot, LightningB, player);
                if (changeID == ItemID.GlowingMushroom)
                    LetsChange(ItemID.GlowingMushroom, GlowingM, player);
                if (changeID == ItemID.Glass)
                    LetsChange(ItemID.Glass, Glass, player);
            }
            else
                Main.npcChatText = "蘑菇人也没蘑菇了。";
        }

        public void LetsChange(int findID, int findInt32, Player player)
        {
            if (findInt32>=changeAmount)
            {
                int needAmount = changeAmount;
                for (int i = 0; i < 59; i++)
                {
                    Item item = player.inventory[i];
                    if (item != null)
                    {
                        if (item.type == findID && needAmount > 0)
                        {
                            if (item.stack >= needAmount)
                            {
                                item.stack -= needAmount;
                                needAmount = 0;
                            }
                            else
                            {
                                needAmount -= item.stack;
                                item.active = false;
                                item.TurnToAir();
                            }
                        }
                    }
                }
                for (int i = 0; i < Main.rand.Next(10, 18 + 1); i++)
                {
                    player.QuickSpawnItem(GettingMushrooms());
                }
                Main.npcChatText = "这是你的蘑菇，希望你能好好照顾他们！";
                changed = true;
            }
            else
            {
                if (Main.rand.NextBool(2))
                    Main.npcChatText = "别觉得真菌很好骗！";
                else
                    Main.npcChatText = "你可以现在去拿些，不过不要跑太远，我没有耐心等你那么久。";
                Main.npcChatCornerItem = changeID;
            }
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            if (changed)
                button = "交换（不可用）";
            else
                button = "交换（" + changeAmount + "个" + Lang.GetItemNameValue(changeID) + "）";

            button2 = "对局";
        }

        public override bool CanChat()
        {
            return true;
        }

        public override void NPCLoot()
        {
            for (int i = 0; i < Main.rand.Next(3, 5 + 1); i++)
            {
                Item.NewItem(npc.getRect(), GettingMushrooms());
            }
        }
        private int GettingMushrooms()
        {
            int heyouroomhere = Main.rand.Next(1, 101);
            if (heyouroomhere <= 8)//第三类
            {
                switch (Main.rand.Next(4))
                {
                    case 0:
                        return ItemType<Ganoderma>();
                    case 1:
                        return ItemType<ToxiclessMushroom>();
                    case 2:
                        return ItemType<Shiitake>();
                    default:
                        return ItemType<Voidshroom>();
                }
            }
            else if (heyouroomhere <= 30)//第二类
            {
                switch (Main.rand.Next(5))
                {
                    case 0:
                        return ItemType<Flammulina>();
                    case 1:
                        return ItemType<Boletus>();
                    case 2:
                        return ItemType<Twinshroom>();
                    case 3:
                        return ItemType<LactariusEdulis>();
                    default:
                        return ItemType<UmbrellaMushroom>();
                }
            }
            else//第一类
            {
                switch (Main.rand.Next(3))
                {
                    case 0:
                        return ItemType<Fungus>();
                    case 1:
                        return ItemType<GrassMushroom>();
                    default:
                        return ItemType<LactariusEdulis>();
                }
            }
        }
    }
}
