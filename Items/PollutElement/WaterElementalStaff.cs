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
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            item.mana = 10;
<<<<<<< HEAD:Items/PollutElement/WaterElementalStaff.cs
            item.damage = 58;
            item.useStyle = ItemUseStyleID.Swing;
=======
            item.damage = 66;
            item.useStyle = ItemUseStyleID.SwingThrow;
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96:Items/PollutElement/水元素召唤杖.cs
            item.width = 48;
            item.height = 46;
            item.useTime = 30;
            item.useAnimation = 30;
            item.noMelee = true;
            item.knockBack = 3f;
            item.value = Item.sellPrice(0, 5);
            item.rare = RareID.LV8;
            item.UseSound = SoundID.Item113;
<<<<<<< HEAD:Items/PollutElement/WaterElementalStaff.cs
            item.shoot = ProjectileType<WaterElemental>();
=======
            item.shoot = ProjectileType<运动水元素>();
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96:Items/PollutElement/水元素召唤杖.cs
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
<<<<<<< HEAD:Items/PollutElement/WaterElementalStaff.cs
            bool summonSuccessful = true;
            Projectile minion = Projectile.NewProjectileDirect(vector, Vector2.Zero, ProjectileType<WaterElemental>(), num, num2, Main.myPlayer, 0f, Main.rand.Next(2));
            if (player.slotsMinions + 0.33f > (float)player.maxMinions)
            {
                summonSuccessful = false;
            }
            Projectile.NewProjectile(vector, Vector2.Zero, ProjectileType<WaterElemental>(), num, num2, Main.myPlayer, 0f, Main.rand.Next(2));
            Projectile.NewProjectile(vector, Vector2.Zero, ProjectileType<WaterElemental>(), num, num2, Main.myPlayer, 0f, Main.rand.Next(2));
=======
            for (int i = 0; i < 3; i++)
                Projectile.NewProjectile(vector, Vector2.Zero, ProjectileType<运动水元素>(), num, num2, Main.myPlayer, 0f, 0f);
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96:Items/PollutElement/水元素召唤杖.cs

            if (summonSuccessful) return false;
            foreach (Projectile proj in Main.projectile)
            {
<<<<<<< HEAD:Items/PollutElement/WaterElementalStaff.cs
                if (proj.active && proj.type == ProjectileType<WaterElemental>())
=======
                if (proj.active && proj.type == ProjectileType<运动水元素>())
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96:Items/PollutElement/水元素召唤杖.cs
                {
                    proj.Center = vector;
                    return false;
                }
            }
            return false;
        }
    }
}
