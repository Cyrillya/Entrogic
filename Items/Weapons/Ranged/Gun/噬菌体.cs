using Terraria;
using Terraria.ModLoader;
using Entrogic.Projectiles.Ranged.Bullets;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;

namespace Entrogic.Items.Weapons.Ranged.Gun
{
    public class 噬菌体 : ModItem
    {
        public override void SetDefaults()
        {
            item.damage = 36;
            item.crit -= 9;
            item.knockBack = 5f;
            item.DamageType = DamageClass.Ranged;
            item.useTime = 15;
            item.useAnimation = 15;
            item.useStyle = ItemUseStyleID.Shoot;
            item.shootSpeed = 10f;
            item.noMelee = true;
            item.shoot = Bullet;
            item.value = Item.sellPrice(0, 0, 80, 0);
        }
        public int Bullet => ProjectileType<Projectiles.Ranged.Bullets.噬菌体>();
    }
}
