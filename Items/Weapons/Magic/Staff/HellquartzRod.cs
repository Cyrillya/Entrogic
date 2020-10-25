using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.Weapons.Magic.Staff
{
    public class HellquartzRod : ModItem
    {
        public override void SetStaticDefaults() { Item.staff[item.type] = true; }
        public override void SetDefaults()
        {
            item.UseSound = SoundID.Item20;
            item.Size = new Vector2(44, 62);
            item.useTime = 20;
            item.useAnimation = 20;
            item.useStyle = ItemUseStyleID.Shoot;
            item.knockBack = 8f;
            item.value = Item.sellPrice(0, 5);
            item.rare = RareID.LV6;
            item.autoReuse = item.channel = true;
            item.damage = 63;
            item.crit += 4;
            item.knockBack = 7f;
            item.shoot = ProjectileType<Projectiles.Magic.Staff.Hellquartz>();
            item.shootSpeed = 20f;
            item.noMelee = true;
            item.mana = 4;
            item.DamageType = DamageClass.Magic;
        }
        public override void UseStyle(Player player)
        {
            player.itemRotation = MathHelper.ToRadians(-90f + 8f) * player.direction;
            player.itemLocation = player.Center + new Vector2(0, 16f);
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            player.itemRotation = MathHelper.ToRadians(-90f + 8f) * player.direction;
            player.itemLocation = player.Center + new Vector2(0, 16f);

            speedX = 0f;
            speedY = -8f;
            for (int i = 0; i < 3; i++)
            {
                Vector2 radRand = new Vector2(Main.rand.Next(-50, 51), Main.rand.Next(10, 51) + 30);
                Projectile proj = Projectile.NewProjectileDirect(position + radRand, new Vector2(speedX, speedY), type, damage, knockBack, player.whoAmI);
                proj.localAI[0] = Main.MouseWorld.X;
                proj.localAI[1] = Main.MouseWorld.Y;
            }
            return false;
        }
    }
}