using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace Entrogic.Items.Equipables.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class 福波斯头盔 : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("福波斯头盔");
            
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 30;
            item.value = Item.sellPrice(0, 7, 50);
            item.rare = ItemRarityID.Green;
            item.defense = 30;
            
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ItemType<福波斯胸甲>() && legs.type == ItemType<福波斯护胫>();
        }
        public override void UpdateEquip(Player player)
        {
            player.endurance += 0.1f;
            player.GetDamage(DamageClass.Melee) += 0.07f;
            player.GetCrit(DamageClass.Melee) += 5;
            player.meleeSpeed += 0.09f;
            player.GetDamage(DamageClass.Summon) += 0.08f;
        }
        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "\n" + Language.GetTextValue("Mods.Entrogic.福波斯奖励") +
                "\n" + Language.GetTextValue("Mods.Entrogic.福波斯奖励2");
            //player.GetModPlayer<EntrogicPlayer>().Explode  = true;
        }
        /*public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }*/
       
    }
}
