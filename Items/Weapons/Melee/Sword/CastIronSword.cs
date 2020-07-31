using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.Weapons.Melee.Sword
{
    public class CastIronSword : ModItem
    {
        public override void SetDefaults()
        {
            item.damage = 43;
            item.knockBack = 4f;
            item.crit += 29;
            item.rare = ItemRarityID.Orange;
            item.useTime = 35;
            item.useAnimation = 35;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.autoReuse = false;
            item.melee = true;
            item.value = Item.sellPrice(0, 1, 50, 0);
            item.UseSound = SoundID.Item1;
            item.scale = 1.2f;
            item.width = 48;
            item.height = 48;
            item.maxStack = 1;
            item.useTurn = true;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null,"GelOfLife", 7);
            recipe.AddIngredient(null,"CastIronBar", 10);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            if (Main.rand.Next(4) == 0)
            {
                target.AddBuff(31, 180);
            }
        }
    }
}