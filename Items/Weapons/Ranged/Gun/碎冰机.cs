using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;


namespace Entrogic.Items.Weapons.Ranged.Gun
{
    public class 碎冰机 : ModItem
    {
        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.Flamethrower);
            item.useAmmo = ItemID.IceBlock;
            item.shoot = mod.ProjectileType("FrostFire");
            item.shootSpeed = 8f;
            item.damage = 38;
            item.crit += 24;
            item.rare = RareID.LV6;
            item.value = Item.sellPrice(0, 2, 0, 0);
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            position += new Vector2(speedX, speedY).ToRotation().ToRotationVector2() * 5.2f * 16f;
            return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
        }
        public override bool ConsumeAmmo(Player player)
        {
            return Main.rand.NextFloat() < .10f;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.SnowballCannon);
            recipe.AddIngredient(ItemID.FrostCore);
            recipe.AddIngredient(Entrogic.Instance, "CuteWidget");
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-6.0f, 0.0f);
        }
    }
}
