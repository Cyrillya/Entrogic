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
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

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
            player.GetCrit(DamageClass.Magic) += 8;
            player.GetDamage(DamageClass.Magic) += 0.08f;
            player.GetDamage(DamageClass.Ranged) += 0.11f;
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