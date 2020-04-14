using System;

using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.Weapons.Ranged.Bow
{
    public class ChaosAccelerator : ModItem
    {
        public override string Texture { get { return "Entrogic/Items/Weapons/Ranged/Bow/ChaosAccelerator_1"; } }
        public override void SetDefaults()
        {
            item.damage = 62;
            item.width = 42;
            item.height = 30;
            item.useTime = 12;
            item.useAnimation = 12;
            item.useStyle = 5;
            item.noMelee = true;
            item.value = Item.sellPrice(0, 5);
            item.rare = 5;
            item.UseSound = SoundID.Item5;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("ProGodArrow");
            item.shootSpeed = 12f;
            item.useAmmo = AmmoID.Arrow;
            item.ranged = true;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            type = mod.ProjectileType("ProGodArrow");
            return true;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "CBar", 6);
            recipe.AddIngredient(ItemID.HallowedRepeater, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-6, 0);
        }
        public override bool ConsumeAmmo(Player player)
        {
            return Main.rand.NextFloat() >= .45f;
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override bool CanUseItem(Player player)
        {
            item.useTime = 20;
            item.useAnimation = 20;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            return base.CanUseItem(player);
        }
        public int timeCounter = 0;
        public int timer = 1;

        [Obsolete]
        public override void GetWeaponDamage(Player player, ref int damage)
        {
            timeCounter++;
            Main.itemTexture[mod.ItemType("ChaosAccelerator")] = mod.GetTexture("Items/Weapons/Ranged/Bow/ChaosAccelerator_" + timer);
            if (timeCounter >= 6)
            {
                if (timer >= 4) timer = 0;
                timer++;
                timeCounter = 0;
            }
        }
    }
}