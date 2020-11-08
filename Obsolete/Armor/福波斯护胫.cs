using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;

namespace Entrogic.Items.Equipables.Armor
{
    [AutoloadEquip(EquipType.Legs)]
    public class 福波斯护胫 : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("福波斯护胫");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;


        }
        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 20;
            item.value = Item.sellPrice(0, 4, 50);
            item.rare = ItemRarityID.Green;
            item.defense = 45;
        }

        public override void UpdateEquip(Player player)
        {
            player.moveSpeed += 0.08f;
            player.meleeSpeed += 0.06f;
            player.maxMinions += 1;
        }

        /*public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }*/
    }
}
