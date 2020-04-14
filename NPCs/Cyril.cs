/*using System;
using Terraria;
using Terraria.Localization;
using Terraria.ID;
using System.Text;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.IO;
using Terraria.ModLoader.IO;
using System.Collections.Generic;
using Terraria.Utilities;
using Terraria.GameContent.Events;

namespace Entrogic.NPCs
{
    [AutoloadHead]
    public class Cyril : ModNPC
    {
        public bool monstersAround = false;
        public bool hitbyprojectile = false;
        public int leftTime = 0;
        public override string Texture => "Entrogic/NPCs/Cyril";
        public override string[] AltTextures => new string[] { "Entrogic/NPCs/Cyril" };
        public override string TownNPCName() => "";
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(leftTime);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            leftTime = reader.ReadInt32();
        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cyril");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.Guide];
            NPCID.Sets.ExtraFramesCount[npc.type] = NPCID.Sets.ExtraFramesCount[NPCID.Guide];
            NPCID.Sets.AttackFrameCount[npc.type] = NPCID.Sets.AttackFrameCount[NPCID.Guide];
            NPCID.Sets.DangerDetectRange[npc.type] = NPCID.Sets.DangerDetectRange[NPCID.Guide];
            NPCID.Sets.AttackType[npc.type] = NPCID.Sets.AttackType[NPCID.Guide];
            NPCID.Sets.AttackTime[npc.type] = NPCID.Sets.AttackTime[NPCID.Guide];
            NPCID.Sets.AttackAverageChance[npc.type] = NPCID.Sets.AttackAverageChance[NPCID.Guide];
            NPCID.Sets.HatOffsetY[npc.type] = NPCID.Sets.HatOffsetY[NPCID.Guide];
        }
        public int lMax = 100;
        public override void SetDefaults()
        {
            npc.townNPC = true;
            npc.friendly = true;
            npc.width = 18;
            npc.height = 40;
            npc.aiStyle = 7;
            npc.damage = 10;
            npc.defense = 15;
            npc.lifeMax = lMax;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.knockBackResist = 0.5f;
            animationType = NPCID.Guide;
        }
        public override void AI()
        {
            bool flag = Main.raining;
            if (!Main.dayTime)
            {
                flag = true;
            }
            if (Main.eclipse)
            {
                flag = true;
            }
            if (Main.slimeRain)
            {
                flag = true;
            }//以上为黑夜不离开房子的判断
            int num5 = (int)(npc.position.X + (float)(npc.width / 2)) / 16;
            int num6 = (int)(npc.position.Y + (float)npc.height + 1f) / 16;
            bool flag11 = Collision.DrownCollision(npc.position, npc.width, npc.height, 1f);
            float num15 = 3f;//控制NPC走路速度的参数
            float num16 = 1.2f;//同上（必须少于上一个）
            if (npc.ai[0] == 1f)//对应原版NPC走路时的ai[0]
            {
                if (!(Main.netMode != 1 && flag && num5 == npc.homeTileX && num6 == npc.homeTileY && !NPCID.Sets.TownCritter[npc.type]))//对应原版代码
                {
                    //以下为更改NPC走路速度
                    if (npc.velocity.X < -num15 || npc.velocity.X > num15)
                    {
                        if (npc.velocity.Y == 0f)
                        {
                            npc.velocity *= 0.8f;
                        }
                    }
                    else if (npc.velocity.X < num15 && npc.direction == 1)
                    {
                        npc.velocity.X = npc.velocity.X + num16;
                        if (npc.velocity.X > num15)
                        {
                            npc.velocity.X = num15;
                        }
                    }
                    else if (npc.velocity.X > -num15 && npc.direction == -1)
                    {
                        npc.velocity.X = npc.velocity.X - num16;
                        if (npc.velocity.X > num15)
                        {
                            npc.velocity.X = num15;
                        }
                    }
                }
            }

            if (npc.lifeMax < lMax)
            {
                npc.StrikeNPCNoInteraction(9999, 0f, npc.direction, false, false, false);
                if (Main.netMode != NetmodeID.SinglePlayer)
                {
                    NetMessage.SendData(MessageID.StrikeNPC, -1, -1, null, npc.whoAmI, 2f, 0f, 0f, 0, 0, 0);
                }
            }
            monstersAround = false;
            // 最大寻敌距离为8格
            float distanceMax = 128f;
            foreach (NPC n in Main.npc)
            {
                // 如果npc活着且敌对
                if (n.active && !n.friendly)
                {
                    if (Vector2.Distance(n.Center, npc.Center) < distanceMax)
                    {
                        // 设置为附近有敌对npc
                        monstersAround = true;
                    }
                }
            }
            foreach (Projectile proj in Main.projectile)//以下为弹幕碰到NPC就弹走
            {
                Rectangle hitbox = npc.Hitbox;
                Rectangle value = proj.Hitbox;
                if (hitbox.Intersects(value) && proj.active && proj.hostile)
                {
                    Vector2 vector = npc.Center + new Vector2(proj.Center.X, proj.Center.Y);
                    Vector2 vector2 = proj.Center + new Vector2(proj.Center.X, proj.Center.Y);
                    float num = (float)Math.Atan2((double)(vector2.Y - vector.Y), (double)(vector2.X - vector.X));
                    proj.velocity.X = (float)(Math.Cos((double)num) * 20.0);
                    proj.velocity.Y = (float)(Math.Sin((double)num) * 20.0);
                    if (proj.penetrate != 1)
                    {
                        proj.usesLocalNPCImmunity = true;
                        proj.localNPCHitCooldown = 1;
                    }
                }
            }
            if (monstersAround)
                npc.alpha += 7;
            else
                npc.alpha -= 7;
            if (npc.alpha < 0)
                npc.alpha = 0;
            if (npc.alpha > 255)
                npc.alpha = 255;
            npc.dontTakeDamageFromHostiles = monstersAround;
        }
        public override string GetChat()
        {
            var now = DateTime.Now;
            WeightedRandom<string> chat = new WeightedRandom<string>();
            Player player = Main.player[Main.myPlayer];
            int Guide = NPC.FindFirstNPC(NPCID.Guide);
            if (Guide >= 0)
            {
                chat.Add(Main.npc[Guide].GivenName + "曾经给了我莫大的帮助，他或许能帮助你");
            }
            int Cyborg = NPC.FindFirstNPC(NPCID.Cyborg);
            if (Cyborg >= 0)
            {
                chat.Add("有一次我尝试给" + Main.npc[Cyborg].GivenName + "加入一些功能，然而它自我保护能力很强");
            }
            int Santa = NPC.FindFirstNPC(NPCID.SantaClaus);
            if (Santa >= 0)
            {
                chat.Add(Main.npc[Santa].GivenName + "绝对是冒充的，他甚至不会给我礼物！");
            }
            bool flag = now.Day == 20 && now.Month == 3;
            if (flag)
            {
                chat.Add(BirthdayParty.PartyIsUp ? "在今天开派对真是太合适了，我的蛋糕是五彩缤纷的！" : "能给我一块蛋糕吗？如果有蜡烛就更好了");
            }
            if (BirthdayParty.PartyIsUp)
            {
                chat.Add("派对真是令人兴奋！至少现在没有怪物！");
            }
            if (ModLoader.GetMod("FKBossHealthBar") != null)
            {
                chat.Add("谁能告诉我为什么下面有时会有一个“能量槽”？这总是给我不详的预感");
            }
            if (Main.dayTime)
            {
                chat.Add("这里真惬意...兔子、鸭子..." + (Main.hardMode ? "还有一具骷髅在我面前" : "庆幸的是这里没有独角兽"));
                chat.Add("谁能告诉我为什么这里的太阳落得这么快？？还有为什么过了一秒就跟一分钟那么久？？");
                chat.Add("酒店不一定有酒，茶馆也不一定有茶");
            }
            else
            {
                chat.Add("每个人都开门欢迎怪物，他们到底在想什么？");
                chat.Add("看上去怪物的尸体会堆成山了...似乎不会");
                chat.Add("不必畏惧怪物，反正我们倒下了会再站起来的" + (player.difficulty == 2 ? "...好吧看上去你无法这么做了" : ""));
            }
            if (NPC.downedFishron)
            {
                chat.Add("如果运气不允许你拥有一些武器，我也许可以帮助你");
            }
            chat.Add("你有试过建造一个茶馆吗？有茶的那种");
            chat.Add("瞧瞧我的高科技墨镜！我给它改造了一下，它现在能显示比手机更多的消息");

            return chat; 
        }
        public override bool CanTownNPCSpawn(int numTownNPCs, int money)
        {
            for (int k = 0; k < 255; k++)
            {
                Player player = Main.player[k];
                if (player.active)
                {
                    if (player.statLifeMax >= 160 || ModLoader.GetMod("Entrogic") != null)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = Lang.inter[28].Value;//“商店”翻译文本
            button2 = Lang.inter[64].Value;//“任务”翻译文本
        }
        public override void OnChatButtonClicked(bool firstButton, ref bool shop)
        {
            if (firstButton)
            {
                shop = true;
                return;
            }
            TryQuest();
        }

        public void TryQuest()
        {
            foreach (Player player in Main.player)
            {
                if (player.active && player.talkNPC == npc.whoAmI)
                {
                    CyrilQuest questSystem = player.GetModPlayer<CyrilQuest>(mod);
                    if (questSystem.completedToday)//如果今日任务已完成
                    {
                        switch (Main.rand.Next(3))
                        {
                            case 0:
                                Main.npcChatText = "今天已经没有任务了，明天再来吧！";
                                return;
                            case 1:
                                Main.npcChatText = "我已经不需要了。";
                                return;
                            default:
                                Main.npcChatText = "今日任务待解决数: 0";
                                return;
                        }
                    }
                    else
                    {
                        if (questSystem.questToday == -1)//如果今日没有布置任务/已完成任务
                        {
                            int NewQuest = CyrilQuest.ChooseNewQuest();
                            Main.npcChatText = CyrilQuest.Quests[NewQuest].ToString();
                            if (CyrilQuest.Quests[NewQuest] is ItemQuest)
                            {
                                Main.npcChatCornerItem = (CyrilQuest.Quests[NewQuest] as ItemQuest).ItemType;
                                questSystem.questToday = NewQuest;
                            }
                            break;
                        }
                        if (questSystem.CheckQuest())//如果在提交时符合任务条件
                        {
                            Main.npcChatText = questSystem.GetCurrentQuest().SayThanks();
                            Main.PlaySound(12, -1, -1, 1, 1f, 0f);
                            questSystem.SpawnReward(base.npc);
                            questSystem.CompleteQuest();
                            break;
                        }
                        Main.npcChatText = questSystem.GetCurrentQuest().ToString();
                        if (questSystem.GetCurrentQuest() is ItemQuest)//如果任务类型是提交物品
                        {
                            Main.npcChatCornerItem = (questSystem.GetCurrentQuest() as ItemQuest).ItemType;
                        }
                    }
                }
            }
        }

        public override void TownNPCAttackStrength(ref int damage, ref float knockback)
        {
            damage = 100;
            knockback = 4f;
        }

        public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
        {
            cooldown = 5;
            randExtraCooldown = 5;
        }

        public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
        {
            projType = ProjectileID.FallingStar;//攻击的Proj
            attackDelay = 1;
        }

        public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
        {
            multiplier = 12f;
            randomOffset = 2f;
        }

        public override void DrawTownAttackGun(ref float scale, ref int item, ref int closeness)
        {
            item = ItemID.StarCannon;//手持武器类型(type)
            closeness = 18;//武器更接近NPC(以像素为单位, 数字越大离NPC越近, 当武器位置不对时调整)
        }

        public override bool? CanBeHitByProjectile(Projectile projectile)
        {
            if (projectile.hostile && !projectile.friendly)//如果是敌方弹幕
            {
                hitbyprojectile = true;//设置护盾
                leftTime = 10;//设置护盾持续时间为1/6秒
            }
            return false;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            SpriteEffects effects = (npc.direction == -1) ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            Vector2 halfSize = new Vector2((float)(Main.npcTexture[npc.type].Width / 2), (float)(Main.npcTexture[npc.type].Height / Main.npcFrameCount[npc.type] / 2));
            float num70 = Main.NPCAddHeight(NPCID.Guide);
            if (npc.ai[0] == 12f)//当NPC正在攻击(另外, 这个if里包含的内容只适用于向导这种AI的NPC)
            {
                if (npc.frame.Y / (Main.npcTexture[npc.type].Height / Main.npcFrameCount[npc.type]) >= 21)
                {
                    //为了做到手挡住武器而不是武器挡住手的效果, 需要再draw一只手来覆盖武器
                    Texture2D texture2D3 = mod.GetTexture("NPCs/Cyril_Hand");
                    Rectangle value2 = texture2D3.Frame(1, 5, 0, npc.frame.Y / (Main.npcTexture[npc.type].Height / Main.npcFrameCount[npc.type]) - 21);
                    Main.spriteBatch.Draw(texture2D3, new Vector2(npc.position.X - Main.screenPosition.X + (float)(npc.width / 2) - (float)Main.npcTexture[npc.type].Width * npc.scale / 2f + halfSize.X * npc.scale, npc.position.Y - Main.screenPosition.Y + (float)npc.height - (float)Main.npcTexture[npc.type].Height * npc.scale / (float)Main.npcFrameCount[npc.type] + 4f + halfSize.Y * npc.scale + num70 + npc.gfxOffY), new Rectangle?(value2), npc.GetAlpha(drawColor), npc.rotation, halfSize, npc.scale, effects, 0f);
                }
            }
            if (hitbyprojectile||leftTime > 0)//当开启护盾
            {
                Texture2D tex = mod.GetTexture("NPCs/Cyril_Armor");
                Vector2 worldPos = new Vector2(npc.Center.X, npc.Center.Y);
                spriteBatch.Draw(tex, worldPos - Main.screenPosition, null, Color.White, 0f, tex.Size() * 0.5f, 1f, SpriteEffects.None, 0f);
                leftTime--;
                hitbyprojectile = false;
            }
        }
    }
    public class CyrilQuest : ModPlayer
    {
        public static List<Quest> Quests = new List<Quest>();
        public bool updateDay = false;
        public bool completedToday = false;
        public int questToday = -1;
        public Quest GetCurrentQuest()//今日任务
        {
            return Quests[questToday];
        }
        public void CompleteQuest()//完成任务时运行
        {
            completedToday = true;
        }
        public bool CheckQuest()//任务未完成即为true，已完成或未布置为false
        {
            if (questToday == -1)
            {
                return false;
            }
            Quest quest = Quests[questToday];
            return quest.CheckCompletion(Main.player[Main.myPlayer]);
        }
        public static int ChooseNewQuest()//选择任务
        {
            //随机从所有任务中选一个
            WeightedRandom<int> questChoice = new WeightedRandom<int>();
            for (int i = 0; i < Quests.Count; i++)
            {
                if (Quests[i].IsAvailable())
                {
                    questChoice.Add(i, Quests[i].Weight);
                }
            }
            return questChoice;
        }
        public override void PostUpdate()
        {
            if (completedToday)//如果已完成今日任务
                questToday = -1;//任务设为无(-1即为无)
            SendQuest(-1);//服务器通信
            if (Main.dayTime && !updateDay)//如果任务未重置且经过了一个黑夜
            {
                questToday = -1;//重置任务
                updateDay = true;//任务更新设为是
                completedToday = false;//今日任务完成设为否
            }
            if (!Main.dayTime)//如果为黑夜
            {
                updateDay = false;//任务重置设为否(为下个白天作准备)
            }
        }
        public override void Initialize()
        {
            Quests.Clear();
            //下面这行参数从左到右分别是：任务文本, 所需物品, 所需数量, 凑数的, 提交文本, 提交是否失去物品(true为否, false为是)
            Quest quest = new ItemQuest("给我一副墨镜，我要试试同时戴两副眼镜，请不要问我为什么", ItemID.Sunglasses, 1, 1.0, "同时戴两副眼镜感觉并不好，你留着吧。这是你的报酬", true);
            quest.Reward = ItemID.HiTekSunglasses;
            quest.RewardMoney.Gold = 50; 
            Quests.Add(quest);
            quest = new ItemQuest("酷", ItemID.AngelStatue, 1, 1.0, "酷");
            quest.Reward = ItemID.AngelStatue;
            quest.RewardStack = 999;
            Quests.Add(quest);
        }
        public void SendQuest(int remoteClient)
        {
            if (Main.netMode != 2)
            {
                return;
            }
            if (remoteClient == -1)
            {
                for (int remoteClient2 = 0; remoteClient2 < 255; remoteClient2++)
                {
                    if (Netplay.Clients[remoteClient2].State == 10)
                    {
                        NetMessage.SendData(MessageID.AnglerQuest, remoteClient2, -1, NetworkText.FromLiteral(Main.player[remoteClient2].name), Main.player[Main.myPlayer].GetModPlayer<CyrilQuest>(mod).questToday, 0f, 0f, 0f, 0, 0, 0);
                    }
                }
                return;
            }
            if (Netplay.Clients[remoteClient].State != 10)
            {
                return;
            }
            NetMessage.SendData(MessageID.AnglerQuest, remoteClient, -1, NetworkText.FromLiteral(Main.player[remoteClient].name), Main.player[Main.myPlayer].GetModPlayer<CyrilQuest>(mod).questToday, 0f, 0f, 0f, 0, 0, 0);
        }
        public void SpawnReward(NPC npc)
        {
            Main.PlaySound(SoundID.Chat, -1, -1, 1, 1f, 0f);
            if (GetCurrentQuest().Reward == -1)
            {
                return;
            }
            int number = Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, GetCurrentQuest().Reward, GetCurrentQuest().RewardStack, false, 0, false, false);
            if (GetCurrentQuest().RewardMoney.Platinum > 0)
            {
                number = Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.PlatinumCoin, GetCurrentQuest().RewardMoney.Platinum);
            }
            if (GetCurrentQuest().RewardMoney.Gold > 0)
            {
                number = Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.GoldCoin, GetCurrentQuest().RewardMoney.Gold);
            }
            if (GetCurrentQuest().RewardMoney.Silver > 0)
            {
                number = Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.SilverCoin, GetCurrentQuest().RewardMoney.Silver);
            }
            if (GetCurrentQuest().RewardMoney.Copper > 0)
            {
                number = Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.CopperCoin, GetCurrentQuest().RewardMoney.Copper);
            }
            if (Main.netMode == 1 && number >= 0)
            {
                NetMessage.SendData(MessageID.SyncItem, -1, -1, null, number, 1f, 0f, 0f, 0, 0, 0);
            }
        }
        public override TagCompound Save()
        {
            List<string> list = new List<string>();
            TagCompound tagCompound = new TagCompound();
            tagCompound.Add("list", list);
            tagCompound.Add("questToday", questToday);
            tagCompound.Add("completedToday", completedToday);
            tagCompound.Add("updateDay", updateDay);
            return tagCompound;
        }
        public override void Load(TagCompound tag)
        {
            IList<string> list = tag.GetList<string>("list");
            questToday = tag.GetInt("questToday");
            updateDay = tag.GetBool("updateDay");
            completedToday = tag.GetBool("completedToday");
        }
        public override void LoadLegacy(BinaryReader reader)
        {
            questToday = reader.ReadInt32();
            updateDay = reader.ReadBoolean();
            completedToday = reader.ReadBoolean();
        }
    }
    public abstract class Quest
    {
        public Func<bool> IsAvailable;
        public string Name;
        public int Reward = -1;
        public int RewardStack = 1;
        public Money RewardMoney = new Money(0, 0, 0, 0);
        public Action<NPC> SpawnReward;
        public string ThanksMessage;
        public double Weight;
        protected Quest(string name, double weight = 1.0, string specialThanks = "真谢谢你！")
        {
            Name = name;
            Weight = weight;
            ThanksMessage = specialThanks;
            SpawnReward = delegate (NPC npc)
            {
            };
            IsAvailable = (() => true);
        }
        public abstract bool CheckCompletion(Player player);
        public override string ToString()
        {
            return Language.GetTextValue(Name, Main.LocalPlayer.name);
        }
        public string SayThanks()
        {
            return Language.GetTextValue(ThanksMessage, Main.LocalPlayer.name);
        }
    }
    public class ItemQuest : Quest
    {
        public int ItemAmount;
        public int ItemType;
        public bool KeepItem = false;
        public ItemQuest(string quest, int itemType, int itemAmount = 1, double weight = 1.0, string specialThanks = "真谢谢你！", bool keep = false) : base(quest, weight, specialThanks)
        {
            ItemType = itemType;
            ItemAmount = itemAmount;
            KeepItem = keep;
        }
        public override bool CheckCompletion(Player player)
        {
            if (player.CountItem(ItemType, ItemAmount) >= ItemAmount)//如果提交的物品数量足够
            {
                int leftToRemove = ItemAmount;
                foreach (Item item in player.inventory)//遍历玩家物品栏
                {
                    if (item.type == ItemType)//如果物品栏其中一格有任务物品
                    {
                        int removed = Math.Min(item.stack, leftToRemove);
                        leftToRemove -= removed;
                        if (!KeepItem)//如果提交任务时物品不保留
                        {
                            item.stack -= removed;//物品的堆叠量减去任务所需的物品数量
                            if (item.stack <= 0)//如果物品堆叠数小于或等于0
                            {
                                item.SetDefaults(0, false);//当前物品栏物品消除
                            }
                        }
                        if (leftToRemove <= 0)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}
*/