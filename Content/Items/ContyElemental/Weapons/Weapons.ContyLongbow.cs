using Entrogic.Content.Items.BaseTypes;
using Entrogic.Content.Projectiles.ContyElemental;
using Entrogic.Content.Projectiles.ContyElemental.Friendly;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Content.Items.ContyElemental.Weapons
{
    public class ContyLongbow : ItemBase
    {
        public override void SetStaticDefaults() {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            DisplayName.SetDefault("Contamiated Longbow");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "污痕长弓");
            Tooltip.SetDefault("Turn wooden arrows into corrosive arrows. \n\"Don't forget to wear chemical protective clothing.\"");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, "把木箭变成腐蚀飞箭。\n“别忘了穿防化服”");
        }
        public override void SetDefaults() {
            Item.damage = 96;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 28;
            Item.height = 60;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.crit += 22;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 7f;
            Item.value = Item.sellPrice(0, 5);
            Item.rare = RarityLevelID.MiddleHM;
            Item.UseSound = SoundID.Item10;
            Item.autoReuse = true;
            Item.useAmmo = AmmoID.Arrow;
            Item.shoot = ProjectileType<CorrosiveArrow>();
            Item.shootSpeed = 8f;
        }
        public override Vector2? HoldoutOffset() {
            return new Vector2(-6, 0);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            if (type == ProjectileID.WoodenArrowFriendly) {
                type = ProjectileType<CorrosiveArrow>();
            }
            Vector2 vector = player.RotatedRelativePoint(player.MountedCenter, true);
            float num = Utils.NextFloat(Main.rand) * 6.28318548f;
            float value = 40f;
            float value2 = 110f;
            Vector2 pos = vector + Utils.ToRotationVector2(num) * MathHelper.Lerp(value, value2, Utils.NextFloat(Main.rand));
            for (int i = 0; i < 50; i++) {
                pos = vector + Utils.ToRotationVector2(num) * MathHelper.Lerp(value, value2, Utils.NextFloat(Main.rand));
                if (Collision.CanHit(vector, 0, 0, pos + Utils.SafeNormalize(pos - vector, Vector2.UnitX) * 8f, 0, 0)) {
                    break;
                }
                num = Utils.NextFloat(Main.rand) * 6.28318548f;
            }
            Vector2 vel = Vector2.Normalize(Main.MouseWorld - pos) * Item.shootSpeed;
            Projectile.NewProjectile(source, pos, vel, type, damage, knockback, player.whoAmI, 0f, 0f);
            ApporchEffect(pos, vel);
            return false;
        }
        private void ApporchEffect(Vector2 pos, Vector2 vel) {
            float num = 16f;
            int num2 = 0;
            while ((float)num2 < num) {
                int num3 = MyDustID.White;
                Color color = default(Color);
                if (num2 <= 8) color = Color.Orange;
                Vector2 vector12 = Vector2.UnitX * 0f;
                vector12 += -Vector2.UnitY.RotatedBy((double)((float)num2 * (6.28318548f / num)), default) * new Vector2(1f, 4f);
                vector12 = vector12.RotatedBy((double)vel.ToRotation(), default);
                Dust d = Dust.NewDustDirect(pos, 0, 0, num3, 0f, 0f, 0, new Color(56, 114, 80), 1f);
                d.scale = 1.5f;
                d.noGravity = true;
                d.position = pos + vector12;
                d.velocity = vector12.SafeNormalize(Vector2.UnitY) * 1f;
                int num4 = num2;
                num2 = num4 + 1;
            }
        }
    }
}
