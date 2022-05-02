﻿
using Entrogic;
using Entrogic.NPCs;
using Entrogic.NPCs.CardMerchantSystem;

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.Weapons.Card
{
    public abstract class ModCard : ModItem
    {
        public int rare = CardRareID.Gravitation;
        public string said = "NULL";
        public string tooltip = "NULL";
        public int series = 0;
        public bool activeTooltip = true;

        public bool glove = false;
        public bool minion = false;
        public bool special = false;
        public bool NoUseNormalDelete = false;
        public bool GloveAttackWhileNoCard = false;

        public int healMana = 0;
        public int costMana = 0;
        public int attackCardRemainingTimes = 0;
        public float cardProb = 0;
        protected Vector2 PlaygroundPos => new Vector2(114f, 86f);
        protected Vector2 PlaygroundSize => new Vector2(358f, 220f);
        public sealed override void SetStaticDefaults()
        {
            PreCreated();
            if (!glove)
            {
                CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 2;
                if (rare >= 3)
                    CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
                List<string> rareColorText = new List<string>
                {
                    "[c/555555:",
                    "[c/959595:",
                    "[c/8f7a0d:",
                    "[c/eba329:",
                    "[c/420399:"
                };
                List<string> rareText = new List<string>
                {
                    "品质：" + "{$Mods.Entrogic.Common.CardGrav}",
                    "品质：" + "{$Mods.Entrogic.Common.CardElec}",
                    "品质：" + "{$Mods.Entrogic.Common.CardWeak}",
                    "品质：" + "{$Mods.Entrogic.Common.CardStro}",
                    "品质：" + "{$Mods.Entrogic.Common.CardGran}"
                };
                List<string> seriesText = new List<string>
                {
                    "{$Mods.Entrogic.Common.SeriesNone}",
                    "{$Mods.Entrogic.Common.SeriesElement}",
                    "{$Mods.Entrogic.Common.SeriesOrganism}",
                    "{$Mods.Entrogic.Common.SeriesUndead}"
                };
                string styleText = "{$Mods.Entrogic.Common.CardMinion}";
                if (!minion && !special)
                    styleText = "{$Mods.Entrogic.Common.CardAttack}";
                string toolTip = "[c/EE4000:" + tooltip + "]\r\n";
                if (!activeTooltip)
                    toolTip = "";
                Tooltip.SetDefault("[c/00F5FF:" + said + "]\r\n" +
                  "—————————————————\r\n" +
                  rareColorText[rare] + rareText[rare] + "]\r\n" +
                  rareColorText[rare] + "费用：" + costMana +"]\r\n" +
                  rareColorText[rare] + seriesText[series] + "系列]\r\n" +
                  rareColorText[rare] + styleText + "类牌]\r\n" +
                  toolTip + AnotherMessages());
            }
            else
            {
                CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
                Tooltip.SetDefault("[c/00F5FF:持有时可使用卡牌] \n" + AnotherMessages());
            }
        }
        public virtual void PreCreated() { }
        public virtual string AnotherMessages() {
            return "";
        }
        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            if (Entrogic.ModTimer % 3 == 0)
            {
                for (double i = 0.0; i <= MathHelper.TwoPi; i += MathHelper.TwoPi / 50f)
                {
                    Vector2 vec = ((float)i).ToRotationVector2() * 48f;
                    vec += item.Center;
                    vec -= new Vector2(0f, 24f);
                    Dust d = Dust.NewDustPerfect(vec, 6, Vector2.Zero, 0, Color.MediumPurple, 3f);
                    d.noGravity = true;
                    d.noLight = false;
                }
            }
            maxFallSpeed = 0f;
            base.Update(ref gravity, ref maxFallSpeed);
        }
        public sealed override void SetDefaults()
        {
            PreCreated();
            item.consumable = false;
            List<int> stackMax = new List<int> { 6, 3, 2, 1, 1 };
            item.maxStack = stackMax[rare];
            item.GetGlobalItem<EntrogicItem>().card = true;
            List<float> cProb = new List<float> { 0.45f, 0.25f, 0.15f, 0.10f, 0.05f };
            if (cardProb == 0)
                cardProb = cProb[rare];
            item.GetGlobalItem<EntrogicItem>().cardProb = cardProb;
            if (glove)
            {
                item.GetGlobalItem<EntrogicItem>().card = false;
                item.GetGlobalItem<EntrogicItem>().glove = true;
                item.GetGlobalItem<EntrogicItem>().GloveAttackWhileNoCard = GloveAttackWhileNoCard;
                item.maxStack = 1;
                item.noUseGraphic = true;
                item.noMelee = true;
<<<<<<< HEAD
                item.useStyle = ItemUseStyleID.Swing;
=======
                item.useStyle = ItemUseStyleID.SwingThrow;
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96
                item.shoot = ProjectileID.PurificationPowder;
                item.shootSpeed = 3f;
                item.useTime = item.useAnimation = 20;
            }
            CardDefaults();
            item.DamageType = GetInstance<ArcaneDamageClass>(); // Makes our item use our custom damage type.
            if (minion || special)
                item.value = Item.sellPrice(0, 1, 80, 0);
            else
                item.value = (int)(Item.sellPrice(0, 2, 80, 0) * ((float)item.damage / 30f));
            List<int> price = new List<int> { -Item.sellPrice(0, 1, 0, 0), -Item.sellPrice(0, 0, 60, 0), -Item.sellPrice(0, 0, 20, 0), Item.sellPrice(0, 1, 0, 0), Item.sellPrice(0, 0, 60, 0) };
            item.value += price[rare];
            item.value = (int)MathHelper.Max(Item.sellPrice(silver: 80), item.value);
            item.value = (int)MathHelper.Min(Item.sellPrice(gold: 6), item.value);
        }
        // As a modder, you could also opt to make these overrides also sealed. Up to the modder
        public override void ModifyWeaponDamage(Player player, ref Modifier damage, ref float flat)
        {
            base.ModifyWeaponDamage(player, ref damage, ref flat);
            // 收取30%的魔法伤害加成(也有可能是减少)
            damage.additive += player.GetDamage(DamageClass.Magic) * 0.3f;
        }

        public override void GetWeaponCrit(Player player, ref int crit)
        {
            // Adds crit bonuses
            crit = 0;
        }
        public virtual void CardDefaults() { }

        public virtual void AttackEffects(Player player, int type, Vector2 position, Vector2 shootTo, float speedX, float speedY, int damage, float knockBack, float speed)
        {
        }
        public virtual void MinionEffects(Player player, Vector2 position, int damage, float knockBack, int number)
        {
        }
        public virtual void SpecialEffects(Player player, Vector2 position, int damage, float knockBack, int number, int packType, bool special, bool drawCard)
        {

        }
        /// <summary>
        /// 在玩家点击对局卡牌时执行
        /// </summary>
        /// <param name="attackPlayer">对局中的玩家</param>
        /// <param name="attackNPC">对局中的NPC</param>
        /// <param name="playerDrawPosition">玩家绘制位置（场地位置）</param>
        public virtual void CardGameAttack(Player attackPlayer, NPC attackNPC, Vector2 playerDrawPosition, Vector2 NPCDrawPosition, Vector2 panelPos)
        {

        }
        public virtual void GameAI(float Duration, Vector2 PanelPosition) { }
        /// <summary>
        /// 可被继承，NPC局开始前调用
        /// </summary>
        public virtual void PreStartGaming(ref float RoundDuration) { }
        public virtual void CardGraved(Player player, int number, int myNum, bool special, int packType, bool drawCard) { }
        public virtual void CardGravedWhileMeReady(Player player, int number, int myNum, bool special, int packType, bool drawCard) { }
        public virtual int DrawCardAmount(Player player, int number, ref bool[] series, ref bool[] style)
        {
            return 0;
        }
        public virtual bool HaveDrawCard(Player player, int number, int a, ref int type, ref int cost)
        {
            return true;
        }
        public virtual bool HaveDrawCardMessage(Player player, ref string text, int cardGot, string eventMessageSeries, string eventMessageStyle, int number)
        {
            return true;
        }
        public virtual bool AbleToGetFromRandom(Player player)
        {
            return true;
        }
        public virtual bool CanUseCard()
        {
            return true;
        }
        public virtual void OnGloveUseCard(Player player) { }
        public virtual bool ModifyGloveShootCard(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack) { return true; }
        public virtual int ShootCardTimes(Player player) { return 1; }
        public virtual float AttackCardRemainingTimesReduce(Player player) { return 1f; }

        public virtual void ModifySafeTooltips(List<TooltipLine> tooltips) { }
        // Because we want the damage tooltip to show our custom damage, we need to modify it
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            bool hasGlove = false;
            foreach (Item it in Main.LocalPlayer.inventory)
            {
                if (it.type != ItemID.None && Entrogic.ItemSafe(it) && it.GetGlobalItem<EntrogicItem>().glove)
                {
                    hasGlove = true;
                    break;
                }
            }
            if (!hasGlove && !glove)
            {
                TooltipLine line = new TooltipLine(Mod, Mod.Name, $"你似乎还无法使用这张卡牌，请" +
                    $"{(NPC.AnyNPCs(NPCType<CardMerchant>()) ? "" : "等待卡牌商的到来并")}从卡牌商处购买一个手套以使用")
                {
                    overrideColor = Color.Red
                };
                tooltips.Add(line);
            }
            if (AEntrogicConfigClient.Instance.ShowUsefulInformations)
            {
                TooltipLine line = new TooltipLine(Mod, Mod.Name, $"抽取概率：{item.GetGlobalItem<EntrogicItem>().cardProb * 100f}%") {
                    overrideColor = Color.Gray
                };
                tooltips.Add(line);
            }
            ModifySafeTooltips(tooltips);
        }
        public override ModItem Clone(Item item)
        {
            return base.Clone(item);
        }
    }
}