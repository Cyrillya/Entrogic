using System;
using Terraria;
using Terraria.Localization;
using Terraria.ID;
using System.Text;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Entrogic.Projectiles.Minions;

namespace Entrogic.Items.PollutElement
{
    /// <summary>
    /// 水元素召唤杖 的摘要说明
    /// 创建时间：2019/8/9 21:17:10
    /// </summary>
    public class WaterElementalStaff : ModItem
    {
        public override void SetDefaults()
        {
            item.mana = 10;
            item.damage = 58;
            item.useStyle = ItemUseStyleID.Swing;
            item.width = 48;
            item.height = 46;
            item.useTime = 30;
            item.useAnimation = 30;
            item.noMelee = true;
            item.knockBack = 3f;
            item.value = Item.sellPrice(0, 5);
            item.rare = RareID.LV8;
            item.UseSound = SoundID.Item113;
            item.shoot = ProjectileType<WaterElemental>();
            item.shootSpeed = 8f;
            item.DamageType = DamageClass.Summon;;
            item.buffType = BuffType<Buffs.Minions.WaterElementalBuff>();
            item.buffTime = 2;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            int num = damage;
            float num2 = knockBack;
            num2 = player.GetWeaponKnockback(item, num2);
            player.itemTime = item.useTime;
            Vector2 vector = player.RotatedRelativePoint(player.MountedCenter, true);
            vector.X = Main.mouseX + Main.screenPosition.X;
            vector.Y = Main.mouseY + Main.screenPosition.Y;
            bool summonSuccessful = true;
            Projectile minion = Projectile.NewProjectileDirect(vector, Vector2.Zero, ProjectileType<WaterElemental>(), num, num2, Main.myPlayer, 0f, Main.rand.Next(2));
            if (player.slotsMinions + 0.33f > (float)player.maxMinions)
            {
                summonSuccessful = false;
            }
            Projectile.NewProjectile(vector, Vector2.Zero, ProjectileType<WaterElemental>(), num, num2, Main.myPlayer, 0f, Main.rand.Next(2));
            Projectile.NewProjectile(vector, Vector2.Zero, ProjectileType<WaterElemental>(), num, num2, Main.myPlayer, 0f, Main.rand.Next(2));

            if (summonSuccessful) return false;
            foreach (Projectile proj in Main.projectile)
            {
                if (proj.active && proj.type == ProjectileType<WaterElemental>())
                {
                    proj.Center = vector;
                    return false;
                }
            }
            return false;
        }
    }
}
