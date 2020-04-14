using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.Weapons.Ranged.Gun
{
    public class 老旧的步枪: ModItem
    {
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("老旧的步枪");
            Tooltip.SetDefault("“枪身上的磨痕代表着它昔日的荣耀”\n" +
                "“伪M1-拉栓式”");
        }
        public override void SetDefaults()
        {
            item.damage = 26;
            item.shoot = 10;
            item.shootSpeed = 13f;
            item.useAmmo = AmmoID.Bullet;
            item.ranged = true;
            item.useStyle = 5;
            item.useTime = 65;
            item.useAnimation = 65;
            item.maxStack = 1;
            item.value = 10000;
            item.rare = 1;
            item.knockBack = 4f;
            item.crit = 31;
            item.width = 76;
            item.height = 28;
            item.scale = 0.8f;
            item.noMelee = true;
            item.autoReuse = false;
        }
        Vector2 oldMouse = Vector2.Zero;
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Timer = 0;
            player.direction = 1;
            player.itemRotation = (float)Math.Atan2(((oldMouse.Y + Main.screenPosition.Y) - player.Center.Y) * player.direction, ((oldMouse.X + Main.screenPosition.X) - player.Center.X) * player.direction);
            speedX = ((oldMouse.X + Main.screenPosition.X) - player.Center.X) * player.direction;
            speedY = ((oldMouse.Y + Main.screenPosition.Y) - player.Center.Y) * player.direction;

            bool flag = false;
            Item ammo = null;
            for (int i = 54; i < 58; i++)
            {
                if (player.inventory[i].ammo == item.useAmmo && player.inventory[i].stack > 0)
                {
                    ammo = player.inventory[i];
                    flag = true;
                    break;
                }
            }
            if (!flag)
            {
                for (int j = 0; j < 54; j++)
                {
                    if (player.inventory[j].ammo == item.useAmmo && player.inventory[j].stack > 0)
                    {
                        ammo = player.inventory[j];
                        break;
                    }
                }
            }
            if (ammo.consumable)
            {
                ammo.stack--;
                if (ammo.stack <= 0)
                {
                    ammo.active = false;
                    ammo.TurnToAir();
                }
            }
            Projectile.NewProjectile(position, new Vector2(speedX, speedY), type, damage, knockBack, player.whoAmI);
            Main.PlaySound(SoundID.Item40, player.position);
            return false;
        }
        public float Timer = 0;
        public override void UseStyle(Player player)
        {
            Timer++;
            if (player.itemAnimation >= player.itemAnimationMax - 1 && Timer <= 60)
            {
                player.direction = oldMouse.X + Main.screenPosition.X > player.Center.X ? 1 : -1;
                player.itemRotation = (float)Math.Atan2(((oldMouse.Y + Main.screenPosition.Y) - player.Center.Y) * player.direction, ((oldMouse.X + Main.screenPosition.X) - player.Center.X) * player.direction);
                player.itemAnimation = (int)player.itemAnimationMax;
                player.itemTime = (int)item.useTime;
                if (Timer == 60)
                {
                    player.itemAnimation = 15;
                    player.itemTime = 15;
                }
                if (Timer == 2)
                {
                    oldMouse.X = Main.mouseX;
                    oldMouse.Y = Main.mouseY;
                }
            }
        }
    }
}
