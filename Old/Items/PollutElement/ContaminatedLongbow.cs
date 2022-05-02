using Entrogic.Projectiles.Ranged.Bows;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.PollutElement
{
    public class ContaminatedLongbow : ModItem
    {
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            DisplayName.SetDefault("污痕长弓");
            Tooltip.SetDefault("“别忘了穿防化服”\n" +
                "把木箭变成腐蚀飞箭");
        }
        public override void SetDefaults()
        {
            item.damage = 50;
            item.DamageType = DamageClass.Ranged;
            item.width = 28;
            item.height = 60;
            item.useAnimation = 5;
            item.useTime = 5;
            item.reuseDelay = item.useAnimation + 5;
            item.crit += 22;
<<<<<<< HEAD:Items/PollutElement/ContaminatedLongbow.cs
            item.useStyle = ItemUseStyleID.Shoot;
=======
            item.useStyle = ItemUseStyleID.HoldingOut;
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96:Items/PollutElement/污痕长弓.cs
            item.noMelee = true;
            item.knockBack = 7f;
            item.value = Item.sellPrice(0, 5);
            item.rare = ItemRarityID.Yellow;
            item.UseSound = SoundID.Item10;
            item.autoReuse = true;
            item.useAmmo = AmmoID.Arrow;
            item.shoot = ProjectileType<污痕射弹>();
            item.shootSpeed = 28f;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-6, 0);
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (type == ProjectileID.WoodenArrowFriendly)
            {
                type = ProjectileType<污痕射弹>();
            }
            Vector2 vector = player.RotatedRelativePoint(player.MountedCenter, true);
            float shootSpeed = item.shootSpeed;
            float x = (float)Main.mouseX + Main.screenPosition.X - vector.X;
            float y = (float)Main.mouseY + Main.screenPosition.Y - vector.Y;
            float num = Utils.NextFloat(Main.rand) * 6.28318548f;
            float value = 20f;
            float value2 = 60f;
            Vector2 vector2 = vector + Utils.ToRotationVector2(num) * MathHelper.Lerp(value, value2, Utils.NextFloat(Main.rand));
            for (int i = 0; i < 50; i++)
            {
                vector2 = vector + Utils.ToRotationVector2(num) * MathHelper.Lerp(value, value2, Utils.NextFloat(Main.rand));
                if (Collision.CanHit(vector, 0, 0, vector2 + Utils.SafeNormalize(vector2 - vector, Vector2.UnitX) * 8f, 0, 0))
                {
                    break;
                }
                num = Utils.NextFloat(Main.rand) * 6.28318548f;
            }
            Vector2 mouseWorld = Main.MouseWorld;
            Vector2 vector3 = mouseWorld - vector2;
            Vector2 vector4 = Utils.SafeNormalize(new Vector2(x, y), Vector2.UnitY) * shootSpeed;
            vector3 = Utils.SafeNormalize(vector3, vector4) * shootSpeed;
            vector3 = Vector2.Lerp(vector3, vector4, 0.35f);
            Projectile.NewProjectile(vector2, vector3, type, damage, knockBack, player.whoAmI, 0f, 0f);
            return false;
        }
    }
}
