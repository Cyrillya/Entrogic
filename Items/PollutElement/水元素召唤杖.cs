using System;
using Terraria;
using Terraria.Localization;
using Terraria.ID;
using System.Text;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;


namespace Entrogic.Items.PollutElement
{
    /// <summary>
    /// 水元素召唤杖 的摘要说明
    /// 创建人机器名：DESKTOP-QDVG7GB
    /// 创建时间：2019/8/9 21:17:10
    /// </summary>
    public class 水元素召唤杖 : ModItem
    {
        public override void SetDefaults()
        {
            item.mana = 10;
            item.damage = 66;
            item.useStyle = 1;
            item.width = 48;
            item.height = 46;
            item.useTime = 30;
            item.useAnimation = 30;
            item.noMelee = true;
            item.knockBack = 3f;
            item.value = Item.sellPrice(0, 5);
            item.rare = RareID.LV8;
            item.UseSound = SoundID.Item113;
            item.shoot = mod.ProjectileType("运动水元素");
            item.shootSpeed = 8f;
            item.summon = true;
            item.buffType = BuffType<Buffs.Minions.水元素>();
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
            for (int i = 0; i < 3; i++)
                Projectile.NewProjectile(vector, Vector2.Zero, mod.ProjectileType("运动水元素"), num, num2, Main.myPlayer, 0f, 0f);

            foreach (Projectile proj in Main.projectile)
            {
                if (proj.active && proj.type == mod.ProjectileType("运动水元素"))
                {
                    proj.Center = vector;
                    return false;
                }
            }
            return false;
        }
    }
}
