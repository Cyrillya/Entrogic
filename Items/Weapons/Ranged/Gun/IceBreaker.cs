using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Entrogic.Projectiles.Ranged.Bullets;
<<<<<<< HEAD:Items/Weapons/Ranged/Gun/IceBreaker.cs
using Entrogic.Items.Materials;
=======
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96:Items/Weapons/Ranged/Gun/碎冰机.cs

namespace Entrogic.Items.Weapons.Ranged.Gun
{
    public class IceBreaker : ModItem
    {
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.Flamethrower);
            item.useAmmo = ItemID.IceBlock;
            item.shoot = ModContent.ProjectileType<FrostFire>();
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
            CreateRecipe()
                .AddIngredient(ItemID.SnowballCannon)
                .AddIngredient(ItemID.FrostCore)
                .AddIngredient(ModContent.ItemType<CuteWidget>())
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-6.0f, 0.0f);
        }
    }
}
