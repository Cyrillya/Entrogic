using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.Equipables.Armor
{
    [AutoloadEquip(EquipType.Body)]
    public class 福波斯胸甲 : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("福波斯胸甲");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 24;
            item.value = Item.sellPrice(gold: 6);
            item.rare = ItemRarityID.Green;
            item.defense = 60;
        }
        public override void UpdateEquip(Player player)
        {
            player.minionDamage += 0.1f;
            player.meleeDamage += 0.11f;
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