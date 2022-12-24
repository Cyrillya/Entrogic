using Entrogic.Core.BaseTypes;
using Terraria.Enums;

namespace Entrogic.Content.RockGolem.Items
{
    public class TitansOrder : ItemBase
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
        }
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 32;
            Item.maxStack = 20;
            Item.rare = (int)ItemRarityColor.LightRed4;
            Item.useAnimation = 45;
            Item.useTime = 45;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = true;
        }

        public override bool CanUseItem(Player player) {
            return !NPC.AnyNPCs(ModContent.NPCType<RockGolem.Enemies.Athanasy>());
        }
    }
}
