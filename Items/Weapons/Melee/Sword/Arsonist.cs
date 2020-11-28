using Entrogic.Projectiles.Melee.Swords;

using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.Weapons.Melee.Sword
{
    public class Arsonist : ModItem
    {
        public override void SetDefaults()
        {
            item.damage = 36;           //The damage of your weapon 武器的伤害
            item.width = 42;            //Weapon's texture's width 武器材质的宽度
            item.height = 54;           //Weapon's texture's height 武器材质的高度
            item.useTime = 10;          //The time span of using the weapon. Remember in terraria, 60 frames is a second. 使用武器的速度的时间跨度(以帧为单位)。在Terraria中，60帧是一秒。
            item.useAnimation = 20;          //The time span of the using animation of the weapon, suggest set it the same as useTime. 使用武器的动画的时间跨度(以帧为单位),通常和useTime设置成一样
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.knockBack = 5;         //The force of knockback of the weapon. Maximum is 20 武器的击退,最高是20
            item.value = Item.buyPrice(gold: 10);
            item.rare = RareID.LV5;              //The rarity of the weapon, from -1 to 13 武器的稀有度,从-1到13
            item.UseSound = SoundID.Item1;      //The sound when the weapon is using 使用武器时的音效
            item.autoReuse = true;
            item.shoot = ProjectileType<Arsonists>();
            item.shootSpeed = 6f;
            item.melee = true;
            item.noUseGraphic = false;
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.Next(35) == 0)
            {
                //Emit dusts when swing the sword
                //使用时出现粒子效果
                Dust dust = Dust.NewDustDirect(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 174);
                dust.noGravity = true;
            }
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.SetResult(this);
            recipe.AddIngredient(ItemID.HellstoneBar, 10);
            recipe.AddIngredient(ItemID.SoulofLight, 5);
            recipe.AddIngredient(ItemID.SoulofNight, 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.AddRecipe();
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(5));
            speedX = perturbedSpeed.X;
            speedY = perturbedSpeed.Y;
            return true;
        }
    }
}