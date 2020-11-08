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
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
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
            player.GetDamage(DamageClass.Summon) += 0.1f;
            player.GetDamage(DamageClass.Melee) += 0.11f;
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