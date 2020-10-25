using System;
using Terraria;
using Terraria.Localization;
using Terraria.ID;
using System.Text;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Entrogic.Items.Weapons.Card;

using Entrogic.Items.Materials;
using Entrogic.Tiles;

namespace Entrogic.Items.VoluGels.Armor
{
    /// <summary>
    /// 凝胶胸甲 的摘要说明
    /// 创建人机器名：DESKTOP-QDVG7GB
    /// 创建时间：2019/8/18 14:37:08
    /// </summary>
    [AutoloadEquip(EquipType.Body)]
    public class 凝胶胸甲 :ModItem
    {
        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 30;
            item.value = Item.sellPrice(0, 0, 80) * 2;
            item.rare = RareID.LV2;
            item.defense = 5;
        }
        public override void DrawHands(ref bool drawHands, ref bool drawArms)
        {
            drawArms = true;
            drawHands = true;
        }
        public override void UpdateEquip(Player player)
        {
            player.maxMinions++;
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return head.type == ItemType<凝胶头饰>() && legs.type == ItemType<凝胶护胫>();
        }
        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "\n原版及本Mod的凝胶类召唤物只占0.75个召唤位\n所有生物系列卡牌费用为1\n无摔落伤害";
            // 无摔落伤害
            player.noFallDmg = true;
            // 所有生物系列卡牌费用为1
            EntrogicPlayer ePlayer = EntrogicPlayer.ModPlayer(player);
            for (int a = 0; a < ePlayer.CardHandCost.Length; a++)
            {
                if (ePlayer.CardHandType[a] != 0)
                {
                    Item item = new Item();
                    item.SetDefaults(ePlayer.CardHandType[a]);
                    ModCard card = (ModCard)item.modItem;
                    if (card.series == CardSeriesID.Organism)
                    {
                        ePlayer.CardHandCost[a] = Math.Min(1, ePlayer.CardHandCost[a]);
                    }
                }
            }
            for (int a = 0; a < ePlayer.CardReadyCost.Length; a++)
            {
                if (ePlayer.CardReadyType[a] != 0)
                {
                    Item item = new Item();
                    item.SetDefaults(ePlayer.CardReadyType[a]);
                    ModCard card = (ModCard)item.modItem;
                    if (card.series == CardSeriesID.Organism)
                    {
                        ePlayer.CardReadyCost[a] = Math.Min(1, ePlayer.CardReadyCost[a]);
                    }
                }
            }
            // 凝胶类召唤物只占0.75个召唤位
            float[] i = new float[1001];
            foreach (Projectile proj in Main.projectile)
            {
                if (proj.DamageType == DamageClass.Summon && proj.active && proj.owner == player.whoAmI && proj.minionSlots > 0.75f && (proj.type == ProjectileID.BabySlime/* || proj.type == ProjectileType<Projectiles.衰落魔像.Stoneslime>()*/))
                {
                    i[proj.whoAmI] = proj.minionSlots;
                }
            }
            float MoreMinionSlots = 0;
            foreach (Projectile proj in Main.projectile)
            {
                if (i[proj.whoAmI] != 0)
                    MoreMinionSlots += i[proj.whoAmI] - 0.75f;
            }
            player.maxMinions += (int)MoreMinionSlots;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemType<CastIronBar>(), 20)
                .AddIngredient(ItemType<GelOfLife>(), 4)
                .AddTile(TileType<MagicDiversionPlatformTile>())
                .Register();
        }
    }
}