using Entrogic.Projectiles.Melee.Swords;

using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.Weapons.Melee.Sword
{
    public class SkylineItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            Tooltip.SetDefault("黎明的利刃将永夜撕裂");
        }

        public override void SetDefaults()
        {
            item.damage = 79;           //The damage of your weapon 武器的伤害
            item.width = 42;            //Weapon's texture's width 武器材质的宽度
            item.height = 54;           //Weapon's texture's height 武器材质的高度
            item.useTime = 15;          //The time span of using the weapon. Remember in terraria, 60 frames is a second. 使用武器的速度的时间跨度(以帧为单位)。在Terraria中，60帧是一秒。
            item.useAnimation = 15;          //The time span of the using animat3ion of the weapon, suggest set it the same as useTime. 使用武器的动画的时间跨度(以帧为单位),通常和useTime设置成一样
<<<<<<< HEAD:Items/Weapons/Melee/Sword/SkylineItem.cs
            item.useStyle = ItemUseStyleID.Swing;
=======
            item.useStyle = ItemUseStyleID.SwingThrow;
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96:Items/Weapons/Melee/Sword/晨昏线.cs
            item.knockBack = 5;         //The force of knockback of the weapon. Maximum is 20 武器的击退,最高是20
            item.value = Item.buyPrice(gold: 40);
            item.rare = ItemRarityID.Red;              //The rarity of the weapon, from -1 to 13 武器的稀有度,从-1到13
            item.UseSound = SoundID.Item1;      //The sound when the weapon is using 使用武器时的音效
            item.autoReuse = true;
            item.shoot = ProjectileType<Skyline>();
            item.shootSpeed = 13f;
            item.DamageType = DamageClass.Melee;
            item.noUseGraphic = false;
        }
        
        public override void AddRecipes()
        {
<<<<<<< HEAD:Items/Weapons/Melee/Sword/SkylineItem.cs
            CreateRecipe()
                .AddIngredient(ItemID.LunarBar, 18)
                .AddIngredient(ItemID.FragmentSolar, 15)
                .AddIngredient(ItemID.MagicDagger, 1)
                .AddIngredient(ItemID.ShadowFlameKnife, 1)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
=======
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.LunarBar, 18);
            recipe.AddIngredient(ItemID.FragmentSolar, 15);
            recipe.AddIngredient(ItemID.MagicDagger, 1);
            recipe.AddIngredient(ItemID.ShadowFlameKnife, 1);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this);
            recipe.AddRecipe();
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96:Items/Weapons/Melee/Sword/晨昏线.cs
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.Next(35) == 0)
            {
                //Emit dusts when swing the sword
                //使用时出现粒子效果
                Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 6);
            }
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(20));
            speedX = perturbedSpeed.X;
            speedY = perturbedSpeed.Y;
            return true;
        }

        public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Daybreak, 300);
        }
    }
}
//啊 啊 啊 啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊
//野 兽 咆 哮（半恼）