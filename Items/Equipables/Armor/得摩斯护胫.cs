using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;

namespace Entrogic.Items.Equipables.Armor
{
    [AutoloadEquip(EquipType.Legs)]
    public class 得摩斯护胫 : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("得摩斯护胫");
            
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 20;
            item.value = Item.sellPrice(0, 4, 50);
            item.rare = ItemRarityID.Green;
            item.defense = 13;
        }

        public override void UpdateEquip(Player player)
        {
            player.moveSpeed += 0.06f;
            player.magicCrit += 4;
            player.rangedDamage += 0.05f;
        }

        /*public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }*/
    }
}
