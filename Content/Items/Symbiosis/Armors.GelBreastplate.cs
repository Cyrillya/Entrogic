using Entrogic.Content.Items.BaseTypes;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Content.Items.Symbiosis
{
    [AutoloadEquip(EquipType.Body)]
    public class GelBreastplate : ItemBase
    {
        public override void SetStaticDefaults() {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            DisplayName.SetDefault("Gel Breastplate");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "凝胶胸甲");
            Tooltip.AddArmorTranslation("+1 仆从容量");
            Tooltip.AddIntoDefault("\"You are absolutely stuck\"");
            Tooltip.AddIntoTranslation((int)GameCulture.CultureName.Chinese, "“你完全地被黏住了”");
            ArmorIDs.Body.Sets.HidesArms[Item.bodySlot] = false;
            ArmorIDs.Body.Sets.HidesHands[Item.bodySlot] = false;
        }

        public override void SetDefaults() {
            Item.width = 32;
            Item.height = 30;
            Item.value = Item.sellPrice(0, 0, 80) * 2;
            Item.rare = RarityLevelID.MiddlePHM;
            Item.defense = 5;
        }

        public override void UpdateEquip(Player player) {
            player.maxMinions++;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs) {
            return head.type == ItemType<GelHeadgear>() && legs.type == ItemType<GelLeggings>();
        }

        public override void UpdateArmorSet(Player player) {
            player.setBonus = "\n原版及本Mod的凝胶类召唤物只占0.75个召唤位\n所有生物系列卡牌费用为1\n无摔落伤害";
            // 无摔落伤害
            player.noFallDmg = true;
            // 所有生物系列卡牌费用为1
            //EntrogicPlayer ePlayer = EntrogicPlayer.ModPlayer(player);
            //for (int a = 0; a < ePlayer.CardHandCost.Length; a++) {
            //    if (ePlayer.CardHandType[a] != 0) {
            //        Item Item = new Item();
            //        Item.SetDefaults(ePlayer.CardHandType[a]);
            //        ModCard card = (ModCard)Item.modItem;
            //        if (card.series == CardSeriesID.Organism) {
            //            ePlayer.CardHandCost[a] = Math.Min(1, ePlayer.CardHandCost[a]);
            //        }
            //    }
            //}
            //for (int a = 0; a < ePlayer.CardReadyCost.Length; a++) {
            //    if (ePlayer.CardReadyType[a] != 0) {
            //        Item Item = new Item();
            //        Item.SetDefaults(ePlayer.CardReadyType[a]);
            //        ModCard card = (ModCard)Item.modItem;
            //        if (card.series == CardSeriesID.Organism) {
            //            ePlayer.CardReadyCost[a] = Math.Min(1, ePlayer.CardReadyCost[a]);
            //        }
            //    }
            //}
            // 凝胶类召唤物只占0.75个召唤位
            float[] i = new float[1001];
            foreach (Projectile proj in Main.projectile) {
                if (proj.CountsAsClass(DamageClass.Summon) && proj.active && proj.owner == player.whoAmI && proj.minionSlots > 0.75f && (proj.type == ProjectileID.BabySlime/* || proj.type == ProjectileType<Projectiles.衰落魔像.Stoneslime>()*/)) {
                    i[proj.whoAmI] = proj.minionSlots;
                }
            }
            float MoreMinionSlots = 0;
            foreach (Projectile proj in Main.projectile) {
                if (i[proj.whoAmI] != 0)
                    MoreMinionSlots += i[proj.whoAmI] - 0.75f;
            }
            player.maxMinions += (int)MoreMinionSlots;
        }
        public override void AddRecipes() {
            //CreateRecipe()
            //    .AddIngredient(ItemType<CastIronBar>(), 20)
            //    .AddIngredient(ItemType<GelOfLife>(), 4)
            //    .AddTile(TileType<MagicDiversionPlatformTile>())
            //    .Register();
        }
    }
}
