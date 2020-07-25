using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;


namespace Entrogic.Items.Equipables.Armor
{
    [AutoloadEquip(EquipType.Body)]
    public class 得摩斯胸甲 : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("得摩斯胸甲");
            
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 24;
            item.value = Item.sellPrice(gold:6);
            item.rare = ItemRarityID.Green;
            item.defense = 19;
        }

        public override void UpdateEquip(Player player)
        {
            player.magicCrit += 8;
            player.magicDamage += 0.08f;
            player.rangedDamage += 0.11f;
            //player.GetModPlayer<(mod).ammoCost85 = true;
        }

        /*public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }*/
    }
}