using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

using Entrogic.Items.Weapons.Card;
using Entrogic.Items.Materials;
using Entrogic.Items.Books.卡牌入门手册;
using System.IO;
using Terraria.ModLoader.IO;
using System.Collections.Generic;
using System;
using Terraria.Utilities;
using Entrogic.Items.Weapons.Card.Gloves;
using static Entrogic.Entrogic;
using Entrogic.Items.Weapons.Card.Organisms;
using Entrogic.Items.Weapons.Card.Elements;

namespace Entrogic.NPCs
{
    [AutoloadHead]
    public class CardMerchant : ModNPC
    {
        internal static float CardWillingness = 0f;
        public override void SetStaticDefaults()
        {
            // DisplayName automatically assigned from .lang files, but the commented line below is the normal approach.
            Main.npcFrameCount[npc.type] = 25;
            NPCID.Sets.ExtraFramesCount[npc.type] = 16;
            NPCID.Sets.AttackFrameCount[npc.type] = 4;
            NPCID.Sets.DangerDetectRange[npc.type] = 700;
            NPCID.Sets.AttackType[npc.type] = 0;
            NPCID.Sets.AttackTime[npc.type] = 90;
            NPCID.Sets.AttackAverageChance[npc.type] = 30;
            NPCID.Sets.HatOffsetY[npc.type] = 4;

            ModTranslation modTranslation = mod.CreateTranslation("Card_Unknow");
            modTranslation.AddTranslation(GameCulture.Chinese, "小伙子，你不懂的事还有很多");
            modTranslation.SetDefault("Young man, there are still many things you don't understand");
            mod.AddTranslation(modTranslation);
            modTranslation = mod.CreateTranslation("Card_Unknow2");
            modTranslation.AddTranslation(GameCulture.Chinese, "你不会想知道我的身份的");
            modTranslation.SetDefault("You'll never want to know who I'm");
            mod.AddTranslation(modTranslation);
            modTranslation = mod.CreateTranslation("Card_Manual");
            modTranslation.AddTranslation(GameCulture.Chinese, "来一份卡牌手册吧！你需要了解一下！");
            modTranslation.SetDefault("Get a card manual! You have to know!");
            mod.AddTranslation(modTranslation);
            modTranslation = mod.CreateTranslation("Card_Slime");
            modTranslation.AddTranslation(GameCulture.Chinese, "你有没有从带着皇冠的史莱姆那边找到过卡牌？只是问问罢了");
            modTranslation.SetDefault("Have you ever found a card from Slime with a crown? Just asking");
            mod.AddTranslation(modTranslation);
            modTranslation = mod.CreateTranslation("Card_Always");
            modTranslation.AddTranslation(GameCulture.Chinese, "常来问问我，我可能会有你想要的东西！");
            modTranslation.SetDefault("Come and ask me often, I may have what you want!");
            mod.AddTranslation(modTranslation);

            modTranslation = mod.CreateTranslation("Card_NWelcome");
            modTranslation.AddTranslation(GameCulture.Chinese, "好吧，一个来历不明的人是没有人会殷勤接待的");
            modTranslation.SetDefault("Well, a person of unknown origin is no one will be reception.");
            mod.AddTranslation(modTranslation);
            modTranslation = mod.CreateTranslation("Card_NShop");
            modTranslation.AddTranslation(GameCulture.Chinese, "除非你给我找个住所，否则我不会给你贩卖物品");
            modTranslation.SetDefault("I won't sell you items unless you find the place where I live.");
            mod.AddTranslation(modTranslation);

            modTranslation = mod.CreateTranslation("Card_Merchant");
            modTranslation.AddTranslation(GameCulture.Chinese, "");
            modTranslation.SetDefault("What ");
            mod.AddTranslation(modTranslation);
            modTranslation = mod.CreateTranslation("Card_Merchant2");
            modTranslation.AddTranslation(GameCulture.Chinese, "卖的东西根本一文不值！");
            modTranslation.SetDefault(" sells is worthless!");
            mod.AddTranslation(modTranslation);

            modTranslation = mod.CreateTranslation("Card_CardDay");
            modTranslation.AddTranslation(GameCulture.Chinese, "这些卡牌多有趣，不是吗？");
            modTranslation.SetDefault("These cards are so interesting, isn't it?");
            mod.AddTranslation(modTranslation);
            modTranslation = mod.CreateTranslation("Card_CardDay2");
            modTranslation.AddTranslation(GameCulture.Chinese, "要不要来看看最新的卡牌！");
            modTranslation.SetDefault("Come and see the latest cards!");
            mod.AddTranslation(modTranslation);
            modTranslation = mod.CreateTranslation("Card_CardDay3");
            modTranslation.AddTranslation(GameCulture.Chinese, "我一直不希望我与他们之间站着你这么一个瘦弱的孩子");
            modTranslation.SetDefault("I always hope a scrawny kid like you isn't all that is standing between us and them.");
            mod.AddTranslation(modTranslation);

            modTranslation = mod.CreateTranslation("Card_CardNight");
            modTranslation.AddTranslation(GameCulture.Chinese, "不来点想星星一样闪耀的卡牌吗？");
            modTranslation.SetDefault("Want some cards shining like stars?");
            mod.AddTranslation(modTranslation);
            modTranslation = mod.CreateTranslation("Card_CardNight2");
            modTranslation.AddTranslation(GameCulture.Chinese, "来些奇妙的卡牌吧，你总会用到的");
            modTranslation.SetDefault("Here and get some fantastic cards, you'll use it.");
            mod.AddTranslation(modTranslation);
            modTranslation = mod.CreateTranslation("Card_CardNight3");
            modTranslation.AddTranslation(GameCulture.Chinese, "黑暗中隐藏着无名的敌人");
            modTranslation.SetDefault("Unknown enemies hides in the dark.");
            mod.AddTranslation(modTranslation);

            //modTranslation = mod.CreateTranslation("CardShop");
            //modTranslation.AddTranslation(GameCulture.Chinese, "（卡牌物品商店）");
            //modTranslation.SetDefault("(Card Item Shop)");
            //mod.AddTranslation(modTranslation);
            //modTranslation = mod.CreateTranslation("SinShop");
            //modTranslation.AddTranslation(GameCulture.Chinese, "（罪恶物品商店）");
            //modTranslation.SetDefault("(Sins Item Shop)");
            //mod.AddTranslation(modTranslation);
            //modTranslation = mod.CreateTranslation("CShop");
            //modTranslation.AddTranslation(GameCulture.Chinese, "切换商店");
            //modTranslation.SetDefault("Cycle Shop");
            //mod.AddTranslation(modTranslation);
            modTranslation = mod.CreateTranslation("NoShop");
            modTranslation.AddTranslation(GameCulture.Chinese, "（禁用）");
            modTranslation.SetDefault("(Disable)");
            mod.AddTranslation(modTranslation);
            modTranslation = mod.CreateTranslation("Mission");
            modTranslation.AddTranslation(GameCulture.Chinese, "任务");
            modTranslation.SetDefault("Mission");
            mod.AddTranslation(modTranslation);
        }
        public override bool UsesPartyHat()
        {
            return false;
        }
        public override void SetDefaults()
        {
            npc.townNPC = true;
            npc.friendly = true;
            npc.width = 18;
            npc.height = 40;
            npc.aiStyle = 7;
            npc.damage = 10;
            npc.defense = 15;
            npc.lifeMax = 500;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.knockBackResist = 0.5f;
            animationType = NPCID.Merchant;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
        }

        public override bool CanTownNPCSpawn(int numTownNPCs, int money)
        {
            if ((NPC.downedBoss1 && (NPC.downedBoss2 || NPC.downedBoss3)) || EntrogicWorld.downedAthanasy || NPC.downedQueenBee || EntrogicWorld.downedGelSymbiosis || Main.hardMode)
                return true;
            for (int k = 0; k < 255; k++)
            {
                Player player = Main.player[k];
                if (player.active)
                {
                    int inventoryMNAmount = 0;
                    for (int j = 0; j < player.inventory.Length; j++)
                    {
                        if (player.inventory[j].type != ItemID.None)
                        {
                            if (player.inventory[j].GetGlobalItem<EntrogicItem>().card)
                                return true;
                            if (player.inventory[j].type == ItemType<拟态魔能>())
                                inventoryMNAmount += player.inventory[j].stack;
                        }
                    }
                    EntrogicPlayer ePlayer = player.GetModPlayer<EntrogicPlayer>();
                    for (int j = 0; j < ePlayer.CardType.Length; j++)
                    {
                        if (ePlayer.CardType[j] != 0)
                        {
                            return true;
                        }
                    }
                    return inventoryMNAmount >= 25;
                }
            }
            return false;
        }

        public override string TownNPCName()
        {
            // Bob、Hass、Thales、Zeno、Parmenides
            switch (WorldGen.genRand.Next(5))
            {
                case 0:
                    return "Bob";
                case 1:
                    return "Hass";
                case 2:
                    return "Thales";
                case 3:
                    return "Zeno";
                default:
                    return "Parmenides";
            }
        }

        public override void FindFrame(int frameHeight)
        {
            /*npc.frame.Width = 40;
            if (((int)Main.time / 10) % 2 == 0)
            {
                npc.frame.X = 40;
            }
            else
            {
                npc.frame.X = 0;
            }*/
        }
        public override string GetChat()
        {
            string Unknown = Language.GetTextValue("Mods.Entrogic.Card_Unknow");
            string Unknown2 = Language.GetTextValue("Mods.Entrogic.Card_Unknow2");
            string Manual = Language.GetTextValue("Mods.Entrogic.Card_Manual");
            string Slime = Language.GetTextValue("Mods.Entrogic.Card_Slime");
            string Always = Language.GetTextValue("Mods.Entrogic.Card_Always");

            string NoWelcome = Language.GetTextValue("Mods.Entrogic.Card_NWelcome");
            string Live = Language.GetTextValue("Mods.Entrogic.Card_NShop");

            string CardDay = Language.GetTextValue("Mods.Entrogic.Card_CardDay");
            string CardDay2 = Language.GetTextValue("Mods.Entrogic.Card_CardDay2");
            string CardDay3 = Language.GetTextValue("Mods.Entrogic.Card_CardDay3");
            string CardNight = Language.GetTextValue("Mods.Entrogic.Card_CardNight");
            string CardNight2 = Language.GetTextValue("Mods.Entrogic.Card_CardNight2");
            string CardNight3 = Language.GetTextValue("Mods.Entrogic.Card_CardNight3");

            string Merchant = Language.GetTextValue("Mods.Entrogic.Card_Merchant");
            string Merchant2 = Language.GetTextValue("Mods.Entrogic.Card_Merchant2");

            if (npc.homeless)
            {
                int i = Main.rand.Next(3);
                if (i == 0) return NoWelcome;
                else if (i == 1) return Language.GetTextValue("LegacyDialog.191") + "\n" + Language.GetTextValue("LegacyDialog.192") + "\n" + Language.GetTextValue("LegacyDialog.193") + "\n" + Language.GetTextValue("LegacyDialog.194");
                else return Live;
            }
            else
            {
                int merchant = NPC.FindFirstNPC(NPCID.Merchant);
                if (merchant >= 1 && Main.rand.Next(3) == 0)
                {
                    return Merchant + Main.npc[merchant].GivenName + Merchant2;
                }
                if (Main.dayTime && Main.rand.Next(3) == 0)
                {
                    int i = Main.rand.Next(3);
                    if (i == 0) return CardDay;
                    else if (i == 1) return CardDay2;
                    else return CardDay3;
                }
                if (!Main.dayTime && Main.rand.Next(3) == 0)
                {
                    int i = Main.rand.Next(3);
                    if (i == 0) return CardNight;
                    else if (i == 1) return CardNight2;
                    else return CardNight3;
                }
                if (NPC.downedSlimeKing && Main.rand.NextBool(3))
                    return Slime;
                switch (Main.rand.Next(4))
                {
                    case 0:
                        return Unknown;
                    case 1:
                        return Manual;
                    case 2:
                        return Always;
                    default:
                        return Unknown2;
                }
            }
        }
        public bool S1 = true;
        public override void SetChatButtons(ref string button, ref string button2)
        {
            string shopN = Language.GetTextValue("LegacyInterface.28");
            string shop3 = Language.GetTextValue("Mods.Entrogic.NoShop");
            string misson = Language.GetTextValue("Mods.Entrogic.Mission");
            //string shop1 = Language.GetTextValue("Mods.Entrogic.CardShop");
            //string shop2 = Language.GetTextValue("Mods.Entrogic.SinShop");
            if (npc.homeless)
            {
                button = shopN + shop3;
                button2 = "";
            }
            else
            {
                button = shopN;
                button2 = misson;
            }
        }

        public override void OnChatButtonClicked(bool firstButton, ref bool shop)
        {
            string Careful = Language.GetTextValue("Mods.Entrogic.Card_Careful");
            string NoWelcome = Language.GetTextValue("Mods.Entrogic.Card_NWelcome");
            string Live = Language.GetTextValue("Mods.Entrogic.Card_NShop");
            foreach (Player player in Main.player)
            {
                if (player.active && player.talkNPC == npc.whoAmI && npc.homeless)
                {
                    int i = Main.rand.Next(3);
                    if (i == 0) Main.npcChatText = NoWelcome;
                    else if (i == 1) Main.npcChatText = Language.GetTextValue("LegacyDialog.191") + "\n" + Language.GetTextValue("LegacyDialog.192") + "\n" + Language.GetTextValue("LegacyDialog.193") + "\n" + Language.GetTextValue("LegacyDialog.194");
                    else Main.npcChatText = Live;
                }
            }
            if (!npc.homeless)
            {
                if (firstButton)
                {
                    shop = true;
                }
                else
                {
                    TryQuest();
                }
            }
        }
        public override bool CanGoToStatue(bool toKingStatue)
        {
            return true;
        }
        public override void SetupShop(Chest shop, ref int nextSlot)
        {
            shop.item[nextSlot].SetDefaults(ItemType<卡牌入门手册>());
            nextSlot++;
            shop.item[nextSlot].SetDefaults(ItemType<RandomCard>());
            shop.item[nextSlot].shopCustomPrice = 15 - (int)(CardWillingness * 10.0);
            shop.item[nextSlot].shopSpecialCurrency = MimicryCustomCurrencyId;
            nextSlot++;
            shop.item[nextSlot].SetDefaults(ItemType<NoviceCardPack>());
            nextSlot++;
            shop.item[nextSlot].SetDefaults(ItemType<Glove>());
            nextSlot++;
            if (NPC.downedMechBossAny)
            {
                shop.item[nextSlot].SetDefaults(ItemType<MagicGatling>());
                nextSlot++;
            }
            //shop.item[nextSlot].SetDefaults(ItemType<BoneGlove>());
            //nextSlot++;
        }

        public override void NPCLoot()
        {

        }

        public override void TownNPCAttackStrength(ref int damage, ref float knockback)
        {
            damage = 20;
            knockback = 4f;
        }

        public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
        {
            cooldown = 30;
            randExtraCooldown = 30;
        }

        public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
        {
            projType = ProjectileType<Projectiles.Arcane.ArcaneMissle>();
            attackDelay = 3;
        }

        public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
        {
            multiplier = 12f;
            randomOffset = 2f;
        }

        public override void AI()
        {
            CardWillingness -= 0.001f;
            CardWillingness = Math.Max(-1f, CardWillingness);
            CardWillingness = Math.Min(1f, CardWillingness);
            base.AI();
        }

        public override void ModifyHitByItem(Player player, Item item, ref int damage, ref float knockback, ref bool crit)
        {
            CardWillingness -= 0.07f;
            base.ModifyHitByItem(player, item, ref damage, ref knockback, ref crit);
        }

        public override void ModifyHitByProjectile(Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            CardWillingness -= 0.07f;
            base.ModifyHitByProjectile(projectile, ref damage, ref knockback, ref crit, ref hitDirection);
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit)
        {
            CardWillingness -= 0.07f;
            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit);
        }

        public override void ModifyHitPlayer(Player target, ref int damage, ref bool crit)
        {
            CardWillingness -= 0.23f;
            base.ModifyHitPlayer(target, ref damage, ref crit);
        }

        public void TryQuest()
        {
            foreach (Player player in Main.player)
            {
                if (player.active && player.talkNPC == npc.whoAmI)
                {
                    CardMerchantQuest questSystem = player.GetModPlayer<CardMerchantQuest>();
                    EndMission:
                    if (questSystem.quest >= CardMerchantQuest.Quests.Count) // 如果完成了所有任务
                    {
                        questSystem.quest = Math.Min(questSystem.quest, CardMerchantQuest.Quests.Count);
                        int NewQuest = questSystem.ChooseNewQuest();
                        if (NewQuest < CardMerchantQuest.Quests.Count)
                        {
                            Main.npcChatText = CardMerchantQuest.Quests[NewQuest].ToString();
                            questSystem.quest = NewQuest;
                            return;
                        }
                        switch (Main.rand.Next(3))
                        {
                            case 0:
                                Main.npcChatText = "我真的一滴也不剩了。";
                                return;
                            case 1:
                                Main.npcChatText = "我没啥可以告诉你的。";
                                return;
                            default:
                                Main.npcChatText = "或许你要等待世界再次发生变动再来问我。";
                                return;
                        }
                    }
                    else
                    {
                        if (questSystem.quest == -1) // 如果今日布置任务/已完成任务
                        {
                            int NewQuest = questSystem.ChooseNewQuest();
                            if (NewQuest >= CardMerchantQuest.Quests.Count)
                            {
                                goto EndMission;
                            }
                            Main.npcChatText = CardMerchantQuest.Quests[NewQuest].ToString();
                            Main.npcChatCornerItem = CardMerchantQuest.Quests[NewQuest].CornerItem;
                            questSystem.quest = NewQuest;
                            break;
                        }
                        if (questSystem.CheckQuest())//如果在提交时符合任务条件
                        {
                            Main.npcChatText = questSystem.GetCurrentQuest().SayThanks();
                            // 角落物品先给你整个心心，有需要再改
                            Main.npcChatCornerItem = ItemID.Heart;
                            Main.PlaySound(SoundID.MenuTick, -1, -1, 1, 1f, 0f);
                            questSystem.SpawnReward(npc);
                            questSystem.Complete += "_" + CardMerchantQuest.Quests[questSystem.quest].ID;
                            if (Main.netMode == NetmodeID.Server)
                            {
                                var packet = mod.GetPacket();
                                packet.Write((byte)EntrogicModMessageType.SendCompletedCardMerchantMissionRequest);
                                packet.Write((byte)player.whoAmI);
                                packet.Write(questSystem.Complete);
                                packet.Send();
                            }
                            questSystem.quest = -1;
                            break;
                        }
                        if (questSystem.quest == -1 || questSystem.quest >= CardMerchantQuest.Quests.Count)
                        {
                            goto EndMission;
                        }
                        Main.npcChatText = questSystem.GetCurrentQuest().ToString();
                        Main.npcChatCornerItem = questSystem.GetCurrentQuest().CornerItem;
                    }
                }
            }
        }
    }
    public class CardMerchantQuest : ModPlayer
    {
        public string Complete = "";
        public static List<Quest> Quests = new List<Quest>();
        public int quest = -1;
        private EntrogicPlayer entrogicPlayer => player.GetModPlayer<EntrogicPlayer>();
        public Quest GetCurrentQuest() // 获取任务
        {
            return Quests[quest];
        }
        public bool CheckQuest() // 任务未完成即为true，已完成或未布置为false
        {
            if (quest == -1 || quest >= Quests.Count)
            {
                return false;
            }
            Quest q = Quests[quest];
            return q.CheckCompletion(Main.player[Main.myPlayer]);
        }
        public int ChooseNewQuest()//选择任务
        {
            string[] s = Complete.Split('_');
            Dictionary<string, bool> comp = new Dictionary<string, bool>();
            foreach (Quest q in Quests) // 先遍历一遍任务列表，添加字典中的所有键值，bool初始值为false（未完成）
            {
                comp.Add(q.ID, false);
            }
            for (int i = 0; i < s.Length; i++) // 愉快地将指定值设为true
            {
                comp[s[i]] = true;
            }
            foreach (Quest q in Quests)
            {
                if (!q.CheckCompletion(player) || !comp[q.ID]) // 这里就不会找不到键值了
                {
                    quest = Quests.IndexOf(q);
                    return quest;
                }
            }
            quest = Quests.Count;
            return quest;
        }
        public override void PostUpdate()
        {
            SendQuest(-1);
            //Complete = "";
        }
        public override void Initialize()
        {
            Quests.Clear();
        }
        public override void OnEnterWorld(Player player)
        {
            // 下面这行参数从左到右分别是：任务文本, 相关Boolean, 感谢词
            Quest quest = new Quest(
                "0",
                "（再次点击“任务”按钮领取第一个任务！）",
                true,
                "（再次点击“任务”按钮领取第一个任务！）");
            quest.RewardMoney.Gold = 30;
            Quests.Add(quest);

            quest = new Quest(
                "NoviceCard",
                "...你看上去十分迷惑，当然，你是第一次见到这些“新事物”，疑惑是难免的，现在，我将一步一步引导你深入这个世界。“任务”是" +
                "方式，完成“任务”我会给你一些奖赏，帮助你更好地完成理想中的大业。现在，请你购买一份新手卡包，阅读卡牌手册，尝试尝试“新事物”吧" +
                "\n\n（装备卡牌，并试着使用一次！）",
                entrogicPlayer.CardUseCount >= 1,
                $"你出色地完成了任务！现在让我送你一张卡牌[i:{ItemType<EnergyRecovery>()}]，这是一张出色的卡牌，相信他会发挥伟大的作用的。");
            quest.CornerItem = ItemType<ArcaneMissle>();
            quest.Reward.Add(ItemType<EnergyRecovery>());
            quest.RewardStack.Add(1);
            Quests.Add(quest);
            base.OnEnterWorld(player);
        }
        public void SendQuest(int remoteClient)
        {
            if (Main.netMode != NetmodeID.Server)
            {
                return;
            }
            if (remoteClient == -1)
            {
                for (int remoteClient2 = 0; remoteClient2 < 255; remoteClient2++)
                {
                    if (Netplay.Clients[remoteClient2].State == 10)
                    {
                        NetMessage.SendData(MessageID.AnglerQuest, remoteClient2, -1, NetworkText.FromLiteral(Main.player[remoteClient2].name), Main.player[Main.myPlayer].GetModPlayer<CardMerchantQuest>().quest, 0f, 0f, 0f, 0, 0, 0);
                    }
                }
                return;
            }
            if (Netplay.Clients[remoteClient].State != 10)
            {
                return;
            }
            NetMessage.SendData(MessageID.AnglerQuest, remoteClient, -1, NetworkText.FromLiteral(Main.player[remoteClient].name), Main.player[Main.myPlayer].GetModPlayer<CardMerchantQuest>().quest, 0f, 0f, 0f, 0, 0, 0);
        }
        public void SpawnReward(NPC npc)
        {
            Main.PlaySound(SoundID.Chat, -1, -1, 1, 1f, 0f);
            for (int i = 0; i < GetCurrentQuest().Reward.Count; i++)
            {
                int number = Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, GetCurrentQuest().Reward[i], GetCurrentQuest().RewardStack[i], false, 0, false, false);
                if (Main.netMode == NetmodeID.MultiplayerClient && number >= 0)
                {
                    NetMessage.SendData(MessageID.SyncItem, -1, -1, null, number, 1f, 0f, 0f, 0, 0, 0);
                }
            }
            if (GetCurrentQuest().RewardMoney.Platinum > 0)
            {
                int number = Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.PlatinumCoin, GetCurrentQuest().RewardMoney.Platinum);
                if (Main.netMode == NetmodeID.MultiplayerClient && number >= 0)
                {
                    NetMessage.SendData(MessageID.SyncItem, -1, -1, null, number, 1f, 0f, 0f, 0, 0, 0);
                }
            }
            if (GetCurrentQuest().RewardMoney.Gold > 0)
            {
                int number = Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.GoldCoin, GetCurrentQuest().RewardMoney.Gold);
                if (Main.netMode == NetmodeID.MultiplayerClient && number >= 0)
                {
                    NetMessage.SendData(MessageID.SyncItem, -1, -1, null, number, 1f, 0f, 0f, 0, 0, 0);
                }
            }
            if (GetCurrentQuest().RewardMoney.Silver > 0)
            {
                int number = Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.SilverCoin, GetCurrentQuest().RewardMoney.Silver);
                if (Main.netMode == NetmodeID.MultiplayerClient && number >= 0)
                {
                    NetMessage.SendData(MessageID.SyncItem, -1, -1, null, number, 1f, 0f, 0f, 0, 0, 0);
                }
            }
            if (GetCurrentQuest().RewardMoney.Copper > 0)
            {
                int number = Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.CopperCoin, GetCurrentQuest().RewardMoney.Copper);
                if (Main.netMode == NetmodeID.MultiplayerClient && number >= 0)
                {
                    NetMessage.SendData(MessageID.SyncItem, -1, -1, null, number, 1f, 0f, 0f, 0, 0, 0);
                }
            }
        }
        public override void clientClone(ModPlayer clientClone)
        {
            CardMerchantQuest clone = clientClone as CardMerchantQuest;
            // Here we would make a backup clone of values that are only correct on the local players Player instance.
            // Some examples would be RPG stats from a GUI, Hotkey states, and Extra Item Slots
            clone.Complete = Complete;
        }

        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            ModPacket packet = mod.GetPacket();
            packet.Write((byte)EntrogicModMessageType.MerchantPlayerSyncPlayer);
            packet.Write((byte)player.whoAmI);
            packet.Write(Complete); // While we sync nonStopParty in SendClientChanges, we still need to send it here as well so newly joining players will receive the correct value.
            packet.Send(toWho, fromWho);
        }

        public override void SendClientChanges(ModPlayer clientPlayer)
        {
            // Here we would sync something like an RPG stat whenever the player changes it.
            CardMerchantQuest clone = clientPlayer as CardMerchantQuest;
            if (clone.Complete != Complete)
            {
                // Send a Mod Packet with the changes.
                var packet = mod.GetPacket();
                packet.Write((byte)EntrogicModMessageType.SendCompletedCardMerchantMissionRequest);
                packet.Write((byte)player.whoAmI);
                packet.Write(Complete);
                packet.Send();
            }
        }
        public override TagCompound Save()
        {
            TagCompound tagCompound = new TagCompound();
            //tagCompound.Add("quest", quest);
            tagCompound.Add("Complete", Complete);
            return tagCompound;
        }
        public override void Load(TagCompound tag)
        {
            //quest = tag.GetInt("quest");
            Complete = tag.GetString("Complete");
        }
        public override void LoadLegacy(BinaryReader reader)
        {
            quest = reader.ReadInt32();
        }
    }
    public class Quest
    {
        internal int CornerItem;
        public Func<bool> IsAvailable;
        public string ID;
        public string Name;
        public List<int> Reward = new List<int>();
        public List<int> RewardStack = new List<int>();
        public Money RewardMoney = new Money(0, 0, 0, 0);
        public Action<NPC> SpawnReward;
        public string ThanksMessage;
        public bool CheckBoolean;
        public Quest(string id, string name, bool checkBoolean, string specialThanks = "真谢谢你！")
        {
            ID = id;
            Name = name;
            CheckBoolean = checkBoolean;
            ThanksMessage = specialThanks;
            SpawnReward = delegate (NPC npc)
            {
            };
            IsAvailable = () => true;
        }
        public bool CheckCompletion(Player player)
        {
            return CheckBoolean;
        }
        public override string ToString()
        {
            return Language.GetTextValue(Name, Main.LocalPlayer.name);
        }
        public string SayThanks()
        {
            return Language.GetTextValue(ThanksMessage, Main.LocalPlayer.name);
        }
    }
}